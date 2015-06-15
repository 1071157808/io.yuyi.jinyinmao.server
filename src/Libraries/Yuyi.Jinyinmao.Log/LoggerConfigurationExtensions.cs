// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-06-14  3:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-14  6:54 PM
// ***********************************************************************
// <copyright file="LoggerConfigurationExtensions.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.WindowsAzure.Storage;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Yuyi.Jinyinmao.Log
{
    /// <summary>
    ///     LoggerConfigurationExtensions.
    /// </summary>
    public static class LoggerConfigurationExtensions
    {
        /// <summary>
        ///     A reasonable default for the number of events posted in
        ///     each batch.
        /// </summary>
        public const int DefaultBatchPostingLimit = 50;

        /// <summary>
        ///     A reasonable default time to wait between checking for event batches.
        /// </summary>
        public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(2);

        /// <summary>
        ///     Adds a sink that writes log events as records in the 'LogEventEntity' Azure Table Storage table in the given storage account.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="storageAccount">The Cloud Storage Account to use to insert the log entries to.</param>
        /// <param name="storageTableName">Table name that log entries will be written to. Note: Optional, setting this may impact performance</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="batchPostingLimit">The maximum number of events to post in a single batch.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        /// <returns>Logger configuration, allowing configuration to continue.</returns>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        public static LoggerConfiguration AzureTableStorage(
            this LoggerSinkConfiguration loggerConfiguration,
            CloudStorageAccount storageAccount,
            string storageTableName = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            TimeSpan? period = null,
            int? batchPostingLimit = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException(nameof(loggerConfiguration));
            if (storageAccount == null) throw new ArgumentNullException(nameof(storageAccount));

            var sink = (ILogEventSink)new AzureBatchingTableStorageSink(storageAccount, formatProvider, batchPostingLimit ?? DefaultBatchPostingLimit, period ?? DefaultPeriod, storageTableName);

            return loggerConfiguration.Sink(sink, restrictedToMinimumLevel);
        }
    }
}