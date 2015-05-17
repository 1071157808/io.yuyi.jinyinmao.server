// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-27  6:08 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-16  1:52 AM
// ***********************************************************************
// <copyright file="AuthenticateResultedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
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
    ///     AddBankCardResultedProcessor.
    /// </summary>
    public class AuthenticateResultedProcessor : EventProcessor<AuthenticateResulted>, IAuthenticateResultedProcessor
    {
        #region IAuthenticateResultedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(AuthenticateResulted @event)
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

            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncUser(e.UserInfo));

            await base.ProcessEventAsync(@event);
        }

        #endregion IAuthenticateResultedProcessor Members
    }
}