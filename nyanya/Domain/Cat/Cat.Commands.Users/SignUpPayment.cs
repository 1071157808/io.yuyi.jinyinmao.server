// FileInformation: nyanya/Cat.Commands.Users/SignUpPayment.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:23 PM

using Domian.Commands;
using Infrastructure.Lib.Utility;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Cat.Commands.Users
{
    /// <summary>
    /// 开通快捷支付，绑定银行卡请求类
    /// </summary>
    [Route("/SignUpPayment")]
    public class SignUpPayment : Command
    {
        /// <summary>
        ///     初始化<see cref="SignUpPayment" />类的新实例.
        /// </summary>
        public SignUpPayment()
        {
        }

        /// <summary>
        ///     初始化<see cref="SignUpPayment" />类的新实例.
        /// </summary>
        /// <param name="identifier">用户唯一标示符</param>
        public SignUpPayment(string identifier)
            : base("USER_" + identifier)
        {
            this.UserIdentifier = identifier;
        }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        /// 银行卡所在城市
        /// </summary>
        public string CityName { get; set; }

        public long ClientType { get; set; }

        /// <summary>
        /// 证件号类型
        /// </summary>
        public Credential Credential { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string CredentialNo { get; set; }

        public long FlgMoreI1 { get; set; }

        public long FlgMoreI2 { get; set; }

        public string FlgMoreS1 { get; set; }

        public string FlgMoreS2 { get; set; }

        public string IpClient { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 用户唯一标示符
        /// </summary>
        public string UserIdentifier { get; set; }
    }

    /// <summary>
    /// 开通快捷支付，绑定银行卡验证类
    /// </summary>
    public class SignUpPaymentValidator : AbstractValidator<SignUpPayment>
    {
        /// <summary>
        ///     初始化<see cref="SignUpPaymentValidator" />类的新实例.
        /// </summary>
        public SignUpPaymentValidator()
        {
            this.RuleFor(c => c.BankCardNo).NotNull();
            this.RuleFor(c => c.BankCardNo).NotEmpty();
            this.RuleFor(c => c.BankCardNo).Length(15, 19);

            this.RuleFor(c => c.BankName).NotNull();
            this.RuleFor(c => c.BankName).NotEmpty();
            this.RuleFor(c => c.BankName).Length(1, 20);

            this.RuleFor(c => c.Cellphone).NotNull();
            this.RuleFor(c => c.Cellphone).NotEmpty();
            this.RuleFor(c => c.Cellphone).Matches(RegexUtils.CellphoneRegex.ToString());

            this.RuleFor(c => c.CityName).NotNull();
            this.RuleFor(c => c.CityName).NotEmpty();
            this.RuleFor(c => c.CityName).Length(1, 20);

            this.RuleFor(c => c.Credential).NotNull();

            this.RuleFor(c => c.RealName).NotNull();
            this.RuleFor(c => c.RealName).NotEmpty();
            this.RuleFor(c => c.RealName).Length(1, 20);

            this.RuleFor(c => c.CredentialNo).NotNull();
            this.RuleFor(c => c.CredentialNo).NotEmpty();
            this.RuleFor(c => c.CredentialNo).Length(1, 50);

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
            this.RuleFor(c => c.UserIdentifier).Length(10, 50);
        }
    }
}
