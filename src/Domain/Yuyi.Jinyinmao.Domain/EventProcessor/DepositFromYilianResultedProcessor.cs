// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  10:27 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-09  3:59 PM
// ***********************************************************************
// <copyright file="DepositFromYilianResultedProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     DepositFromYilianResultedProcessor.
    /// </summary>
    public class DepositFromYilianResultedProcessor : EventProcessor<DepositFromYilianResulted>, IDepositFromYilianResultedProcessor
    {
        #region IDepositFromYilianResultedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(DepositFromYilianResulted @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                string message = e.Result ? Resources.Sms_DepositSuccessed.FormatWith(e.BankCardNo.GetLast(4), e.Amount / 100)
                    : Resources.Sms_DepositFailed.FormatWith(e.BankCardNo.GetLast(4), e.Amount / 100, e.TransDesc);
                if (!await this.SmsService.SendMessageAsync(e.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(e.Cellphone, message));
                }
            });

            await this.ProcessingEventAsync(@event, async e =>
            {
                string transcationIdentifier = e.TranscationId.ToGuidString();
                using (JYMDBContext db = new JYMDBContext())
                {
                    AccountTranscation transcation = await db.Query<AccountTranscation>().FirstAsync(t => t.TranscationIdentifier == transcationIdentifier);

                    transcation.ResultTime = e.ResultTime;
                    transcation.ResultCode = e.ResultCode;
                    transcation.TransDesc = e.TransDesc;

                    await db.ExecuteSaveChangesAsync();
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IDepositFromYilianResultedProcessor Members
    }
}