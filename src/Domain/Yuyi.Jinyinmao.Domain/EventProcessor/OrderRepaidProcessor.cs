// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  9:59 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  3:11 AM
// ***********************************************************************
// <copyright file="OrderRepaidProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Domain.Events
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
                await DBSyncHelper.SyncOrder(e.OrderInfo);
                await DBSyncHelper.SyncSettleAccountTranscation(e.InterestTranscationInfo);
                await DBSyncHelper.SyncSettleAccountTranscation(e.PrincipalTranscationInfo);
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IOrderRepaidProcessor Members
    }
}