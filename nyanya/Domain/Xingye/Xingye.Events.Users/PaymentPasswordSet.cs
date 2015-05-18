// FileInformation: nyanya/Xingye.Events.Users/PaymentPasswordSet.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   10:42 AM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Users
{
    public class PaymentPasswordSet : Event
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PaymentPasswordSet" /> class.
        ///     Only for Serialization
        /// </summary>
        public PaymentPasswordSet()
        {
        }

        public PaymentPasswordSet(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
        }

        public string UserIdentifier { get; set; }
    }

    public class PaymentPasswordSetValidator : AbstractValidator<PaymentPasswordSet>
    {
        public PaymentPasswordSetValidator()
        {
            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
        }
    }
}