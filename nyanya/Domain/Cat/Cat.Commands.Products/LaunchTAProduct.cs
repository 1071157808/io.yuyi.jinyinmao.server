// FileInformation: nyanya/Cat.Commands.Products/LaunchTAProduct.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/17   12:22 PM

using System;
using Domian.Commands;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Cat.Commands.Products
{
    /// <summary>
    /// 商承产品上架请求类
    /// </summary>
    [Route("/ProductMaterial/TA")]
    public class LaunchTAProduct : Command
    {
        /// <summary>
        ///     初始化<see cref="LaunchTAProduct" />类的新实例.
        /// </summary>
        public LaunchTAProduct()
            : base("JB")
        {
        }

        /// <summary>
        /// 编号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 委托协议
        /// </summary>
        public string ConsignmentAgreement { get; set; }

        /// <summary>
        /// 委托协议名称
        /// </summary>
        public string ConsignmentAgreementName { get; set; }

        /// <summary>
        /// 付款方
        /// </summary>
        public string Drawee { get; set; }

        /// <summary>
        /// 付款方信息
        /// </summary>
        public string DraweeInfo { get; set; }

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
        public string EnterpriseInfo { get; set; }

        /// <summary>
        /// 营业执照号码
        /// </summary>
        public string EnterpriseLicense { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 融资总份数
        /// </summary>
        public int FinancingSumCount { get; set; }

        /// <summary>
        /// 担保方式
        /// </summary>
        public GuaranteeMode GuaranteeMode { get; set; }

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
        /// 担保协议名称
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
        /// 产品期数
        /// </summary>
        public int ProductNumber { get; set; }

        /// <summary>
        /// 最迟还款日期
        /// </summary>
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        /// 担保方
        /// </summary>
        public string Securedparty { get; set; }

        /// <summary>
        /// 担保方信息（还款来源）
        /// </summary>
        public string SecuredpartyInfo { get; set; }

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
        /// 产品分类 （10金银猫 20富滇 30施秉金鼎产品）
        /// </summary>
        public ProductCategory ProductCategory { get; set; }
    }

    /// <summary>
    /// 商承产品上架（验证）
    /// </summary>
    public class LaunchTAProductValidator : AbstractValidator<LaunchTAProduct>
    {
        /// <summary>
        ///     初始化<see cref="LaunchTAProductValidator" />类的新实例.
        /// </summary>
        public LaunchTAProductValidator()
        {
            this.RuleFor(c => c.BillNo).NotNull();
            this.RuleFor(c => c.BillNo).NotEmpty();

            this.RuleFor(c => c.ConsignmentAgreement).NotNull();
            this.RuleFor(c => c.ConsignmentAgreement).NotEmpty();

            this.RuleFor(c => c.ConsignmentAgreementName).NotNull();
            this.RuleFor(c => c.ConsignmentAgreementName).NotEmpty();

            this.RuleFor(c => c.Drawee).NotNull();
            this.RuleFor(c => c.Drawee).NotEmpty();

            this.RuleFor(c => c.DraweeInfo).NotNull();
            this.RuleFor(c => c.DraweeInfo).NotEmpty();

            this.RuleFor(c => c.EndorseImageLink).NotNull();
            this.RuleFor(c => c.EndorseImageLink).NotEmpty();

            this.RuleFor(c => c.EndorseImageThumbnailLink).NotNull();
            this.RuleFor(c => c.EndorseImageThumbnailLink).NotEmpty();

            this.RuleFor(c => c.EnterpriseName).NotNull();
            this.RuleFor(c => c.EnterpriseName).NotEmpty();

            this.RuleFor(c => c.EnterpriseInfo).NotNull();
            this.RuleFor(c => c.EnterpriseInfo).NotEmpty();

            this.RuleFor(c => c.EnterpriseLicense).NotNull();
            this.RuleFor(c => c.EnterpriseLicense).NotEmpty();

            this.RuleFor(c => c.PledgeAgreementName).NotNull();
            this.RuleFor(c => c.PledgeAgreementName).NotEmpty();

            this.RuleFor(c => c.PledgeAgreement).NotNull();
            this.RuleFor(c => c.PledgeAgreement).NotEmpty();

            this.RuleFor(c => c.ProductName).NotNull();
            this.RuleFor(c => c.ProductName).NotEmpty();

            this.RuleFor(c => c.ProductNo).NotNull();
            this.RuleFor(c => c.ProductNo).NotEmpty();

            this.RuleFor(c => c.Securedparty).NotNull();
            this.RuleFor(c => c.Securedparty).NotEmpty();

            this.RuleFor(c => c.SecuredpartyInfo).NotNull();
            this.RuleFor(c => c.SecuredpartyInfo).NotEmpty();

            this.RuleFor(c => c.Usage).NotNull();
            this.RuleFor(c => c.Usage).NotEmpty();
        }
    }
}