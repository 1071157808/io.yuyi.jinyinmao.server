// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  6:00 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  6:01 PM
// ***********************************************************************
// <copyright file="IEventProcessingLogger.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IEventProcessingLogger
    /// </summary>
    public interface IEventProcessingLogger
    {
        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="EventId">The event identifier.</param>
        /// <param name="event">The event.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void LogError(Guid EventId, IEvent @event, string message, Exception exception = null);
    }
}
