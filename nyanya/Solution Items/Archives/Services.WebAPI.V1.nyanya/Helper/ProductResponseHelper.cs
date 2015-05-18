// FileInformation: nyanya/Services.WebAPI.V1.nyanya/ProductResponseHelper.cs
// CreatedTime: 2014/08/16   4:52 PM
// LastUpdatedTime: 2014/08/16   4:54 PM

using System;
using Cqrs.Domain.Products.ReadModels;
using Services.WebAPI.V1.nyanya.Models;

namespace Services.WebAPI.V1.nyanya.Helper
{
    internal static class ProductResponseHelper
    {
        #region Internal Methods

        internal static ProductShowingStatus GetProductShowingStatus(ProductInfo info, bool soldOut)
        {
            if (info.Repaid || info.RepaymentDeadline < DateTime.Today.AddDays(1))
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

        #endregion Internal Methods
    }
}