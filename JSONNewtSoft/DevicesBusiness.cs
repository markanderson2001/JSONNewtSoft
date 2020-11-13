using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using QT.Common.ConfigurationService.Interfaces;
using QT.Common.Logging.Interfaces;
using QT.Common.StoreMachineResolution.Interfaces;
using QT.KitchenManagement.WMI.Interfaces;
using QT.KitchenManagementAdmin.Business.Interfaces;
using QT.KitchenManagementAdmin.Business.Models;
using QT.KitchenManagementAdmin.Common.Attributes;
using QT.KitchenManagementAdmin.Common.Extensions;
using QT.KitchenManagementAdmin.Common.Config;
using QT.KitchenManagementAdmin.Common.Enums;
using QT.KitchenManagementAdmin.Repository.Interfaces;
using QT.KitchenSettingsManager.Common.Enums;
using QT.KitchenSettingsManager.Interfaces;
using QT.KitchenSettingsManager.Repository.Interfaces;
using QT.Posera.Interfaces;
using MachineType = QT.Common.StoreMachineResolution.Enums.MachineType;
using QT.Kitchen.Common.Enums;
using QT.Common.StoreMachine.Interfaces;

namespace QT.KitchenManagementAdmin.Business
{
    public class DevicesBusiness : IDevicesBusiness
    {
        private readonly ILogger _logger;
        private readonly IWMIHelper _wmi;
        private readonly IOrderPointAPIRepository _orderPointApiRepository;
        private readonly IKitchenDisplaySystemAdminRepository _kdsRepository;
        private readonly IBusinessCommon _common;
        private readonly ICommandTransmitter _commandTransmitter;
        private readonly IKitchenSettingsManager _kitchenSettingsManager;
        private readonly IConfigurationService _config;
        private readonly IStoreMachineNameResolver _storeResolver;
        private readonly IDivisionToAirportCodeResolver _divisionResolver;
        private readonly List<StatusModel> _statues;
        private readonly IStoreMachineService _storeMachineService;

        public DevicesBusiness(ILogger logger, IWMIHelper wmi, IOrderPointAPIRepository orderPointApiRepository, IKitchenDisplaySystemAdminRepository kdsRepository, IBusinessCommon common, ICommandTransmitter commandTransmitter, IKitchenSettingsManager kitchenSettingsManager, IConfigurationService config, IStoreMachineNameResolver storeResolver, IDivisionToAirportCodeResolver divisionResolver, IStoreMachineService storeMachineService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _wmi = wmi ?? throw new ArgumentNullException(nameof(wmi));
            _orderPointApiRepository = orderPointApiRepository ?? throw new ArgumentNullException(nameof(orderPointApiRepository));
            _kdsRepository = kdsRepository ?? throw new ArgumentNullException(nameof(kdsRepository));
            _common = common ?? throw new ArgumentNullException(nameof(common));
            _commandTransmitter = commandTransmitter ?? throw new ArgumentNullException(nameof(commandTransmitter));
            _kitchenSettingsManager = kitchenSettingsManager ?? throw new ArgumentNullException(nameof(kitchenSettingsManager));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _storeResolver = storeResolver ?? throw new ArgumentNullException(nameof(storeResolver));
            _divisionResolver = divisionResolver ?? throw new ArgumentNullException(nameof(divisionResolver));
            _storeMachineService = storeMachineService ?? throw new ArgumentNullException(nameof(storeMachineService));
            _statues = new List<StatusModel>();
        }

        #region Public Methods
        [MTAThread]
        public async Task<bool> RebootDevice(string deviceName, CancellationToken token)
        {
            //MA New 
            var response = await _storeMachineService.Reboot(deviceName, token);

            if (response != null && response.CompletedSuccessfully)
            {
                return response?.CompletedSuccessfully ?? false;
            }
            else
            {
                return await Task.Run(() => _wmi.Reboot(deviceName), token);
            }
        }

