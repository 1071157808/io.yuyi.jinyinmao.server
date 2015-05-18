// FileInformation: nyanya/nyanya.Xingye/ProductResponseHelper.cs
// CreatedTime: 2014/09/01   10:19 AM
// LastUpdatedTime: 2014/09/11   4:08 PM

using System;
using nyanya.Xingye.Models;
using Xingye.Domain.Products.ReadModels;

namespace nyanya.Xingye.Helper
{
    internal static class ProductResponseHelper
    {
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
    }
}