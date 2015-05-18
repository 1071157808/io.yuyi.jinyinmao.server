// FileInformation: nyanya/Cat.Commands.Users/SetPaymentPassword.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:23 PM

using Domian.Commands;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;

namespace Xingye.Commands.Users
{
    public class SetPaymentPassword : Command
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SetPaymentPassword" /> class.
        ///     Only for Serialization
        /// </summary>
        public SetPaymentPassword()
        {
        }

        public SetPaymentPassword(string identifier)
            : base("USER_" + identifier)
        {
            this.UserIdentifier = identifier;
        }

        public string PaymentPassword { get; set; }

        public string UserIdentifier { get; set; }
    }

    public class SetPaymentPasswordValidator : AbstractValidator<SetPaymentPassword>
    {
        public SetPaymentPasswordValidator()
        {
            this.RuleFor(c => c.PaymentPassword).NotNull();
            this.RuleFor(c => c.PaymentPassword).NotEmpty();
            this.RuleFor(c => c.PaymentPassword).Matches(RegexUtils.PaymentPasswordRegex.ToString());

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
        }
    }
}