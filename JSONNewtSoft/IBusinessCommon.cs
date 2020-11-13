using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using QT.Common.StoreMachineResolution.Enums;
using QT.KitchenManagementAdmin.Business.Models;
using QT.KitchenManagementAdmin.Common.Enums;
using QT.KitchenSettingsManager.DataAccess.Domain.QT_KitchenDisplaySystemAdmin;

namespace QT.KitchenManagementAdmin.Business.Interfaces {
    public interface IBusinessCommon {
        void SendSettingToKDS(string deviceName, List<AllSettingsDTO> settings);
        int GetCurrentUpdateIdFromFile(string deviceName, string status = "Success,Warning");
        int GetCurrentUpdateIdFromLocalApi();
        IEnumerable<int> GetMenuIdsForUpdateId(string deviceName, int updateId);
        bool DoMenuFilesExist(string deviceName, int menuUpdateId);
        bool RunKitchenMenuSync(string device = "localhost");
        FileStream GetLogFile(LogType type, string device = "localhost");
        List<string> GetKitchenMachineDeviceNameList(MachineType type);
        KitchenDeviceMenuModels GetCurrentMenuIdForAllKitchenMachines();
        bool AreSkusMissing(string deviceName, int updateId);

        Task<bool> IsServiceStopped(string serviceName, string deviceName,
          CancellationToken cancellationToken);

        Task<bool> StopService(string serviceName, string deviceName, CancellationToken cancellationToken);

        Task<bool> StartService(string serviceName, string deviceName, CancellationToken cancellationToken);
    }
}