// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  5:22 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-06  3:16 AM
// ***********************************************************************
// <copyright file="OrderBuiltProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Models;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     OrderBuiltProcessor.
    /// </summary>
    public class OrderBuiltProcessor : EventProcessor<OrderBuilt>, IOrderBuiltProcessor
    {
        #region IOrderBuiltProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(OrderBuilt @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                string message = Resources.Sms_OrderBuilt.FormatWith(e.OrderNo, e.Principal / 100);
                await this.SmsService.SendMessageAsync(e.Cellphone, message);
            });

            await this.ProcessingEventAsync(@event, async e =>
            {
                Models.Order order = new Models.Order
                {
                    AccountTranscationIdentifier = e.AccountTranscationId.ToGuidString(),
                    Args = e.Args,
                    Cellphone = e.Cellphone,
                    ExtraInterest = e.ExtraInterest,
                    ExtraYield = e.ExtraYield,
                    Info = e.Info,
                    Interest = e.Interest,
                    IsRepaid = e.IsRepaid,
                    OrderIdentifier = e.OrderId.ToGuidString(),
                    OrderNo = e.OrderNo,
                    OrderTime = e.OrderTime,
                    Principal = e.Principal,
                    ProductIdentifier = e.ProductId.ToGuidString(),
                    ProductSnapshot = e.ProductSnapshot,
                    RepaidTime = e.RepaidTime,
                    ResultCode = e.ResultCode,
                    ResultTime = e.ResultTime,
                    SettleDate = e.SettleDate,
                    TransDesc = e.TransDesc,
                    UserIdentifier = e.UserId.ToGuidString(),
                    UserInfo = e.UserInfo,
                    ValueDate = e.ValueDate,
                    Yield = e.Yield
                };

                string orderIdentifier = @event.OrderId.ToGuidString();
                using (JYMDBContext db = new JYMDBContext())
                {
                    if (await db.Orders.AnyAsync(c => c.OrderIdentifier == orderIdentifier))
                    {
                        return;
                    }

                    await db.SaveAsync(order);
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IOrderBuiltProcessor Members
    }
}