// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : ISettleAccountTransactionInsertedProcessor.cs
// Created          : 2015-07-31  4:28 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-31  4:30 PM
// ***********************************************************************
// <copyright file="ISettleAccountTransactionInsertedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     SettleAccountTransactionInsertedProcessor.
    /// </summary>
    public class SettleAccountTransactionInsertedProcessor : EventProcessor<SettleAccountTransactionInserted>, ISettleAccountTransactionInsertedProcessor
    {
        #region ISettleAccountTransactionInsertedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(SettleAccountTransactionInserted @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncSettleAccountTransaction(e.TransactionInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion ISettleAccountTransactionInsertedProcessor Members
    }
}