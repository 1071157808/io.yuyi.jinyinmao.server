using System;
using Domian.Commands;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Cat.Commands.Products
{
    /// <summary>
    /// 资产包产品更新请求类
    /// </summary>
    [Route("/ZCB/UpdateShareCountShelves")]
    public class ZCBUpdateShareCount : Command
    {
        /// <summary>
        ///     初始化<see cref="ZCBUpdateShareCount" />类的新实例.
        /// </summary>
        public ZCBUpdateShareCount()
            : base("JB")
        {
        }
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
    }

    /// <summary>
    /// 资产包产品更新（验证）
    /// </summary>
    public class ZCBUpdateShareCountValidator : AbstractValidator<ZCBUpdateShareCount>
    {
        /// <summary>
        ///     初始化<see cref="ZCBUpdateShareCountValidator" />类的新实例.
        /// </summary>
        public ZCBUpdateShareCountValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();

            this.RuleFor(c => c.ProductNo).NotNull();
            this.RuleFor(c => c.ProductNo).NotEmpty();

            this.RuleFor(c => c.SubProductNo).NotNull();
            this.RuleFor(c => c.SubProductNo).NotEmpty();

            this.RuleFor(c => c.ProductName).NotNull();
            this.RuleFor(c => c.ProductName).NotEmpty();

            this.RuleFor(c => c.NextStartSellTime).NotNull();
            this.RuleFor(c => c.NextStartSellTime).NotEmpty();

            this.RuleFor(c => c.NextEndSellTime).NotNull();
            this.RuleFor(c => c.NextEndSellTime).NotEmpty();

            this.RuleFor(c => c.ConsignmentAgreementName).NotNull();
            this.RuleFor(c => c.ConsignmentAgreementName).NotEmpty();

            this.RuleFor(c => c.ConsignmentAgreement).NotNull();
            this.RuleFor(c => c.ConsignmentAgreement).NotEmpty();

            this.RuleFor(c => c.PledgeAgreementName).NotNull();
            this.RuleFor(c => c.PledgeAgreementName).NotEmpty();

            this.RuleFor(c => c.PledgeAgreement).NotNull();
            this.RuleFor(c => c.PledgeAgreement).NotEmpty();
        }
    }
}
