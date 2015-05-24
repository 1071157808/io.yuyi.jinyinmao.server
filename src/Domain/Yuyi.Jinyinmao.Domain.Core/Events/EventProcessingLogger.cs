// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-22  6:53 PM
// ***********************************************************************
// <copyright file="EventProcessingLogger.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.WindowsAzure.Storage.Table;
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
        /// <param name="EventId">The event identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void LogError(Guid EventId, string message, Exception exception = null)
        {
            ErrorLog log = new ErrorLog
            {
                Exception = exception.GetExceptionString(),
                Message = message,
                PartitionKey = EventId.ToGuidString(),
                RowKey = "EventProcessingError"
            };

            SiloClusterConfig.ErrorLogsTable.Execute(TableOperation.Insert(log));
        }

        #endregion IEventProcessingLogger Members
    }
}