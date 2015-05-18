// FileInformation: nyanya/nyanya.Internal/CQRSConfig.cs
// CreatedTime: 2014/08/26   1:44 PM
// LastUpdatedTime: 2014/08/26   1:44 PM

using Domian.Config;
using Infrastructure.Lib.Logs.Implementation;

namespace nyanya.Internal.App_Start
{
    /// <summary>
    ///     CQRS 配置
    /// </summary>
    public static class CQRSConfig
    {
        /// <summary>
        ///     CQRS 配置.
        /// </summary>
        public static void Configurate(CqrsConfiguration config)
        {
            ConfigureLoggers(config);
        }

        private static void ConfigureLoggers(CqrsConfiguration config)
        {
            config.Logs.CommandBusLogger = new NLogger("CommandBusLogger");
            config.Logs.EventBusLogger = new NLogger("EventBusLogger");
            config.Logs.EventDispatcherLogger = new NLogger("EventDispatcherLogger");
        }
    }
}