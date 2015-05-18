// FileInformation: nyanya/Cat.Commands.Users/ChangeLoginPassword.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:23 PM

using Domian.Commands;
using Infrastructure.Lib.Utility;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Xingye.Commands.Users
{
    [Route("/ChangeLoginPassword")]
    public class ChangeLoginPassword : Command
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ChangeLoginPassword" /> class.
        ///     Only for Serialization
        /// </summary>
        public ChangeLoginPassword()
        {
        }

        public ChangeLoginPassword(string identifier)
            : base("USER_" + identifier)
        {
            this.UserIdentifier = identifier;
            this.Salt = identifier;
        }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string UserIdentifier { get; set; }
    }

    public class ChangeLoginPasswordValidator : AbstractValidator<ChangeLoginPassword>
    {
        public ChangeLoginPasswordValidator()
        {
            this.RuleFor(c => c.Password).NotNull();
            this.RuleFor(c => c.Password).NotEmpty();
            this.RuleFor(c => c.Password).Matches(RegexUtils.PasswordRegex.ToString());

            this.RuleFor(c => c.Salt).NotNull();
            this.RuleFor(c => c.Salt).NotEmpty();
            this.RuleFor(c => c.Salt).Length(10, 80);

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
            this.RuleFor(c => c.UserIdentifier).Length(10, 50);
        }
    }
}