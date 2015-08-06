// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  7:51 PM
// ***********************************************************************
// <copyright file="WithdrawalResultedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     WithdrawalResultedProcessor.
    /// </summary>
    public class WithdrawalResultedProcessor : EventProcessor<WithdrawalResulted>, IWithdrawalResultedProcessor
    {
        #region IWithdrawalResultedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(WithdrawalResulted @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                string message = Resources.Sms_WithdrawalResulted.FormatWith(e.WithdrawalTransactionInfo.BankCardNo.GetLast(4), e.WithdrawalTransactionInfo.Amount / 100);
                if (!await this.SmsService.SendMessageAsync(e.UserInfo.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(e.UserInfo.Cellphone, message));
                }
            });

            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncSettleAccountTransactionAsync(e.WithdrawalTransactionInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion IWithdrawalResultedProcessor Members
    }
}