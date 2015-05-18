// FileInformation: nyanya/Cat.Events.Yilian/YilianPaymentRequestSended.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using System;
using Domian.Events;
using ServiceStack.FluentValidation;

namespace Cat.Events.Yilian
{
    /// <summary>
    ///     易联支付请求已发送
    /// </summary>
    public class YilianPaymentRequestSended : Event, IYilianPaymentResultEvent
    {
        /// <summary>
        ///     初始化 <see cref="YilianPaymentRequestSended" /> 类的新实例.
        /// </summary>
        /// <param name="requestIdentifier">请求标识.</param>
        /// <param name="sourceType">源类型</param>
        public YilianPaymentRequestSended(string requestIdentifier, Type sourceType)
            : base(requestIdentifier, sourceType)
        {
            this.RequestIdentifier = requestIdentifier;
        }

        /// <summary>
        ///    初始化 <see cref="YilianPaymentRequestSended" /> 类的新实例.
        /// </summary>
        private YilianPaymentRequestSended()
        {
        }
        /// <summary>
        ///     请求标识
        /// </summary>
        public string RequestIdentifier { get; set; }

        #region IYilianPaymentResultEvent Members
        /// <summary>
        ///     消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        ///     订单标识
        /// </summary>
        public string OrderIdentifier { get; set; }
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

        #endregion IYilianPaymentResultEvent Members
    }
    /// <summary>
    ///     易联支付请求已发送（验证）
    /// </summary>
    public class YilianPaymentRequestSendedValidator : AbstractValidator<YilianPaymentRequestSended>
    {
        /// <summary>
        ///     初始化 <see cref="YilianPaymentRequestSendedValidator" /> 类的新实例.
        /// </summary>
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