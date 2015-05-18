// FileInformation: nyanya/Cat.Events.Yilian/YilianAuthRequestCallbackReceived.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using Domian.Events;
using Infrastructure.Lib.Utility;
using ServiceStack.FluentValidation;

namespace Cat.Events.Yilian
{
    /// <summary>
    ///     易联认证请求回调接收
    /// </summary>
    public class YilianAuthRequestCallbackReceived : Event
    {
        /// <summary>
        ///     初始化 <see cref="YilianAuthRequestCallbackReceived" /> 类的新实例.
        /// </summary>
        public YilianAuthRequestCallbackReceived()
        {
        }

        /// <summary>
        ///     初始化 <see cref="YilianAuthRequestCallbackReceived" /> 类的新实例.
        /// </summary>
        public YilianAuthRequestCallbackReceived(string reply)
            : base("YL" + GuidUtils.NewGuidString())
        {
            this.Reply = reply;
        }
        /// <summary>
        ///     响应
        /// </summary>
        public string Reply { get; set; }
    }
    /// <summary>
    ///     易联认证请求回调接收（验证）
    /// </summary>
    public class YilianAuthRequestCallbackReceivedValidator : AbstractValidator<YilianAuthRequestCallbackReceived>
    {
        /// <summary>
        ///     初始化 <see cref="YilianAuthRequestCallbackReceivedValidator" /> 类的新实例.
        /// </summary>
        public YilianAuthRequestCallbackReceivedValidator()
        {
            this.RuleFor(c => c.Reply).NotNull();
            this.RuleFor(c => c.Reply).NotEmpty();
        }
    }
}