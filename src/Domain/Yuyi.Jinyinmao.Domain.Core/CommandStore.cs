// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  4:08 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  4:43 PM
// ***********************************************************************
// <copyright file="CommandStore.cs" company="Shanghai Yuyi">
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
    ///     CommandStore.
    /// </summary>
    public class CommandStore : Dictionary<Guid, string>, ICommandStore
    {
        #region ICommandStore Members

        /// <summary>
        ///     Gets the entity identifier.
        /// </summary>
        /// <value>The entity identifier.</value>
        public Guid EntityId { get; set; }

        /// <summary>
        ///     Stores the command record asynchronous.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>Task.</returns>
        public Task StoreCommandRecordAsync(CommandRecord record)
        {
            if (this.ContainsKey(record.CommandId))
            {
                return TaskDone.Done;
            }

            this.Add(record.CommandId, record.Command.GetType().Name);

            record.PartitionKey = this.EntityId.ToGuidString();
            record.RowKey = record.CommandId.ToGuidString();

            SiloClusterConfig.CommandStoreTable.ExecuteAsync(TableOperation.Insert(record));

            return TaskDone.Done;
        }

        #endregion ICommandStore Members
    }
}
