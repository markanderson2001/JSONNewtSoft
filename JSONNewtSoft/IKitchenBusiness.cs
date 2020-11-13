using System.Collections.Generic;
using System.IO;
using System.Threading;
using QT.KitchenManagementAdmin.Business.Models;
using QT.KitchenManagementAdmin.Repository.Models;
using System.Threading.Tasks;
using QT.KitchenSettingsManager.Common.Enums;

namespace QT.KitchenManagementAdmin.Business.Interfaces
{
    public interface IKitchenBusiness
    {
        Task<KitchenModel> GetOrderCounts(CancellationToken token, int timeSpan = 30);
        Task<StatusModel> GetKitchenStatus(CancellationToken token);
        Task<Templates> GetActiveBuildScreenMode(CancellationToken token);
        Task<Templates> UpdateBuildScreenMode(Templates template, CancellationToken token);
        Task<bool> ResetBuildScreens(CancellationToken token);
        Task<dynamic> GetCurrentMenuId(CancellationToken token);
        Task<bool> RunKitchenMenuSync(CancellationToken token);
        Task<FileStream> GetLogFile(CancellationToken token);
        Task<Dictionary<string, string>> GetKitchenSettings(CancellationToken token);
        Task<KitchenDeviceMenuModels> GetCurrentMenuIdForKpcAndAllCss(CancellationToken token);
        Task<int> GetKitchenStaffingCount(CancellationToken token);
        Task<string> GetDeliveryNotificationText(CancellationToken token);
    }
}