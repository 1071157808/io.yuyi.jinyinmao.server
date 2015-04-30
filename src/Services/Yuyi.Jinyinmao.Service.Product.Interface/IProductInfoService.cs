// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  10:57 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-30  4:41 AM
// ***********************************************************************
// <copyright file="IProductInfoService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
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
        Task<bool> CheckProductNoExistsAsync(string productNo);

        /// <summary>
        /// Gets the agreement asynchronous.
        /// </summary>
        /// <param name="productNo">The product no.</param>
        /// <param name="productIdentifier">The product identifier.</param>
        /// <param name="agreementIndex">Index of the agreement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> GetAgreementAsync(string productNo, string productIdentifier, int agreementIndex);

        /// <summary>
        ///     Gets the product information asynchronous.
        /// </summary>
        /// <param name="productNo">The product no.</param>
        /// <param name="productIdentifier">The product identifier.</param>
        /// <returns>Task&lt;RegularProductInfo&gt;.</returns>
        Task<RegularProductInfo> GetProductInfoAsync(string productNo, string productIdentifier);

        /// <summary>
        ///     Gets the product paid amount asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> GetProductPaidAmountAsync(Guid productId);
    }
}
