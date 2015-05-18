// FileInformation: nyanya/EventService/EventHandlerService.cs
// CreatedTime: 2014/07/30   5:40 PM
// LastUpdatedTime: 2014/08/06   4:46 PM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cqrs.Domain.Events;
using Infrastructure.Lib.Logs;
using ServiceStack;

namespace EventService
{
    /// <summary>
    ///     EventHandlerService
    /// </summary>
    public partial class EventHandlerService : Service
    {
        #region Private Fields

        private readonly IEventHandlers eventHandlers;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EventHandlerService" /> class.
        /// </summary>
        /// <param name="eventHandlers">The event handlers.</param>
        /// <param name="logger">The logger.</param>
        public EventHandlerService(IEventHandlers eventHandlers, ILogger logger)
        {
            this.eventHandlers = eventHandlers;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Private Methods

        private async Task<object> Handler<T>(T @event) where T : IEvent
        {
            try
            {
                IEnumerable<IEventHandler<T>> handlers = this.eventHandlers.GetHandlers<T>();

                Task[] tasks = handlers.Select(eventHandler => eventHandler.Handler(@event)).ToArray();

                await Task.WhenAll(tasks);
            }
            catch (AggregateException e)
            {
                foreach (Exception innerException in e.InnerExceptions)
                {
                    this.logger.Fatal(innerException, innerException.Message);
                }
            }
            catch (Exception e)
            {
                this.logger.Fatal(e, e.Message);
            }
            return null;
        }

        #endregion Private Methods
    }
}