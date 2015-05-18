// FileInformation: nyanya/Xingye.Events.Yilian/YilianPaymentRequestCallbackReceived.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using Domian.Events;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Yilian
{
    public class YilianPaymentRequestCallbackReceived : Event
    {
        /// <summary>
        ///     Initializes a new instance of the <c>Event</c> class.
        /// </summary>
        public YilianPaymentRequestCallbackReceived(string reply)
            : base("YL" + GuidUtils.NewGuidString())
        {
            this.Reply = reply;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianAuthRequestCallbackReceived" /> class.
        ///     Only for Serialization
        /// </summary>
        public YilianPaymentRequestCallbackReceived()
        {
        }

        public string Reply { get; set; }
    }

    public class YilianPaymentRequestCallbackReceivedValidator : AbstractValidator<YilianPaymentRequestCallbackReceived>
    {
        public YilianPaymentRequestCallbackReceivedValidator()
        {
            this.RuleFor(c => c.Reply).NotNull();
            this.RuleFor(c => c.Reply).NotEmpty();
        }
    }
}