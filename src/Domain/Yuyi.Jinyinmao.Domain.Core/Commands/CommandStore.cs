// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-23  12:28 PM
// ***********************************************************************
// <copyright file="CommandStore.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     CommandStore.
    /// </summary>
    public class CommandStore : ICommandStore
    {
        #region ICommandStore Members

        /// <summary>
        ///     Stores the command record asynchronous.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>Task.</returns>
        public Task StoreCommandRecordAsync(CommandRecord record)
        {
            CloudBlockBlob blob = SiloClusterConfig.CommandStoreContainer.GetBlockBlobReference("{0}-{1}".FormatWith(record.CommandName, record.CommandId.ToGuidString()));
            blob.Properties.ContentType = "application/json; charset=utf-8";
            return blob.UploadTextAsync(record.Command);
        }

        #endregion ICommandStore Members
    }
}