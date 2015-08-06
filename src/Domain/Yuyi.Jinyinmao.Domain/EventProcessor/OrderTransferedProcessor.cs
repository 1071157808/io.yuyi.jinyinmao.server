// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : OrderTransferedProcessor.cs
// Created          : 2015-08-06  12:09 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-06  12:13 AM
// ***********************************************************************
// <copyright file="OrderTransferedProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     OrderTransferedProcessor.
    /// </summary>
    public class OrderTransferedProcessor : EventProcessor<OrderTransfered>, IOrderTransferedProcessor
    {
        #region IOrderTransferedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(OrderTransfered @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                await DBSyncHelper.RemoveSettleAccountTransactionAsync(e.TransactionInfo);
                await DBSyncHelper.RemoveOrderAsync(e.OrderInfo);
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IOrderTransferedProcessor Members
    }
}