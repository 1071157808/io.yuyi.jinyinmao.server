// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-22  5:04 PM
// ***********************************************************************
// <copyright file="IEventProcessingLogger.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
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
        /// <param name="eventId">The event identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void LogError(Guid eventId, string message, Exception exception = null);
    }
}