using System;
using System.Collections.Generic;

namespace Cat.Domain.Products.Models
{
    public partial class ZCBProduct : Product
    {
        /// <summary>
        /// 产品是否在售 1是 0否
        /// </summary>
        public int EnableSale { get; set; }

        /// <summary>
        /// 累计售出
        /// </summary>
        public decimal TotalSaleAmount { get; set; }

        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal TotalInterest { get; set; }

        /// <summary>
        /// 累计赎回金额
        /// </summary>
        public decimal TotalRedeemAmount { get; set; }

        /// <summary>
        /// 累计赎回收益
        /// </summary>
        public decimal TotalRedeemInterest { get; set; }

        /// <summary>
        /// 子项目编号
        /// </summary>
        public string SubProductNo { get; set; }

        /// <summary>
        /// 当日剩余取款金额
        /// </summary>
        public decimal PerRemainRedeemAmount { get; set; }

        /// <summary>
        /// 授权委托书名称
        /// </summary>
        public string ConsignmentAgreementName { get; set; }

        /// <summary>
        /// 投资协议名称
        /// </summary>
        public string PledgeAgreementName { get; set; }

        /// <summary>
        /// 产品更新记录
        /// </summary>
        public ICollection<ZCBHistory> ZCBHistorys { get; set; }
    }
}
