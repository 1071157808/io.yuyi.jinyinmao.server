// FileInformation: nyanya/Infrastructure.Lib.CQRS/MessageDispatcherEventHandler.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/06/15   12:01 PM

namespace Infrastructure.Lib.CQRS.Messages.Implementation
{
    public class MessageDispatcherEventHandler : IMessageDispatcherEventHandler
    {
        #region IMessageDispatcherEventHandler Members

        /// <summary>
        ///     Occurs when the message dispatcher has finished dispatching the message.
        /// </summary>
        public event System.EventHandler<MessageDispatchEventArgs> Dispatched;

        /// <summary>
        ///     Occurs when the message dispatcher failed to dispatch a message.
        /// </summary>
        public event System.EventHandler<MessageDispatchEventArgs> DispatchFailed;

        /// <summary>
        ///     Occurs when the message dispatcher is going to dispatch a message.
        /// </summary>
        public event System.EventHandler<MessageDispatchEventArgs> Dispatching;

        #endregion IMessageDispatcherEventHandler Members

        /// <summary>
        ///     Occurs when the message dispatcher has finished dispatching the message.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnDispatched(MessageDispatchEventArgs e)
        {
            System.EventHandler<MessageDispatchEventArgs> temp = this.Dispatched;
            if (temp != null)
                temp(this, e);
        }

        /// <summary>
        ///     Occurs when the message dispatcher failed to dispatch a message.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnDispatchFailed(MessageDispatchEventArgs e)
        {
            System.EventHandler<MessageDispatchEventArgs> temp = this.DispatchFailed;
            if (temp != null)
                temp(this, e);
        }

        /// <summary>
        ///     Occurs when the message dispatcher is going to dispatch a message.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnDispatching(MessageDispatchEventArgs e)
        {
            System.EventHandler<MessageDispatchEventArgs> temp = this.Dispatching;
            if (temp != null)
                temp(this, e);
        }
    }
}