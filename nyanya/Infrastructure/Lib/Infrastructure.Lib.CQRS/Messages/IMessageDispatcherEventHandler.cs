// FileInformation: nyanya/Infrastructure.Lib.CQRS/IMessageDispatcherEventHandler.cs
// CreatedTime: 2014/06/05   10:49 AM
// LastUpdatedTime: 2014/06/05   11:52 AM

using System;

namespace Infrastructure.Lib.CQRS.Messages
{
    public interface IMessageDispatcherEventHandler
    {
        /// <summary>
        ///     Occurs when the message dispatcher has finished dispatching the message.
        /// </summary>
        event EventHandler<MessageDispatchEventArgs> Dispatched;

        /// <summary>
        ///     Occurs when the message dispatcher failed to dispatch a message.
        /// </summary>
        event EventHandler<MessageDispatchEventArgs> DispatchFailed;

        /// <summary>
        ///     Occurs when the message dispatcher is going to dispatch a message.
        /// </summary>
        event EventHandler<MessageDispatchEventArgs> Dispatching;
    }
}