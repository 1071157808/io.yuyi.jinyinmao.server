// FileInformation: nyanya/Infrastructure.Lib/LogSection.cs
// CreatedTime: 2014/07/06   10:47 PM
// LastUpdatedTime: 2014/07/06   10:48 PM

using Infrastructure.Lib.Logs;
using Infrastructure.Lib.Logs.Implementation;

namespace Infrastructure.Lib.Config
{
    public class LogSection
    {
        public LogSection()
        {
            this.CommandBusLogger = new DefaultLogger();
            this.CommandStoreLogger = new DefaultLogger();
            this.CommandHandlerLogger = new DefaultLogger();
            this.EventBusLogger = new DefaultLogger();
            this.EventDispatcherLogger = new DefaultLogger();
            this.EventHandlerLogger = new DefaultLogger();
        }

        public ILogger CommandBusLogger { get; set; }

        public ILogger CommandHandlerLogger { get; set; }

        public ILogger CommandStoreLogger { get; set; }

        public ILogger EventBusLogger { get; set; }

        public ILogger EventDispatcherLogger { get; set; }

        public ILogger EventHandlerLogger { get; set; }
    }
}