// FileInformation: nyanya/nyanya.Cat/CQRSConfig.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   4:31 PM

using Domian.Config;
using Infrastructure.Lib.Logs.Implementation;

namespace nyanya.Cat
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