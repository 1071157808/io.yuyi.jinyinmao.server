// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  9:59 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  10:26 PM
// ***********************************************************************
// <copyright file="OrderRepaidProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
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
    ///     OrderRepaidProcessor.
    /// </summary>
    public class OrderRepaidProcessor : EventProcessor<OrderRepaid>, IOrderRepaidProcessor
    {
        #region IOrderRepaidProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(OrderRepaid @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                string message = Resources.Sms_OrderRepaid.FormatWith(e.OrderInfo.OrderNo, (e.OrderInfo.Principal + e.OrderInfo.Interest + e.OrderInfo.ExtraInterest) / 100);
                if (!await this.SmsService.SendMessageAsync(e.OrderInfo.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(e.OrderInfo.Cellphone, message));
                }
            });

            await this.ProcessingEventAsync(@event, async e =>
            {
                string orderIdentifier = e.OrderInfo.OrderId.ToGuidString();

                AccountTranscation principalTranscation = e.PrincipalTranscationInfo.ToDBAccountTranscationModel(e.Args);
                AccountTranscation interestTranscation = e.InterestTranscationInfo.ToDBAccountTranscationModel(e.Args);

                using (JYMDBContext db = new JYMDBContext())
                {
                    Models.Order order = await db.Query<Models.Order>().FirstAsync(o => o.OrderIdentifier == orderIdentifier);

                    order.IsRepaid = true;
                    order.RepaidTime = e.RepaidTime;

                    if (!await db.Query<AccountTranscation>().AnyAsync(t => t.TranscationIdentifier != principalTranscation.TranscationIdentifier))
                    {
                        db.AccountTranscations.Add(principalTranscation);
                    }

                    if (!await db.Query<AccountTranscation>().AnyAsync(t => t.TranscationIdentifier != interestTranscation.TranscationIdentifier))
                    {
                        db.AccountTranscations.Add(interestTranscation);
                    }

                    await db.ExecuteSaveChangesAsync();
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IOrderRepaidProcessor Members
    }
}