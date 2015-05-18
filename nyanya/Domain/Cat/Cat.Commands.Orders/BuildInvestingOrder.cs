// FileInformation: nyanya/Cat.Commands.Orders/BuildInvestingOrder.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using Cat.Commands.Products;
using Cat.Commands.Users;
using Domian.Commands;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;

namespace Cat.Commands.Orders
{
    /// <summary>
    /// 生成投资理财产品请求类
    /// </summary>
    [Route("/BuildInvestingOrder")]
    public class BuildInvestingOrder : Command
    {
        /// <summary>
        ///     初始化<see cref="BuildInvestingOrder" />类的新实例.
        /// </summary>
        public BuildInvestingOrder()
        {
        }

        /// <summary>
        ///     初始化<see cref="BuildInvestingOrder" />类的新实例.
        /// </summary>
        /// <param name="userIdentifier">用户唯一标示符</param>
        /// <param name="orderIdentifer">订单唯一标示符</param>
        public BuildInvestingOrder(string userIdentifier, string orderIdentifer)
            : base("USER_" + userIdentifier)
        {
            this.UserIdentifier = userIdentifier;
            this.OrderIdentifier = orderIdentifer;
        }

        /// <summary>
        /// 参加活动编号
        /// </summary>
        public int ActivityNo { get; set; }

        public long ClientType { get; set; }

        /// <summary>
        /// 产品票据大图链接
        /// </summary>
        public string EndorseImageLink { get; set; }

        /// <summary>
        /// 产品票据缩略图链接
        /// </summary>
        public string EndorseImageThumbnailLink { get; set; }

        public long FlgMoreI1 { get; set; }

        public long FlgMoreI2 { get; set; }

        public string FlgMoreS1 { get; set; }

        public string FlgMoreS2 { get; set; }

        /// <summary>
        /// 投资者所用银行卡卡号
        /// </summary>
        public string InvestorBankCardNo { get; set; }

        /// <summary>
        /// 投资者所用银行卡所属银行
        /// </summary>
        public string InvestorBankName { get; set; }

        /// <summary>
        /// 投资者手机号
        /// </summary>
        public string InvestorCellphone { get; set; }

        /// <summary>
        /// 投资者所用银行卡所属城市
        /// </summary>
        public string InvestorCity { get; set; }

        /// <summary>
        /// 投资者所用证件类型
        /// 0身份证 1护照 2台湾 3军官证
        /// </summary>
        public Credential InvestorCredential { get; set; }

        /// <summary>
        /// 投资者所用证件号
        /// </summary>
        public string InvestorCredentialNo { get; set; }

        /// <summary>
        /// 投资者真实姓名
        /// </summary>
        public string InvestorRealName { get; set; }

        public string IpClient { get; set; }

        /// <summary>
        /// 产品最大售卖份数
        /// </summary>
        public int MaxShareCount { get; set; }

        /// <summary>
        /// 产品最小售卖份数
        /// </summary>
        public int MinShareCount { get; set; }

        /// <summary>
        /// 订单唯一标示符
        /// </summary>
        public string OrderIdentifier { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 下订单时间
        /// </summary>
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 产品分类
        /// 10金银猫 20富滇 30施秉金鼎产品
        /// </summary>
        public ProductCategory ProductCategory { get; set; }

        /// <summary>
        /// 产品唯一标示符
        /// </summary>
        public string ProductIdentifier { get; set; }

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
        /// 产品类型
        /// 10银承 20商承 30资产包
        /// </summary>
        public ProductType ProductType { get; set; }

        /// <summary>
        /// 最迟还款日期
        /// </summary>
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        /// 结息日期
        /// </summary>
        public DateTime SettleDate { get; set; }

        /// <summary>
        /// 购买份数
        /// </summary>
        public int ShareCount { get; set; }

        /// <summary>
        /// 产品子编号（只有资产包产品有值）
        /// </summary>
        public string SubProductNo { get; set; }

        /// <summary>
        /// 产品单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 投资者唯一标示符
        /// </summary>
        public string UserIdentifier { get; set; }

        /// <summary>
        /// 起息日期
        /// </summary>
        public DateTime? ValueDate { get; set; }

        /// <summary>
        /// 起息方式
        /// 10购买当天起息 20指定日期起息 30其他
        /// </summary>
        public ValueDateMode ValueDateMode { get; set; }

        /// <summary>
        /// 产品年利化率
        /// </summary>
        public decimal Yield { get; set; }
    }

    /// <summary>
    /// 投资理财产品（验证）
    /// </summary>
    public class BuildInvestingOrderValidator : AbstractValidator<BuildInvestingOrder>
    {
        /// <summary>
        ///     初始化<see cref="BuildInvestingOrderValidator" />类的新实例.
        /// </summary>
        public BuildInvestingOrderValidator()
        {
            this.RuleFor(c => c.EndorseImageLink).NotNull();
            this.RuleFor(c => c.EndorseImageLink).NotEmpty();

            this.RuleFor(c => c.EndorseImageThumbnailLink).NotNull();
            this.RuleFor(c => c.EndorseImageThumbnailLink).NotEmpty();

            this.RuleFor(c => c.InvestorBankCardNo).NotNull();
            this.RuleFor(c => c.InvestorBankCardNo).NotEmpty();

            this.RuleFor(c => c.InvestorBankName).NotNull();
            this.RuleFor(c => c.InvestorBankName).NotEmpty();

            this.RuleFor(c => c.InvestorCellphone).NotNull();
            this.RuleFor(c => c.InvestorCellphone).NotEmpty();

            this.RuleFor(c => c.InvestorCity).NotNull();
            this.RuleFor(c => c.InvestorCity).NotEmpty();

            this.RuleFor(c => c.InvestorCredentialNo).NotNull();
            this.RuleFor(c => c.InvestorCredentialNo).NotEmpty();

            this.RuleFor(c => c.InvestorRealName).NotNull();
            this.RuleFor(c => c.InvestorRealName).NotEmpty();

            this.RuleFor(c => c.OrderIdentifier).NotNull();
            this.RuleFor(c => c.OrderIdentifier).NotEmpty();

            this.RuleFor(c => c.OrderNo).NotNull();
            this.RuleFor(c => c.OrderNo).NotEmpty();

            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();

            this.RuleFor(c => c.ProductName).NotNull();
            this.RuleFor(c => c.ProductName).NotEmpty();

            this.RuleFor(c => c.ProductNo).NotNull();
            this.RuleFor(c => c.ProductNo).NotEmpty();

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();

            this.RuleFor(c => c.ShareCount).GreaterThanOrEqualTo(c => c.MinShareCount);
            this.RuleFor(c => c.ShareCount).LessThanOrEqualTo(c => c.MaxShareCount);

            this.RuleFor(c => c.Yield).GreaterThan(Decimal.Zero);

            this.When(c => c.ValueDateMode == ValueDateMode.FixedDate, () => this.RuleFor(c => c.ValueDate).NotNull());
        }
    }
}
