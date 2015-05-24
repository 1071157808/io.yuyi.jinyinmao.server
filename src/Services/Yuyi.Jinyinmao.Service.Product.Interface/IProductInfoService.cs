// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  10:57 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-20  4:01 PM
// ***********************************************************************
// <copyright file="IProductInfoService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Interface IProductInfoService
    /// </summary>
    public interface IProductInfoService
    {
        /// <summary>
        ///     Checks the product no exists.
        /// </summary>
        /// <param name="productNo">The product no.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> CheckJBYProductNoExistsAsync(string productNo);

        /// <summary>
        ///     Checks the product no exists.
        /// </summary>
        /// <param name="productNo">The product no.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> CheckProductNoExistsAsync(string productNo);

        /// <summary>
        ///     Gets the agreement asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> GetAgreementAsync(Guid productId, int agreementIndex);

        /// <summary>
        ///     Gets the jby agreement asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> GetJBYAgreementAsync(Guid productId, int agreementIndex);

        /// <summary>
        ///     Gets the jby product information asynchronous.
        /// </summary>
        /// <returns>Task&lt;JBYProductInfo&gt;.</returns>
        Task<JBYProductInfo> GetJBYProductInfoAsync();

        /// <summary>
        ///     Gets the jby product paid amount asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int64&gt;.</returns>
        Task<long> GetJBYProductPaidAmountAsync();

        /// <summary>
        ///     Gets the product information asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task&lt;RegularProductInfo&gt;.</returns>
        Task<RegularProductInfo> GetProductInfoAsync(Guid productId);

        /// <summary>
        ///     Gets the product infos asynchronous.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="productCategories">The product categories.</param>
        /// <returns>Task&lt;PaginatedList&lt;RegularProductInfo&gt;&gt;.</returns>
        Task<PaginatedList<RegularProductInfo>> GetProductInfosAsync(int pageIndex, int pageSize, params long[] productCategories);

        /// <summary>
        ///     Gets the product paid amount asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<long> GetProductPaidAmountAsync(Guid productId);

        /// <summary>
        ///     Gets the top product infos asynchronous.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="productCategories">The product categories.</param>
        /// <returns>Task&lt;List&lt;RegularProductInfo&gt;&gt;.</returns>
        Task<IList<RegularProductInfo>> GetTopProductInfosAsync(int number, params long[] productCategories);
    }
}