// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JinyinmaoException.cs
// Created          : 2015-08-16  21:34
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-16  22:54
// ***********************************************************************
// <copyright file="JinyinmaoException.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Packages
{
    /// <summary>
    ///     ExceptionEx.
    /// </summary>
    public static class ExceptionEx
    {
        /// <summary>
        ///     Gets the jinyinmao exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>JinyinmaoException.</returns>
        public static JinyinmaoException GetJinyinmaoException(this Exception exception)
        {
            JinyinmaoException jinyinmaoException = exception as JinyinmaoException;
            if (jinyinmaoException != null)
            {
                return jinyinmaoException;
            }

            if (exception.InnerException != null)
            {
                jinyinmaoException = GetJinyinmaoException(exception.InnerException);
                if (jinyinmaoException != null)
                {
                    return jinyinmaoException;
                }
            }

            AggregateException aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                foreach (Exception e in aggregateException.InnerExceptions)
                {
                    jinyinmaoException = GetJinyinmaoException(e);
                    if (jinyinmaoException != null)
                    {
                        return jinyinmaoException;
                    }
                }
            }

            return null;
        }
    }

    /// <summary>
    ///     JinyinmaoException.
    /// </summary>
    public class JinyinmaoException : ApplicationException
    {
        /// <summary>
        ///     Initializes a new instance of the <c>JinyinmaoException</c> class.
        /// </summary>
        public JinyinmaoException()
        {
            this.EventId = Guid.NewGuid();
        }

        /// <summary>
        ///     Initializes a new instance of the <c>JinyinmaoException</c> class with the specified
        ///     error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public JinyinmaoException(string message)
            : base(message)
        {
            this.EventId = Guid.NewGuid();
        }

        /// <summary>
        ///     Initializes a new instance of the <c>JinyinmaoException</c> class with the specified
        ///     error message and the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception.</param>
        public JinyinmaoException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.EventId = Guid.NewGuid();
        }

        /// <summary>
        ///     Initializes a new instance of the <c>JinyinmaoException</c> class with the specified
        ///     string formatter and the arguments that are used for formatting the message which
        ///     describes the error.
        /// </summary>
        /// <param name="format">The string formatter which is used for formatting the error message.</param>
        /// <param name="args">The arguments that are used by the formatter to build the error message.</param>
        public JinyinmaoException(string format, params object[] args)
            : base(string.Format(format, args))
        {
            this.EventId = Guid.NewGuid();
        }

        /// <summary>
        ///     Gets or sets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public Guid EventId { get; set; }
    }
}