// FileInformation: nyanya/nyanya.Internal/CQRSConfig.cs
// CreatedTime: 2014/08/28   11:02 AM
// LastUpdatedTime: 2014/09/01   4:32 PM

using Domian.Config;
using Infrastructure.Lib.Dependencies;
using Infrastructure.Lib.Logs.Implementation;
using Infrastructure.Lib.Utility;
using Ninject;

namespace nyanya.Internal.Service.Config
{
    /// <summary>
    ///     CQRSConfig
    /// </summary>
    public static class CQRSConfig
    {
        /// <summary>
        ///     Configurates the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Configurate(CqrsConfiguration config)
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
            config.Logs.CommandHandlerLogger = new NLogger("CommandHandlerLogger");
            config.Logs.CommandStoreLogger = new NLogger("CommandStoreLogger");
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