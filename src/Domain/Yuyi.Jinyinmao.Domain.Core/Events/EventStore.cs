// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : EventStore.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  1:08
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
            string blobName = $"{record.SourceId}/{record.EventName}/{record.EventId.ToGuidString()}";
            CloudBlockBlob blob = SiloClusterConfig.EventStoreContainer.GetBlockBlobReference(blobName);
            blob.Properties.ContentType = "application/json; charset=utf-8";
            return blob.UploadTextAsync(record.Event);
        }

        #endregion IEventStore Members
    }
}