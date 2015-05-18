// FileInformation: nyanya/Xingye.Events.Yilian/YilianPaymentRequestCallbackProcessed.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Yilian
{
    public class YilianPaymentRequestCallbackProcessed : Event, IYilianPaymentResultEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <c>Event</c> class.
        /// </summary>
        /// <param name="reqeustIdentifier">The reqeust identifier.</param>
        /// <param name="sourceType">Type of the source.</param>
        public YilianPaymentRequestCallbackProcessed(string reqeustIdentifier, Type sourceType)
            : base(reqeustIdentifier, sourceType)
        {
            this.RequestIdentifier = reqeustIdentifier;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianAuthRequestCallbackProcessed" /> class.
        ///     Only for Serialization
        /// </summary>
        public YilianPaymentRequestCallbackProcessed()
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

    public class YilianPaymentRequestCallbackProcessedValidator : AbstractValidator<YilianPaymentRequestCallbackProcessed>
    {
        public YilianPaymentRequestCallbackProcessedValidator()
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