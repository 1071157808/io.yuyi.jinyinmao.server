// ***********************************************************************
// Assembly         : nyanya
// Author           : Siqi Lu
// Created          : 2015-03-04  6:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-24  3:07 PM
// ***********************************************************************
// <copyright file="ProductResponseHelper.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Cat.Domain.Products.ReadModels;
using nyanya.Cat.Models;

namespace nyanya.Cat.Helper
{
    internal static class ProductResponseHelper
    {
        #region Internal Methods

        internal static ProductShowingStatus GetProductShowingStatus(ProductInfo info, bool soldOut)
        {
            if (info.Repaid || info.RepaymentDeadline < DateTime.Today)
            {
                return ProductShowingStatus.Finished;
            }
            if (soldOut)
            {
                return ProductShowingStatus.SoldOut;
            }
            if (info.OnSale)
            {
                return ProductShowingStatus.OnSale;
            }
            if (info.OnPreSale && info.PreSale)
            {
                return ProductShowingStatus.OnPreSale;
            }
            if (info.StartSellTime > DateTime.Now)
            {
                return ProductShowingStatus.BeforeSale;
            }
            return ProductShowingStatus.Finished;
        }

        internal static ProductShowingStatus GetProductShowingStatus(Xingye.Domain.Products.ReadModels.ProductInfo info, bool soldOut)
        {
            if (info.Repaid || info.RepaymentDeadline < DateTime.Today)
            {
                return ProductShowingStatus.Finished;
            }
            if (soldOut)
            {
                return ProductShowingStatus.SoldOut;
            }
            if (info.OnSale)
            {
                return ProductShowingStatus.OnSale;
            }
            if (info.OnPreSale && info.PreSale)
            {
                return ProductShowingStatus.OnPreSale;
            }
            if (info.StartSellTime > DateTime.Now)
            {
                return ProductShowingStatus.BeforeSale;
            }
            return ProductShowingStatus.Finished;
        }

        internal static ProductShowingStatus GetProductShowingStatus(ZCBProductInfo info, int availableShareCount, int payingShareCount)
        {
            if (info.EnableSale == 0)
                return ProductShowingStatus.Finished;

            if (info.OnSale && (availableShareCount != 0 || payingShareCount != 0))
                return ProductShowingStatus.OnSale;

            if ((info.StartSellTime <= DateTime.Now && info.EndSellTime >= DateTime.Now && availableShareCount == 0 && payingShareCount == 0) || (info.EndSellTime < DateTime.Now))
                return ProductShowingStatus.Finished;

            if ( /*info.StartSellTime.Day == DateTime.Now.Day &&*/ info.StartSellTime > DateTime.Now)
                return ProductShowingStatus.BeforeSale;

            return ProductShowingStatus.Finished;
        }

        internal static ProductShowingStatus GetZcbProductShowingStatus(ProductInfo info, int enableSale, int availableShareCount, int payingShareCount)
        {
            if (enableSale == 0)
                return ProductShowingStatus.Finished;

            if (info.OnSale && (availableShareCount != 0 || payingShareCount != 0))
                return ProductShowingStatus.OnSale;

            if ((info.StartSellTime <= DateTime.Now && info.EndSellTime >= DateTime.Now && availableShareCount == 0 && payingShareCount == 0) || (info.EndSellTime < DateTime.Now))
                return ProductShowingStatus.Finished;

            if ( /*info.StartSellTime.Day == DateTime.Now.Day &&*/ info.StartSellTime > DateTime.Now)
                return ProductShowingStatus.BeforeSale;

            return ProductShowingStatus.Finished;
        }

        #endregion Internal Methods
    }
}