        [MTAThread]
        public async Task<bool> InstallMSIAndRebootDevice(string deviceName, CancellationToken token)
        {
            //MA flag here old/new way
            var response = await _storeMachineService.RebootAndInstallUpdates(deviceName, token);
            if (response != null && response.CompletedSuccessfully)
            {
                return response?.CompletedSuccessfully ?? false;
            }
            else
            {
                //MA old
                var packageInstallerPath = ConfigurationManager.AppSettings[ConfigConstants.MsiInstallPath];
                if (string.IsNullOrEmpty(packageInstallerPath))
                    throw new Exception(
                        $"Config value for {ConfigConstants.MsiInstallPath} was blank. Set this value in the config and try again.");
                packageInstallerPath = $@"{packageInstallerPath}\PackageInstaller.exe";

                return await Task.Run(
                    () => _wmi.InstallMSIAndReboot(deviceName, packageInstallerPath,
                        (byte)KitchenAdminCustomCommands.Install), token);
            }
        }

        [MTAThread]
        public async Task<bool> RestartApp(string deviceName, CancellationToken token)
        {
            if (deviceName.GetApplication() == Applications.PoseraKDS)
            {
                await Task.Run(() => _commandTransmitter.ExitApplication(deviceName), token);
            }
            else
            {
                //MA new
                var deviceStatus = _statues.FirstOrDefault(x => x.DeviceName == deviceName);
                var response = await _storeMachineService.StopApplication(deviceStatus?.ProcessExecutable, deviceName, token);

                if (response != null && response.CompletedSuccessfully)
                {
                    return response?.CompletedSuccessfully ?? false;
                }
                else
                {
                    await Task.Run(() =>
                    {
                        if (deviceStatus != null) _wmi.KillExistingProcess(deviceName, deviceStatus.ProcessExecutable);
                    }, token);
                }

            }

            return true;
        }

        [MTAThread]
        public async Task<bool> Lock(string deviceName, CancellationToken token)
        {
            //MA flag here old/new way
            var response = await _storeMachineService.Lock(deviceName, token);
            if (response != null && response.CompletedSuccessfully)
            {
                return response?.CompletedSuccessfully ?? false;
            }
            else
            {
                var deviceStatus = _statues.FirstOrDefault(x => x.DeviceName == deviceName);

                if (!await Task.Run(() => _wmi.LockDevice(deviceName, deviceStatus.ProcessExecutable), token)) return false;

                await Task.Run(() => KillExplorer(deviceName), token);
                return await Task.Run(() => RestartServices(deviceName), token);
            }
        }

        [MTAThread]
        public async Task<bool> Unlock(string deviceName, CancellationToken token)
        {
            //MA flag here old/new way
            var unlockResponse = await _storeMachineService.Unlock(deviceName, token);

            if (unlockResponse != null && unlockResponse.CompletedSuccessfully)
            {
                return unlockResponse?.CompletedSuccessfully ?? false;
            }
            else
            {
                var deviceStatus = _statues.FirstOrDefault(x => x.DeviceName == deviceName);
                if (await Task.Run(() => _wmi.UnlockDevice(deviceName), token))
                {
                    if (await Task.Run(() => StartExplorer(deviceName), token))
                    {
                        //Kills either the QT.Kiosk app or the Posera KDS app depending on device name.
                        await Task.Run(() => StopService(deviceName, "QT Bootstrapper Service", token), token);
                        await Task.Run(() => _wmi.KillExistingProcess(deviceName, deviceStatus.ProcessExecutable), token);
                        return await Task.Run(() => RestartServices(deviceName), token);
                    }
                }
                return false;
            }
        }

