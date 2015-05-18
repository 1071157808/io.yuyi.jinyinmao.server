// FileInformation: nyanya/Services.WebAPI.Common/JsonRequestMessage.cs
// CreatedTime: 2014/04/16   12:47 PM
// LastUpdatedTime: 2014/04/16   12:50 PM

namespace Services.WebAPI.Common.MessageHandlers
{
    public class JsonRequestMessage
    {
        public string Method { get; set; }

        public string RelativeUrl { get; set; }
    }
}