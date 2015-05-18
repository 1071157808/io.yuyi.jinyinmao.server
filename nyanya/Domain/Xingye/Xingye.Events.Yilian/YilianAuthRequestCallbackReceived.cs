// FileInformation: nyanya/Xingye.Events.Yilian/YilianAuthRequestCallbackReceived.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using Domian.Events;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;

namespace Xingye.Events.Yilian
{
    public class YilianAuthRequestCallbackReceived : Event
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianAuthRequestCallbackReceived" /> class.
        ///     Only for Serialization
        /// </summary>
        public YilianAuthRequestCallbackReceived()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Event</c> class.
        /// </summary>
        public YilianAuthRequestCallbackReceived(string reply)
            : base("YL" + GuidUtils.NewGuidString())
        {
            this.Reply = reply;
        }

        public string Reply { get; set; }
    }

    public class YilianAuthRequestCallbackReceivedValidator : AbstractValidator<YilianAuthRequestCallbackReceived>
    {
        public YilianAuthRequestCallbackReceivedValidator()
        {
            this.RuleFor(c => c.Reply).NotNull();
            this.RuleFor(c => c.Reply).NotEmpty();
        }
    }
}