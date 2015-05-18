// FileInformation: nyanya/Cat.Events.Users/AppliedForAddBankCard.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/12   11:14 AM

using Domian.Events;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;
using System;

namespace Xingye.Events.Users
{
    public class AppliedForAddBankCard : Event, IAppliedForYilianAuth
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AppliedForAddBankCard" /> class.
        ///     Only for Serialization
        /// </summary>
        public AppliedForAddBankCard()
        {
        }

        public AppliedForAddBankCard(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
        }

        public int CredentialCode { get; set; }

        #region IAppliedForYilianAuth Members

        public decimal Amount
        {
            get { return new decimal(1.08); }
        }

        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string Cellphone { get; set; }

        public string CityName { get; set; }

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

        public Yilian.RequestType YilianType
        {
            get { return Yilian.RequestType.Auth; }
        }

        #endregion IAppliedForYilianAuth Members
    }

    public class AppliedForAddBankCardValidator : AbstractValidator<AppliedForAddBankCard>
    {
        public AppliedForAddBankCardValidator()
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

            this.RuleFor(c => c.RealName).NotNull();
            this.RuleFor(c => c.RealName).NotEmpty();

            this.RuleFor(c => c.CredentialNo).NotNull();
            this.RuleFor(c => c.CredentialNo).NotEmpty();

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();

            this.RuleFor(c => c.SequenceNo).NotNull();
            this.RuleFor(c => c.SequenceNo).NotEmpty();
        }
    }
}