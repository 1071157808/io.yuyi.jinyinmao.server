// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:00 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  7:45 PM
// ***********************************************************************
// <copyright file="ProductService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
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
using Yuyi.Jinyinmao.Domain.Products;
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
        public async Task<bool> CheckJBYProductNoExistsAsync(string productNo)
        {
            using (JYMDBContext db = new JYMDBContext())
            {
                return await db.ReadonlyQuery<JBYProduct>().AnyAsync(p => p.ProductNo == productNo);
            }
        }

        /// <summary>
        ///     Checks the product no exists.
        /// </summary>
        /// <param name="productNo">The product no.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> CheckProductNoExistsAsync(string productNo)
        {
            using (JYMDBContext db = new JYMDBContext())
            {
                return await db.ReadonlyQuery<RegularProduct>().AnyAsync(p => p.ProductNo == productNo);
            }
        }

        /// <summary>
        ///     Gets the agreement asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public Task<string> GetAgreementAsync(Guid productId, int agreementIndex)
        {
            IRegularProduct regularProduct = RegularProductFactory.GetGrain(productId);
            return regularProduct.GetAgreementAsync(agreementIndex);
        }

        /// <summary>
        ///     Gets the jby agreement asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public Task<string> GetJBYAgreementAsync(Guid productId, int agreementIndex)
        {
            IJBYProduct product = JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey());
            return product.GetAgreementAsync(agreementIndex);
        }

        /// <summary>
        /// Gets the jby issue no asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<int> GetJBYIssueNoAsync()
        {
            using (JYMDBContext db = new JYMDBContext())
            {
                return await db.ReadonlyQuery<JBYProduct>().MaxAsync(p => p.IssueNo);
            }
        }

        /// <summary>
        ///     Gets the jby product information asynchronous.
        /// </summary>
        /// <returns>Task&lt;JBYProductInfo&gt;.</returns>
        public async Task<JBYProductInfo> GetJBYProductInfoAsync()
        {
            IJBYProduct jbyProduct = JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey());
            return await jbyProduct.GetProductInfoAsync();
        }

        /// <summary>
        ///     Gets the jby product paid amount asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int64&gt;.</returns>
        public async Task<long> GetJBYProductPaidAmountAsync()
        {
            IJBYProduct jbyProduct = JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey());
            return await jbyProduct.GetJBYProductPaidAmountAsync();
        }

        /// <summary>
        ///     Gets the product information asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task&lt;RegularProductInfo&gt;.</returns>
        public async Task<RegularProductInfo> GetProductInfoAsync(Guid productId)
        {
            IRegularProduct regularProduct = RegularProductFactory.GetGrain(productId);
            return await regularProduct.GetRegularProductInfoAsync();
        }

        /// <summary>
        ///     get product infos as an asynchronous operation.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="productCategory">The product category.</param>
        /// <returns>Task&lt;PaginatedList&lt;RegularProductInfo&gt;&gt;.</returns>
        public async Task<PaginatedList<RegularProductInfo>> GetProductInfosAsync(int pageIndex, int pageSize, params long[] productCategory)
        {
            pageIndex = pageIndex < 1 ? 0 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;

            int totalCount;
            IList<RegularProduct> items;

            using (JYMDBContext db = new JYMDBContext())
            {
                totalCount = await db.ReadonlyQuery<RegularProduct>().CountAsync(p => productCategory.Contains(p.ProductCategory));
                items = await GetSortedProductContext<RegularProduct>(db).Where(p => productCategory.Contains(p.ProductCategory))
                    .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            }

            IList<RegularProductInfo> infos = items.Select(i => i.ToInfo()).ToList();

            await this.FillWithSaleData(infos);

            return new PaginatedList<RegularProductInfo>(pageIndex, pageSize, totalCount, infos);
        }

        /// <summary>
        ///     Gets the product paid amount asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public Task<long> GetProductPaidAmountAsync(Guid productId)
        {
            IRegularProduct regularProduct = RegularProductFactory.GetGrain(productId);
            return regularProduct.GetProductPaidAmountAsync();
        }

        /// <summary>
        ///     Gets the top product infos asynchronous.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="productCategories">The product categories.</param>
        /// <returns>Task&lt;List&lt;RegularProductInfo&gt;&gt;.</returns>
        public async Task<IList<RegularProductInfo>> GetTopProductInfosAsync(int number, params long[] productCategories)
        {
            number = number < 1 ? 1 : number;

            IList<RegularProduct> items;

            using (JYMDBContext db = new JYMDBContext())
            {
                items = await GetSortedProductContext<RegularProduct>(db).Where(p => productCategories.Contains(p.ProductCategory))
                    .Take(number).ToListAsync();
            }

            IList<RegularProductInfo> infos = items.Select(i => i.ToInfo()).ToList();

            await this.FillWithSaleData(infos);

            return infos;
        }

        /// <summary>
        ///     Hits the shelves.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task HitShelvesAsync(IssueRegularProduct command)
        {
            IRegularProduct product = RegularProductFactory.GetGrain(command.ProductId);
            return product.HitShelvesAsync(command);
        }

        /// <summary>
        ///     Hits the shelves.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task HitShelvesAsync(IssueJBYProduct command)
        {
            IJBYProduct jbyProduct = JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey());
            await jbyProduct.HitShelvesAsync(command);
        }

        /// <summary>
        ///     Refreshes the jyb product asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task RefreshJybProductAsync()
        {
            IJBYProduct jbyProduct = JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey());
            await jbyProduct.RefreshAsync();
        }

        /// <summary>
        ///     Reloads the jby product asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task ReloadJBYProductAsync()
        {
            IJBYProduct jbyProduct = JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey());
            await jbyProduct.ReloadAsync();
        }

        /// <summary>
        ///     Reloads the regular product asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task.</returns>
        public async Task ReloadRegularProductAsync(Guid productId)
        {
            IRegularProduct product = RegularProductFactory.GetGrain(productId);
            await product.ReloadAsync();
        }

        /// <summary>
        ///     Repays the asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task.</returns>
        public async Task RepayRegularProductAsync(Guid productId)
        {
            IRegularProduct product = RegularProductFactory.GetGrain(productId);
            await product.RepayAsync();
        }

        /// <summary>
        ///     Sets the current jby product to sold out asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SetCurrentJBYProductToSoldOutAsync()
        {
            IJBYProduct jbyProduct = JBYProductFactory.GetGrain(GrainTypeHelper.GetJBYProductGrainTypeLongKey());
            await jbyProduct.SetToSoldOutAsync();
        }

        /// <summary>
        ///     Sets the regular product to sold out asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task.</returns>
        public async Task SetRegularProductToSoldOutAsync(Guid productId)
        {
            IRegularProduct product = RegularProductFactory.GetGrain(productId);
            await product.SetToSoldOutAsync();
        }

        #endregion IProductService Members

        private static IQueryable<TProduct> GetSortedProductContext<TProduct>(JYMDBContext context) where TProduct : RegularProduct => context.ReadonlyQuery<TProduct>().OrderBy(p => p.SoldOut) // 未售罄 => 0, 售罄 =>1.  => 未售罄 > 售罄
            .ThenBy(p => p.StartSellTime) // 先开售的产品排前面 => 即在售 > 待售
            .ThenByDescending(p => p.IssueNo);

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
                    i.PaidAmount = await this.GetProductPaidAmountAsync(i.ProductId);
                }
            });

            await Task.WhenAll(tasks);
        }
    }
}