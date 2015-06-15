// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-06-14  3:42 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-14  6:30 PM
// ***********************************************************************
// <copyright file="LogEventEntity.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Yuyi.Jinyinmao.Log
{
    /// <summary>
    ///     Represents a single log event for the Serilog Azure Table Storage Sink.
    /// </summary>
    /// <remarks>
    ///     The PartitionKey is set to "0" followed by the ticks of the log event time (in UTC) as per what Azure Diagnostics logging has.
    ///     The RowKey is set to "{Level}|{MessageTemplate}" to allow you to search for certain categories of log messages or indeed for a
    ///     specific log message quickly using the indexing in Azure Table Storage.
    /// </remarks>
    public class LogEventEntity : TableEntity
    {
        private static readonly Regex RowKeyNotAllowedMatch = new Regex(@"(\\|/|#|\?)");

        /// <summary>
        ///     Default constructor for the Storage Client library to re-hydrate entities when querying.
        /// </summary>
        public LogEventEntity()
        {
        }

        /// <summary>
        ///     Create a log event entity from a Serilog <see cref="LogEvent" />.
        /// </summary>
        /// <param name="log">The event to log</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="partitionKey"></param>
        public LogEventEntity(LogEvent log, IFormatProvider formatProvider, long partitionKey)
        {
            this.Timestamp = log.Timestamp.ToUniversalTime().DateTime;
            this.LocalTimestamp = log.Timestamp.ToUniversalTime().DateTime.AddHours(8);
            this.UnixTimestamp = log.Timestamp.ToUniversalTime().DateTime.UnixTimeStamp();
            this.PartitionKey = partitionKey.ToString();
            this.RowKey = GetValidRowKey($"{log.Level}|{Guid.NewGuid().ToGuidString()}");
            this.MessageTemplate = log.MessageTemplate.Text;
            this.Level = log.Level.ToString();
            this.Exception = log.Exception?.GetExceptionString();
            this.RenderedMessage = log.RenderMessage(formatProvider);
            StringWriter s = new StringWriter();
            new JsonFormatter(closingDelimiter: "", formatProvider: formatProvider).Format(log, s);
            this.Data = s.ToString();
            this.DeploymentId = RoleEnvironment.DeploymentId;
            this.RoleInstance = $"{RoleEnvironment.CurrentRoleInstance.Role.Name}_{RoleEnvironment.CurrentRoleInstance.Id}";
        }

        /// <summary>
        ///     A JSON-serialised representation of the data attached to the log message.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        ///     Gets the unique identifier of the deployment in which the role instance is running.
        /// </summary>
        public string DeploymentId { get; set; }

        /// <summary>
        ///     A string representation of the exception that was attached to the log (if any).
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        ///     The level of the log.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        ///     Local time.
        /// </summary>
        public DateTime LocalTimestamp { get; set; }

        /// <summary>
        ///     The template that was used for the log message.
        /// </summary>
        public string MessageTemplate { get; set; }

        /// <summary>
        ///     The rendered log message.
        /// </summary>
        public string RenderedMessage { get; set; }

        /// <summary>
        ///     Represents a role that is defined as part of a hosted service.
        /// </summary>
        public string RoleInstance { get; set; }

        /// <summary>
        ///     Unix timestamp.
        /// </summary>
        public long UnixTimestamp { get; set; }

        // http://msdn.microsoft.com/en-us/library/windowsazure/dd179338.aspx
        private static string GetValidRowKey(string rowKey)
        {
            rowKey = RowKeyNotAllowedMatch.Replace(rowKey, "");
            return rowKey.Length > 1024 ? rowKey.Substring(0, 1024) : rowKey;
        }
    }
}