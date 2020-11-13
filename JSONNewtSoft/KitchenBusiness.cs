using QT.KitchenManagementAdmin.Business.Interfaces;
using QT.KitchenManagementAdmin.Business.Models;
using QT.KitchenManagementAdmin.Common.Attributes;
using QT.KitchenManagementAdmin.Common.Enums;
using QT.KitchenManagementAdmin.Repository.Interfaces;
using QT.KitchenManagementAdmin.Repository.Models;
using QT.Posera.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using QT.Common.Logging.Interfaces;
using QT.KitchenManagementAdmin.Common.Config;
using QT.KitchenSettingsManager.Common.Enums;
using QT.KitchenSettingsManager.Helpers.Interfaces;
using QT.KitchenSettingsManager.Interfaces;
using QT.KitchenSettingsManager.Repository.Interfaces;
using System.Configuration;
using System.Threading;
using QT.KitchenManagement.WMI.Interfaces;
using QT.KitchenOrderTransaction.Interfaces;

namespace QT.KitchenManagementAdmin.Business {
    public class KitchenBusiness : IKitchenBusiness {
        private readonly IWMIHelper _wmi;
        private readonly IKitchenOrderRepository _repository;
        private readonly IKitchenDisplaySystemAdminRepository _kdsRepository;
        private readonly IKitchenStaffingCalculator _kitchenStaffingCalculator;
        private readonly IBusinessCommon _common;
        private readonly ICommandTransmitter _commandTransmitter;
        private readonly ILogger _logger;
        private readonly IKitchenDisplaySystemHelpers _kitchenDisplaySystemHelper;
        private readonly IKitchenSettingsManager _kitchenSettingsManager;
        private readonly IDevicesBusiness _deviceBusiness;
        private StatusModel _kitchenModel;
        private Dictionary<string, string> _kitchenSettings;

        private readonly int _minEmployeeCountDisplayed;
        private readonly int _maxEmployeeCountDisplayed;
        private readonly int _maxOrdersPerEmployee;
        private readonly int _bumpedOrderTimeFrame;
        private readonly int _maxPrepTimePerEmployee;

        #region Public Methods

