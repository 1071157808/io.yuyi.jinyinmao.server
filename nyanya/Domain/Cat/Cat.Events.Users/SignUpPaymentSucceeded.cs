// FileInformation: nyanya/Cat.Events.Users/SignUpPaymentSucceeded.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Users
{
    /// <summary>
    ///     注册支付成功
    /// </summary>
    public class SignUpPaymentSucceeded : Event, IAddedABankCard
    {
        /// <summary>
        ///     初始化<see cref="AddBankCardSucceeded" />类的新实例. 
        /// </summary>
        public SignUpPaymentSucceeded()
        {
        }

        /// <summary>
        ///     初始化<see cref="AddBankCardSucceeded" />类的新实例. 
        /// </summary>
        /// <param name="userIdentifier">用户标识.</param>
        /// <param name="sourceType">源类型.</param>
        public SignUpPaymentSucceeded(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
        }

        #region IAddedABankCard Members
        /// <summary>
        ///     银行卡号
        /// </summary>
        public string BankCardNo { get; set; }
        /// <summary>
        ///     银行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        ///     手机号
        /// </summary>
        public string Cellphone { get; set; }
        /// <summary>
        ///     用户标识
        /// </summary>
        public string UserIdentifier { get; set; }

        #endregion IAddedABankCard Members
    }
    /// <summary>
    ///     注册支付成功（验证）
    /// </summary>
    public class SignUpPaymentSucceededValidator : AbstractValidator<SignUpPaymentSucceeded>
    {
        /// <summary>
        ///     初始化<see cref="SignUpPaymentSucceededValidator" />类的新实例. 
        /// </summary>
        public SignUpPaymentSucceededValidator()
        {
            this.RuleFor(c => c.BankCardNo).NotNull();
            this.RuleFor(c => c.BankCardNo).NotEmpty();

            this.RuleFor(c => c.BankName).NotNull();
            this.RuleFor(c => c.BankName).NotEmpty();

            this.RuleFor(c => c.Cellphone).NotNull();
            this.RuleFor(c => c.Cellphone).NotEmpty();

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
        }
    }
}