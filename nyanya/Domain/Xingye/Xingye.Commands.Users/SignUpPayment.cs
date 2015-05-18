// FileInformation: nyanya/Cat.Commands.Users/SignUpPayment.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:23 PM

using Domian.Commands;
using Infrastructure.Lib.Utility;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Xingye.Commands.Users
{
    [Route("/SignUpPayment")]
    public class SignUpPayment : Command
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SignUpPayment" /> class.
        ///     Only for Serialization
        /// </summary>
        public SignUpPayment()
        {
        }

        public SignUpPayment(string identifier)
            : base("USER_" + identifier)
        {
            this.UserIdentifier = identifier;
        }

        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string Cellphone { get; set; }

        public string CityName { get; set; }

        public Credential Credential { get; set; }

        public string CredentialNo { get; set; }

        public string RealName { get; set; }

        public string UserIdentifier { get; set; }
    }

    public class SignUpPaymentValidator : AbstractValidator<SignUpPayment>
    {
        public SignUpPaymentValidator()
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
            this.RuleFor(c => c.CityName).Length(1, 20);

            this.RuleFor(c => c.Credential).NotNull();

            this.RuleFor(c => c.RealName).NotNull();
            this.RuleFor(c => c.RealName).NotEmpty();
            this.RuleFor(c => c.RealName).Length(1, 20);

            this.RuleFor(c => c.CredentialNo).NotNull();
            this.RuleFor(c => c.CredentialNo).NotEmpty();
            this.RuleFor(c => c.CredentialNo).Length(1, 50);

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
            this.RuleFor(c => c.UserIdentifier).Length(10, 50);
        }
    }
}