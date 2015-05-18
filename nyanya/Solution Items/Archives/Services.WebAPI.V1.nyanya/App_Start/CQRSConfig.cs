// FileInformation: nyanya/Services.WebAPI.V1.nyanya/CQRSConfig.cs
// CreatedTime: 2014/07/14   4:42 PM
// LastUpdatedTime: 2014/08/12   8:38 PM

using Cqrs.Domain.Config;
using Infrastructure.Lib.Logs.Implementation;

namespace Services.WebAPI.V1.nyanya.App_Start
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