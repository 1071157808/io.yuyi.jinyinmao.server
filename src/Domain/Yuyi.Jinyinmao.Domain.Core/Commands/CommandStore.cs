// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-06  3:25 AM
// ***********************************************************************
// <copyright file="CommandStore.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

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
            return SiloClusterConfig.CommandStoreTable.ExecuteAsync(TableOperation.InsertOrReplace(record));
        }

        #endregion ICommandStore Members
    }
}