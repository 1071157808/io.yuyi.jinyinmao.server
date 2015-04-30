// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:00 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-30  4:42 AM
// ***********************************************************************
// <copyright file="ProductService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Data.Entity;
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
        /// Gets the agreement asynchronous.
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
        ///     Hits the shelves.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task HitShelves(IssueRegularProduct command)
        {
            IRegularProduct product = RegularProductFactory.GetGrain(Guid.NewGuid());
            return product.HitShelvesAsync(command);
        }

        #endregion IProductService Members
    }
}
