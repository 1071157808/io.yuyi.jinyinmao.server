// FileInformation: nyanya/Infrastructure.Lib/ApplicationBusinessException.cs
// CreatedTime: 2014/07/19   5:55 PM
// LastUpdatedTime: 2014/07/24   10:52 AM

using System;

namespace Infrastructure.Lib.Exceptions
{
    /// <summary>
    /// 该异常表示内部系统间出现错误，属于不可忽略的异常，需要日志和警报
    /// </summary>
    public class ApplicationBusinessException : NyanyaException
    {
        /// <summary>
        ///     Initializes a new instance of the <c>ApplicationBusinessException</c> class.
        /// </summary>
        public ApplicationBusinessException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>ApplicationBusinessException</c> class with the specified
        ///     error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ApplicationBusinessException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>ApplicationBusinessException</c> class with the specified
        ///     error message and the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception.</param>
        public ApplicationBusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>ApplicationBusinessException</c> class with the specified
        ///     string formatter and the arguments that are used for formatting the message which
        ///     describes the error.
        /// </summary>
        /// <param name="format">The string formatter which is used for formatting the error message.</param>
        /// <param name="args">The arguments that are used by the formatter to build the error message.</param>
        public ApplicationBusinessException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}