// FileInformation: nyanya/Cat.Commands.Users/SetPaymentPassword.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:23 PM

using Domian.Commands;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;

namespace Cat.Commands.Users
{
    /// <summary>
    /// 设置支付密码请求类
    /// </summary>
    public class SetPaymentPassword : Command
    {
        /// <summary>
        ///     初始化<see cref="SetPaymentPassword" />类的新实例.
        /// </summary>
        public SetPaymentPassword()
        {
        }

        /// <summary>
        ///     初始化<see cref="SetPaymentPassword" />类的新实例.
        /// </summary>
        /// <param name="identifier">用户唯一标示符</param>
        public SetPaymentPassword(string identifier)
            : base("USER_" + identifier)
        {
            this.UserIdentifier = identifier;
        }

        /// <summary>
        /// 支付密码
        /// </summary>
        public string PaymentPassword { get; set; }

        /// <summary>
        /// 用户唯一标示符
        /// </summary>
        public string UserIdentifier { get; set; }
    }

    /// <summary>
    /// 设置支付密码验证类
    /// </summary>
    public class SetPaymentPasswordValidator : AbstractValidator<SetPaymentPassword>
    {
        /// <summary>
        ///     初始化<see cref="SetPaymentPasswordValidator" />类的新实例.
        /// </summary>
        public SetPaymentPasswordValidator()
        {
            this.RuleFor(c => c.PaymentPassword).NotNull();
            this.RuleFor(c => c.PaymentPassword).NotEmpty();
            this.RuleFor(c => c.PaymentPassword).Matches(RegexUtils.PaymentPasswordRegex.ToString());

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
        }
    }
}