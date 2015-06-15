// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-06-14  6:18 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-14  6:23 PM
// ***********************************************************************
// <copyright file="AzureBatchingTableStorageSink.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Yuyi.Jinyinmao.Log
{
    public class AzureBatchingTableStorageSink : PeriodicBatchingSink
    {
        private readonly IFormatProvider formatProvider;
        private readonly CloudTable table;
        private int batchRowId;
        private long partitionKey;

        /// <summary>
        ///     Construct a sink that saves logs to the specified storage account.
        /// </summary>
        /// <param name="storageAccount">The Cloud Storage Account to use to insert the log entries to.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="batchSizeLimit"></param>
        /// <param name="period"></param>
        /// <param name="storageTableName">Table name that log entries will be written to. Note: Optional, setting this may impact performance</param>
        public AzureBatchingTableStorageSink(CloudStorageAccount storageAccount, IFormatProvider formatProvider, int batchSizeLimit, TimeSpan period, string storageTableName = null)
            : base(batchSizeLimit, period)
        {
            if (batchSizeLimit < 1 || batchSizeLimit > 100)
                throw new ArgumentException("batchSizeLimit must be between 1 and 100 for Azure Table Storage");
            this.formatProvider = formatProvider;
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            if (string.IsNullOrEmpty(storageTableName)) storageTableName = typeof(LogEventEntity).Name;

            this.table = tableClient.GetTableReference(storageTableName);
            this.table.CreateIfNotExists();
        }

        /// <summary>
        ///     Emit a batch of log events, running to completion synchronously.
        /// </summary>
        /// <param name="events">The events to emit.</param>
        /// <remarks>
        ///     Override either <see cref="PeriodicBatchingSink.EmitBatch" /> or <see cref="PeriodicBatchingSink.EmitBatchAsync" />,
        ///     not both.
        /// </remarks>
        protected override void EmitBatch(IEnumerable<LogEvent> events)
        {
            TableBatchOperation operation = new TableBatchOperation();

            bool first = true;

            foreach (LogEvent logEvent in events)
            {
                if (first)
                {
                    //check to make sure the partition key is not the same as the previous batch
                    long ticks = logEvent.Timestamp.ToUniversalTime().Ticks;
                    if (this.partitionKey != ticks)
                    {
                        this.batchRowId = 0; //the partitionkey has been reset
                        this.partitionKey = ticks; //store the new partition key
                    }
                    first = false;
                }

                LogEventEntity logEventEntity = new LogEventEntity(logEvent, this.formatProvider, this.partitionKey);
                logEventEntity.RowKey += "|" + this.batchRowId;
                operation.Add(TableOperation.Insert(logEventEntity));

                this.batchRowId++;
            }
            this.table.ExecuteBatch(operation);
        }
    }
}