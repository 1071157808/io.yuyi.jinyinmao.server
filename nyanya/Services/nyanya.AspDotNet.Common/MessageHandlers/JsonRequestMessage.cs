// FileInformation: nyanya/nyanya.AspDotNet.Common/JsonRequestMessage.cs
// CreatedTime: 2014/04/16   12:47 PM
// LastUpdatedTime: 2014/04/16   12:50 PM

namespace nyanya.AspDotNet.Common.MessageHandlers
{
    public class JsonRequestMessage
    {
        public string Method { get; set; }

        public string RelativeUrl { get; set; }
    }
}