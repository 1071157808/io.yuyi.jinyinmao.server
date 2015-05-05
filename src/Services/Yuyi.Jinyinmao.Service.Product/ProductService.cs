// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:00 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-03  2:51 PM
// ***********************************************************************
// <copyright file="ProductService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Models;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     ProductService.
    /// </summary>
    public class ProductService : IProductService
    {
        #region IProductService Members

        /// <summary>
        ///     Checks the product no exists.
        /// </summary>
        /// <param name="productNo">The product no.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> CheckProductNoExistsAsync(string productNo)
        {
            using (JYMDBContext db = new JYMDBContext())
            {
                return db.ReadonlyQuery<RegularProduct>().AnyAsync(p => p.ProductNo == productNo);
            }
        }

        /// <summary>
        ///     Gets the agreement asynchronous.
        /// </summary>
        /// <param name="productNo">The product no.</param>
        /// <param name="productIdentifier">The product identifier.</param>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public Task<string> GetAgreementAsync(string productNo, string productIdentifier, int agreementIndex)
        {
            Guid productId;
            if (productIdentifier.IsNullOrEmpty() || !Guid.TryParseExact(productIdentifier, "N", out productId))
            {
                return null;
            }

            IRegularProduct regularProduct = RegularProductFactory.GetGrain(productId);
            return regularProduct.GetAgreementAsync(agreementIndex);
        }

        /// <summary>
        ///     Gets the product information asynchronous.
        /// </summary>
        /// <param name="productNo">The product no.</param>
        /// <param name="productIdentifier">The product identifier.</param>
        /// <returns>Task&lt;RegularProductInfo&gt;.</returns>
        public Task<RegularProductInfo> GetProductInfoAsync(string productNo, string productIdentifier)
        {
            Guid productId;
            if (productIdentifier.IsNullOrEmpty() || !Guid.TryParseExact(productIdentifier, "N", out productId))
            {
                return null;
            }

            IRegularProduct regularProduct = RegularProductFactory.GetGrain(productId);
            return regularProduct.GetRegularProductInfoAsync();
        }

        /// <summary>
        ///     get product infos as an asynchronous operation.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="productCategory">The product category.</param>
        /// <returns>Task&lt;PaginatedList&lt;RegularProductInfo&gt;&gt;.</returns>
        public async Task<PaginatedList<RegularProductInfo>> GetProductInfosAsync(int pageIndex, int pageSize, long productCategory)
        {
            pageIndex = pageIndex < 1 ? 0 : pageIndex;

            int totalCount;
            IList<RegularProduct> items;

            using (JYMDBContext db = new JYMDBContext())
            {
                totalCount = await db.ReadonlyQuery<RegularProduct>().CountAsync(p => p.ProductCategory == productCategory);
                items = await GetSortedProductContext<RegularProduct>(db).Where(p => p.ProductCategory == productCategory)
                    .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            }

            IList<RegularProductInfo> infos = items.Select(i => new RegularProductInfo
            {
                EndSellTime = i.EndSellTime,
                FinancingSumAmount = i.FinancingSumAmount,
                Info = i.Info,
                IssueNo = i.IssueNo,
                IssueTime = i.IssueTime,
                PaidAmount = null,
                PledgeNo = i.PledgeNo,
                ProductCategory = i.ProductCategory,
                ProductIdentifier = i.ProductIdentifier,
                ProductName = i.ProductName,
                ProductNo = i.ProductNo,
                Repaid = i.Repaid,
                RepaidTime = i.RepaidTime,
                RepaymentDeadline = i.RepaymentDeadline,
                SettleDate = i.SettleDate,
                SoldOut = i.SoldOut,
                SoldOutTime = i.SoldOutTime,
                StartSellTime = i.StartSellTime,
                UnitPrice = i.UnitPrice,
                ValueDate = i.ValueDate,
                ValueDateMode = i.ValueDateMode,
                Yield = i.Yield
            }).ToList();

            await this.FillWithSaleData(infos);

            return new PaginatedList<RegularProductInfo>(pageIndex, pageSize, totalCount, infos);
        }

        /// <summary>
        ///     Gets the product paid amount asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public Task<int> GetProductPaidAmountAsync(Guid productId)
        {
            IRegularProduct regularProduct = RegularProductFactory.GetGrain(productId);
            return regularProduct.GetProductPaidAmountAsync();
        }

        /// <summary>
        ///     Gets the top product infos asynchronous.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="productCategory">The product category.</param>
        /// <returns>Task&lt;List&lt;RegularProductInfo&gt;&gt;.</returns>
        public async Task<IList<RegularProductInfo>> GetTopProductInfosAsync(int number, long productCategory)
        {
            number = number < 1 ? 1 : number;

            IList<RegularProduct> items;

            using (JYMDBContext db = new JYMDBContext())
            {
                items = await GetSortedProductContext<RegularProduct>(db).Where(p => p.ProductCategory == productCategory)
                    .Take(number).ToListAsync();
            }

            IList<RegularProductInfo> infos = items.Select(i => new RegularProductInfo
            {
                EndSellTime = i.EndSellTime,
                FinancingSumAmount = i.FinancingSumAmount,
                Info = i.Info,
                IssueNo = i.IssueNo,
                IssueTime = i.IssueTime,
                PaidAmount = null,
                PledgeNo = i.PledgeNo,
                ProductCategory = i.ProductCategory,
                ProductIdentifier = i.ProductIdentifier,
                ProductName = i.ProductName,
                ProductNo = i.ProductNo,
                Repaid = i.Repaid,
                RepaidTime = i.RepaidTime,
                RepaymentDeadline = i.RepaymentDeadline,
                SettleDate = i.SettleDate,
                SoldOut = i.SoldOut,
                SoldOutTime = i.SoldOutTime,
                StartSellTime = i.StartSellTime,
                UnitPrice = i.UnitPrice,
                ValueDate = i.ValueDate,
                ValueDateMode = i.ValueDateMode,
                Yield = i.Yield
            }).ToList();

            await this.FillWithSaleData(infos);

            return infos;
        }

        /// <summary>
        ///     Hits the shelves.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task HitShelves(IssueRegularProduct command)
        {
            IRegularProduct product = RegularProductFactory.GetGrain(Guid.NewGuid());
            return product.HitShelvesAsync(command);
        }

        /// <summary>
        ///     Repays the asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="productNo">The product no.</param>
        /// <param name="productCategory">The product category.</param>
        /// <returns>Task.</returns>
        public Task RepayAsync(Guid productId, string productNo, string productCategory)
        {
            IRegularProduct product = RegularProductFactory.GetGrain(productId);
            return product.RepayAsync();
        }

        #endregion IProductService Members

        private static IQueryable<TProduct> GetSortedProductContext<TProduct>(JYMDBContext context) where TProduct : RegularProduct
        {
            return context.ReadonlyQuery<TProduct>().OrderBy(p => p.SoldOut) // 未售罄 => 0, 售罄 =>1.  => 未售罄 > 售罄
                .ThenBy(p => p.StartSellTime) // 先开售的产品排前面 => 即在售 > 待售
                .ThenByDescending(p => p.IssueNo);
        }

        private async Task FillWithSaleData(IEnumerable<RegularProductInfo> infos)
        {
            IEnumerable<Task> tasks = infos.Select(async i =>
            {
                if (i.SoldOut)
                {
                    i.PaidAmount = i.FinancingSumAmount;
                }
                else
                {
                    i.PaidAmount = await this.GetProductPaidAmountAsync(Guid.ParseExact(i.ProductIdentifier, "N"));
                }
            });

            await Task.WhenAll(tasks);
        }
    }
}