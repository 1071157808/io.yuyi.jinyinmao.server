// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  4:45 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  11:48 PM
// ***********************************************************************
// <copyright file="JBYProductSoldOutProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Models;
using Yuyi.Jinyinmao.Domain.Products;
using JBYProduct = Yuyi.Jinyinmao.Domain.Models.JBYProduct;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
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
            await this.ProcessingEventAsync(@event, async e =>
            {
                string productIdentifier = e.ProductId.ToGuidString();

                using (JYMDBContext db = new JYMDBContext())
                {
                    JBYProduct product = await db.Query<JBYProduct>().FirstAsync(p => p.ProductIdentifier == productIdentifier);

                    if (!product.SoldOut)
                    {
                        product.SoldOut = true;
                        product.SoldOutTime = @event.SoldOutTime;

                        await db.ExecuteSaveChangesAsync();
                    }
                }
            });

            await this.ProcessingEventAsync(@event, async e =>
            {
                string productIdentifier = e.ProductId.ToGuidString();
                DateTime now = DateTime.UtcNow.AddHours(8);
                JBYProduct nextProduct;
                using (JYMDBContext db = new JYMDBContext())
                {
                    nextProduct = await db.Query<JBYProduct>()
                        .Where(p => p.ProductIdentifier != productIdentifier && p.IssueNo > e.IssueNo && !p.SoldOut && p.EndSellTime > now)
                        .OrderBy(p => p.StartSellTime).ThenBy(p => p.IssueTime).FirstOrDefaultAsync();

                    if (nextProduct != null)
                    {
                        nextProduct.IssueTime = DateTime.UtcNow.AddHours(8);

                        await db.ExecuteSaveChangesAsync();

                        IJBYProduct product = JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYGrainTypeLongKey());
                        await product.UpdateSaleInfoAsync(nextProduct);
                    }
                }

                if (nextProduct != null)
                {
                    IJBYProduct product = JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYGrainTypeLongKey());
                    await product.UpdateSaleInfoAsync(nextProduct);
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IJBYProductSoldOutProcessor Members
    }
}