// FileInformation: nyanya/Infrastructure.Lib.CQRS/IMessageDispatcher.cs
// CreatedTime: 2014/06/05   1:08 AM
// LastUpdatedTime: 2014/06/05   1:21 PM

namespace Infrastructure.Lib.CQRS.Messages
{
    /// <summary>
    ///     Represents the message dispatcher.
    /// </summary>
    public interface IMessageDispatcher : IMessageDispatcherEventHandler
    {
        /// <summary>
        ///     Clears the registration of the message handlers.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Dispatches the message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message to be dispatched.</param>
        /// <returns>Dispatch Successed</returns>
        bool Dispatch<T>(T message);

        /// <summary>
        ///     Registers a message handler into message dispatcher.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="handler">The handler to be registered.</param>
        void Register<T>(IHandler<T> handler);

        /// <summary>
        ///     Unregisters a message handler from the message dispatcher.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="handler">The handler to be registered.</param>
        void UnRegister<T>(IHandler<T> handler);
    }
}