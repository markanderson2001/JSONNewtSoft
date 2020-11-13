using Autofac;
using QT.KitchenManagementAdmin.Common.Config;
using QT.KitchenManagementAdmin.Common.Security;
using QT.KitchenManagementAdmin.Common.Security.Interfaces;
using QT.Posera;
using QT.Posera.Interfaces;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using QT.Common.ConfigurationService;
using QT.Common.ConfigurationService.Interfaces;
using QT.Common.Logging;
using QT.Common.StoreMachineResolution;
using QT.Common.StoreMachineResolution.Interfaces;
using QT.KitchenManagementAdmin.Business;
using QT.KitchenManagementAdmin.Business.Interfaces;
using QT.KitchenSettingsManager.Interfaces;
using QT.Common.Logging.Interfaces;
using QT.KitchenManagementAdmin.Repository;
using QT.KitchenManagementAdmin.Repository.Interfaces;
using QT.KitchenSettingsManager.Helpers;
using QT.KitchenSettingsManager.Helpers.Interfaces;
using QT.KitchenSettingsManager.Repository;
using QT.KitchenSettingsManager.Repository.Interfaces;
using Constants = QT.KitchenOrderTransaction.Common.Config.Constants;
using QT.KitchenManagement.WMI;
using QT.KitchenManagement.WMI.Interfaces;
using QT.Posera.Common;
using QT.Posera.Common.Interfaces;
using QT.Common.StoreMachine;
using QT.Common.StoreMachine.Interfaces;
using QT.Common.Cryptography.Interfaces;
using QT.Common.Cryptography;

namespace QT.KitchenManagementAdmin.IoC.Modules
{
    [ExcludeFromCodeCoverage]
    public class MvcModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var kitchenOrderingConnectionString = ConfigurationManager.ConnectionStrings[ConfigConstants.FSCDetailConnectionString].ConnectionString;
            var kdsConnectionString = ConfigurationManager.ConnectionStrings[ConfigConstants.KitchenDisplaySystemAdminContext].ConnectionString;
            var storeKitchenMenuConnectionString = ConfigurationManager.ConnectionStrings[ConfigConstants.StoreKitchenMenuContext].ConnectionString;
            var poseraTimeoutInMs = ConfigurationManager.AppSettings[Constants.TransmitToPoseraIpTimeOutInMillisecondsKey];
            var poseraPort = ConfigurationManager.AppSettings[Constants.PortToTransmitToPoseraKey];

            builder.RegisterType<JWTHelper>().As<IJWTHelpers>();
            builder.RegisterType<ConfigurationService>().As<IConfigurationService>();
            builder.RegisterType<Logger>().As<ILogger>();
            builder.RegisterType<StoreMachineNameResolver>().As<IStoreMachineNameResolver>();
            builder.RegisterType<KitchenOrderRepository>().As<IKitchenOrderRepository>()
                  .WithParameter("connectionString", kitchenOrderingConnectionString);
            builder.RegisterType<KitchenDisplaySystemAdminRepository>().As<IKitchenDisplaySystemAdminRepository>()
                .WithParameter("connectionString", kdsConnectionString)
                .WithParameter("storeKitchenMenuConnectionString", storeKitchenMenuConnectionString);
            builder.RegisterType<DivisionToAirportCodeResolver>().As<IDivisionToAirportCodeResolver>();
            builder.RegisterType<CommandTransmitter>().As<ICommandTransmitter>();
            builder.RegisterType<BusinessCommon>().As<IBusinessCommon>();
            builder.RegisterType<KitchenSettingsManager.KitchenSettingsManager>().As<IKitchenSettingsManager>()
                .WithParameter("kitchenName", "localhost")
                .WithParameter("useWebConfig", true);
            builder.RegisterType<KitchenDisplaySystemHelpers>().As<IKitchenDisplaySystemHelpers>();
            builder.RegisterType<ConnectionStateHelpers>().As<IConnectionStateHelpers>();
            builder.RegisterType<OrderPointAPIRepository>().As<IOrderPointAPIRepository>();
            builder.RegisterType<DevicesBusiness>().As<IDevicesBusiness>();
            builder.RegisterType<CommandTransmitter>().As<ICommandTransmitter>()
                   .WithParameter("poseraTimeOutInMS", poseraTimeoutInMs)
                   .WithParameter("port", poseraPort);
            builder.RegisterType<WMIHelper>().As<IWMIHelper>().WithParameter("searchOptionsTimeout", 10).SingleInstance();
            builder.RegisterType<MachineNameHelper>().As<IMachineNameHelper>();
            //MA Add
            builder.RegisterType<HttpClientAdaptor>().As<IHttpClientAdaptor>();
            builder.RegisterType<StoreMachineClient>().As<IStoreMachineClient>();
            builder.RegisterType<StoreMachineService>().As<IStoreMachineService>();
            builder.RegisterType<CryptographyService>().As<ICryptographyService>();           
        }
    }
}

