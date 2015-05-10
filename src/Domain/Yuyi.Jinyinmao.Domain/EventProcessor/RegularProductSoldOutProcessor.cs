// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-09  1:27 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-09  1:34 AM
// ***********************************************************************
// <copyright file="RegularProductSoldOutProcessor.cs" company="Shanghai Yuyi">
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
            await this.ProcessingEventAsync(@event, async e =>
            {
                string productIdentifier = e.ProductId.ToGuidString();

                using (JYMDBContext db = new JYMDBContext())
                {
                    Models.RegularProduct product = await db.Query<Models.RegularProduct>().FirstAsync(p => p.ProductIdentifier == productIdentifier);

                    product.SoldOut = true;
                    product.SoldOutTime = @event.SoldOutTime;

                    await db.ExecuteSaveChangesAsync();
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IRegularProductSoldOutProcessor Members
    }
}