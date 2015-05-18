// FileInformation: nyanya/Xingye.Events.Users/PasswordChanged.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   10:42 AM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Users
{
    public class PasswordChanged : Event
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PasswordChanged" /> class.
        ///     Only for Serialization
        /// </summary>
        public PasswordChanged()
        {
        }

        public PasswordChanged(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
        }

        public string UserIdentifier { get; set; }
    }

    public class PasswordChangedValidator : AbstractValidator<PasswordChanged>
    {
        public PasswordChangedValidator()
        {
            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
            this.RuleFor(c => c.UserIdentifier).Length(10, 50);
        }
    }
}