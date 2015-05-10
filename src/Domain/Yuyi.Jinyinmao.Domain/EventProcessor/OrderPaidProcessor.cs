// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  5:22 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-10  12:38 PM
// ***********************************************************************
// <copyright file="OrderPaidProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Models;
using Yuyi.Jinyinmao.Packages.Helper;

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
                Models.Order order = new Models.Order
                {
                    AccountTranscationIdentifier = e.Order.AccountTranscationId.ToGuidString(),
                    Args = e.Args,
                    Cellphone = e.Order.Cellphone,
                    ExtraInterest = e.Order.ExtraInterest,
                    ExtraYield = e.Order.ExtraYield,
                    Info = e.Order.Info.ToJson(),
                    Interest = e.Order.Interest,
                    IsRepaid = e.Order.IsRepaid,
                    OrderIdentifier = e.Order.OrderId.ToGuidString(),
                    OrderNo = e.Order.OrderNo,
                    OrderTime = e.Order.OrderTime,
                    Principal = e.Order.Principal,
                    ProductIdentifier = e.Order.ProductId.ToGuidString(),
                    ProductSnapshot = e.Order.ProductSnapshot.ToJson(),
                    RepaidTime = e.Order.RepaidTime,
                    ResultCode = e.Order.ResultCode,
                    ResultTime = e.Order.ResultTime,
                    SettleDate = e.Order.SettleDate,
                    TransDesc = e.Order.TransDesc,
                    UserIdentifier = e.Order.UserId.ToGuidString(),
                    UserInfo = e.Order.UserInfo.ToJson(),
                    ValueDate = e.Order.ValueDate,
                    Yield = e.Order.Yield
                };

                AccountTranscation transcation = new AccountTranscation
                {
                    AgreementsInfo = e.Transcation.AgreementsInfo.ToJson(),
                    Amount = e.Transcation.Amount,
                    Args = e.Args,
                    BankCardInfo = new Dictionary<string, object> { { "BankCardNo", e.Transcation.BankCardNo } }.ToJson(),
                    Cellphone = e.Order.Cellphone,
                    ChannelCode = e.Transcation.ChannelCode,
                    Info = JsonHelper.NewDictionary,
                    ResultCode = e.Transcation.ResultCode,
                    ResultTime = e.Transcation.ResultTime,
                    TradeCode = e.Transcation.TradeCode,
                    TransDesc = e.Transcation.TransDesc,
                    TranscationIdentifier = e.Transcation.TransactionId.ToGuidString(),
                    TranscationTime = e.Transcation.TransactionTime,
                    UserIdentifier = e.Order.UserId.ToGuidString(),
                    UserInfo = e.Order.UserInfo.ToJson()
                };

                string orderIdentifier = e.Order.OrderId.ToGuidString();
                string transcationIdentifier = e.Transcation.TransactionId.ToGuidString();
                using (JYMDBContext db = new JYMDBContext())
                {
                    if (!await db.Orders.AnyAsync(c => c.OrderIdentifier == orderIdentifier))
                    {
                        db.Add(order);
                    }

                    if (!await db.AccountTranscations.AnyAsync(t => t.TranscationIdentifier == transcationIdentifier))
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