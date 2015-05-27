﻿// FileInformation: nyanya/Infrastructure.Lib/NyanyaException.cs
// CreatedTime: 2014/07/06   5:45 PM
// LastUpdatedTime: 2014/07/06   5:45 PM

using System;

namespace Infrastructure.Lib.Exceptions
{
    public class NyanyaException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <c>NyanyaException</c> class.
        /// </summary>
        public NyanyaException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>NyanyaException</c> class with the specified
        ///     error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NyanyaException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>NyanyaException</c> class with the specified
        ///     error message and the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception.</param>
        public NyanyaException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>NyanyaException</c> class with the specified
        ///     string formatter and the arguments that are used for formatting the message which
        ///     describes the error.
        /// </summary>
        /// <param name="format">The string formatter which is used for formatting the error message.</param>
        /// <param name="args">The arguments that are used by the formatter to build the error message.</param>
        public NyanyaException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}