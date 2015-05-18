// FileInformation: nyanya/Infrastructure.Lib.CQRS/IEventBus.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/06/13   10:30 AM

using System.Collections.Generic;

namespace Infrastructure.Lib.CQRS.Bus
{
    /// <summary>
    ///     Represents the event bus.
    /// </summary>
    public interface IEventBus : IBus
    {
        /// <summary>
        ///     Publishes the specified event to the bus.
        /// </summary>
        /// <typeparam name="T">The type of the event to be published.</typeparam>
        /// <param name="event">The event to be published.</param>
        void Publish<T>(T @event) where T : IEvent;

        /// <summary>
        ///     Publishes a collection of events to the bus.
        /// </summary>
        /// <typeparam name="T">The type of the event to be published.</typeparam>
        /// <param name="events">The events to be published.</param>
        void Publish<T>(IEnumerable<T> events) where T : IEvent;
    }
}