using System;
using Cat.Commands.Products;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Infrastructure.Lib.Extensions;
using nyanya.Cat.Helper;

namespace nyanya.Cat.Models
{
    /// <summary>
    ///     资产包产品详情
    /// </summary>
    public class ZCBProductInfoResponse
    {
        /// <summary>
        ///     Gets or sets the available share count.
        /// </summary>
        /// <value>
        ///     The available share count.
        /// </value>
        public int AvailableShareCount { get; set; }

        /// <summary>
        ///     授权委托书名称
        /// </summary>
        public string ConsignmentAgreementName { get; set; }

        /// <summary>
        ///     Gets or sets the current value date.
        /// </summary>
        /// <value>
        ///     The current value date.
        /// </value>
        public string CurrentValueDate { get; set; }

        /// <summary>
        ///     Gets or sets the enable share
        /// </summary>
        /// <value>
        ///     The enable share.
        /// </value>
        public int EnableSale { get; set; }

        /// <summary>
        ///     Gets or sets the end sell time.
        /// </summary>
        /// <value>
        ///     The end sell time.
        /// </value>
        public string EndSellTime { get; set; }

        /// <summary>
        ///     Gets or sets the financing sum.
        /// </summary>
        /// <value>
        ///     The financing sum.
        /// </value>
        public decimal FinancingSum { get; set; }

        /// <summary>
        ///     Gets or sets the financing sum count.
        /// </summary>
        /// <value>
        ///     The financing sum count.
        /// </value>
        public int FinancingSumCount { get; set; }

        /// <summary>
        ///     Gets or sets the launch time.
        /// </summary>
        /// <value>
        ///     The launch time.
        /// </value>
        public string LaunchTime { get; set; }

        /// <summary>
        ///     Gets or sets the maximum share count.
        /// </summary>
        /// <value>
        ///     The maximum share count.
        /// </value>
        public int MaxShareCount { get; set; }

