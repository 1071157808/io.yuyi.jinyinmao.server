// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-16  1:48 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-17  7:08 PM
// ***********************************************************************
// <copyright file="DepositResultedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Events;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     DepositResultedProcessor.
    /// </summary>
    public class DepositResultedProcessor : EventProcessor<DepositResulted>, IDepositResultedProcessor
    {
        #region IDepositResultedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(DepositResulted @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                string message = e.Result ? Resources.Sms_DepositSuccessed.FormatWith(e.TranscationInfo.BankCardNo.GetLast(4), e.TranscationInfo.Amount / 100)
                    : Resources.Sms_DepositFailed.FormatWith(e.TranscationInfo.BankCardNo.GetLast(4), e.TranscationInfo.Amount / 100, e.TransDesc);
                if (!await this.SmsService.SendMessageAsync(e.TranscationInfo.BankCardInfo.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(e.TranscationInfo.BankCardInfo.Cellphone, message));
                }
            });

            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncSettleAccountTranscation(e.TranscationInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion IDepositResultedProcessor Members
    }
}