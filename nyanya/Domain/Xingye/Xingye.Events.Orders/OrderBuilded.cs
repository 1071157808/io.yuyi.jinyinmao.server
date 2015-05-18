// FileInformation: nyanya/Xingye.Events.Orders/OrderBuilded.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   4:11 PM

using System;
using Domian.Events;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;
using Xingye.Commands.Orders;

namespace Xingye.Events.Orders
{
    public class OrderBuilded : Event, IAppliedForYilianPayment
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderBuilded" /> class.
        ///     Only for Serialization
        /// </summary>
        public OrderBuilded()
        {
        }

        public OrderBuilded(string orderIdentifier, Type sourceType)
            : base(orderIdentifier, sourceType)
        {
            this.OrderIdentifier = orderIdentifier;
        }

        public OrderType OrderType { get; set; }

        #region IAppliedForYilianPayment Members

        public decimal Amount { get; set; }

        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string Cellphone { get; set; }

        public string CityName { get; set; }

        public int CredentialCode { get; set; }

        public string CredentialNo { get; set; }

        public string OrderIdentifier { get; set; }

        public string ProductIdentifier { get; set; }

        public string ProductNo { get; set; }

        public string RealName { get; set; }

        public string SequenceNo { get; set; }

        public string UserIdentifier { get; set; }

        public Yilian.RequestType YilianType
        {
            get { return Yilian.RequestType.Order; }
        }

        #endregion IAppliedForYilianPayment Members
    }

    public class OrderBuildedValidator : AbstractValidator<OrderBuilded>
    {
        public OrderBuildedValidator()
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

            this.RuleFor(c => c.SequenceNo).NotNull();
            this.RuleFor(c => c.SequenceNo).NotEmpty();
            this.RuleFor(c => c.SequenceNo).Length(14);
        }
    }
}