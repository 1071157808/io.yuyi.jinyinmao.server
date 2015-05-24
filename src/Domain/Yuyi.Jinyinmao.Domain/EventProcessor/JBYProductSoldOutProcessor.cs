// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  4:45 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-23  1:33 PM
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
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncJBYProduct(e.ProductInfo, e.Agreement1, e.Agreement2));

            await this.ProcessingEventAsync(@event, async e =>
            {
                IJBYProduct product = JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey());
                await product.RefreshAsync();
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IJBYProductSoldOutProcessor Members
    }
}