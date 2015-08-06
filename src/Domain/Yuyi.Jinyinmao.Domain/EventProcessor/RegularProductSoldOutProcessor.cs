// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-09  1:27 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-22  6:11 PM
// ***********************************************************************
// <copyright file="RegularProductSoldOutProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     RegularProductSoldOutProcessor.
    /// </summary>
    public class RegularProductSoldOutProcessor : EventProcessor<RegularProductSoldOut>, IRegularProductSoldOutProcessor
    {
        #region IRegularProductSoldOutProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(RegularProductSoldOut @event)
        {
            await this.ProcessingEventAsync(@event, async e => await DBSyncHelper.SyncRegularProductAsync(e.ProductInfo, e.Agreement1, e.Agreement2));

            await base.ProcessEventAsync(@event);
        }

        #endregion IRegularProductSoldOutProcessor Members
    }
}