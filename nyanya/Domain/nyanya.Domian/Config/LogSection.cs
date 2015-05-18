// FileInformation: nyanya/Domian/LogSection.cs
// CreatedTime: 2014/07/14   4:42 PM
// LastUpdatedTime: 2014/07/20   4:41 PM

using Infrastructure.Lib.Logs;
using Infrastructure.Lib.Logs.Implementation;

namespace Domian.Config
{
    public class LogsConfig
    {
        public LogsConfig()
        {
            this.CommandBusLogger = new DefaultLogger();
            this.CommandHandlerLogger = new DefaultLogger();
            this.CommandStoreLogger = new DefaultLogger();
            this.EventBusLogger = new DefaultLogger();
            this.EventDispatcherLogger = new DefaultLogger();
            this.EventHandlerLogger = new DefaultLogger();
        }

        // Info Error
        public ILogger CommandBusLogger { get; set; }

        // Info Error Fatal
        public ILogger CommandHandlerLogger { get; set; }

        // Error
        public ILogger CommandStoreLogger { get; set; }

        // Info Error
        public ILogger EventBusLogger { get; set; }

        // Info Error
        public ILogger EventDispatcherLogger { get; set; }

        // Info Error
        public ILogger EventHandlerLogger { get; set; }
    }
}