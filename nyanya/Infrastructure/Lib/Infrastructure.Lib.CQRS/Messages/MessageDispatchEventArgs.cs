using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Lib.CQRS.Messages
{
    /// <summary>
    /// Represents the event data that is generated when dispatching messages.
    /// </summary>
    public class MessageDispatchEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of <c>MessageDispatchEventArgs</c> class.
        /// </summary>
        public MessageDispatchEventArgs()
        { }

        /// <summary>
        /// Initializes a new instance of <c>MessageDispatchEventArgs</c> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="handlerType">The type of the message handler.</param>
        /// <param name="handler">The handler.</param>
        public MessageDispatchEventArgs(dynamic message, Type handlerType, object handler)
        {
            this.Message = message;
            this.HandlerType = handlerType;
            this.Handler = handler;
        }

        /// <summary>
        /// Gets or sets the handler.
        /// </summary>
        public object Handler { get; set; }

        /// <summary>
        /// Gets or sets the type of the message handler.
        /// </summary>
        public Type HandlerType { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public dynamic Message { get; set; }
    }
}