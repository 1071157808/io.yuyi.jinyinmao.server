// FileInformation: nyanya/Infrastructure.Lib.CQRS/CommandExcuteException.cs
// CreatedTime: 2014/07/01   1:28 PM
// LastUpdatedTime: 2014/07/07   1:16 AM

using System;
using Infrastructure.Lib.Exceptions;

namespace Infrastructure.Lib.CQRS.Exceptions
{
    public class CommandExcuteException : NyanyaException
    {
        /// <summary>
        ///     Initializes a new instance of the <c>CommandExcuteException</c> class.
        /// </summary>
        public CommandExcuteException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>CommandExcuteException</c> class with the specified
        ///     error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CommandExcuteException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>CommandExcuteException</c> class with the specified
        ///     error message and the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception.</param>
        public CommandExcuteException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>CommandExcuteException</c> class with the specified
        ///     string formatter and the arguments that are used for formatting the message which
        ///     describes the error.
        /// </summary>
        /// <param name="format">The string formatter which is used for formatting the error message.</param>
        /// <param name="args">The arguments that are used by the formatter to build the error message.</param>
        public CommandExcuteException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}