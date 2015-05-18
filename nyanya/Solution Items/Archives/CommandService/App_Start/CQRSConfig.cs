// FileInformation: nyanya/CommandService/CQRSConfig.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/11   2:51 PM

using Cqrs.Domain.Config;
using Infrastructure.Lib.Dependencies;
using Infrastructure.Lib.Logs.Implementation;
using Infrastructure.Lib.Utility;
using Ninject;

namespace CommandService.App_Start
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