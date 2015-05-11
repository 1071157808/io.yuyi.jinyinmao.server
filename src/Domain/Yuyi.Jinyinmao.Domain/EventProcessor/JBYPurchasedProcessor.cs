// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  9:57 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  10:25 PM
// ***********************************************************************
// <copyright file="JBYPurchasedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     JBYPurchasedProcessor.
    /// </summary>
    public class JBYPurchasedProcessor : EventProcessor<JBYPurchased>, IJBYPurchasedProcessor
    {
        #region IJBYPurchasedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(JBYPurchased @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                string message = Resources.Sms_JBYPurchased.FormatWith(e.JBYTranscation.Amount / 100);
                if (!await this.SmsService.SendMessageAsync(e.JBYTranscation.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(e.JBYTranscation.Cellphone, message));
                }
            });

            await this.ProcessingEventAsync(@event, async e =>
            {
                JBYTranscation jbyTranscation = e.JBYTranscation.ToDBJBYTranscationModel(e.SettleTranscation.TransactionId, e.Args);

                AccountTranscation accountTranscation = e.SettleTranscation.ToDBAccountTranscationModel(e.Args);

                using (JYMDBContext db = new JYMDBContext())
                {
                    if (!await db.ReadonlyQuery<JBYTranscation>().AnyAsync(t => t.TranscationIdentifier == jbyTranscation.TranscationIdentifier))
                    {
                        db.Add(jbyTranscation);
                    }

                    if (!await db.ReadonlyQuery<AccountTranscation>().AnyAsync(t => t.TranscationIdentifier == accountTranscation.TranscationIdentifier))
                    {
                        db.Add(accountTranscation);
                    }

                    await db.ExecuteSaveChangesAsync();
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IJBYPurchasedProcessor Members
    }
}