// FileInformation: nyanya/Infrastructure.Lib/CouchbaseOperationFailedException.cs
// CreatedTime: 2014/09/12   12:45 PM
// LastUpdatedTime: 2014/09/12   12:45 PM

using System;

namespace Infrastructure.Lib.Exceptions
{
    public class CouchbaseOperationFailedException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <c>NyanyaException</c> class with the specified
        ///     error message and the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception.</param>
        public CouchbaseOperationFailedException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = errorCode;
        }

        public int ErrorCode { get; set; }
    }
}