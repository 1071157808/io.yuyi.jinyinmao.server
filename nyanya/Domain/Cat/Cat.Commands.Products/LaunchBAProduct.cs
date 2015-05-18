// FileInformation: nyanya/Cat.Commands.Products/LaunchBAProduct.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:23 PM

using System;
using Domian.Commands;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Cat.Commands.Products
{
    /// <summary>
    /// 担保类型
    /// </summary>
    public enum AgreementType
    {
        /// <summary>
        ///     担保公司
        /// </summary>
        Suretycompany = 10,

        /// <summary>
        ///     群体担保
        /// </summary>
        Groupguarantee = 20
    }

    /// <summary>
    /// 产品类型
    /// </summary>
    public enum ProductType
    {
        /// <summary>
        ///     银承
        /// </summary>
        BankAcceptance = 10,

        /// <summary>
        ///     商承
        /// </summary>
        TradeAcceptance = 20,

        /// <summary>
        ///     资产包
        /// </summary>
        ZCBAcceptance = 30
    }

    /// <summary>
    /// 产品分类
    /// </summary>
    public enum ProductCategory
    {
        /// <summary>
        /// 金银猫产品
        /// </summary>
        JINYINMAO = 10,

        /// <summary>
        /// 富滇产品
        /// </summary>
        FUDIAN = 20,

        /// <summary>
        /// 施秉金鼎产品
        /// </summary>
        SHIBING = 30,

        /// <summary>
        /// 阜新产品
        /// </summary>
        FUXIN = 40,

        /// <summary>
        /// 其它产品
        /// </summary>
        Other = 1
    }

    /// <summary>
    /// 起息方式
    /// </summary>
    public enum ValueDateMode
    {
        /// <summary>
        /// 购买当天起息
        /// </summary>
        T0 = 10,

        /// <summary>
        /// 指定日期起息
        /// </summary>
        T1 = 20,

        /// <summary>
        /// 其他
        /// </summary>
        FixedDate = 30
    }

    /// <summary>
    /// 银承产品上架请求类
    /// </summary>
    [Route("/BA/ProductMaterial")]
    public class LaunchBAProduct : Command
    {
        public LaunchBAProduct()
            : base("JB")
        {
        }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 银票编号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 营业执照号码
        /// </summary>
        public string BusinessLicense { get; set; }

        /// <summary>
        /// 委托协议
        /// </summary>
        public string ConsignmentAgreement { get; set; }

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
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

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
        /// 担保协议
        /// </summary>
        public string PledgeAgreement { get; set; }

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
        /// 融资用途
        /// </summary>
        public string Usage { get; set; }

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
    }

    /// <summary>
    /// 银承产品上架（验证）
    /// </summary>
    public class LaunchBAProductValidator : AbstractValidator<LaunchBAProduct>
    {
        /// <summary>
        ///     初始化<see cref="LaunchBAProductValidator" />类的新实例.
        /// </summary>
        public LaunchBAProductValidator()
        {
            this.RuleFor(c => c.BankName).NotNull();
            this.RuleFor(c => c.BankName).NotEmpty();

            this.RuleFor(c => c.BillNo).NotNull();
            this.RuleFor(c => c.BillNo).NotEmpty();

            this.RuleFor(c => c.BusinessLicense).NotNull();
            this.RuleFor(c => c.BusinessLicense).NotEmpty();

            this.RuleFor(c => c.ConsignmentAgreement).NotNull();
            this.RuleFor(c => c.ConsignmentAgreement).NotEmpty();

            this.RuleFor(c => c.EndorseImageLink).NotNull();
            this.RuleFor(c => c.EndorseImageLink).NotEmpty();

            this.RuleFor(c => c.EndorseImageThumbnailLink).NotNull();
            this.RuleFor(c => c.EndorseImageThumbnailLink).NotEmpty();

            this.RuleFor(c => c.EnterpriseName).NotNull();
            this.RuleFor(c => c.EnterpriseName).NotEmpty();

            this.RuleFor(c => c.PledgeAgreement).NotNull();
            this.RuleFor(c => c.PledgeAgreement).NotEmpty();

            this.RuleFor(c => c.ProductName).NotNull();
            this.RuleFor(c => c.ProductName).NotEmpty();

            this.RuleFor(c => c.ProductNo).NotNull();
            this.RuleFor(c => c.ProductNo).NotEmpty();

            this.RuleFor(c => c.Usage).NotNull();
            this.RuleFor(c => c.Usage).NotEmpty();
        }
    }
}