// FileInformation: nyanya/nyanya.Xingye/CQRSConfig.cs
// CreatedTime: 2014/09/01   10:08 AM
// LastUpdatedTime: 2014/09/02   2:07 PM

using Domian.Config;
using Infrastructure.Lib.Logs.Implementation;

namespace nyanya.Xingye
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