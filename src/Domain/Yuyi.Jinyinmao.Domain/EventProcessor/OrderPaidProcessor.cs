// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  5:22 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  2:56 AM
// ***********************************************************************
// <copyright file="OrderPaidProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Domain.Events
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
                string message = Resources.Sms_OrderBuilt.FormatWith(e.OrderInfo.OrderNo, e.OrderInfo.Principal / 100);
                if (!await this.SmsService.SendMessageAsync(e.OrderInfo.Cellphone, message))
                {
                    throw new ApplicationException("Sms sending failed. {0}-{1}".FormatWith(e.OrderInfo.Cellphone, message));
                }
            });

            await this.ProcessingEventAsync(@event, async e =>
            {
                await DBSyncHelper.SyncSettleAccountTranscation(e.TranscationInfo);
                await DBSyncHelper.SyncOrder(e.OrderInfo);
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IOrderPaidProcessor Members
    }
}