// FileInformation: nyanya/Infrastructure.Lib.CQRS/EventHandlerBase.cs
// CreatedTime: 2014/06/15   11:51 AM
// LastUpdatedTime: 2014/06/24   4:36 PM

using System;
using Infrastructure.Lib.CQRS.Bus;
using Infrastructure.Lib.CQRS.Config;
using Infrastructure.Lib.CQRS.Log;
using Infrastructure.Lib.CQRS.MessageLogs;

namespace Infrastructure.Lib.CQRS.Messages.Implementation
{
    public abstract class EventHandlerBase
    {
        private readonly EventLogStore eventLogStore;
        private readonly ILogger logger;

        protected EventHandlerBase()
        {
            this.eventLogStore = new EventLogStore();
            //this.logger = CqrsConfigration.Loggers.EventHandlerLogger;
        }

        public string Name
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }

        protected void OnHandled(IEvent @event)
        {
            // ReSharper disable once CSharpWarnings::CS4014
            this.eventLogStore.Delivered(@event);
            this.logger.Info("Handled Event {0} With Handler {1}.", @event.Guid, this.Name);
        }

        protected void OnHandledFailed(IEvent @event, Exception e)
        {
            // ReSharper disable once CSharpWarnings::CS4014
            this.eventLogStore.Handled(@event, false);
            this.logger.Error(e, "Exception Duraing Handling Event {0} With Handler {1}.\n{2}", @event.Guid, this.Name, e.Message);
        }
    }
}