// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-06  3:26 AM
// ***********************************************************************
// <copyright file="EventStore.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     EventStore.
    /// </summary>
    public class EventStore : IEventStore
    {
        #region IEventStore Members

        /// <summary>
        ///     Stores the event record asynchronous.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>Task.</returns>
        public Task StoreEventRecordAsync(EventRecord record)
        {
            return SiloClusterConfig.EventStoreTable.ExecuteAsync(TableOperation.InsertOrReplace(record));
        }

        #endregion IEventStore Members
    }
}