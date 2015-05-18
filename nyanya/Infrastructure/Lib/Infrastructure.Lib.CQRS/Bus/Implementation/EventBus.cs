// FileInformation: nyanya/Infrastructure.Lib.CQRS/EventBus.cs
// CreatedTime: 2014/06/24   3:02 PM
// LastUpdatedTime: 2014/06/25   10:05 AM

using System;
using System.Collections.Generic;
using Infrastructure.Lib.CQRS.Config;
using Infrastructure.Lib.CQRS.Log;
using Infrastructure.Lib.CQRS.MessageLogs;
using Infrastructure.Lib.CQRS.Messages;

namespace Infrastructure.Lib.CQRS.Bus.Implementation
{
    public class EventBus : DisposableObject, IEventBus
    {
        private readonly IEventDispatcher dispatcher;
        private readonly EventLogStore eventLogStore;
        private readonly ILogger logger;

        public EventBus(IEventDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.eventLogStore = new EventLogStore();
            //this.logger = CqrsConfigration.Loggers.EventBusLogger;
        }

        #region IEventBus Members

        public async void Publish<T>(T @event) where T : IEvent
        {
            try
            {
                await this.eventLogStore.Create(@event);
                this.DoPublish(@event);
            }
            catch (Exception e)
            {
                this.logger.Fatal(e, "Unknown Exception During Publishing Event({0}).\n{2}", @event.Guid, e.Message);
            }
        }

        public void Publish<T>(IEnumerable<T> events) where T : IEvent
        {
            throw new NotImplementedException();
        }

        #endregion IEventBus Members

        /// <summary>
        ///     Disposes the object.
        /// </summary>
        /// <param name="disposing">
        ///     A <see cref="System.Boolean" /> value which indicates whether
        ///     the object should be disposed explicitly.
        /// </param>
        protected override void Dispose(bool disposing)
        {
        }

        private void DoPublish<T>(T @event) where T : IEvent
        {
            try
            {
                this.dispatcher.Dispatch(@event);
                this.logger.Info("Published Event {0}.", @event.Guid);
            }
            catch (Exception e)
            {
                this.logger.Error(e, "Exception During Published Event({0}).\n{2}", @event.Guid, e.Message);
            }
        }
    }
}