        public KitchenBusiness(IWMIHelper wmi, IKitchenOrderRepository repository, IKitchenDisplaySystemAdminRepository kdsRepository, IBusinessCommon common, ICommandTransmitter commandTransmitter, ILogger logger, IKitchenDisplaySystemHelpers kitchenDisplaySystemHelper, IKitchenSettingsManager kitchenSettingsManager, IDevicesBusiness devicesBusiness, IKitchenStaffingCalculator kitchenStaffingCalculator)
        {
            _wmi = wmi ?? throw new ArgumentNullException(nameof(wmi));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _kdsRepository = kdsRepository ?? throw new ArgumentNullException(nameof(kdsRepository));
            _common = common ?? throw new ArgumentNullException(nameof(common));
            _commandTransmitter = commandTransmitter ?? throw new ArgumentNullException(nameof(commandTransmitter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _kitchenDisplaySystemHelper = kitchenDisplaySystemHelper ?? throw new ArgumentNullException(nameof(kitchenDisplaySystemHelper));
            _kitchenSettingsManager = kitchenSettingsManager ?? throw new ArgumentNullException(nameof(kitchenSettingsManager));
            _deviceBusiness = devicesBusiness ?? throw new ArgumentNullException(nameof(devicesBusiness));
            _kitchenStaffingCalculator = kitchenStaffingCalculator ?? throw new ArgumentNullException(nameof(_kitchenStaffingCalculator));

            _minEmployeeCountDisplayed = int.TryParse(_kitchenSettingsManager.GetKitchenSetting("MinEmployeeCountDisplayed"), out var parsedValueMinEmployeesDisplayed) ? parsedValueMinEmployeesDisplayed : 1;
            _maxEmployeeCountDisplayed = int.TryParse(_kitchenSettingsManager.GetKitchenSetting("MaxEmployeeCountDisplayed"), out var parsedValueMaxEmployeesDisplayed) ? parsedValueMaxEmployeesDisplayed : 6;
            _maxOrdersPerEmployee = int.TryParse(_kitchenSettingsManager.GetKitchenSetting("MaxOrdersPerEmployee"), out var parsedValueMaxOrdersPerEmployee) ? parsedValueMaxOrdersPerEmployee : 3;
            _bumpedOrderTimeFrame = int.TryParse(_kitchenSettingsManager.GetKitchenSetting("BumpedOrderTimeFrame"), out var parsedValueBumpedTimeFrame) ? parsedValueBumpedTimeFrame : 60;
            _maxPrepTimePerEmployee = int.TryParse(_kitchenSettingsManager.GetKitchenSetting("MaxPrepTimePerEmployee"), out var parsedValueMaxPrepTime) ? parsedValueMaxPrepTime : 300;
        }

        [MTAThread]
        public async Task<KitchenModel> GetOrderCounts(CancellationToken token, int timeSpan = 30)
        {
            return await Task.Run(() => _repository.GetOrders(timeSpan), token);
        }

        [MTAThread]
        public async Task<StatusModel> GetKitchenStatus(CancellationToken token)
        {
            return await Task.Run(() => BuildKitchenStatus(), token);
        }

        [MTAThread]
        public async Task<Dictionary<string, string>> GetKitchenSettings(CancellationToken token)
        {
            return await Task.Run(() => BuildKitchenSettings(), token);
        }

        [MTAThread]
        public async Task<Templates> GetActiveBuildScreenMode(CancellationToken token)
        {
            return await Task.Run(() => _kdsRepository.GetActiveBuildScreenMode(), token);
        }

        [MTAThread]
        public async Task<Templates> UpdateBuildScreenMode(Templates template, CancellationToken token)
        {
            try
            {
                var maxAttempts = Convert.ToInt32(ConfigurationManager.AppSettings[ConfigConstants.MaxDeviceStatusAttempts]);
                var delayBetweenCalls = Convert.ToInt32(ConfigurationManager.AppSettings[ConfigConstants.DelayBetweenApiCalls]);
                var delayForKdsAppToBeFullyStarted = Convert.ToInt32(ConfigurationManager.AppSettings[ConfigConstants.DelayForKdsAppToBeFullyStarted]);

                //Stop the transaction service so we don't have mobile orders being sent to the build screen while we are in transition.
                if (_wmi.IsServiceStarted("localhost", "QT Kitchen Order Transaction Service"))
                {
                    _wmi.StopService("localhost", "QT Kitchen Order Transaction Service");
                }

                var templateName = "AllScreens";
                if (template != Templates.AllScreens)
                {
                    var currentUpdateId = _common.GetCurrentUpdateIdFromLocalApi();
                    templateName = _kdsRepository.GetTemplateNameFromMenuId(currentUpdateId);
                }

                _kdsRepository.SetActiveTemplate(templateName);

                var devices = _kitchenDisplaySystemHelper.GetAllKitchenDisplayHostnames();
                var tasks = devices.Select(x => UpdateBuildScreen(devices.FirstOrDefault(), x, token)).ToList();

                await Task.WhenAll(tasks);

                if (!tasks.All(x => x.Result))
                {
                    foreach (var task in tasks)
                    {
                        if (!task.Result)
                        {
                            _logger.ErrorWithInner(new Exception("An error occurred updating build screens", task.Exception));
                        }
                    }
                }

                var resendKDSSettingsAfterAppRestart = devices.Select(device => SendSettingsAfterAppRestart(device, maxAttempts, delayBetweenCalls, delayForKdsAppToBeFullyStarted, token)).ToList();
                await Task.WhenAll(resendKDSSettingsAfterAppRestart);
            }
            finally
            {
                if (!_wmi.IsServiceStarted("localhost", "QT Kitchen Order Transaction Service"))
                {
                    _wmi.StartService("localhost", "QT Kitchen Order Transaction Service");
                }
            }
            return _kdsRepository.GetActiveBuildScreenMode(); ;
        }

        [MTAThread]
        public async Task<bool> ResetBuildScreens(CancellationToken token)
        {
            try
            {
                //Stop the transaction service so we don't have mobile orders being sent to the build screen while we are in transition.
                if (_wmi.IsServiceStarted("localhost", "QT Kitchen Order Transaction Service"))
                {
                    _wmi.StopService("localhost", "QT Kitchen Order Transaction Service");
                }

                var devices = _kitchenDisplaySystemHelper.GetAllKitchenDisplayHostnames();
                var tasks = devices.Select(x => ResetBuildScreen(x, token)).ToList();
                await Task.WhenAll(tasks);
                return tasks.All(x => x.Result);
            }
            finally
            {
                if (!_wmi.IsServiceStarted("localhost", "QT Kitchen Order Transaction Service"))
                {
                    _wmi.StartService("localhost", "QT Kitchen Order Transaction Service");
                }
            }
        }

        [MTAThread]
        public async Task<dynamic> GetCurrentMenuId(CancellationToken token)
        {
            return await Task.Run(() =>
            {
                var updateId = _common.GetCurrentUpdateIdFromLocalApi();
                var folderExists = _common.DoMenuFilesExist("localhost", updateId);
                const bool status = false;
                return new { updateId, folderExists, status };
            }, token);
        }

        [MTAThread]
        public async Task<KitchenDeviceMenuModels> GetCurrentMenuIdForKpcAndAllCss(CancellationToken token)
        {
            return await Task.Run(() => _common.GetCurrentMenuIdForAllKitchenMachines(), token);
        }

        [MTAThread]
        public async Task<bool> RunKitchenMenuSync(CancellationToken token)
        {
            return await Task.Run(() => _common.RunKitchenMenuSync(), token);
        }

        [MTAThread]
        public async Task<FileStream> GetLogFile(CancellationToken token)
        {
            return await Task.Run(() => _common.GetLogFile(LogType.SyncLog), token);
        }

        [MTAThread]
        public async Task<int> GetKitchenStaffingCount(CancellationToken token)
        {
            return await Task.Run(() => BuildStaffingCount(), token);
        }

        [MTAThread]
        public async Task<string> GetDeliveryNotificationText(CancellationToken token)
        {
            var delivery = await Task.Run(() => _repository.GetUnacknowledgedDeliveryCounts(), token);

            if (delivery.OnLotCheckIns > 0)
            {
                return "ONLOT";
            }

            return delivery.InStoreCheckIns > 0 ? "INSTORE" : string.Empty;
        }

        #endregion

        #region Private Methods

        private async Task<bool> SendSettingsAfterAppRestart(string device, int maxAttempts, int delayBetweenApiCalls, int delayForKdsAppToBeFullyStarted, CancellationToken token)
        {
            var attempts = 0;
            while (attempts < maxAttempts)
            {
                var deviceStatus = await _deviceBusiness.Status(device, token);
                if (deviceStatus.IsAppRunning)
                {
                    await Task.Delay(delayForKdsAppToBeFullyStarted, token);
                    return await SendSettingsToAllKDSes(device, token);
                }
                await Task.Delay(delayBetweenApiCalls, token);
                attempts++;
            }
            return false;
        }

        private StatusModel BuildKitchenStatus()
        {
            if (_kitchenModel == null)
            {
                _kitchenModel = new StatusModel();
                _kitchenModel.ServicesToCheck = Applications.KitchenPC.GetAttributes<ApplicationAttribute>().Services;
            }

            _kitchenModel.DeviceName = "localhost";
            _kitchenModel.LastReboot = _wmi.LastRebootTime("localhost");
            foreach (var service in _kitchenModel.ServicesToCheck)
            {
                if (!_wmi.IsServiceStarted("localhost", service))
                {
                    if (!_kitchenModel.ServicesStopped.Exists(x => x == service))
                    {
                        _kitchenModel.ServicesStopped.Add(service);
                    }
                }
                else
                {
                    if (_kitchenModel.ServicesStopped.Exists(x => x == service))
                    {
                        _kitchenModel.ServicesStopped.Remove(service);
                    }
                }
            }
            _kitchenModel.LastUpdated = DateTime.Now;

            if (_kitchenModel.ServicesStopped.Count == 0)
            {
                _kitchenModel.Status = "Up";
            }
            else if (_kitchenModel.ServicesStopped.Count == _kitchenModel.ServicesToCheck.Count)
            {
                _kitchenModel.Status = "Down";
            }
            else
            {
                _kitchenModel.Status = "Warning";
            }

            return _kitchenModel;
        }

        private int BuildStaffingCount()
        {
            var calculatedNeed = _kitchenStaffingCalculator.GetKitchenEmployeeCountData(_bumpedOrderTimeFrame, _maxOrdersPerEmployee, _maxPrepTimePerEmployee).Max();

            if (calculatedNeed > _maxEmployeeCountDisplayed)
                calculatedNeed = _maxEmployeeCountDisplayed;

            // Cap MIN employee need displayed
            if (calculatedNeed < _minEmployeeCountDisplayed)
                calculatedNeed = _minEmployeeCountDisplayed;

            return calculatedNeed;
        }

        private Dictionary<string, string> BuildKitchenSettings()
        {
            if (_kitchenSettings == null)
            {
                _kitchenSettings = new Dictionary<string, string>();
                _kitchenSettings.Add(ConfigConstants.CSSMachineCount, _kitchenSettingsManager.GetKitchenSetting(ConfigConstants.CSSMachineCount));
                _kitchenSettings.Add(ConfigConstants.KDSMachineCount, _kitchenSettingsManager.GetKitchenSetting(ConfigConstants.KDSMachineCount));
            }

            return _kitchenSettings;
        }

        private async Task<bool> ResetBuildScreen(string device, CancellationToken token)
        {
            _commandTransmitter.ClearAllOrders(device);
            SendSettingsToAllKDSes(device, token).Wait(token);
            RestartAllKds(device, token).Wait(token);
            return true;
        }

        private async Task<bool> UpdateBuildScreen(string sourceDevice, string device, CancellationToken token)
        {
            CopyOrders(sourceDevice, device, token).Wait(token);
            SendSettingsToAllKDSes(device, token).Wait(token);
            RestartAllKds(device, token).Wait(token);
            return true;
        }

        private async Task<bool> CopyOrders(string sourceDevice, string destDevice, CancellationToken token)
        {
            if (sourceDevice == destDevice) return false;

            try
            {
                await Task.Run(() =>
                {
                    if (!KitchenManagement.WMI.Helpers.Pinger.IsPingable(sourceDevice)) throw new Exception($"Could not connect to source {sourceDevice} to copy the Orders.dat file");
                    var copyFromPath = $@"\\{sourceDevice}\C-Drive\Users\Public\CVM";

                    if (!KitchenManagement.WMI.Helpers.Pinger.IsPingable(destDevice)) throw new Exception($"Could not copy Orders.dat from {copyFromPath} to {destDevice}.");

                    var source = new FileInfo(Path.Combine(copyFromPath, "Orders.dat"));

                    if (File.Exists(source.FullName))
                    {
                        var dest = $@"\\{destDevice}\C-Drive\Users\Public\CVM\Orders.dat";
                        source.CopyTo(dest, true);
                    }
                }, token);
                return true;
            }
            catch (Exception ex)
            {
                _logger.ErrorWithInner(new Exception("Error copying Orders.dat file.", ex));
            }

            return false;
        }

        private async Task<bool> RestartAllKds(string device, CancellationToken token)
        {
            try
            {
                await Task.Run(() => _commandTransmitter.ExitApplication(device), token);
                return true;
            }
            catch (Exception ex)
            {
                _logger.ErrorWithInner(new Exception($"Could not restart app on {device}", ex));
            }

            return false;
        }

        private async Task<bool> SendSettingsToAllKDSes(string device, CancellationToken token)
        {
            try
            {
                await Task.Run(() =>
                {
                    var settings = _kdsRepository.GetKDSSettingsForDevice(device);
                    _common.SendSettingToKDS(device, settings);
                }, token);
                return true;
            }
            catch (Exception ex)
            {
                _logger.ErrorWithInner(new Exception($"Could not send settings to {device}", ex));
            }

            return false;
        }

        #endregion
    }
}