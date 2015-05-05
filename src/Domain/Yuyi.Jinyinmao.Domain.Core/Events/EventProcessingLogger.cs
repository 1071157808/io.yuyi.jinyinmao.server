// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  4:27 PM
// ***********************************************************************
// <copyright file="EventProcessingLogger.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    /// ErrorLog.
    /// </summary>
    public class ErrorLog : TableEntity
    {
        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        /// <value>The event.</value>
        public string Event { get; set; }

        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public Guid EventId { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public string Exception { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public long TimeStamp { get; set; }
    }

    /// <summary>
    /// EventProcessingLogger.
    /// </summary>
    public class EventProcessingLogger : IEventProcessingLogger
    {
        #region IEventProcessingLogger Members

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="EventId">The event identifier.</param>
        /// <param name="event">The event.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void LogError(Guid EventId, IEvent @event, string message, Exception exception = null)
        {
            ErrorLog log = new ErrorLog
            {
                Event = @event.ToJson(),
                EventId = EventId,
                Exception = exception.GetExceptionString(),
                Message = message,
                PartitionKey = EventId.ToGuidString(),
                RowKey = Guid.NewGuid().ToGuidString(),
                TimeStamp = DateTime.UtcNow.Ticks
            };

            SiloClusterConfig.EventProcessingErrorsTable.Execute(TableOperation.Insert(log));
        }

        #endregion IEventProcessingLogger Members
    }
}