using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac;
using Autofac.Core;
using QT.Common.Logging;
using QT.Common.Logging.Interfaces;

namespace QT.KitchenManagementAdmin.IoC.Modules
{
    [ExcludeFromCodeCoverage]
    public class LoggingModule : Module
    {
        /// <summary>
        /// Method for loading this module
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register((c, p) => new Logger(p.TypedAs<Type>()))
                .AsImplementedInterfaces();
        }

        /// <summary>
        /// Method for AttachToComponentRegistration
        /// </summary>
        /// <param name="componentRegistry"></param>
        /// <param name="registration"></param>
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing +=
                (sender, args) =>
                {
                    var forType = args.Component.Activator.LimitType;

                    var logParameter = new ResolvedParameter(
                        (p, c) => p.ParameterType == typeof(ILogger),
                        (p, c) => c.Resolve<ILogger>(TypedParameter.From(forType)));

                    args.Parameters = args.Parameters.Union(new[] { logParameter });
                };
        }
    }
}