// FileInformation: nyanya/Xingye.Commands.Users/TempRegisterANewUser.cs
// CreatedTime: 2014/09/03   14:19 PM
// LastUpdatedTime: 2014/09/03   14:19 PM

using Domian.Commands;
using Infrastructure.Lib.Utility;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Xingye.Commands.Users
{
    [Route("/TempRegisterANewUser")]
    public class TempRegisterANewUser : Command
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TempRegisterANewUser" /> class.
        ///     Only for Serialization
        /// </summary>
        public TempRegisterANewUser()
        {
        }

        public TempRegisterANewUser(string identifier)
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

    public class TempRegisterANewUserValidator : AbstractValidator<TempRegisterANewUser>
    {
        public TempRegisterANewUserValidator()
        {
            this.RuleFor(c => c.Cellphone).NotNull();
            this.RuleFor(c => c.Cellphone).NotEmpty();
            this.RuleFor(c => c.Cellphone).Matches(RegexUtils.CellphoneRegex.ToString());

            this.RuleFor(c => c.IdentificionCode).NotNull();
            this.RuleFor(c => c.IdentificionCode).NotEmpty();

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
        }
    }
}