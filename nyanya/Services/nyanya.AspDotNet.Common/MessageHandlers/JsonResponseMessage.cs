using System.Collections.Generic;

namespace nyanya.AspDotNet.Common.MessageHandlers
{
    public class JsonResponseMessage
    {
        public JsonResponseMessage()
        {
            Headers = new Dictionary<string, string>();
        }

        public string Body { get; set; }

        public int Code { get; set; }

        public Dictionary<string, string> Headers { get; set; }
    }
}