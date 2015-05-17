// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-16  12:59 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  12:27 AM
// ***********************************************************************
// <copyright file="VerifyBankCardResultedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     VerifyBankCardResultedProcessor.
    /// </summary>
    public class VerifyBankCardResultedProcessor : EventProcessor<VerifyBankCardResulted>, IVerifyBankCardResultedProcessor
    {
        #region IVerifyBankCardResultedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(VerifyBankCardResulted @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                string message = e.Result ? Resources.Sms_VerifyBankCardSuccessed.FormatWith(e.BankCardInfo.BankCardNo.GetLast(4))
                    : Resources.Sms_VerifyBankCardFailed.FormatWith(e.BankCardInfo.BankCardNo.GetLast(4), e.TranDesc);
                if (!await this.SmsService.SendMessageAsync(e.BankCardInfo.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(e.BankCardInfo.Cellphone, message));
                }
            });

            string userIdentifier = @event.UserInfo.UserId.ToGuidString();
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncBankCard(e.BankCardInfo, userIdentifier));

            await base.ProcessEventAsync(@event);
        }

        #endregion IVerifyBankCardResultedProcessor Members
    }
}