        [MTAThread]
        public async Task<bool> CacheStatus(string deviceName, CancellationToken token)
        {
            var status = _statues.FirstOrDefault(x => x.DeviceName == deviceName) ?? new StatusModel
            {
                DeviceName = deviceName,
                ServicesToCheck = deviceName.GetApplication().GetAttributes<ApplicationAttribute>().Services,
                ProcessExecutable = deviceName.GetApplication().GetAttributes<ApplicationAttribute>().Executable
            };

            var tasks = new List<Task>();
            try
            {
                if (!KitchenManagement.WMI.Helpers.Pinger.IsPingable(deviceName))
                {
                    throw new Exception($"Unable to ping device {deviceName}.");
                }

                status.Status = _wmi.GetComputerStatus(deviceName);

                tasks.Add(Task.Run(() => status.CPUPercentage = _wmi.GetCPUPercentage(deviceName), token));
                tasks.Add(Task.Run(() => status.MemoryUsage = _wmi.GetMemoryUsage(deviceName), token));
                tasks.Add(Task.Run(() => status.HardDriveFree = _wmi.GetHardDiskUsage(deviceName), token));

                //MA New 
                var unlockResponse = await _storeMachineService.Unlock(deviceName, token);
                //MA check validate line
                var response = await _storeMachineService.GetServiceStatus(status.ServicesToCheck.ToString(), deviceName, token);
                if (response != null && response.CompletedSuccessfully)
                {
                    tasks.Add(Task.Run(async () => status.ServicesStopped = await GetStoppedServices(status.ServicesToCheck, deviceName, token), token));
                }
                else
                {
                    //MAold
                    tasks.Add(Task.Run(() => status.ServicesStopped = _wmi.GetStoppedServices(deviceName, status.ServicesToCheck), token));
                }

                tasks.Add(Task.Run(() => status.LastReboot = _wmi.LastRebootTime(deviceName), token));

                await Task.WhenAll(tasks);

                if (!deviceName.Contains("KDS")) await KioskAPIStatus(status, token);
            }
            catch (Exception ex)
            {
                _logger.ErrorWithInner(ex, $"Device {deviceName} encountered an exception. ");
                status.Status = status.Status == string.Empty ? "Down" : status.Status;
            }
            finally
            {
                tasks.ForEach(delegate (Task t)
                {
                    t.Dispose();
                });
                tasks = null;
            }

            status.LastUpdated = DateTime.Now;

            if (!_statues.Exists(x => x.DeviceName == status.DeviceName))
            {
                _statues.Add(status);
            }

            return true;
        }

        [MTAThread]
        public async Task<StatusModel> Status(string deviceName, CancellationToken token, bool refresh = false)
        {
            deviceName = deviceName.ToUpper();

            var deviceStatus = _statues.FirstOrDefault(x => x.DeviceName == deviceName) ?? new StatusModel();
            deviceStatus.DeviceName = deviceName;

            if (KitchenManagement.WMI.Helpers.Pinger.IsPingable(deviceName))
            {

                if (!_statues.Exists(x => x.DeviceName == deviceName) || refresh)
                {
                    CacheStatus(deviceName, token).Wait(token);
                    deviceStatus = _statues.FirstOrDefault(x => x.DeviceName == deviceName) ?? new StatusModel();
                }

                var tasks = new List<Task>();
                try
                {
                    if (!KitchenManagement.WMI.Helpers.Pinger.IsPingable(deviceName))
                    {
                        throw new Exception($"Unable to ping device {deviceName}.");
                    }

                    deviceStatus.Status = _wmi.GetComputerStatus(deviceName);
                    tasks.Add(Task.Run(() => deviceStatus.IsLocked = _wmi.IsShellEnabled(deviceName), token));
                    tasks.Add(Task.Run(() => deviceStatus.IsAppRunning = _wmi.ProcessExists(deviceName, deviceStatus.ProcessExecutable), token));

                    await Task.WhenAll(tasks);

                    if (deviceName.ContainsInsensitive("kds")) return deviceStatus;

                    await KioskAPIStatus(deviceStatus, token);
                }
                catch (Exception ex)
                {
                    _logger.ErrorWithInner(ex);
                    deviceStatus.Status = deviceStatus.Status == string.Empty ? "Down" : deviceStatus.Status;
                }

                finally
                {
                    tasks.ForEach(delegate (Task t)
                    {
                        t.Dispose();
                    });
                    tasks = null;
                }

                return deviceStatus;
            }

            deviceStatus.Description = _kdsRepository.GetBuildScreenName(deviceName);
            deviceStatus.Status = "Down";
            deviceStatus.LastUpdated = DateTime.Now;

            return deviceStatus;
        }

