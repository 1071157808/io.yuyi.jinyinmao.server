// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  10:27 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  12:22 AM
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
        public override Task ProcessEventAsync(DepositFromYilianResulted @event)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    string message = @event.Result ? Resources.Sms_DepositSuccessed : Resources.Sms_DepositFailed;
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

        #endregion IDepositFromYilianResultedProcessor Members
    }
}