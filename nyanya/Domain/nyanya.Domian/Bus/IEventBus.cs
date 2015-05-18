// FileInformation: nyanya/Domian/IEventBus.cs
// CreatedTime: 2014/07/07   3:25 PM
// LastUpdatedTime: 2014/07/07   3:25 PM

using System.Collections.Generic;
using Domian.Events;

namespace Domian.Bus
{
    /// <summary>
    ///     Represents the event bus.
    /// </summary>
    public interface IEventBus : IBus
    {
        #region Public Methods

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

        #endregion Public Methods
    }
}