        /// <summary>
        ///     Gets or sets the minimum share count.
        /// </summary>
        /// <value>
        ///     The minimum share count.
        /// </value>
        public int MinShareCount { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [on pre sale].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [on pre sale]; otherwise, <c>false</c>.
        /// </value>
        public bool OnPreSale { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [on sale].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [on sale]; otherwise, <c>false</c>.
        /// </value>
        public bool OnSale { get; set; }

        /// <summary>
        ///     销售百分比
        /// </summary>
        public int PaidPercent
        {
            get { return this.SumShareCount == 0 ? 100 : (int)(Decimal.Divide(this.PaidShareCount, this.SumShareCount) * 100); }
        }

        public int PaidShareCount { get; set; }

        /// <summary>
        ///     投资协议名称
        /// </summary>
        public string PledgeAgreementName { get; set; }

        /// <summary>
        ///     Gets or sets the pre end sell time.
        /// </summary>
        /// <value>
        ///     The pre end sell time.
        /// </value>
        public string PreEndSellTime { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [pre sale].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [pre sale]; otherwise, <c>false</c>.
        /// </value>
        public bool PreSale { get; set; }

        /// <summary>
        ///     Gets or sets the pre start sell time.
        /// </summary>
        /// <value>
        ///     The pre start sell time.
        /// </value>
        public string PreStartSellTime { get; set; }

        /// <summary>
        /// 产品分类 （10金银猫产品 20富滇产品）
        /// </summary>
        public ProductCategory ProductCategory { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>
        ///     The product identifier.
        /// </value>
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     Gets or sets the name of the product.
        /// </summary>
        /// <value>
        ///     The name of the product.
        /// </value>
        public string ProductName { get; set; }

        /// <summary>
        ///     Gets or sets the product no.
        /// </summary>
        /// <value>
        ///     The product no.
        /// </value>
        public string ProductNo { get; set; }

        /// <summary>
        ///     Gets or sets the product number.
        /// </summary>
        /// <value>
        ///     The product number.
        /// </value>
        public int ProductNumber { get; set; }

        /// <summary>
        ///     Gets or sets the server time.
        /// </summary>
        /// <value>
        ///     The server time.
        /// </value>
        public string ServerTime { get; set; }

        /// <summary>
        ///     Gets or sets the showing status.
        /// </summary>
        /// <value>
        ///     The showing status.
        /// </value>
        public ProductShowingStatus ShowingStatus { get; set; }

        /// <summary>
        ///     Gets or sets the start sell time.
        /// </summary>
        /// <value>
        ///     The start sell time.
        /// </value>
        public string StartSellTime { get; set; }

        /// <summary>
        ///     Gets or sets the sum share count.
        /// </summary>
        /// <value>
        ///     The sum share count.
        /// </value>
        public int SumShareCount { get; set; }

        /// <summary>
        ///     Gets or sets the total interest
        /// </summary>
        /// <value>
        ///     The total interest.
        /// </value>
        public decimal TotalInterest { get; set; }

        /// <summary>
        ///     Gets or sets the total sale amount
        /// </summary>
        /// <value>
        ///     The total share amount.
        /// </value>
        public decimal TotalSaleAmount { get; set; }

        /// <summary>
        ///     Gets or sets the unit price.
        /// </summary>
        /// <value>
        ///     The unit price.
        /// </value>
        public decimal UnitPrice { get; set; }

        /// <summary>
        ///     Gets or sets the value date.
        /// </summary>
        /// <value>
        ///     The value date.
        /// </value>
        public string ValueDate { get; set; }

        /// <summary>
        ///     Gets or sets the value date mode.
        /// </summary>
        /// <value>
        ///     The value date mode.
        /// </value>
        public ValueDateMode ValueDateMode { get; set; }

        /// <summary>
        ///     Gets or sets the value date string.
        /// </summary>
        /// <value>
        ///     The value date string.
        /// </value>
        public string ValueDateString
        {
            get
            {
                switch (this.ValueDateMode)
                {
                    case ValueDateMode.T0:
                        return "购买成功立刻起息";

                    case ValueDateMode.T1:
                        return "购买成功下一工作日起息";

                    default:
                        return "";
                }
            }
        }

        /// <summary>
        ///     Gets or sets the yield.
        /// </summary>
        /// <value>
        ///     The yield.
        /// </value>
        public decimal Yield { get; set; }
    }

    internal static class ZCBProductWithSaleInfoExtensions
    {
        #region Internal Methods

        internal static ZCBProductInfoResponse ToZCBProductInfoResponse(this ProductWithSaleInfo<ZCBProductInfo> info)
        {
            ZCBProductInfoResponse product = new ZCBProductInfoResponse
            {
                EnableSale = info.ProductInfo.EnableSale,
                TotalInterest = info.ProductInfo.TotalInterest,
                TotalSaleAmount = info.ProductInfo.TotalSaleAmount,
                CurrentValueDate = info.ProductInfo.CurrentValueDate.ToMeowFormat(),
                EndSellTime = info.ProductInfo.EndSellTime.ToMeowFormat(),
                FinancingSum = info.ProductInfo.FinancingSum.ToIntFormat(),
                FinancingSumCount = info.ProductInfo.FinancingSumCount,
                LaunchTime = info.ProductInfo.LaunchTime.ToMeowFormat(),
                MaxShareCount = info.ProductInfo.MaxShareCount,
                MinShareCount = info.ProductInfo.MinShareCount,
                OnPreSale = info.ProductInfo.OnPreSale,
                OnSale = info.ProductInfo.OnSale,
                PreEndSellTime = info.ProductInfo.PreEndSellTime.ToMeowFormat(),
                PreSale = info.ProductInfo.PreSale,
                PreStartSellTime = info.ProductInfo.PreStartSellTime.ToMeowFormat(),
                ProductIdentifier = info.ProductInfo.ProductIdentifier,
                ProductName = info.ProductInfo.ProductName,
                ProductNo = info.ProductInfo.ProductNo,
                ProductNumber = info.ProductInfo.ProductNumber,
                ServerTime = DateTime.Now.AddSeconds(2).ToMeowFormat(),
                StartSellTime = info.ProductInfo.StartSellTime.ToMeowFormat(),
                SumShareCount = info.SumShareCount,
                UnitPrice = info.ProductInfo.UnitPrice.ToIntFormat(),
                ValueDate = info.ProductInfo.ValueDate.ToMeowFormat(),
                ValueDateMode = ValueDateMode.T1,
                Yield = info.ProductInfo.Yield.RoundScale(1, 2),
                ProductCategory = info.ProductInfo.ProductCategory,
                ConsignmentAgreementName = info.ProductInfo.ConsignmentAgreementName,
                PledgeAgreementName = info.ProductInfo.PledgeAgreementName,
                PaidShareCount = info.PaidShareCount
            };
            product.ShowingStatus = ProductResponseHelper.GetProductShowingStatus(info.ProductInfo, info.AvailableShareCount, info.PayingShareCount);
            product.AvailableShareCount = product.ShowingStatus == ProductShowingStatus.Finished ? 0 : info.AvailableShareCount;
            return product;
        }

        #endregion Internal Methods
    }
}