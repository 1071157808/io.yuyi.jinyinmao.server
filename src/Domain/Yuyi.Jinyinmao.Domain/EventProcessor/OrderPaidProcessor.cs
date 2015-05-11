// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  5:22 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  10:24 PM
// ***********************************************************************
// <copyright file="OrderPaidProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
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
    ///     OrderPaidProcessor.
    /// </summary>
    public class OrderPaidProcessor : EventProcessor<OrderPaid>, IOrderPaidProcessor
    {
        #region IOrderPaidProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(OrderPaid @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                string message = Resources.Sms_OrderBuilt.FormatWith(e.Order.OrderNo, e.Order.Principal / 100);
                if (!await this.SmsService.SendMessageAsync(e.Order.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(e.Order.Cellphone, message));
                }
            });

            await this.ProcessingEventAsync(@event, async e =>
            {
                Models.Order order = e.Order.ToDBModel(e.Args);

                AccountTranscation transcation = e.Transcation.ToDBAccountTranscationModel(e.Args);

                string orderIdentifier = e.Order.OrderId.ToGuidString();
                string transcationIdentifier = e.Transcation.TransactionId.ToGuidString();
                using (JYMDBContext db = new JYMDBContext())
                {
                    if (!await db.ReadonlyQuery<Models.Order>().AnyAsync(c => c.OrderIdentifier == orderIdentifier))
                    {
                        db.Add(order);
                    }

                    if (!await db.ReadonlyQuery<AccountTranscation>().AnyAsync(t => t.TranscationIdentifier == transcationIdentifier))
                    {
                        db.Add(transcation);
                    }

                    await db.ExecuteSaveChangesAsync();
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IOrderPaidProcessor Members
    }
}