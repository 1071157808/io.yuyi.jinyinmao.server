// FileInformation: nyanya/EventService/CQRSConfig.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/12   9:50 AM

using Cqrs.Domain.Config;
using Infrastructure.Lib.Dependencies;
using Infrastructure.Lib.Logs.Implementation;
using Infrastructure.Lib.Utility;
using Ninject;

namespace EventService.App_Start
{
    /// <summary>
    ///     CQRSConfig
    /// </summary>
    public static class CQRSConfig
    {
        /// <summary>
        ///     Configures the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Configure(CqrsConfiguration config)
        {
            Guard.ArgumentNotNull(config, "CqrsConfiguration");

            NinjectConfigure(config);
            LogsConfigure(config);
        }

        /// <summary>
        ///     Logses the configure.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void LogsConfigure(CqrsConfiguration config)
        {
            config.Logs.EventBusLogger = new NLogger("EventBusLogger");
            config.Logs.EventDispatcherLogger = new NLogger("EventDispatcherLogger");
            config.Logs.EventHandlerLogger = new NLogger("EventHandlerLogger");
        }

        /// <summary>
        ///     Ninjects the configure.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void NinjectConfigure(CqrsConfiguration config)
        {
            IKernel kernel = new StandardKernel();

            config.DependencyResolver = new DependencyResolver(kernel);
        }
    }
}