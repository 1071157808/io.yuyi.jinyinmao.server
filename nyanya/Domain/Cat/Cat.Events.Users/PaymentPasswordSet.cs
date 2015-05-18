// FileInformation: nyanya/Cat.Events.Users/PaymentPasswordSet.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/01   10:17 AM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Users
{
    /// <summary>
    ///     支付密码设置
    /// </summary>
    public class PaymentPasswordSet : Event
    {
        /// <summary>
        ///     初始化<see cref="PaymentPasswordSet" />类的新实例. 
        /// </summary>
        public PaymentPasswordSet()
        {
        }
        /// <summary>
        ///     初始化<see cref="PaymentPasswordSet" />类的新实例. 
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="sourceType">源类型</param>
        public PaymentPasswordSet(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
        }
        /// <summary>
        ///     用户标识
        /// </summary>
        public string UserIdentifier { get; set; }
    }
    /// <summary>
    ///     支付密码设置（验证）
    /// </summary>
    public class PaymentPasswordSetValidator : AbstractValidator<PaymentPasswordSet>
    {
        /// <summary>
        ///     初始化<see cref="PaymentPasswordSetValidator" />类的新实例. 
        /// </summary>
        public PaymentPasswordSetValidator()
        {
            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
        }
    }
}