// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : OrderTransferedProcessor - Copy.cs
// Created          : 2015-08-06  5:25 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-06  5:27 PM
// ***********************************************************************
// <copyright file="OrderTransferedProcessor - Copy.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     OrderCanceledProcessor.
    /// </summary>
    public class OrderCanceledProcessor : EventProcessor<OrderCanceled>, IOrderCanceledProcessor
    {
        #region IOrderCanceledProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(OrderCanceled @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                await DBSyncHelper.RemoveSettleAccountTransactionAsync(e.TransactionInfo);
                await DBSyncHelper.RemoveOrderAsync(e.OrderInfo);
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IOrderCanceledProcessor Members
    }
}