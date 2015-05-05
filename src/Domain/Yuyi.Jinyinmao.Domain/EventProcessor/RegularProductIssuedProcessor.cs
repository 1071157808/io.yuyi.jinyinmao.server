// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  6:16 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-06  2:40 AM
// ***********************************************************************
// <copyright file="RegularProductIssuedProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Models;
using Yuyi.Jinyinmao.Helper;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     RegularProductIssuedProcessor.
    /// </summary>
    public class RegularProductIssuedProcessor : EventProcessor<RegularProductIssued>, IRegularProductIssuedProcessor
    {
        #region IRegularProductIssuedProcessor Members

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task ProcessEventAsync(RegularProductIssued @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                string info = new
                {
                    e.BankName,
                    e.Drawee,
                    e.DraweeInfo,
                    e.EndorseImageLink,
                    e.EnterpriseInfo,
                    e.EnterpriseLicense,
                    e.EnterpriseName,
                    e.Period,
                    e.RiskManagement,
                    e.RiskManagementInfo,
                    e.RiskManagementMode,
                    e.Usage
                }.ToJson();
                RegularProductInfo product = new RegularProductInfo
                {
                    ProductIdentifier = e.ProductId.ToGuidString(),
                    EndSellTime = e.EndSellTime,
                    FinancingSumAmount = e.FinancingSumCount,
                    IssueNo = e.IssueNo,
                    IssueTime = e.IssueTime,
                    PledgeNo = e.PledgeNo,
                    ProductCategory = e.ProductCategory,
                    ProductName = e.ProductName,
                    ProductNo = e.ProductNo,
                    Repaid = false,
                    RepaidTime = null,
                    RepaymentDeadline = e.RepaymentDeadline,
                    SettleDate = e.SettleDate,
                    SoldOut = false,
                    SoldOutTime = null,
                    StartSellTime = e.StartSellTime,
                    UnitPrice = e.UnitPrice,
                    ValueDate = e.ValueDate,
                    ValueDateMode = e.ValueDateMode,
                    Yield = e.Yield,
                    Info = info
                };

                string cacheId = "-{1}-{2}".FormatWith(product.ProductNo, product.ProductIdentifier);
                await SiloClusterConfig.ProductCacheTable.SetDataToStorageCacheAsync("product", cacheId, product);
                await SiloClusterConfig.ProductCacheTable.SetDataToStorageCacheAsync("agreement", "-1" + cacheId, e.Agreement1);
                await SiloClusterConfig.ProductCacheTable.SetDataToStorageCacheAsync("agreement", "-2" + cacheId, e.Agreement2);
            });

            await this.ProcessingEventAsync(@event, async e =>
            {
                string info = new
                {
                    e.BankName,
                    e.Drawee,
                    e.DraweeInfo,
                    e.EndorseImageLink,
                    e.EnterpriseInfo,
                    e.EnterpriseLicense,
                    e.EnterpriseName,
                    e.Period,
                    e.RiskManagement,
                    e.RiskManagementInfo,
                    e.RiskManagementMode,
                    e.Usage
                }.ToJson();
                Models.RegularProduct product = new Models.RegularProduct
                {
                    ProductIdentifier = e.ProductId.ToGuidString(),
                    EndSellTime = e.EndSellTime,
                    FinancingSumAmount = e.FinancingSumCount,
                    IssueNo = e.IssueNo,
                    IssueTime = e.IssueTime,
                    PledgeNo = e.PledgeNo,
                    ProductCategory = e.ProductCategory,
                    ProductName = e.ProductName,
                    ProductNo = e.ProductNo,
                    Repaid = false,
                    RepaidTime = null,
                    RepaymentDeadline = e.RepaymentDeadline,
                    SettleDate = e.SettleDate,
                    SoldOut = false,
                    SoldOutTime = null,
                    StartSellTime = e.StartSellTime,
                    UnitPrice = e.UnitPrice,
                    ValueDate = e.ValueDate,
                    ValueDateMode = e.ValueDateMode,
                    Yield = e.Yield,
                    Info = info
                };

                string productIdentifier = @event.ProductId.ToGuidString();
                using (JYMDBContext db = new JYMDBContext())
                {
                    if (await db.RegularProducts.AnyAsync(p => p.ProductIdentifier == productIdentifier || p.ProductNo == e.ProductNo))
                    {
                        return;
                    }

                    await db.SaveAsync(product);
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IRegularProductIssuedProcessor Members
    }
}