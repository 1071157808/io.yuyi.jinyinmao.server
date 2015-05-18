// FileInformation: nyanya/Cat.Commands.Users/RegisterANewUser.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:23 PM

using Domian.Commands;
using Infrastructure.Lib.Utility;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Xingye.Commands.Users
{
    [Route("/RegisterANewUser")]
    public class RegisterANewUser : Command
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisterANewUser" /> class.
        ///     Only for Serialization
        /// </summary>
        public RegisterANewUser()
        {
        }

        public RegisterANewUser(string identifier)
            : base("USER_" + identifier)
        {
            this.UserIdentifier = identifier;
        }

        public string Cellphone { get; set; }

        public string IdentificionCode { get; set; }

        public string Password { get; set; }

        public string UserIdentifier { get; set; }

        /// <summary>
        /// 用户类型 10金银猫 20兴业
        /// </summary>
        public int UserType { get; set; }
    }

    public class RegisterANewUserValidator : AbstractValidator<RegisterANewUser>
    {
        public RegisterANewUserValidator()
        {
            this.RuleFor(c => c.Cellphone).NotNull();
            this.RuleFor(c => c.Cellphone).NotEmpty();
            this.RuleFor(c => c.Cellphone).Matches(RegexUtils.CellphoneRegex.ToString());

            this.RuleFor(c => c.Password).NotNull();
            this.RuleFor(c => c.Password).NotEmpty();
            this.RuleFor(c => c.Password).Matches(RegexUtils.PasswordRegex.ToString());

            this.RuleFor(c => c.IdentificionCode).NotNull();
            this.RuleFor(c => c.IdentificionCode).NotEmpty();

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
        }
    }
}