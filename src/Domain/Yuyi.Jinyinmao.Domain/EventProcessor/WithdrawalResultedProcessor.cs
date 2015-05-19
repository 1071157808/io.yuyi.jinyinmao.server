// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  2:22 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  11:16 PM
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
                string message = Resources.Sms_WithdrawalResulted.FormatWith(e.WithdrawalTranscationInfo.BankCardNo.GetLast(4), e.WithdrawalTranscationInfo.Amount / 100);
                if (!await this.SmsService.SendMessageAsync(e.WithdrawalTranscationInfo.BankCardInfo.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(e.WithdrawalTranscationInfo.BankCardInfo.Cellphone, message));
                }
            });

            await this.ProcessingEventAsync(@event, async e => { await DBSyncHelper.SyncSettleAccountTranscation(e.WithdrawalTranscationInfo); });

            await base.ProcessEventAsync(@event);
        }

        #endregion IWithdrawalResultedProcessor Members
    }
}