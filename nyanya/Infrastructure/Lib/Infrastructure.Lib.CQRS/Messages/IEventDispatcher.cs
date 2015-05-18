// FileInformation: nyanya/Infrastructure.Lib.CQRS/IEventDispatcher.cs
// CreatedTime: 2014/06/13   1:41 PM
// LastUpdatedTime: 2014/06/19   10:17 AM

using Infrastructure.Lib.CQRS.Bus;

namespace Infrastructure.Lib.CQRS.Messages
{
    public interface IEventDispatcher
    {
        /// <summary>
        ///     Dispatches the event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event">The event to be dispatched.</param>
        /// <returns>Dispatch Successed</returns>
        bool Dispatch<T>(T @event) where T : IEvent;

        /// <summary>
        ///     Registers a event handler into event dispatcher.
        /// </summary>
        /// <typeparam name="T">The type of the event.</typeparam>
        /// <param name="handler">The handler to be registered.</param>
        void Register<T>(IEventHandler<T> handler) where T : IEvent;

        /// <summary>
        ///     Unregisters a event handler from the event dispatcher.
        /// </summary>
        /// <typeparam name="T">The type of the event.</typeparam>
        void UnRegister<T>(IEventHandler<T> handler) where T : IEvent;
    }
}