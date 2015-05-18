// FileInformation: nyanya/Cat.Events.Users/AddBankCardFailed.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/29   6:40 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Users
{
    /// <summary>
    ///     添加银行卡失败
    /// </summary>
    public class AddBankCardFailed : Event, IAddingBankCardFailed
    {
        /// <summary>
        ///     初始化<see cref="AddBankCardFailed" />类的新实例. 
        /// </summary>
        public AddBankCardFailed()
        {
        }

        /// <summary>
        ///     初始化<see cref="AddBankCardFailed" />类的新实例. 
        /// </summary>
        /// <param name="userIdentifier">用户标识.</param>
        /// <param name="sourceType">源类型.</param>
        public AddBankCardFailed(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
            this.Message = "银行卡信息错误";
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
        ///     添加银行卡失败消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        ///     用户标识
        /// </summary>
        public string UserIdentifier { get; set; }
    }
    /// <summary>
    ///     添加银行卡失败（验证）
    /// </summary>
    public class AddBankCardFailedValidator : AbstractValidator<AddBankCardFailed>
    {
        /// <summary>
        ///     初始化<see cref="AddBankCardFailedValidator" />类的新实例. 
        /// </summary>
        public AddBankCardFailedValidator()
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