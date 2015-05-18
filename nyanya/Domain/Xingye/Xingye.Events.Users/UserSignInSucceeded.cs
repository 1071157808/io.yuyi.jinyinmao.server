// FileInformation: nyanya/Xingye.Events.Users/UserSignInSucceeded.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   10:42 AM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Users
{
    /// <summary>
    ///     UserSignInSucceeded
    /// </summary>
    public class UserSignInSucceeded : Event
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UserSignInSucceeded" /> class.
        ///     Only for Serialization
        /// </summary>
        public UserSignInSucceeded()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserSignInSucceeded" /> class.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="sourceType">Type of the source.</param>
        public UserSignInSucceeded(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
        }

        public string AmpAuthToken { get; set; }

        public string GoldCatAuthToken { get; set; }

        public string UserIdentifier { get; set; }
    }

    public class UserSignInSucceededValidator : AbstractValidator<UserSignInSucceeded>
    {
        public UserSignInSucceededValidator()
        {
            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
            this.RuleFor(c => c.AmpAuthToken).NotNull();
            this.RuleFor(c => c.AmpAuthToken).NotEmpty();
            this.RuleFor(c => c.GoldCatAuthToken).NotNull();
            this.RuleFor(c => c.GoldCatAuthToken).NotEmpty();
        }
    }
}