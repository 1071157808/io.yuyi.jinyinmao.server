// FileInformation: nyanya/Cat.Events.Yilian/IYilianPaymentResultEvent.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

namespace Cat.Events.Yilian
{
    /// <summary>
    ///     易联支付结果接口
    /// </summary>
    public interface IYilianPaymentResultEvent : IYilianResultEvent
    {
        /// <summary>
        ///     订单标识
        /// </summary>
        string OrderIdentifier { get; set; }
    }
}