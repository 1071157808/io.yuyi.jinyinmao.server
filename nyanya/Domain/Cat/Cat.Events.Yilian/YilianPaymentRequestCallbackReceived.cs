// FileInformation: nyanya/Cat.Events.Yilian/YilianPaymentRequestCallbackReceived.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using Domian.Events;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;

namespace Cat.Events.Yilian
{
    /// <summary>
    ///     易联支付请求接收
    /// </summary>
    public class YilianPaymentRequestCallbackReceived : Event
    {
        /// <summary>
        ///     初始化 <see cref="YilianPaymentRequestCallbackReceived" /> 类的新实例.
        /// </summary>
        public YilianPaymentRequestCallbackReceived(string reply)
            : base("YL" + GuidUtils.NewGuidString())
        {
            this.Reply = reply;
        }

        /// <summary>
        ///     初始化 <see cref="YilianPaymentRequestCallbackReceived" /> 类的新实例.
        /// </summary>
        public YilianPaymentRequestCallbackReceived()
        {
        }
        /// <summary>
        ///     响应
        /// </summary>
        public string Reply { get; set; }
    }
    /// <summary>
    ///     易联支付请求接收（验证）
    /// </summary>
    public class YilianPaymentRequestCallbackReceivedValidator : AbstractValidator<YilianPaymentRequestCallbackReceived>
    {
        /// <summary>
        ///     初始化 <see cref="YilianPaymentRequestCallbackReceivedValidator" /> 类的新实例.
        /// </summary>
        public YilianPaymentRequestCallbackReceivedValidator()
        {
            this.RuleFor(c => c.Reply).NotNull();
            this.RuleFor(c => c.Reply).NotEmpty();
        }
    }
}