// FileInformation: nyanya/Xingye.Events.Yilian/YilianPaymentRequestSended.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Yilian
{
    public class YilianPaymentRequestSended : Event, IYilianPaymentResultEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianPaymentRequestSended" /> class.
        /// </summary>
        /// <param name="requestIdentifier">The request identifier.</param>
        /// <param name="sourceType">Type of the source.</param>
        public YilianPaymentRequestSended(string requestIdentifier, Type sourceType)
            : base(requestIdentifier, sourceType)
        {
            this.RequestIdentifier = requestIdentifier;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianPaymentRequestSended" /> class.
        ///     Only for Serialization
        /// </summary>
        private YilianPaymentRequestSended()
        {
        }

        public string RequestIdentifier { get; set; }

        #region IYilianPaymentResultEvent Members

        public string Message { get; set; }

        public string OrderIdentifier { get; set; }

        public bool Result { get; set; }

        public string SequenceNo { get; set; }

        public string UserIdentifier { get; set; }

        #endregion IYilianPaymentResultEvent Members
    }

    public class YilianPaymentRequestSendedValidator : AbstractValidator<YilianPaymentRequestSended>
    {
        public YilianPaymentRequestSendedValidator()
        {
            this.RuleFor(c => c.RequestIdentifier).NotNull();
            this.RuleFor(c => c.RequestIdentifier).NotEmpty();

            this.RuleFor(c => c.SequenceNo).NotNull();
            this.RuleFor(c => c.SequenceNo).NotEmpty();

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();

            this.RuleFor(c => c.OrderIdentifier).NotNull();
            this.RuleFor(c => c.OrderIdentifier).NotEmpty();
        }
    }
}