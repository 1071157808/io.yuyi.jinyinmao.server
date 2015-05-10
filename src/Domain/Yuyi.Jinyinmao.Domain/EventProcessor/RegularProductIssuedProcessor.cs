// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  6:16 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-08  1:20 PM
// ***********************************************************************
// <copyright file="RegularProductIssuedProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Models;
using Yuyi.Jinyinmao.Service;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Domain.EventProcessor
{
    /// <summary>
    ///     RegularProductIssuedProcessor.
    /// </summary>
    public class RegularProductIssuedProcessor : EventProcessor<RegularProductIssued>, IRegularProductIssuedProcessor
    {
        private IProductInfoService productInfoService;

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

            await this.ProcessingEventAsync(@event, async e =>
            {
                await this.productInfoService.GetProductInfoAsync(@event.ProductId);
                for (int i = 1; i < 6; i++)
                {
                    await this.productInfoService.GetAgreementAsync(@event.ProductId, i);
                }
            });

            await base.ProcessEventAsync(@event);
        }

        #endregion IRegularProductIssuedProcessor Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.productInfoService = new ProductInfoService(new ProductService());

            return base.OnActivateAsync();
        }
    }
}