        [MTAThread]
        public async Task<bool> StartService(string deviceName, string serviceDisplayName, CancellationToken token)
        {
            return await Task.Run(() => _wmi.StartService(deviceName, serviceDisplayName), token);
        }

        [MTAThread]
        public async Task<bool> StopService(string deviceName, string serviceDisplayName, CancellationToken token)
        {
            return await Task.Run(() => _wmi.StopService(deviceName, serviceDisplayName), token);
        }

        [MTAThread]
        public async Task<bool> SendKDSSettingsToDevice(string deviceName, CancellationToken token)
        {
            await Task.Run(() =>
            {
                var settings = _kdsRepository.GetKDSSettingsForDevice(deviceName);
                if (settings == null || settings.Count <= 0) throw new Exception($"Could not find any settings for device {deviceName}.");

                _common.SendSettingToKDS(deviceName, settings);
                _commandTransmitter.ExitApplication(deviceName);
            }, token);

            return true;
        }

        [MTAThread]
        public async Task<dynamic> GetCurrentMenuId(string deviceName, CancellationToken token)
        {
            var updateId = 0;
            var folderExists = false;
            var skusMissing = false;
            const bool status = false;

            await Task.Run(() =>
            {
                updateId = _common.GetCurrentUpdateIdFromFile(deviceName);
                folderExists = _common.DoMenuFilesExist(deviceName, updateId);
                skusMissing = _common.AreSkusMissing(deviceName, updateId);
            }, token);

            return new { updateId, folderExists, status, skusMissing };
        }

        [MTAThread]
        public async Task<bool> RunKitchenMenuSync(string device, CancellationToken token)
        {
            return await Task.Run(() => _common.RunKitchenMenuSync(device), token);
        }

        [MTAThread]
        public async Task<bool> CloseOrderPoint(string device, CancellationToken token)
        {
            return await _orderPointApiRepository.CloseOrderPoint(device, token);
        }

        [MTAThread]
        public async Task<bool> OpenOrderPoint(string device, CancellationToken token)
        {
            return await _orderPointApiRepository.OpenOrderPoint(device, token);
        }

        [MTAThread]
        public async Task<FileStream> GetLogFile(LogType type, string deviceName, CancellationToken token)
        {
            return await Task.Run(() => _common.GetLogFile(type, deviceName), token);
        }

        [MTAThread]
        public List<DeviceModel> GetKitchenDevices()
        {
            var devices = new List<DeviceModel>();
            var numKdsDevices = Convert.ToInt32(_kitchenSettingsManager.GetKitchenSetting(ConfigConstants.KDSMachineCount));
            var numOrderPointDevices = Convert.ToInt32(_kitchenSettingsManager.GetKitchenSetting(ConfigConstants.CSSMachineCount));

            for (var i = 1; i <= numOrderPointDevices; i++)
            {
                devices.Add(BuildDeviceModel(MachineType.Css, i));
            }

            for (var i = 1; i <= numKdsDevices; i++)
            {
                devices.Add(BuildDeviceModel(MachineType.Kds, i));
            }

            return devices;
        }

        #endregion

        #region Private Methods

        private async Task<bool> KioskAPIStatus(StatusModel deviceStatus, CancellationToken token)
        {
            try
            {
                if (deviceStatus.IsAppRunning)
                {
                    //check to see the status of Kiosk app
                    var kioskStatus = await _orderPointApiRepository.GetDeviceStatus(deviceStatus.DeviceName, token);

                    if (kioskStatus != null)
                    {
                        deviceStatus.Status = StatusText(kioskStatus.currentScreen);
                    }
                    else
                    {
                        //if we didn't get back a good response from the Kiosk API and the machine is locked (meaning
                        //Kiosk should be starting) then the device is down. If we are unlocked then the device is Up.
                        deviceStatus.Status = deviceStatus.IsLocked ? (deviceStatus.Status == string.Empty ? "Down" : deviceStatus.Status) : "Up";
                    }
                }
            }
            catch (Exception)
            {
                deviceStatus.Status = deviceStatus.IsLocked ? (deviceStatus.Status == string.Empty ? "Down" : deviceStatus.Status) : "Up";
            }
            return true;
        }

