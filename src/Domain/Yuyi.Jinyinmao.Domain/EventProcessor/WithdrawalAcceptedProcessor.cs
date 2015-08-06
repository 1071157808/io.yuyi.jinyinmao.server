// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  11:01 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  11:10 PM
// ***********************************************************************
// <copyright file="WithdrawalAcceptedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     WithdrawalAcceptedProcessor.
    /// </summary>
    public class WithdrawalAcceptedProcessor : EventProcessor<WithdrawalAccepted>, IWithdrawalAcceptedProcessor
    {
        #region IWithdrawalAcceptedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(WithdrawalAccepted @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                await DBSyncHelper.SyncSettleAccountTransactionAsync(e.WithdrawalTransaction);

                await DBSyncHelper.SyncSettleAccountTransactionAsync(e.ChargeTransaction);
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IWithdrawalAcceptedProcessor Members
    }
}