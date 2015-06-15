// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-14  10:55 PM
// ***********************************************************************
// <copyright file="EventProcessingLogger.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     EventProcessingLogger.
    /// </summary>
    public class EventProcessingLogger : IEventProcessingLogger
    {
        #region IEventProcessingLogger Members

        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void LogError(Guid eventId, string message, Exception exception = null)
        {
            SiloClusterErrorLogger.Log(exception, "EventProcessingError: {0}".FormatWith(message));
        }

        #endregion IEventProcessingLogger Members
    }
}