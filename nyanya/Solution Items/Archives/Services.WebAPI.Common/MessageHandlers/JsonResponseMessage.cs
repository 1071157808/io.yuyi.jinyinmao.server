using System.Collections.Generic;

namespace Services.WebAPI.Common.MessageHandlers
{
    public class JsonResponseMessage
    {
        public string Body { get; set; }

        public int Code { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public JsonResponseMessage()
        {
            Headers = new Dictionary<string, string>();
        }
    }
}