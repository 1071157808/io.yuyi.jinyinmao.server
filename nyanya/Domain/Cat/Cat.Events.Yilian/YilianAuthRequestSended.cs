// FileInformation: nyanya/Cat.Events.Yilian/YilianAuthRequestSended.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Yilian
{
    /// <summary>
    ///     易联认证请求已发送
    /// </summary>
    public class YilianAuthRequestSended : Event, IYilianVerifyResultEvent
    {
        /// <summary>
        ///     初始化 <see cref="YilianAuthRequestSended" /> 类的新实例.
        /// </summary>
        public YilianAuthRequestSended()
        {
        }

        /// <summary>
        ///     初始化 <see cref="YilianAuthRequestSended" /> 类的新实例.
        /// </summary>
        /// <param name="reqeustIdentifier">请求标识.</param>
        /// <param name="sourceType">源类型.</param>
        public YilianAuthRequestSended(string reqeustIdentifier, Type sourceType)
            : base(reqeustIdentifier, sourceType)
        {
            this.RequestIdentifier = reqeustIdentifier;
        }
        /// <summary>
        ///     请求标识
        /// </summary>
        public string RequestIdentifier { get; set; }

        #region IYilianVerifyResultEvent Members
        /// <summary>
        ///     消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        ///     结果
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        ///     订单编号
        /// </summary>
        public string SequenceNo { get; set; }
        /// <summary>
        ///     用户标识
        /// </summary>
        public string UserIdentifier { get; set; }

        #endregion IYilianVerifyResultEvent Members
    }
    /// <summary>
    ///     易联认证请求已发送（验证）
    /// </summary>
    public class YilianAuthRequestSendedValidator : AbstractValidator<YilianAuthRequestSended>
    {
        /// <summary>
        ///      初始化 <see cref="YilianAuthRequestSendedValidator" /> 类的新实例.
        /// </summary>
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