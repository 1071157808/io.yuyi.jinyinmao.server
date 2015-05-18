// FileInformation: nyanya/Xingye.Events.Users/SignUpPaymentSucceeded.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   10:42 AM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Users
{
    public class SignUpPaymentSucceeded : Event, IAddedABankCard
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SignUpPaymentSucceeded" /> class.
        ///     Only for Serialization
        /// </summary>
        public SignUpPaymentSucceeded()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SignUpPaymentSucceeded" /> class.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="sourceType">Type of the source.</param>
        public SignUpPaymentSucceeded(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
        }

        #region IAddedABankCard Members

        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string Cellphone { get; set; }

        public string UserIdentifier { get; set; }

        #endregion IAddedABankCard Members
    }

    public class SignUpPaymentSucceededValidator : AbstractValidator<SignUpPaymentSucceeded>
    {
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