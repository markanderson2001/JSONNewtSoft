using QT.KitchenManagementAdmin.Business.Interfaces;
using QT.Posera.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QT.KitchenManagementAdmin.Common.Config;
using QT.KitchenManagementAdmin.Common.Enums;
using QT.Common.StoreMachineResolution.Enums;
using QT.Common.StoreMachineResolution.Interfaces;
using QT.KitchenManagement.WMI.Interfaces;
using QT.KitchenSettingsManager.Interfaces;
using QT.KitchenManagementAdmin.Business.Models;
using QT.KitchenManagementAdmin.Repository.Interfaces;
using QT.KitchenSettingsManager.DataAccess.Domain.QT_KitchenDisplaySystemAdmin;
using System.Configuration;
using QT.Common.ConfigurationService.Interfaces;
using QT.Common.Logging.Interfaces;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using System.ServiceProcess;
using System.Threading;
using QT.Common.StoreMachine.Interfaces;

namespace QT.KitchenManagementAdmin.Business
{
    public class BusinessCommon : IBusinessCommon
    {
        private readonly ILogger _logger;
        private readonly IConfigurationService _config;
        private readonly IWMIHelper _wmi;
        private readonly ICommandTransmitter _commandTransmitter;
        private readonly IStoreMachineNameResolver _storeMachineResolver;
        private readonly IDivisionToAirportCodeResolver _divisionResolver;
        private readonly IKitchenSettingsManager _kitchenSettingsManager;
        private readonly IKitchenOrderRepository _kitchenOrderRepository;
        private readonly IStoreMachineService _storeMachineService;

        public BusinessCommon(ILogger logger, IConfigurationService config, ICommandTransmitter commandTransmitter, IStoreMachineNameResolver storeResolver, IDivisionToAirportCodeResolver divisionResolver, IKitchenSettingsManager kitchenSettingsManager, IKitchenOrderRepository kitchenOrderRepository, IWMIHelper wmi, IStoreMachineService storeMachineService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _commandTransmitter = commandTransmitter ?? throw new ArgumentNullException(nameof(commandTransmitter));
            _storeMachineResolver = storeResolver ?? throw new ArgumentNullException(nameof(storeResolver));
            _divisionResolver = divisionResolver ?? throw new ArgumentNullException(nameof(divisionResolver));
            _kitchenSettingsManager = kitchenSettingsManager ?? throw new ArgumentNullException(nameof(kitchenSettingsManager));
            _kitchenOrderRepository = kitchenOrderRepository ?? throw new ArgumentNullException(nameof(kitchenOrderRepository));
            _wmi = wmi ?? throw new ArgumentNullException(nameof(wmi));
            _storeMachineService = storeMachineService ?? throw new ArgumentNullException(nameof(storeMachineService));
        }

        #region Public Methods

        public void SendSettingToKDS(string deviceName, List<AllSettingsDTO> settings)
        {
            if (settings == null) return;

            var options = (from setting in settings where !string.IsNullOrEmpty(setting.ItemValue) select $"{setting.SettingName}={setting.ItemValue}").ToList();
            _commandTransmitter.SendOptions(options, deviceName);
        }

        public int GetCurrentUpdateIdFromFile(string deviceName, string status = "Success,Warning")
        {
            var filePath = $@"{_config.AppSettings("KitchenPCMenuJsonPath")}\updates.json";
            return !_kitchenOrderRepository.FileExists($@"{filePath}") ? 0 : ExtractCurrentUpdateIdFromJson(_kitchenOrderRepository.GetFileContents(filePath), deviceName, status);
        }

        public int GetCurrentUpdateIdFromLocalApi()
        {
            var url = $@"http://localhost{_kitchenOrderRepository.GetConfigurationSetting(ConfigConstants.StoreSegmentDeployAPIURL)}";
            var apiResponse = _kitchenOrderRepository.GetStoreSegmentDeployApiResponse(url);

            if (string.IsNullOrEmpty(apiResponse)) return 0;

            var data = JsonConvert.DeserializeObject<dynamic>(apiResponse);
            return ExtractCurrentUpdateIdFromAPIJson(JsonConvert.SerializeObject(data.PaginatedData));
        }

