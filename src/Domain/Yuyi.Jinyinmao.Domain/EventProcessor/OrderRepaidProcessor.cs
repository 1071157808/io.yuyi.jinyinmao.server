// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  9:59 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  10:05 AM
// ***********************************************************************
// <copyright file="OrderRepaidProcessor.cs" company="Shanghai Yuyi">
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
            string orderIdentifier = @event.OrderId.ToGuidString();
            await this.ProcessingEventAsync(@event, async e =>
            {
                string message = Resources.Sms_OrderRepaid.FormatWith(e.OrderNo, (e.Principal + e.Interest + e.ExtraInterest) / 100);
                if (!await this.SmsService.SendMessageAsync(e.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(@event.Cellphone, message));
                }
            });

            await this.ProcessingEventAsync(@event, async e =>
            {
                using (JYMDBContext db = new JYMDBContext())
                {
                    Models.Order order = await db.Query<Models.Order>().FirstAsync(o => o.OrderIdentifier == orderIdentifier);

                    order.IsRepaid = true;
                    order.RepaidTime = e.RepaidTime;

                    await db.ExecuteSaveChangesAsync();
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IOrderRepaidProcessor Members
    }
}