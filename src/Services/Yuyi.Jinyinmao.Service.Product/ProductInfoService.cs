// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:03 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-19  4:23 PM
// ***********************************************************************
// <copyright file="ProductInfoService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Helper;
using Yuyi.Jinyinmao.Packages.Helper;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     ProductInfoService.
    /// </summary>
    public class ProductInfoService : IProductInfoService
    {
        private readonly IProductService innerService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductInfoService" /> class.
        /// </summary>
        /// <param name="innerService">The inner service.</param>
        public ProductInfoService(IProductService innerService)
        {
            this.innerService = innerService;
        }

        #region IProductInfoService Members

        /// <summary>
        ///     Checks the product no exists.
        /// </summary>
        /// <param name="productNo">The product no.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> CheckJBYProductNoExistsAsync(string productNo)
        {
            return this.innerService.CheckJBYProductNoExistsAsync(productNo);
        }

        /// <summary>
        ///     Checks the product no exists.
        /// </summary>
        /// <param name="productNo">The product no.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> CheckProductNoExistsAsync(string productNo)
        {
            return this.innerService.CheckProductNoExistsAsync(productNo);
        }

        /// <summary>
        ///     Gets the agreement asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> GetAgreementAsync(Guid productId, int agreementIndex)
        {
            string cacheName = "agreement";
            string cacheId = "{0}-{1}".FormatWith(productId.ToGuidString(), agreementIndex);
            string agreement = SiloClusterConfig.CacheTable.ReadDataFromTableCache<string>(cacheName, cacheId, TimeSpan.FromDays(1000));

            if (agreement.IsNullOrEmpty())
            {
                agreement = await this.innerService.GetAgreementAsync(productId, agreementIndex);
                if (agreement.IsNotNullOrEmpty())
                {
                    await SiloClusterConfig.CacheTable.SetDataToStorageCacheAsync(cacheName, cacheId, agreement);
                }
            }

            return agreement;
        }

        /// <summary>
        ///     Gets the jby agreement asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> GetJBYAgreementAsync(Guid productId, int agreementIndex)
        {
            string cacheName = "jby-agreement";
            string cacheId = "{0}-{1}".FormatWith(productId.ToGuidString(), agreementIndex);
            string agreement = SiloClusterConfig.CacheTable.ReadDataFromTableCache<string>(cacheName, cacheId, TimeSpan.FromDays(1000));

            if (agreement.IsNullOrEmpty())
            {
                agreement = await this.innerService.GetJBYAgreementAsync(productId, agreementIndex);
                if (agreement.IsNotNullOrEmpty())
                {
                    await SiloClusterConfig.CacheTable.SetDataToStorageCacheAsync(cacheName, cacheId, agreement);
                }
            }

            return agreement;
        }

        /// <summary>
        ///     Gets the jby product information asynchronous.
        /// </summary>
        /// <returns>Task&lt;JBYProductInfo&gt;.</returns>
        public async Task<JBYProductInfo> GetJBYProductInfoAsync()
        {
            string cacheName = "jby";
            string cacheId = ProductCategoryCodeHelper.PC100000030.ToString();
            JBYProductInfo product = SiloClusterConfig.CacheTable.ReadDataFromTableCache<JBYProductInfo>(cacheName, cacheId, TimeSpan.FromMinutes(1));

            if (product == null)
            {
                product = await this.innerService.GetJBYProductInfoAsync();
                await SiloClusterConfig.CacheTable.SetDataToStorageCacheAsync(cacheName, cacheId, product);
            }

            return product;
        }

        /// <summary>
        ///     Gets the jby product paid amount asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int64&gt;.</returns>
        public Task<long> GetJBYProductPaidAmountAsync()
        {
            return this.innerService.GetJBYProductPaidAmountAsync();
        }

        /// <summary>
        ///     Gets the product information asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task&lt;RegularProductInfo&gt;.</returns>
        public async Task<RegularProductInfo> GetProductInfoAsync(Guid productId)
        {
            string cacheName = "product";
            string cacheId = productId.ToGuidString();
            RegularProductInfo product = SiloClusterConfig.CacheTable.ReadDataFromTableCache<RegularProductInfo>(cacheName, cacheId, TimeSpan.FromMinutes(1));

            if (product == null)
            {
                product = await this.innerService.GetProductInfoAsync(productId);
                await SiloClusterConfig.CacheTable.SetDataToStorageCacheAsync(cacheName, cacheId, product);
            }

            return product;
        }

        /// <summary>
        ///     Gets the product infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="productCategories">The product categories.</param>
        /// <returns>Task&lt;PaginatedList&lt;RegularProductInfo&gt;&gt;.</returns>
        public async Task<PaginatedList<RegularProductInfo>> GetProductInfosAsync(int pageIndex, int pageSize, params long[] productCategories)
        {
            string cacheName = "product-page";
            string cacheId = "{0}-{1}-{2}".FormatWith(pageIndex, pageSize, productCategories.Join("-"));
            PaginatedList<RegularProductInfo> infos = SiloClusterConfig.CacheTable.ReadDataFromTableCache<PaginatedList<RegularProductInfo>>(cacheName, cacheId, TimeSpan.FromMinutes(1));

            if (infos == null)
            {
                infos = await this.innerService.GetProductInfosAsync(pageIndex, pageSize, productCategories);
                await SiloClusterConfig.CacheTable.SetDataToStorageCacheAsync(cacheName, cacheId, infos);
            }

            return infos;
        }

        /// <summary>
        ///     Gets the product paid amount asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public Task<long> GetProductPaidAmountAsync(Guid productId)
        {
            return this.innerService.GetProductPaidAmountAsync(productId);
        }

        /// <summary>
        ///     Gets the top product infos asynchronous.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="productCategories">The product categories.</param>
        /// <returns>Task&lt;List&lt;RegularProductInfo&gt;&gt;.</returns>
        public async Task<IList<RegularProductInfo>> GetTopProductInfosAsync(int number, params long[] productCategories)
        {
            string cacheName = "product-top";
            string cacheId = "{0}-{1}".FormatWith(number, productCategories.Join("-"));
            IList<RegularProductInfo> infos = SiloClusterConfig.CacheTable.ReadDataFromTableCache<IList<RegularProductInfo>>(cacheName, cacheId, TimeSpan.FromMinutes(1));

            if (infos == null)
            {
                infos = await this.innerService.GetTopProductInfosAsync(number, productCategories);
                await SiloClusterConfig.CacheTable.SetDataToStorageCacheAsync(cacheName, cacheId, infos);
            }

            return infos;
        }

        #endregion IProductInfoService Members
    }
}