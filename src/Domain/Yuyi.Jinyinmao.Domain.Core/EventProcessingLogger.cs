// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  6:02 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  8:40 PM
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
    ///     EventProcessingLogger.
    /// </summary>
    public class EventProcessingLogger : IEventProcessingLogger
    {
        #region IEventProcessingLogger Members

        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="EventId">The event identifier.</param>
        /// <param name="event">The event.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void LogError(Guid EventId, IEvent @event, string message, Exception exception = null)
        {
            SiloClusterConfig.EventProcessingErrorsTable.Execute(TableOperation.Insert(
                new ErrorLog
                {
                    Event = @event.ToJson(),
                    EventId = EventId,
                    Exception = exception.GetExceptionString(),
                    Message = message,
                    PartitionKey = EventId.ToGuidString(),
                    RowKey = Guid.NewGuid().ToGuidString(),
                    TimeStamp = DateTime.UtcNow.Ticks
                }));
        }

        #endregion IEventProcessingLogger Members
    }

    internal class ErrorLog : TableEntity
    {
        internal Guid EventId { get; set; }
        internal string Event { get; set; }
        internal string Exception { get; set; }
        internal string Message { get; set; }
        internal long TimeStamp { get; set; }
    }
}
