// FileInformation: nyanya/Cqrs.Domain.Product/SaleInfo.cs
// CreatedTime: 2014/07/15   3:35 PM
// LastUpdatedTime: 2014/07/21   2:20 AM

namespace Xingye.Domain.Products.Models
{
    public class SaleInfo
    {
        /// <summary>
        ///     融资总份数
        /// </summary>
        public int FinancingSumCount { get; set; }

        /// <summary>
        /// 最大投资份数
        /// </summary>
        public int MaxShareCount { get; set; }

        /// <summary>
        /// 最小投资份数
        /// </summary>
        public int MinShareCount { get; set; }

        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     投资单价
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}