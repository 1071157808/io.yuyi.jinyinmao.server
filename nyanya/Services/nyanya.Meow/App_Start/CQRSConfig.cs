// FileInformation: nyanya/nyanya.Meow/CQRSConfig.cs
// CreatedTime: 2014/08/29   2:24 PM
// LastUpdatedTime: 2014/09/01   5:29 PM

using Domian.Config;
using Infrastructure.Lib.Logs.Implementation;

namespace nyanya.Meow
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