// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-22  5:49 PM
// ***********************************************************************
// <copyright file="EventStore.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Moe.Lib;

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
            CloudBlockBlob blob = SiloClusterConfig.EventStoreContainer.GetBlockBlobReference("{0}-{1}".FormatWith(record.EventName, record.EventId.ToGuidString()));
            blob.Properties.ContentType = "application/json; charset=utf-8";
            return blob.UploadTextAsync(record.Event);
        }

        #endregion IEventStore Members
    }
}