// FileInformation: nyanya/Cat.Events.Users/AddBankCardSucceeded.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/12   10:04 AM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Users
{
    public class AddBankCardSucceeded : Event, IAddedABankCard
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AddBankCardSucceeded" /> class.
        ///     Only for Serialization
        /// </summary>
        public AddBankCardSucceeded()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Event</c> class.
        /// </summary>
        /// <param name="userIdentifier">The source identifier.</param>
        /// <param name="sourceType">Type of the source.</param>
        public AddBankCardSucceeded(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
        }

        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string Cellphone { get; set; }

        public string UserIdentifier { get; set; }
    }

    public class AddBankCardSucceededValidator : AbstractValidator<AddBankCardSucceeded>
    {
        public AddBankCardSucceededValidator()
        {
            this.RuleFor(c => c.BankCardNo).NotNull();
            this.RuleFor(c => c.BankCardNo).NotEmpty();

            this.RuleFor(c => c.BankName).NotNull();
            this.RuleFor(c => c.BankName).NotEmpty();

            this.RuleFor(c => c.Cellphone).NotNull();
            this.RuleFor(c => c.Cellphone).NotEmpty();

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
        }
    }
}