        public IEnumerable<int> GetMenuIdsForUpdateId(string deviceName, int updateId)
        {
            var filePath = $@"{_config.AppSettings("KitchenPCMenuJsonPath")}\updates.json";
            return !_kitchenOrderRepository.FileExists(filePath) ? null : ExtractMenuIdsFromJsonForUpdateId(_kitchenOrderRepository.GetFileContents(filePath), updateId);
        }

        public bool DoMenuFilesExist(string deviceName, int menuUpdateId)
        {
            var path = "";
            var machineType = _storeMachineResolver.ResolveMachineTypeFromMachineName(deviceName);

            if (deviceName == "localhost" || machineType == MachineType.Kpc)
            {
                path = _kitchenOrderRepository.GetConfigurationSetting(ConfigConstants.KitchenPCMenuDataPath);
                path = $@"{path}\{menuUpdateId}";
            }
            else
            {
                path = _kitchenOrderRepository.GetConfigurationSetting(ConfigConstants.CSSUpdateJsonPath);
                path = $@"\\{deviceName}\{path}\{menuUpdateId}";
            }

            return _kitchenOrderRepository.DirectoryExists(path) && _kitchenOrderRepository.SearchForFiles(path, "store*.json").Any();
        }

        public bool RunKitchenMenuSync(string device = "localhost")
        {
            var retryCnt = 0;
            var serviceName = ConfigurationManager.AppSettings[ConfigConstants.SyncServiceName];
            while (!_wmi.IsServiceStarted(device, serviceName))
            {
                if (retryCnt > 2) return false;
                try
                {
                    _wmi.StartService(device, serviceName);
                }
                catch (Exception ex)
                {
                    _logger.ErrorWithInner(new Exception($"Error trying to start {serviceName} on device {device}. Will retry.", ex));
                }
                retryCnt++;
                Task.Delay(3000).Wait();
            }

            var url = string.Format(_kitchenOrderRepository.GetConfigurationSetting(ConfigConstants.KitchenSyncServiceApiUrl), device);
            return _kitchenOrderRepository.ForceMenuSyncApiCall(url);
        }

        public FileStream GetLogFile(LogType type, string device = "localhost")
        {
            var file = GetLogFilePathName(type, device);
            var fileName = file.Substring(file.LastIndexOf('\\') + 1);

            if (!_kitchenOrderRepository.FileExists(file)) throw new Exception($"File {fileName} not found on device {device}");

            return _kitchenOrderRepository.GetFileLogStreamContent(file, $"{device}-{fileName}");
        }

        public List<string> GetKitchenMachineDeviceNameList(MachineType type)
        {
            var devicesList = new List<string>();

            if (type != MachineType.Css && type != MachineType.Kds)
                return devicesList;

            var configSetting = (type == MachineType.Css) ? ConfigConstants.CSSMachineCount : ConfigConstants.KDSMachineCount;
            var numberOfDevices = int.TryParse(_kitchenSettingsManager.GetKitchenSetting(configSetting), out int devices) ? devices : 0;

            for (var i = 1; i <= numberOfDevices; i++)
            {
                var deviceName = _storeMachineResolver.BuildMachineName(_divisionResolver.GetDivisionNumberFromComputerName(), _divisionResolver.GetDivisionCodeFromComputerName(), type, i, true);
                devicesList.Add(deviceName);
            }
            return devicesList;
        }

        public KitchenDeviceMenuModels GetCurrentMenuIdForAllKitchenMachines()
        {
            KitchenDeviceMenuModels deviceModels = new KitchenDeviceMenuModels();

            var kpcName = _storeMachineResolver.BuildMachineName(_divisionResolver.GetDivisionNumberFromComputerName(), _divisionResolver.GetDivisionCodeFromComputerName(), MachineType.Kpc, false);

            deviceModels.Devices.Add(GetKitchenDeviceMenuModelForKitchenPc(kpcName));

            foreach (var device in GetKitchenMachineDeviceNameList(MachineType.Css))
            {
                deviceModels.Devices.Add(GetKitchenDeviceMenuModelForCssDevice(device));
            }

            foreach (var device in GetKitchenMachineDeviceNameList(MachineType.Kds))
            {
                deviceModels.Devices.Add(GetKitchenDeviceMenuModelForKdsDevice(device));
            }

            return deviceModels;
        }

