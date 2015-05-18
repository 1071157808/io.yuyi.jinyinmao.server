// FileInformation: nyanya/Xingye.Events.Yilian/IYilianPaymentResultEvent.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   4:11 PM

namespace Xingye.Events.Yilian
{
    public interface IYilianPaymentResultEvent : IYilianResultEvent
    {
        string OrderIdentifier { get; set; }
    }
}