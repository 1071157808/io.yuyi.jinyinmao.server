using Cat.Commands.Products;

namespace Cat.Domain.Products.ReadModels
{
    public class ZCBProductInfo : ProductInfo
    {
        /// <summary>
        ///     是否可以售卖 0否 1是
        /// </summary>
        public int EnableSale { get; set; }

        /// <summary>
        ///     累计已售卖份额
        /// </summary>
        public decimal TotalSaleAmount { get; set; }

        /// <summary>
        ///     累计总收益
        /// </summary>
        public decimal TotalInterest { get; set; }

        /// <summary>
        ///     累计赎回金额
        /// </summary>
        public decimal TotalRedeemAmount { get; set; }

        /// <summary>
        ///     累计赎回收益
        /// </summary>
        public decimal TotalRedeemInterest { get; set; }

        /// <summary>
        ///     当日可取款金额
        /// </summary>
        public decimal PerRemainRedeemAmount { get; set; }

        /// <summary>
        ///     授权委托书名称
        /// </summary>
        public string ConsignmentAgreementName { get; set; }

        /// <summary>
        ///     投资协议名称
        /// </summary>
        public string PledgeAgreementName { get; set; }
    }
}