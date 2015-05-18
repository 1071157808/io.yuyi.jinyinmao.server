// FileInformation: nyanya/Cat.Events.Users/AddBankCardSucceeded.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/12   10:04 AM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Users
{
    /// <summary>
    ///     添加银行卡成功
    /// </summary>
    public class AddBankCardSucceeded : Event, IAddedABankCard
    {
        /// <summary>
        ///     初始化<see cref="AddBankCardSucceeded" />类的新实例. 
        /// </summary>
        public AddBankCardSucceeded()
        {
        }

        /// <summary>
        ///     初始化<see cref="AddBankCardSucceeded" />类的新实例. 
        /// </summary>
        /// <param name="userIdentifier">用户标识.</param>
        /// <param name="sourceType">源类型.</param>
        public AddBankCardSucceeded(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
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
        ///     用户标识
        /// </summary>
        public string UserIdentifier { get; set; }
    }
    /// <summary>
    ///     添加银行卡成功（验证）
    /// </summary>
    public class AddBankCardSucceededValidator : AbstractValidator<AddBankCardSucceeded>
    {
        /// <summary>
        ///     初始化<see cref="AddBankCardSucceededValidator" />类的新实例. 
        /// </summary>
        public AddBankCardSucceededValidator()
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