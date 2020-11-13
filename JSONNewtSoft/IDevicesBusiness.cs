using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using QT.KitchenManagementAdmin.Business.Models;
using QT.KitchenManagementAdmin.Common.Enums;

namespace QT.KitchenManagementAdmin.Business.Interfaces
{
    public interface IDevicesBusiness
    {
        Task<bool> RebootDevice(string deviceName, CancellationToken token);
        Task<bool> InstallMSIAndRebootDevice(string deviceName, CancellationToken token);
        Task<bool> RestartApp(string deviceName, CancellationToken token);
        Task<bool> Lock(string deviceName, CancellationToken token);
        Task<bool> Unlock(string deviceName, CancellationToken token);
        Task<bool> CacheStatus(string deviceName, CancellationToken token);
        Task<StatusModel> Status(string deviceName, CancellationToken token, bool refresh = false);
        Task<bool> StartService(string deviceName, string serviceDisplayName, CancellationToken token);
        Task<bool> StopService(string deviceName, string serviceDisplayName, CancellationToken token);
        Task<bool> SendKDSSettingsToDevice(string deviceName, CancellationToken token);
        Task<dynamic> GetCurrentMenuId(string deviceName, CancellationToken token);
        Task<bool> RunKitchenMenuSync(string device, CancellationToken token);
        Task<bool> CloseOrderPoint(string device, CancellationToken token);
        Task<bool> OpenOrderPoint(string device, CancellationToken token);
        Task<FileStream> GetLogFile(LogType type, string deviceName, CancellationToken token);
        List<DeviceModel> GetKitchenDevices();
    }
}