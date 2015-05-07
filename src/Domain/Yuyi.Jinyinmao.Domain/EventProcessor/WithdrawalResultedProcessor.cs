// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  2:22 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  12:55 PM
// ***********************************************************************
// <copyright file="WithdrawalResultedProcessor.cs" company="Shanghai Yuyi">
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
            string transcationIdentifier = @event.TranscationId.ToGuidString();
            await this.ProcessingEventAsync(@event, async e =>
            {
                string message = Resources.Sms_WithdrawalResulted.FormatWith(e.BankCardNo.GetLast(4), e.Amount / 100);
                if (!await this.SmsService.SendMessageAsync(e.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(@event.Cellphone, message));
                }
            });

            await this.ProcessingEventAsync(@event, async e =>
            {
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

        #endregion IWithdrawalResultedProcessor Members
    }
}