        public bool AreSkusMissing(string deviceName, int updateId)
        {
            try
            {
                var currentUpdateId = GetCurrentUpdateIdFromFile("localhost");

                //If the CSS has the correct updateId then we know we aren't missing skus.
                if (updateId == currentUpdateId) return false;

                if (currentUpdateId == 0) return false;

                updateId = GetCurrentUpdateIdFromFile(deviceName, "Failed");

                var menus = GetMenuIdsForUpdateId("localhost", updateId);
                var entries = _wmi.GetEventLogs(deviceName, "Application", "QT.API.MenuData").OrderByDescending(x => x.TimeWritten);

                foreach (var menuId in menus)
                {
                    var menuStatus = entries.Where(x => x.Message.StartsWith("SUCCESS") || x.Message.StartsWith("FAIL"))
                        .FirstOrDefault(x => (int.TryParse(x.Message.Split('-')[0].Split(':')[1], out var id) ? id : 0) == menuId)?.Message;
                    if (menuStatus?.Contains("FAIL") ?? false) return true;
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorWithInner(ex);
            }

            return false;
        }

        #endregion

        #region Private Methods

        private KitchenDeviceMenuModel GetKitchenDeviceMenuModelForKitchenPc(string deviceName)
        {
            var model = new KitchenDeviceMenuModel()
            {
                Name = deviceName,
                CurrentUpdateID = "0",
                MenuIds = new List<int>(),
                Status = "Down",
                Downloaded = false,
                DeviceType = nameof(MachineType.Kpc)
            };

            if (!KitchenManagement.WMI.Helpers.Pinger.IsPingable(deviceName)) return model;

            var menuId = GetCurrentUpdateIdFromLocalApi();
            model.CurrentUpdateID = menuId.ToString();
            model.MenuIds = new List<int>();
            model.Status = "Up";
            model.Downloaded = DoMenuFilesExist(deviceName, menuId);
            return model;
        }

        private KitchenDeviceMenuModel GetKitchenDeviceMenuModelForCssDevice(string deviceName)
        {
            var model = new KitchenDeviceMenuModel()
            {
                Name = deviceName,
                CurrentUpdateID = "0",
                MenuIds = new List<int>(),
                Status = "Down",
                Downloaded = false,
                DeviceType = nameof(MachineType.Css),
                SkusMissing = false
            };

            if (!KitchenManagement.WMI.Helpers.Pinger.IsPingable(deviceName))
                return model;
            
            var menu = GetCurrentUpdateIdFromFile(deviceName);
            model.CurrentUpdateID = menu.ToString();
            model.MenuIds = new List<int>();
            model.Status = "Up";
            model.Downloaded = DoMenuFilesExist(deviceName, menu);
            model.SkusMissing = AreSkusMissing(deviceName, menu);

            return model;
        }

        private KitchenDeviceMenuModel GetKitchenDeviceMenuModelForKdsDevice(string deviceName)
        {
            return new KitchenDeviceMenuModel()
            {
                Name = deviceName,
                CurrentUpdateID = "0",
                MenuIds = new List<int>(),
                Status = (KitchenManagement.WMI.Helpers.Pinger.IsPingable(deviceName)) ? "Up" : "Down",
                Downloaded = false,
                DeviceType = nameof(MachineType.Kds)
            };
        }

        private string GetLogFilePathName(LogType type, string deviceName)
        {
            string path;
            var filePathName = "";
            switch (type)
            {
                case LogType.SyncLog:
                    path = _kitchenOrderRepository.GetConfigurationSetting(ConfigConstants.SyncServiceLogFilePath);
                    filePathName = $@"C:\{path}";

                    if (deviceName != "localhost")
                    {
                        filePathName = $@"\\{deviceName}\C-Drive\{path}";
                    }
                    break;

                case LogType.KioskLog:
                    path = _kitchenOrderRepository.GetConfigurationSetting(ConfigConstants.KioskLogFilePath);
                    filePathName = $@"\\{deviceName}\{path}";
                    break;
                case LogType.KDSError:
                    path = _kitchenOrderRepository.GetConfigurationSetting(ConfigConstants.KDSErrorLog);
                    filePathName = $@"\\{deviceName}\{path}";
                    break;
                case LogType.KDSData:
                    path = _kitchenOrderRepository.GetConfigurationSetting(ConfigConstants.KDSRawDataLog);
                    filePathName = $@"\\{deviceName}\{path}";
                    break;
            }

            return filePathName;
        }

        private static int ExtractCurrentUpdateIdFromJson(string json, string deviceName, string status = "Success,Warning")
        {
            if (string.IsNullOrEmpty(json)) return 0;

            var menus = JsonConvert.DeserializeObject<dynamic>(json);

            if (menus == null) return 0;

            foreach (var update in ((IEnumerable<dynamic>)menus.Updates).
                                        Where(x => x.UpdateStart <= DateTime.Now.ToUniversalTime() &&
                                                   (x.UpdateEnd >= DateTime.Now.ToUniversalTime() || x.UpdateEnd == null))
                                        .OrderByDescending(x => x.UpdateId))
            {
                if (deviceName == "localhost")
                {
                    return update.UpdateId;
                }

                if (update.Kiosks.ContainsKey(deviceName))
                {
                    if (status.Split(',').ToList().Contains(update.Kiosks[deviceName].ToString()))
                    {
                        return update.UpdateId;
                    }
                }
            }

            return 0;
        }

        private static int ExtractCurrentUpdateIdFromAPIJson(string json)
        {
            if (string.IsNullOrEmpty(json)) return 0;

            var menus = JsonConvert.DeserializeObject<List<dynamic>>(json);

            if (menus == null) return 0;

            return menus.Count > 0 ? menus.OrderByDescending(x => x.UpdateId).FirstOrDefault(x => x.UpdateStart <= DateTime.Now.ToUniversalTime() && (x.UpdateEnd >= DateTime.Now.ToUniversalTime() || x.UpdateEnd == null))?.UpdateId : 0;
        }

        private static IEnumerable<int> ExtractMenuIdsFromJsonForUpdateId(string json, int updateId)
        {
            var menusIds = new List<int>();
            var deserializer = new DeserializerBuilder().Build();

            var updates = deserializer.Deserialize<dynamic>(json);

            foreach (var update in updates["Updates"])
            {
                if (update["UpdateId"] == updateId.ToString())
                {
                    for (int i = 0; i < update["MenuIds"].Count; i++)
                    {
                        menusIds.Add(int.TryParse(update["MenuIds"][i], out int output) ? output : 0);
                    }
                    return menusIds;
                }
            }

            return null;
        }

        public async Task<bool> StopService(string serviceName, string deviceName, CancellationToken cancellationToken)
        {
            var response = await _storeMachineService.StopService(serviceName, deviceName, cancellationToken)
                .ConfigureAwait(false);
            var status = response?.Service?.Status ?? ServiceControllerStatus.ContinuePending;

            return (status == ServiceControllerStatus.Stopped);
        }

        public async Task<bool> StartService(string serviceName, string deviceName, CancellationToken cancellationToken)
        {
            var response = await _storeMachineService.StartService(serviceName, deviceName, cancellationToken)
                .ConfigureAwait(false);
            var status = response?.Service?.Status ?? ServiceControllerStatus.Stopped;

            return (status == ServiceControllerStatus.Running);
        }

        public async Task<bool> IsServiceStopped(string serviceName, string deviceName,
            CancellationToken cancellationToken)
        {
            var response = await _storeMachineService.GetServiceStatus(serviceName, deviceName, cancellationToken)
                .ConfigureAwait(false);
            var status = response?.Service.Status ?? ServiceControllerStatus.ContinuePending;

            return (status == ServiceControllerStatus.Stopped);
        }

        #endregion
    }
}