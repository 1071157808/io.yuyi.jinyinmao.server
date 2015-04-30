// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:03 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-30  4:48 AM
// ***********************************************************************
// <copyright file="ProductInfoService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
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
        /// Gets the agreement asynchronous.
        /// </summary>
        /// <param name="productNo">The product no.</param>
        /// <param name="productIdentifier">The product identifier.</param>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> GetAgreementAsync(string productNo, string productIdentifier, int agreementIndex)
        {
            string cacheId = "-{0}-{1}-{2}".FormatWith(agreementIndex, productNo, productIdentifier);
            string agreement = SiloClusterConfig.ProductCacheTable.ReadDataFromTableCache<string>("agreement", cacheId, TimeSpan.FromDays(1000));

            if (agreement.IsNullOrEmpty())
            {
                agreement = await this.innerService.GetAgreementAsync(productNo, productIdentifier, agreementIndex);
                await SiloClusterConfig.ProductCacheTable.SetDataToStorageCacheAsync("agreement", cacheId, agreement);
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
            string cacheId = "-{0}-{1}".FormatWith(productNo, productIdentifier);
            RegularProductInfo product = SiloClusterConfig.ProductCacheTable.ReadDataFromTableCache<RegularProductInfo>("product", cacheId, TimeSpan.FromMinutes(1));

            if (product == null)
            {
                product = await this.innerService.GetProductInfoAsync(productNo, productIdentifier);
                await SiloClusterConfig.ProductCacheTable.SetDataToStorageCacheAsync("product", cacheId, product);
            }

            return product;
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
