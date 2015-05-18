// FileInformation: nyanya/Xingye.Events.Users/AddBankCardFailed.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   10:43 AM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Users
{
    public class AddBankCardFailed : Event, IAddingBankCardFailed
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AddBankCardFailed" /> class.
        ///     Only for Serialization
        /// </summary>
        public AddBankCardFailed()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Event</c> class.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="sourceType">Type of the source.</param>
        public AddBankCardFailed(string userIdentifier, Type sourceType)
            : base(userIdentifier, sourceType)
        {
            this.UserIdentifier = userIdentifier;
            this.Message = "银行卡信息错误";
        }

        #region IAddingBankCardFailed Members

        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string Cellphone { get; set; }

        public string Message { get; set; }

        public string UserIdentifier { get; set; }

        #endregion IAddingBankCardFailed Members
    }

    public class AddBankCardFailedValidator : AbstractValidator<AddBankCardFailed>
    {
        public AddBankCardFailedValidator()
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