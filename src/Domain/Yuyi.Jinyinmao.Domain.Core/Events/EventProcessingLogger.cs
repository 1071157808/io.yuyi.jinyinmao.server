// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : EventProcessingLogger.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  20:49
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
            SiloClusterErrorLogger.LogError("EventProcessingError: {0}".FormatWith(message), exception);
        }

        #endregion IEventProcessingLogger Members
    }
}