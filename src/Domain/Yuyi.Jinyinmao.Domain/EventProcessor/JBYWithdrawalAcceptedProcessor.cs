// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-18  11:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-19  12:04 AM
// ***********************************************************************
// <copyright file="JBYWithdrawalAcceptedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYWithdrawalAcceptedProcessor.
    /// </summary>
    public class JBYWithdrawalAcceptedProcessor : EventProcessor<JBYWithdrawalAccepted>, IJBYWithdrawalAcceptedProcessor
    {
        #region IJBYWithdrawalAcceptedProcessor Members

        /// <summary>
        ///     process event as an asynchronous operation.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(JBYWithdrawalAccepted @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncJBYAccountTransactionAsync(e.TransactionInfo));

            await this.ProcessingEventAsync(@event, async e =>
            {
                if (DateTime.UtcNow.ToChinaStandardTime() >= e.TransactionInfo.PredeterminedResultDate.GetValueOrDefault(DateTime.MaxValue))
                {
                    await this.GrainFactory.GetGrain<IUser>(e.UserInfo.UserId).JBYWithdrawalResultedAsync(e.TransactionInfo.TransactionId);
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IJBYWithdrawalAcceptedProcessor Members
    }
}