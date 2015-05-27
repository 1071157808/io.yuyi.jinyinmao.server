// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  7:51 PM
// ***********************************************************************
// <copyright file="DepositResultedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Lib;

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
                if (!await this.SmsService.SendMessageAsync(e.UserInfo.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(e.UserInfo.Cellphone, message));
                }
            });

            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncSettleAccountTranscation(e.TranscationInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion IDepositResultedProcessor Members
    }
}