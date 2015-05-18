// FileInformation: nyanya/Xingye.Events.Yilian/IYilianResultEvent.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

namespace Xingye.Events.Yilian
{
    public interface IYilianResultEvent
    {
        string Message { get; set; }

        bool Result { get; set; }

        string SequenceNo { get; set; }

        string UserIdentifier { get; set; }
    }
}