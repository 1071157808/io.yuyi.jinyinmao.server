// FileInformation: nyanya/Xingye.Events.Orders/OrderPaymentFailed.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   4:11 PM

using System;
using Domian.Events;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Orders
{
    public class OrderPaymentFailed : Event
    {
        public OrderPaymentFailed(string orderIdentifier, Type sourceType)
            : base(orderIdentifier, sourceType)
        {
            this.OrderIdentifier = orderIdentifier;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderPaymentFailed" /> class.
        ///     Only for Serialization
        /// </summary>
        public OrderPaymentFailed()
        {
        }

        public decimal Amount { get; set; }

        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string Cellphone { get; set; }

        public string CityName { get; set; }

        public int CredentialCode { get; set; }

        public string CredentialNo { get; set; }

        public string Message { get; set; }

        public string OrderIdentifier { get; set; }

        public string OrderNo { get; set; }

        public string ProductIdentifier { get; set; }

        public string ProductNo { get; set; }

        public string RealName { get; set; }

        public int ShareCount { get; set; }

        public string UserIdentifier { get; set; }
    }

    public class OrderPaymentFailedValidator : AbstractValidator<OrderPaymentFailed>
    {
        public OrderPaymentFailedValidator()
        {
            this.RuleFor(c => c.Amount).GreaterThan(new decimal(0));

            this.RuleFor(c => c.BankCardNo).NotNull();
            this.RuleFor(c => c.BankCardNo).NotEmpty();

            this.RuleFor(c => c.BankName).NotNull();
            this.RuleFor(c => c.BankName).NotEmpty();

            this.RuleFor(c => c.Cellphone).NotNull();
            this.RuleFor(c => c.Cellphone).NotEmpty();
            this.RuleFor(c => c.Cellphone).Matches(RegexUtils.CellphoneRegex.ToString());

            this.RuleFor(c => c.CityName).NotNull();
            this.RuleFor(c => c.CityName).NotEmpty();

            this.RuleFor(c => c.RealName).NotNull();
            this.RuleFor(c => c.RealName).NotEmpty();

            this.RuleFor(c => c.CredentialNo).NotNull();
            this.RuleFor(c => c.CredentialNo).NotEmpty();

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();

            this.RuleFor(c => c.OrderIdentifier).NotNull();
            this.RuleFor(c => c.OrderIdentifier).NotEmpty();

            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();

            this.RuleFor(c => c.ProductNo).NotNull();
            this.RuleFor(c => c.ProductNo).NotEmpty();

            this.RuleFor(c => c.OrderNo).NotNull();
            this.RuleFor(c => c.OrderNo).NotEmpty();
        }
    }
}