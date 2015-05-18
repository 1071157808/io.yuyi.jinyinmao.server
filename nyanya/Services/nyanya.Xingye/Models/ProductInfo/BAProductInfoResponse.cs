// FileInformation: nyanya/nyanya.Xingye/BAProductInfoResponse.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   2:07 PM

using System;
using Infrastructure.Lib.Extensions;
using nyanya.Xingye.Helper;
using Xingye.Domain.Products.ReadModels;
using Xingye.Domain.Products.Services.DTO;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     银承产品详情
    /// </summary>
    public class BAProductInfoResponse : ProductInfoResponse
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        /// <value>
        ///     The name of the bank.
        /// </value>
        public string BankName { get; set; }

        /// <summary>
        ///     Gets or sets the bill no.
        /// </summary>
        /// <value>
        ///     The bill no.
        /// </value>
        public string BillNo { get; set; }

        /// <summary>
        ///     Gets or sets the business license.
        /// </summary>
        /// <value>
        ///     The business license.
        /// </value>
        public string BusinessLicense { get; set; }

        /// <summary>
        ///     Gets or sets the name of the enterprise.
        /// </summary>
        /// <value>
        ///     The name of the enterprise.
        /// </value>
        public string EnterpriseName { get; set; }

        /// <summary>
        ///     Gets or sets the usage.
        /// </summary>
        /// <value>
        ///     The usage.
        /// </value>
        public string Usage { get; set; }

        #endregion Public Properties
    }

    internal static class ProductInfoExtensions
    {
        #region Internal Methods

        internal static BAProductInfoResponse ToBAProductInfoResponse(this ProductWithSaleInfo<BAProductInfo> info)
        {
            bool soldOut = info.ProductInfo.SoldOut || info.SumShareCount == 0 || (info.PaidShareCount == info.SumShareCount && info.AvailableShareCount == 0 && info.PayingShareCount == 0);
            string soldOutTime = "";
            if (soldOut)
            {
                soldOutTime = (info.ProductInfo.SoldOutTime ?? DateTime.Now).ToMeowFormat();
            }

            BAProductInfoResponse product = new BAProductInfoResponse
            {
                AvailableShareCount = info.AvailableShareCount,
                BankName = info.ProductInfo.BankName,
                BillNo = info.ProductInfo.BillNo,
                BusinessLicense = info.ProductInfo.BusinessLicense,
                CurrentValueDate = info.ProductInfo.CurrentValueDate.ToMeowFormat(),
                EndorseImageLink = info.ProductInfo.EndorseImageLink,
                EndorseImageThumbnailLink = info.ProductInfo.EndorseImageThumbnailLink,
                EndSellTime = info.ProductInfo.EndSellTime.ToMeowFormat(),
                EnterpriseName = info.ProductInfo.EnterpriseName,
                FinancingSum = info.ProductInfo.FinancingSum.ToIntFormat(),
                FinancingSumCount = info.ProductInfo.FinancingSumCount,
                LaunchTime = info.ProductInfo.LaunchTime.ToMeowFormat(),
                MaxShareCount = info.ProductInfo.MaxShareCount,
                MinShareCount = info.ProductInfo.MinShareCount,
                OnPreSale = info.ProductInfo.OnPreSale,
                OnSale = info.ProductInfo.OnSale,
                PaidShareCount = info.PaidShareCount,
                PayingShareCount = info.PayingShareCount,
                Period = info.ProductInfo.Period,
                PreEndSellTime = info.ProductInfo.PreEndSellTime.ToMeowFormat(),
                PreSale = info.ProductInfo.PreSale,
                PreStartSellTime = info.ProductInfo.PreStartSellTime.ToMeowFormat(),
                ProductIdentifier = info.ProductInfo.ProductIdentifier,
                ProductName = info.ProductInfo.ProductName,
                ProductNo = info.ProductInfo.ProductNo,
                ProductNumber = info.ProductInfo.ProductNumber,
                Repaid = info.ProductInfo.Repaid,
                RepaymentDeadline = info.ProductInfo.RepaymentDeadline.ToMeowFormat(),
                ServerTime = DateTime.Now.AddSeconds(2).ToMeowFormat(),
                SettleDate = info.ProductInfo.SettleDate.ToMeowFormat(),
                SoldOut = soldOut,
                SoldOutTime = soldOutTime,
                StartSellTime = info.ProductInfo.StartSellTime.ToMeowFormat(),
                SumShareCount = info.SumShareCount,
                UnitPrice = info.ProductInfo.UnitPrice.ToIntFormat(),
                Usage = info.ProductInfo.Usage,
                ValueDate = info.ProductInfo.ValueDate.ToMeowFormat(),
                ValueDateMode = info.ProductInfo.ValueDateMode,
                Yield = info.ProductInfo.Yield.RoundScale(1, 2)
            };

            product.ShowingStatus = ProductResponseHelper.GetProductShowingStatus(info.ProductInfo, soldOut);

            return product;
        }

        #endregion Internal Methods
    }
}