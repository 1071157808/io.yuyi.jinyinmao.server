// FileInformation: nyanya/Xingye.Events.Users/SignUpPaymentFailed.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   10:42 AM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Users
{
    public class SignUpPaymentFailed : Event, IAddingBankCardFailed
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SignUpPaymentFailed" /> class.
        ///     Only for Serialization
        /// </summary>
        public SignUpPaymentFailed()
        {
        }

        public SignUpPaymentFailed(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
            this.Message = "身份信息错误";
        }

        #region IAddingBankCardFailed Members

        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string Cellphone { get; set; }

        public string Message { get; set; }

        public string UserIdentifier { get; set; }

        #endregion IAddingBankCardFailed Members
    }

    public class SignUpPaymentFailedValidator : AbstractValidator<SignUpPaymentFailed>
    {
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