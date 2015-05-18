// FileInformation: nyanya/Xingye.Events.Yilian/YilianAuthRequestCallbackProcessed.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Yilian
{
    public class YilianAuthRequestCallbackProcessed : Event, IYilianVerifyResultEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianAuthRequestCallbackProcessed" /> class.
        ///     Only for Serialization
        /// </summary>
        public YilianAuthRequestCallbackProcessed()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Event</c> class.
        /// </summary>
        /// <param name="reqeustIdentifier">The reqeust identifier.</param>
        /// <param name="sourceType">Type of the source.</param>
        public YilianAuthRequestCallbackProcessed(string reqeustIdentifier, Type sourceType)
            : base(reqeustIdentifier, sourceType)
        {
            this.RequestIdentifier = reqeustIdentifier;
        }

        public string RequestIdentifier { get; set; }

        #region IYilianVerifyResultEvent Members

        public string Message { get; set; }

        public bool Result { get; set; }

        public string SequenceNo { get; set; }

        public string UserIdentifier { get; set; }

        #endregion IYilianVerifyResultEvent Members
    }

    public class YilianAuthRequestCallbackProcessedValidator : AbstractValidator<YilianAuthRequestCallbackProcessed>
    {
        public YilianAuthRequestCallbackProcessedValidator()
        {
            this.RuleFor(c => c.RequestIdentifier).NotNull();
            this.RuleFor(c => c.RequestIdentifier).NotEmpty();

            this.RuleFor(c => c.SequenceNo).NotNull();
            this.RuleFor(c => c.SequenceNo).NotEmpty();

            this.RuleFor(c => c.UserIdentifier).NotNull();
            this.RuleFor(c => c.UserIdentifier).NotEmpty();
        }
    }
}