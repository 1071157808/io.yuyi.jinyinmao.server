// FileInformation: nyanya/nyanya.Xingye/ProductInfoResponse.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   2:07 PM

using System;
using Xingye.Commands.Products;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     用于显示的状态
    /// </summary>
    public enum ProductShowingStatus
    {
        /// <summary>
        ///     The before sale
        /// </summary>
        BeforeSale = 10,

        /// <summary>
        ///     The on pre sale
        /// </summary>
        OnPreSale = 20,

        /// <summary>
        ///     The on sale
        /// </summary>
        OnSale = 30,

        /// <summary>
        ///     The sold out
        /// </summary>
        SoldOut = 40,

        /// <summary>
        ///     The finished
        /// </summary>
        Finished = 50
    }

    /// <summary>
    ///     产品信息
    /// </summary>
    public class ProductInfoResponse
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the available share count.
        /// </summary>
        /// <value>
        ///     The available share count.
        /// </value>
        public int AvailableShareCount { get; set; }

        /// <summary>
        ///     Gets or sets the current value date.
        /// </summary>
        /// <value>
        ///     The current value date.
        /// </value>
        public string CurrentValueDate { get; set; }

        /// <summary>
        ///     Gets or sets the endorse image link.
        /// </summary>
        /// <value>
        ///     The endorse image link.
        /// </value>
        public string EndorseImageLink { get; set; }

        /// <summary>
        ///     Gets or sets the endorse image thumbnail link.
        /// </summary>
        /// <value>
        ///     The endorse image thumbnail link.
        /// </value>
        public string EndorseImageThumbnailLink { get; set; }

        /// <summary>
        ///     Gets or sets the end sell time.
        /// </summary>
        /// <value>
        ///     The end sell time.
        /// </value>
        public string EndSellTime { get; set; }

        /// <summary>
        ///     额外收益率
        /// </summary>
        public decimal ExtraYield
        {
            get { return 0; }
        }

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

        /// <summary>
        ///     Gets or sets the paid share count.
        /// </summary>
        /// <value>
        ///     The paid share count.
        /// </value>
        public int PaidShareCount { get; set; }

        /// <summary>
        ///     Gets or sets the paying share count.
        /// </summary>
        /// <value>
        ///     The paying share count.
        /// </value>
        public int PayingShareCount { get; set; }

        /// <summary>
        ///     Gets or sets the period.
        /// </summary>
        /// <value>
        ///     The period.
        /// </value>
        public int Period { get; set; }

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
        ///     Gets or sets a value indicating whether this <see cref="BAProductInfoResponse" /> is repaid.
        /// </summary>
        /// <value>
        ///     <c>true</c> if repaid; otherwise, <c>false</c>.
        /// </value>
        public bool Repaid { get; set; }

        /// <summary>
        ///     Gets or sets the repayment deadline.
        /// </summary>
        /// <value>
        ///     The repayment deadline.
        /// </value>
        public string RepaymentDeadline { get; set; }

        /// <summary>
        ///     Gets or sets the server time.
        /// </summary>
        /// <value>
        ///     The server time.
        /// </value>
        public string ServerTime { get; set; }

        /// <summary>
        ///     Gets or sets the settle date.
        /// </summary>
        /// <value>
        ///     The settle date.
        /// </value>
        public string SettleDate { get; set; }

        /// <summary>
        ///     Gets or sets the showing status.
        /// </summary>
        /// <value>
        ///     The showing status.
        /// </value>
        public ProductShowingStatus ShowingStatus { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [sold out].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [sold out]; otherwise, <c>false</c>.
        /// </value>
        public bool SoldOut { get; set; }

        /// <summary>
        ///     Gets or sets the sold out time.
        /// </summary>
        /// <value>
        ///     The sold out time.
        /// </value>
        public string SoldOutTime { get; set; }

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

        #endregion Public Properties
    }
}