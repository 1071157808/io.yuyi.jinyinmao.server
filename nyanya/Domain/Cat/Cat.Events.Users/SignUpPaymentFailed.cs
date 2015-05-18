// FileInformation: nyanya/Cat.Events.Users/SignUpPaymentFailed.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/15   5:21 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Users
{
    /// <summary>
    ///     注册支付失败
    /// </summary>
    public class SignUpPaymentFailed : Event, IAddingBankCardFailed
    {
        /// <summary>
        ///     初始化<see cref="SignUpPaymentFailed" />类的新实例. 
        /// </summary>
        public SignUpPaymentFailed()
        {
        }
        /// <summary>
        ///     初始化<see cref="SignUpPaymentFailed" />类的新实例. 
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="sourceType">源类型</param>
        public SignUpPaymentFailed(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
            this.Message = "身份信息错误";
        }
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
        ///     注册支付失败消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        ///     用户标识
        /// </summary>
        public string UserIdentifier { get; set; }
    }
    /// <summary>
    ///     注册支付失败（验证）
    /// </summary>
    public class SignUpPaymentFailedValidator : AbstractValidator<SignUpPaymentFailed>
    {
        /// <summary>
        ///     初始化<see cref="SignUpPaymentFailedValidator" />类的新实例. 
        /// </summary>
        public SignUpPaymentFailedValidator()
        {
            this.RuleFor(c => c.BankCardNo).NotNull();
            this.RuleFor(c => c.BankCardNo).NotEmpty();

            this.RuleFor(c => c.BankName).NotNull();
            this.RuleFor(c => c.BankName).NotEmpty();

            this.RuleFor(c => c.Cellphone).NotNull();
            this.RuleFor(c => c.Cellphone).NotEmpty();

            this.RuleFor(c => c.Message).NotNull();
            this.RuleFor(c => c.Message).NotEmpty();

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
        }
    }
}