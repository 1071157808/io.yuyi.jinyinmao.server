// FileInformation: nyanya/Xingye.Events.Yilian/YilianQueryAuthRequestProcessed.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Yilian
{
    public class YilianQueryAuthRequestProcessed : Event, IYilianVerifyResultEvent
    {
        public YilianQueryAuthRequestProcessed()
        {
        }

        public YilianQueryAuthRequestProcessed(string reqeustIdentifier, Type sourceType)
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

    public class YilianQueryAuthRequestProcessedValidator : AbstractValidator<YilianQueryAuthRequestProcessed>
    {
        public YilianQueryAuthRequestProcessedValidator()
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