// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JBYProductSoldOutProcessor.cs
// Created          : 2015-05-27  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-12  2:45 AM
// ***********************************************************************
// <copyright file="JBYProductSoldOutProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Products;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYProductSoldOutProcessor.
    /// </summary>
    public class JBYProductSoldOutProcessor : EventProcessor<JBYProductSoldOut>, IJBYProductSoldOutProcessor
    {
        #region IJBYProductSoldOutProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(JBYProductSoldOut @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncJBYProductAsync(e.ProductInfo, e.Agreement1, e.Agreement2));

            await this.ProcessingEventAsync(@event, async e =>
            {
                IJBYProduct product = this.GrainFactory.GetGrain<IJBYProduct>(GrainTypeHelper.GetJBYProductGrainTypeLongKey());
                await product.RefreshAsync();
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IJBYProductSoldOutProcessor Members
    }
}