        private DeviceModel BuildDeviceModel(MachineType type, int iteration)
        {
            try
            {
                var getNamesFromDb = Convert.ToBoolean(_config.AppSettings(ConfigConstants.GetKDSNamesFromDatabase));
                var deviceName = _storeResolver.BuildMachineName(_divisionResolver.GetDivisionNumberFromComputerName(), _divisionResolver.GetDivisionCodeFromComputerName(), type, iteration, true);
                var ipAddress = Dns.GetHostAddresses(deviceName).FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();

                var description = (getNamesFromDb && type == MachineType.Kds) ? _kdsRepository.GetBuildScreenName(iteration) : $"{type.GetDescription()} {iteration:00}";

                return new DeviceModel() { Name = deviceName, IpAddress = ipAddress, Description = description, DeviceType = type, SyncStatus = "Loading" };
            }
            catch (Exception ex)
            {
                _logger.ErrorWithInner(new Exception("An error occurred trying to connect to the kiosk machines for this store. Check the error and try again.", ex));
                throw;
            }
        }

        private static string StatusText(string apiTest)
        {
            switch (apiTest)
            {
                case null:
                case "app":
                case "loading":
                    return "Loading";
                case "app.maintenance":
                case "app.closed":
                    return "Closed";
                case "app.error":
                    return "Error";
                case "app.home":
                    return "Up";
                default:
                    return "In Use";
            }
        }
        #region oldway

        private bool StartExplorer(string deviceName)
        {
            if (_wmi.ExecuteRemoteProcess(deviceName, @"C:\Windows\Explorer.exe") > 0)
            {
                //Windows 10 requires you to kill this process in order for the Desktop to show back up.
                return _wmi.KillExistingProcess(deviceName, "sihost.exe");
            }

            return false;
        }

        private bool KillExplorer(string deviceName)
        {
            return _wmi.KillExistingProcess(deviceName, "explorer.exe");
        }

        private bool RestartServices(string deviceName)
        {
            return _wmi.RestartService(deviceName, "QT Bootstrapper Service") && _wmi.RestartService(deviceName, "QT USB Lock Down Service");
        }

        #endregion

        //MA 
        #region NewWay
        public async Task<bool> IsMachineLocked(string deviceName, CancellationToken cancellationToken)
        {
            var response = await _storeMachineService.GetDeviceStatus(deviceName, cancellationToken)
                .ConfigureAwait(false);

            return response?.Device?.IsLocked ?? false;
        }

        public async Task<bool> IsApplicationRunning(string applicationName, string deviceName,
            CancellationToken cancellationToken)
        {
            var response =
                await _storeMachineService.GetApplicationStatus(applicationName, deviceName, cancellationToken)
                    .ConfigureAwait(false);

            return response?.Application?.IsRunning ?? false;
        }

        public async Task<bool> StopApplication(string applicationName, string deviceName, CancellationToken cancellationToken)
        {
            var response = await _storeMachineService.StopApplication(applicationName, deviceName, cancellationToken)
                .ConfigureAwait(false);

            return !(response?.Application?.IsRunning ?? false);
        }

        public async Task<List<string>> GetStoppedServices(List<string> servicesToCheck, string deviceName,
            CancellationToken cancellationToken)
        {
            var returnList = new List<string>();

            if (servicesToCheck == null)
                return returnList;

            foreach (var service in servicesToCheck)
            {
                var isStopped = await _common.IsServiceStopped(service, deviceName, cancellationToken).ConfigureAwait(false);

                if (isStopped)
                    returnList.Add(service);
            }

            return returnList;
        }
        #endregion

        #endregion
    }
}