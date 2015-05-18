// FileInformation: nyanya/Infrastructure.Lib.CQRS/LogSection.cs
// CreatedTime: 2014/06/27   10:48 AM
// LastUpdatedTime: 2014/07/07   1:22 AM

using Infrastructure.Lib.CQRS.Log;

namespace Infrastructure.Lib.CQRS.Config
{
    public class LogSection
    {
        public ILogger CommandBusLogger { get; set; }

        public ILogger CommandDispatcherLogger { get; set; }

        public ILogger CommandHandlerLogger { get; set; }

        public ILogger EventBusLogger { get; set; }

        public ILogger EventDispatcherLogger { get; set; }

        public ILogger EventHandlerLogger { get; set; }
    }
}