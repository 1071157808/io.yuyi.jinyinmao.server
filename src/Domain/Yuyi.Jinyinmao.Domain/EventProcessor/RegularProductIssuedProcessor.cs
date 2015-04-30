// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  6:16 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-30  5:03 AM
// ***********************************************************************
// <copyright file="RegularProductIssuedProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
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
        public override Task ProcessEventAsync(RegularProductIssued @event)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    string info = new
                    {
                        @event.BankName,
                        @event.Drawee,
                        @event.DraweeInfo,
                        @event.EndorseImageLink,
                        @event.EnterpriseInfo,
                        @event.EnterpriseLicense,
                        @event.EnterpriseName,
                        @event.Period,
                        @event.RiskManagement,
                        @event.RiskManagementInfo,
                        @event.RiskManagementMode,
                        @event.Usage
                    }.ToJson();
                    RegularProductInfo product = new RegularProductInfo
                    {
                        ProductIdentifier = @event.ProductId.ToGuidString(),
                        EndSellTime = @event.EndSellTime,
                        FinancingSumAmount = @event.FinancingSumCount,
                        IssueNo = @event.IssueNo,
                        IssueTime = @event.IssueTime,
                        PledgeNo = @event.PledgeNo,
                        ProductCategory = @event.ProductCategory,
                        ProductName = @event.ProductName,
                        ProductNo = @event.ProductNo,
                        Repaid = false,
                        RepaidTime = null,
                        RepaymentDeadline = @event.RepaymentDeadline,
                        SettleDate = @event.SettleDate,
                        SoldOut = false,
                        SoldOutTime = null,
                        StartSellTime = @event.StartSellTime,
                        UnitPrice = @event.UnitPrice,
                        ValueDate = @event.ValueDate,
                        ValueDateMode = @event.ValueDateMode,
                        Yield = @event.Yield,
                        Info = info
                    };

                    string cacheId = "-{1}-{2}".FormatWith(product.ProductNo, product.ProductIdentifier);
                    await SiloClusterConfig.ProductCacheTable.SetDataToStorageCacheAsync("product", cacheId, product);
                    await SiloClusterConfig.ProductCacheTable.SetDataToStorageCacheAsync("agreement", "-1" + cacheId, @event.Agreement1);
                    await SiloClusterConfig.ProductCacheTable.SetDataToStorageCacheAsync("agreement", "-2" + cacheId, @event.Agreement2);
                }
                catch (Exception e)
                {
                    this.ErrorLogger.LogError(@event.EventId, @event, e.Message, e);
                }
            });

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    string info = new
                    {
                        @event.BankName,
                        @event.Drawee,
                        @event.DraweeInfo,
                        @event.EndorseImageLink,
                        @event.EnterpriseInfo,
                        @event.EnterpriseLicense,
                        @event.EnterpriseName,
                        @event.Period,
                        @event.RiskManagement,
                        @event.RiskManagementInfo,
                        @event.RiskManagementMode,
                        @event.Usage
                    }.ToJson();
                    Models.RegularProduct product = new Models.RegularProduct
                    {
                        ProductIdentifier = @event.ProductId.ToGuidString(),
                        EndSellTime = @event.EndSellTime,
                        FinancingSumAmount = @event.FinancingSumCount,
                        IssueNo = @event.IssueNo,
                        IssueTime = @event.IssueTime,
                        PledgeNo = @event.PledgeNo,
                        ProductCategory = @event.ProductCategory,
                        ProductName = @event.ProductName,
                        ProductNo = @event.ProductNo,
                        Repaid = false,
                        RepaidTime = null,
                        RepaymentDeadline = @event.RepaymentDeadline,
                        SettleDate = @event.SettleDate,
                        SoldOut = false,
                        SoldOutTime = null,
                        StartSellTime = @event.StartSellTime,
                        UnitPrice = @event.UnitPrice,
                        ValueDate = @event.ValueDate,
                        ValueDateMode = @event.ValueDateMode,
                        Yield = @event.Yield,
                        Info = info
                    };

                    using (JYMDBContext db = new JYMDBContext())
                    {
                        if (await db.RegularProducts.AnyAsync(p => p.ProductIdentifier == @event.ProductId.ToGuidString() || p.ProductNo == @event.ProductNo))
                        {
                            return;
                        }

                        await db.SaveAsync(product);
                    }
                }
                catch (Exception e)
                {
                    this.ErrorLogger.LogError(@event.EventId, @event, e.Message, e);
                }
            });

            return base.ProcessEventAsync(@event);
        }

        #endregion IRegularProductIssuedProcessor Members
    }
}
