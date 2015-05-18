// FileInformation: nyanya/nyanya.Xingye.Internal/EventHandlerService.cs
// CreatedTime: 2014/09/03   10:05 AM
// LastUpdatedTime: 2014/09/03   7:11 PM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domian.Config;
using Domian.Events;
using Infrastructure.Lib.Logs;

namespace nyanya.Xingye.Internal.Service
{
    /// <summary>
    ///     EventHandlerService
    /// </summary>
    public partial class EventHandlerService : ServiceStack.Service
    {
        private readonly CqrsConfiguration config;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EventHandlerService" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public EventHandlerService(CqrsConfiguration config)
        {
            this.config = config;
        }

        /// <summary>
        ///     Gets the event handlers.
        /// </summary>
        /// <value>
        ///     The event handlers.
        /// </value>
        private IEventHandlers EventHandlers
        {
            get { return this.config.EventHandlers; }
        }

        /// <summary>
        ///     Gets the logger.
        /// </summary>
        /// <value>
        ///     The logger.
        /// </value>
        private ILogger Logger
        {
            get { return this.config.EventHandlerLogger; }
        }

        private async Task<object> Handler<T>(T @event) where T : IEvent
        {
            try
            {
                IEnumerable<IEventHandler<T>> handlers = this.EventHandlers.GetHandlers<T>();

                Task[] tasks = handlers.Select(eventHandler => eventHandler.Handler(@event)).ToArray();

                await Task.WhenAll(tasks);
            }
            catch (AggregateException e)
            {
                foreach (Exception innerException in e.InnerExceptions)
                {
                    this.Logger.Fatal(innerException, innerException.Message);
                }
            }
            catch (Exception e)
            {
                this.Logger.Fatal(e, e.Message);
            }
            return null;
        }
    }
}