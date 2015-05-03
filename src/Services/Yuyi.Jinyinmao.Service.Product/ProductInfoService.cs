// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:03 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-03  2:57 PM
// ***********************************************************************
// <copyright file="ProductInfoService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Helper;
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
        public Task<bool> CheckProductNoExistsAsync(string productNo)
        {
            return this.innerService.CheckProductNoExistsAsync(productNo);
        }

        /// <summary>
        ///     Gets the agreement asynchronous.
        /// </summary>
        /// <param name="productNo">The product no.</param>
        /// <param name="productIdentifier">The product identifier.</param>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> GetAgreementAsync(string productNo, string productIdentifier, int agreementIndex)
        {
            string cacheName = "agreement";
            string cacheId = "-{0}-{1}-{2}".FormatWith(agreementIndex, productNo, productIdentifier);
            string agreement = SiloClusterConfig.ProductCacheTable.ReadDataFromTableCache<string>(cacheName, cacheId, TimeSpan.FromDays(1000));

            if (agreement.IsNullOrEmpty())
            {
                agreement = await this.innerService.GetAgreementAsync(productNo, productIdentifier, agreementIndex);
                await SiloClusterConfig.ProductCacheTable.SetDataToStorageCacheAsync(cacheName, cacheId, agreement);
            }

            return agreement;
        }

        /// <summary>
        ///     Gets the product information asynchronous.
        /// </summary>
        /// <param name="productNo">The product no.</param>
        /// <param name="productIdentifier">The product identifier.</param>
        /// <returns>Task&lt;RegularProductInfo&gt;.</returns>
        public async Task<RegularProductInfo> GetProductInfoAsync(string productNo, string productIdentifier)
        {
            string cacheName = "product";
            string cacheId = "-{0}-{1}".FormatWith(productNo, productIdentifier);
            RegularProductInfo product = SiloClusterConfig.ProductCacheTable.ReadDataFromTableCache<RegularProductInfo>(cacheName, cacheId, TimeSpan.FromMinutes(1));

            if (product == null)
            {
                product = await this.innerService.GetProductInfoAsync(productNo, productIdentifier);
                await SiloClusterConfig.ProductCacheTable.SetDataToStorageCacheAsync(cacheName, cacheId, product);
            }

            return product;
        }

        /// <summary>
        ///     Gets the product infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="productCategory">The product category.</param>
        /// <returns>Task&lt;PaginatedList&lt;RegularProductInfo&gt;&gt;.</returns>
        public async Task<PaginatedList<RegularProductInfo>> GetProductInfosAsync(int pageIndex, int pageSize, long productCategory)
        {
            string cacheName = "product-page";
            string cacheId = "-{0}-{1}-{2}".FormatWith(pageIndex, pageSize, productCategory);
            PaginatedList<RegularProductInfo> infos = SiloClusterConfig.ProductCacheTable.ReadDataFromTableCache<PaginatedList<RegularProductInfo>>(cacheName, cacheId, TimeSpan.FromMinutes(3));

            if (infos == null)
            {
                infos = await this.innerService.GetProductInfosAsync(pageIndex, pageSize, productCategory);
                await SiloClusterConfig.ProductCacheTable.SetDataToStorageCacheAsync(cacheName, cacheId, infos);
            }

            return infos;
        }

        /// <summary>
        ///     Gets the top product infos asynchronous.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="productCategory">The product category.</param>
        /// <returns>Task&lt;List&lt;RegularProductInfo&gt;&gt;.</returns>
        public async Task<IList<RegularProductInfo>> GetTopProductInfosAsync(int number, long productCategory)
        {
            string cacheName = "product-top";
            string cacheId = "-{0}-{1}".FormatWith(number, productCategory);
            IList<RegularProductInfo> infos = SiloClusterConfig.ProductCacheTable.ReadDataFromTableCache<IList<RegularProductInfo>>(cacheName, cacheId, TimeSpan.FromMinutes(3));

            if (infos == null)
            {
                infos = await this.innerService.GetTopProductInfosAsync(number, productCategory);
                await SiloClusterConfig.ProductCacheTable.SetDataToStorageCacheAsync(cacheName, cacheId, infos);
            }

            return infos;
        }

        /// <summary>
        ///     Gets the product paid amount asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public Task<int> GetProductPaidAmountAsync(Guid productId)
        {
            return this.innerService.GetProductPaidAmountAsync(productId);
        }

        #endregion IProductInfoService Members
    }
}