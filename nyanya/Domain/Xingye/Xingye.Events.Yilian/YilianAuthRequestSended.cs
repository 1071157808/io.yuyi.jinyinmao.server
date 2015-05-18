// FileInformation: nyanya/Xingye.Events.Yilian/YilianAuthRequestSended.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Yilian
{
    public class YilianAuthRequestSended : Event, IYilianVerifyResultEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianAuthRequestSended" /> class.
        ///     Only for Serialization
        /// </summary>
        public YilianAuthRequestSended()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Event</c> class.
        /// </summary>
        /// <param name="reqeustIdentifier">The reqeust identifier.</param>
        /// <param name="sourceType">Type of the source.</param>
        public YilianAuthRequestSended(string reqeustIdentifier, Type sourceType)
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

    public class YilianAuthRequestSendedValidator : AbstractValidator<YilianAuthRequestSended>
    {
        public YilianAuthRequestSendedValidator()
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