// FileInformation: nyanya/Infrastructure.Lib/BusinessValidationFailedException.cs
// CreatedTime: 2014/07/16   5:00 PM
// LastUpdatedTime: 2014/07/21   12:01 PM

using System;

namespace Infrastructure.Lib.Exceptions
{
    /// <summary>
    /// 该异常表示出现内部系统数据异常，需要日志和报警
    /// </summary>
    public class BusinessValidationFailedException : NyanyaException
    {
        /// <summary>
        ///     Initializes a new instance of the <c>BusinessValidationFailedException</c> class.
        /// </summary>
        public BusinessValidationFailedException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>BusinessValidationFailedException</c> class with the specified
        ///     error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BusinessValidationFailedException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>BusinessValidationFailedException</c> class with the specified
        ///     error message and the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception.</param>
        public BusinessValidationFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>BusinessValidationFailedException</c> class with the specified
        ///     string formatter and the arguments that are used for formatting the message which
        ///     describes the error.
        /// </summary>
        /// <param name="format">The string formatter which is used for formatting the error message.</param>
        /// <param name="args">The arguments that are used by the formatter to build the error message.</param>
        public BusinessValidationFailedException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}