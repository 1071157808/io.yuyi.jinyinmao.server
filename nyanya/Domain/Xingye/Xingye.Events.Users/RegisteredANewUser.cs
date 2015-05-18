// FileInformation: nyanya/Xingye.Events.Users/RegisteredANewUser.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   10:42 AM

using System;
using Domian.Events;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Users
{
    public class RegisteredANewUser : Event
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisteredANewUser" /> class.
        ///     Only for Serialization
        /// </summary>
        public RegisteredANewUser()
        {
        }

        public RegisteredANewUser(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
        }

        public string Cellphone { get; set; }

        public string UserIdentifier { get; set; }
    }

    public class RegisteredANewUserValidator : AbstractValidator<RegisteredANewUser>
    {
        public RegisteredANewUserValidator()
        {
            this.RuleFor(c => c.Cellphone).NotNull();
            this.RuleFor(c => c.Cellphone).NotEmpty();
            this.RuleFor(c => c.Cellphone).Matches(RegexUtils.CellphoneRegex.ToString());

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
            this.RuleFor(c => c.UserIdentifier).Length(10, 50);
        }
    }
}