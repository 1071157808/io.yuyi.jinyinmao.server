// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SettleAccountTransactionResultedProcessor.cs
// Created          : 2015-08-05  6:53 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-05  7:04 PM
// ***********************************************************************
// <copyright file="SettleAccountTransactionResultedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     SettleAccountTransactionResultedProcessor.
    /// </summary>
    public class SettleAccountTransactionResultedProcessor : EventProcessor<SettleAccountTransactionResulted>, ISettleAccountTransactionResultedProcessor
    {
        #region ISettleAccountTransactionResultedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(SettleAccountTransactionResulted @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncSettleAccountTransactionAsync(e.TransactionInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion ISettleAccountTransactionResultedProcessor Members
    }
}