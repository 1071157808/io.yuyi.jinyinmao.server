// FileInformation: nyanya/Xingye.Events.Users/AppliedForSignUpPayment.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   10:43 AM

using System;
using Domian.Events;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;
using Xingye.Events.Yilian;

namespace Xingye.Events.Users
{
    public class AppliedForSignUpPayment : Event, IAppliedForYilianAuth
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AppliedForSignUpPayment" /> class.
        ///     Only for Serialization
        /// </summary>
        public AppliedForSignUpPayment()
        {
        }

        public AppliedForSignUpPayment(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
        }

        #region IAppliedForYilianAuth Members

        public decimal Amount
        {
            get { return new decimal(1.08); }
        }

        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string Cellphone { get; set; }

        public string CityName { get; set; }

        public int CredentialCode { get; set; }

        public string CredentialNo { get; set; }

        public string OrderIdentifier
        {
            get { return ""; }
        }

        public string ProductIdentifier
        {
            get { return ""; }
        }

        public string ProductNo
        {
            get { return ""; }
        }

        public string RealName { get; set; }

        public string SequenceNo { get; set; }

        public string UserIdentifier { get; set; }
        public RequestType YilianType
        {
            get { return Yilian.RequestType.Auth; }
        }

        #endregion IAppliedForYilianAuth Members
    }

    public class AppliedForSignUpPaymentValidator : AbstractValidator<AppliedForSignUpPayment>
    {
        public AppliedForSignUpPaymentValidator()
        {
            this.RuleFor(c => c.BankCardNo).NotNull();
            this.RuleFor(c => c.BankCardNo).NotEmpty();

            this.RuleFor(c => c.BankName).NotNull();
            this.RuleFor(c => c.BankName).NotEmpty();

            this.RuleFor(c => c.Cellphone).NotNull();
            this.RuleFor(c => c.Cellphone).NotEmpty();
            this.RuleFor(c => c.Cellphone).Matches(RegexUtils.CellphoneRegex.ToString());

            this.RuleFor(c => c.CityName).NotNull();
            this.RuleFor(c => c.CityName).NotEmpty();

            this.RuleFor(c => c.RealName).NotNull();
            this.RuleFor(c => c.RealName).NotEmpty();

            this.RuleFor(c => c.CredentialNo).NotNull();
            this.RuleFor(c => c.CredentialNo).NotEmpty();

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();

            this.RuleFor(c => c.SequenceNo).NotNull();
            this.RuleFor(c => c.SequenceNo).NotEmpty();
            this.RuleFor(c => c.SequenceNo).Length(14);
        }
    }
}