using System;
using Domian.Commands;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Cat.Commands.Products
{
    /// <summary>
    /// 资产包产品上架请求类
    /// </summary>
    [Route("/ZCB/ProductMaterial")]
    public class LaunchZCBProduct : Command
    {
        /// <summary>
        ///     初始化<see cref="LaunchZCBProduct" />类的新实例.
        /// </summary>
        public LaunchZCBProduct()
            : base("JB")
        {
        }
        /// <summary>
        /// 是否开启售卖
        /// 0否 1是
        /// </summary>
        public int EnableSale { get; set; }

        /// <summary>
        /// 授权委托书
        /// </summary>
        public string ConsignmentAgreement { get; set; }

        /// <summary>
        /// 授权委托书名称
        /// </summary>
        public string ConsignmentAgreementName { get; set; }

        /// <summary>
        /// 票据大图链接
        /// </summary>
        public string EndorseImageLink { get; set; }

        /// <summary>
        /// 票据缩略图链接
        /// </summary>
        public string EndorseImageThumbnailLink { get; set; }

        /// <summary>
        /// 售卖结束时间
        /// </summary>
        public DateTime EndSellTime { get; set; }

        /// <summary>
        /// 融资总份数
        /// </summary>
        public int FinancingSumCount { get; set; }

        /// <summary>
        /// 单笔订单最大购买份数
        /// </summary>
        public int MaxShareCount { get; set; }

        /// <summary>
        /// 单笔订单最小购买份数
        /// </summary>
        public int MinShareCount { get; set; }

        /// <summary>
        /// 项目周期
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// 投资协议
        /// </summary>
        public string PledgeAgreement { get; set; }

        /// <summary>
        /// 投资协议名称
        /// </summary>
        public string PledgeAgreementName { get; set; }

        /// <summary>
        /// 提前购买结束时间
        /// </summary>
        public DateTime? PreEndSellTime { get; set; }

        /// <summary>
        /// 提前购买开始时间
        /// </summary>
        public DateTime? PreStartSellTime { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductNo { get; set; }

        /// <summary>
        /// 产品子编号
        /// </summary>
        public string SubProductNo { get; set; }

        /// <summary>
        /// 产品期数
        /// </summary>
        public int ProductNumber { get; set; }

        /// <summary>
        /// 最迟还款日期
        /// </summary>
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        /// 结息日
        /// </summary>
        public DateTime SettleDate { get; set; }

        /// <summary>
        /// 开售时间
        /// </summary>
        public DateTime StartSellTime { get; set; }

        /// <summary>
        /// 每一份的单价
        /// </summary>
        public int UnitPrice { get; set; }

        /// <summary>
        /// 固定起息日期
        /// </summary>
        public DateTime? ValueDate { get; set; }

        /// <summary>
        /// 起息方式
        /// 10购买当天起息 20指定日期起息 30其它
        /// </summary>
        public ValueDateMode ValueDateMode { get; set; }

        /// <summary>
        /// 收益率
        /// </summary>
        public decimal Yield { get; set; }

        /// <summary>
        /// 产品分类 （10金银猫产品 20富滇产品 30施秉金鼎产品）
        /// </summary>
        public ProductCategory ProductCategory { get; set; }

        /// <summary>
        /// 产品可取款额度
        /// </summary>
        public decimal PerRemainRedeemAmount { get; set; }
    }

    /// <summary>
    /// 资产包产品上架（验证）
    /// </summary>
    public class LaunchZCBProductValidator : AbstractValidator<LaunchZCBProduct>
    {
        /// <summary>
        ///     初始化<see cref="LaunchZCBProductValidator" />类的新实例.
        /// </summary>
        public LaunchZCBProductValidator()
        {

            this.RuleFor(c => c.ConsignmentAgreement).NotNull();
            this.RuleFor(c => c.ConsignmentAgreement).NotEmpty();

            this.RuleFor(c => c.ConsignmentAgreementName).NotNull();
            this.RuleFor(c => c.ConsignmentAgreementName).NotEmpty();

            this.RuleFor(c => c.EndorseImageLink).NotNull();
            this.RuleFor(c => c.EndorseImageLink).NotEmpty();

            this.RuleFor(c => c.EndorseImageThumbnailLink).NotNull();
            this.RuleFor(c => c.EndorseImageThumbnailLink).NotEmpty();

            this.RuleFor(c => c.PledgeAgreement).NotNull();
            this.RuleFor(c => c.PledgeAgreement).NotEmpty();

            this.RuleFor(c => c.PledgeAgreementName).NotNull();
            this.RuleFor(c => c.PledgeAgreementName).NotEmpty();

            this.RuleFor(c => c.ProductName).NotNull();
            this.RuleFor(c => c.ProductName).NotEmpty();

            this.RuleFor(c => c.ProductNo).NotNull();
            this.RuleFor(c => c.ProductNo).NotEmpty();
        }
    }
}
