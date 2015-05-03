// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  5:22 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  5:31 AM
// ***********************************************************************
// <copyright file="OrderBuiltProcessor.cs" company="Shanghai Yuyi">
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
        public override Task ProcessEventAsync(OrderBuilt @event)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    string message = Resources.Sms_OrderBuilt.FormatWith(@event.OrderNo, @event.Principal / 100);
                    await this.SmsService.SendMessageAsync(@event.Cellphone, message);
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
                    Models.Order order = new Models.Order
                    {
                        AccountTranscationIdentifier = @event.AccountTranscationId.ToGuidString(),
                        Args = @event.Args,
                        Cellphone = @event.Cellphone,
                        ExtraInterest = @event.ExtraInterest,
                        ExtraYield = @event.ExtraYield,
                        Info = @event.Info,
                        Interest = @event.Interest,
                        IsRepaid = @event.IsRepaid,
                        OrderIdentifier = @event.OrderId.ToGuidString(),
                        OrderNo = @event.OrderNo,
                        OrderTime = @event.OrderTime,
                        Principal = @event.Principal,
                        ProductIdentifier = @event.ProductId.ToGuidString(),
                        ProductSnapshot = @event.ProductSnapshot,
                        RepaidTime = @event.RepaidTime,
                        ResultCode = @event.ResultCode,
                        ResultTime = @event.ResultTime,
                        SettleDate = @event.SettleDate,
                        TransDesc = @event.TransDesc,
                        UserIdentifier = @event.UserId.ToGuidString(),
                        UserInfo = @event.UserInfo,
                        ValueDate = @event.ValueDate,
                        Yield = @event.Yield
                    };

                    using (JYMDBContext db = new JYMDBContext())
                    {
                        if (await db.Orders.AnyAsync(c => c.OrderIdentifier == @event.OrderId.ToGuidString()))
                        {
                            return;
                        }

                        await db.SaveAsync(order);
                    }
                }
                catch (Exception e)
                {
                    this.ErrorLogger.LogError(@event.EventId, @event, e.Message, e);
                }
            });

            return base.ProcessEventAsync(@event);
        }

        #endregion IOrderBuiltProcessor Members
    }
}