// FileInformation: nyanya/Infrastructure.Lib/IdentifierMissingException.cs
// CreatedTime: 2014/07/06   5:46 PM
// LastUpdatedTime: 2014/07/06   5:50 PM

using System;

namespace Infrastructure.Lib.Exceptions
{
    public class IdentifierMissingException : NyanyaException
    {
        /// <summary>
        ///     Initializes a new instance of the <c>IdentifierMissingException</c> class.
        /// </summary>
        public IdentifierMissingException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>IdentifierMissingException</c> class with the specified
        ///     error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public IdentifierMissingException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>IdentifierMissingException</c> class with the specified
        ///     error message and the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception.</param>
        public IdentifierMissingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>IdentifierMissingException</c> class with the specified
        ///     string formatter and the arguments that are used for formatting the message which
        ///     describes the error.
        /// </summary>
        /// <param name="format">The string formatter which is used for formatting the error message.</param>
        /// <param name="args">The arguments that are used by the formatter to build the error message.</param>
        public IdentifierMissingException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}