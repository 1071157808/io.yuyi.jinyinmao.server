// FileInformation: nyanya/Infrastructure.Lib/CqrsConfigException.cs
// CreatedTime: 2014/07/08   10:23 PM
// LastUpdatedTime: 2014/07/08   10:31 PM

using System;

namespace Infrastructure.Lib.Exceptions
{
    public class CqrsConfigException : NyanyaException
    {
        /// <summary>
        ///     Initializes a new instance of the <c>CqrsConfigException</c> class.
        /// </summary>
        public CqrsConfigException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>CqrsConfigException</c> class with the specified
        ///     error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CqrsConfigException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>CqrsConfigException</c> class with the specified
        ///     error message and the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception.</param>
        public CqrsConfigException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>CqrsConfigException</c> class with the specified
        ///     string formatter and the arguments that are used for formatting the message which
        ///     describes the error.
        /// </summary>
        /// <param name="format">The string formatter which is used for formatting the error message.</param>
        /// <param name="args">The arguments that are used by the formatter to build the error message.</param>
        public CqrsConfigException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}