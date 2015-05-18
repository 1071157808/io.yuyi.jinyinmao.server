using System;
using Domian.Models;

namespace Cat.Domain.Products.Models
{
    /// <summary>
    /// 资产包更新记录表
    /// </summary>
    public class ZCBHistory : IValueObject
    {
        public int Id { get; set; }
        /// <summary>
        /// 产品唯一标示符
        /// </summary>
        public string ProductIdentifier { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductNo { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 融资总份数
        /// </summary>
        public int FinancingSumCount { get; set; }

        /// <summary>
        /// 产品子编号
        /// </summary>
        public string SubProductNo { get; set; }

        /// <summary>
        /// 每一份的单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 下次开售时间
        /// </summary>
        public DateTime NextStartSellTime { get; set; }

        /// <summary>
        /// 下次售卖结束时间
        /// </summary>
        public DateTime NextEndSellTime { get; set; }

        /// <summary>
        /// 下次售卖年化收益
        /// </summary>
        public decimal NextYield { get; set; }

        /// <summary>
        /// 是否开启售卖（0否 1是）
        /// </summary>
        public int EnableSale { get; set; }

        /// <summary>
        /// 投资协议名称
        /// </summary>
        public string PledgeAgreementName { get; set; }

        /// <summary>
        /// 投资协议
        /// </summary>
        public string PledgeAgreement { get; set; }

        /// <summary>
        /// 授权委托书名称
        /// </summary>
        public string ConsignmentAgreementName { get; set; }

        /// <summary>
        /// 授权委托书
        /// </summary>
        public string ConsignmentAgreement { get; set; }

        /// <summary>
        /// 产品下次可取款额度
        /// </summary>
        public decimal PerRemainRedeemAmount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
