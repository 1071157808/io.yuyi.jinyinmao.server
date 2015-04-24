// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  4:45 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  5:46 PM
// ***********************************************************************
// <copyright file="EventStore.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Orleans;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     EventStore.
    /// </summary>
    public class EventStore : Dictionary<Guid, string>, IEventStore
    {
        #region IEventStore Members

        /// <summary>
        ///     Gets the entity identifier.
        /// </summary>
        /// <value>The entity identifier.</value>
        public Guid EntityId { get; set; }

        /// <summary>
        ///     Stores the event record asynchronous.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>Task.</returns>
        public Task StoreEventRecordAsync(EventRecord record)
        {
            if (this.ContainsKey(record.EventId))
            {
                return TaskDone.Done;
            }

            this.Add(record.EventId, record.Event.GetType().Name);

            record.PartitionKey = this.EntityId.ToGuidString();
            record.RowKey = record.EventId.ToGuidString();

            SiloClusterConfig.EventStoreTable.ExecuteAsync(TableOperation.Insert(record));

            return TaskDone.Done;
        }

        #endregion IEventStore Members
    }
}
