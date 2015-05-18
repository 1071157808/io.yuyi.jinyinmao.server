// FileInformation: nyanya/Infrastructure.Lib/BusinessConcurrenceException.cs
// CreatedTime: 2014/07/14   4:42 PM
// LastUpdatedTime: 2014/07/21   10:41 AM

using System;

namespace Infrastructure.Lib.Exceptions
{
    /// <summary>
    /// 该异常表示出现并发问题，可能由于重试导致，系统中有很多模块做了幂等处理，需要日志，待审查即可
    /// </summary>
    public class BusinessConcurrenceException : NyanyaException
    {
        /// <summary>
        ///     Initializes a new instance of the <c>BusinessConcurrenceException</c> class.
        /// </summary>
        public BusinessConcurrenceException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>BusinessConcurrenceException</c> class with the specified
        ///     error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BusinessConcurrenceException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>BusinessConcurrenceException</c> class with the specified
        ///     error message and the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception.</param>
        public BusinessConcurrenceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>BusinessConcurrenceException</c> class with the specified
        ///     string formatter and the arguments that are used for formatting the message which
        ///     describes the error.
        /// </summary>
        /// <param name="format">The string formatter which is used for formatting the error message.</param>
        /// <param name="args">The arguments that are used by the formatter to build the error message.</param>
        public BusinessConcurrenceException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}