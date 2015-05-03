// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  2:22 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  2:29 AM
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
        public override Task ProcessEventAsync(WithdrawalResulted @event)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    string message = Resources.Sms_WithdrawalResulted;
                    await this.SmsService.SendMessageAsync(@event.Cellphone, message.FormatWith(@event.Amount / 100));
                }
                catch (Exception e)
                {
                    this.ErrorLogger.LogError(@event.EventId, @event, e.Message, e);
                }
            });

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    using (JYMDBContext db = new JYMDBContext())
                    {
                        AccountTranscation transcation = await db.Query<AccountTranscation>().FirstAsync(t => t.TranscationIdentifier == @event.TranscationId.ToGuidString());

                        transcation.ResultTime = @event.ResultTime;
                        transcation.ResultCode = @event.ResultCode;
                        transcation.TransDesc = @event.TransDesc;

                        await db.ExecuteSaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    this.ErrorLogger.LogError(@event.EventId, @event, e.Message, e);
                }
            });

            return base.ProcessEventAsync(@event);
        }

        #endregion IWithdrawalResultedProcessor Members
    }
}