// FileInformation: nyanya/nyanya.AspDotNet.Common/HttpResponseDto.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:10 AM

using Newtonsoft.Json.Linq;

namespace nyanya.AspDotNet.Common.Dtos
{
    public class HttpResponseDto
    {
        public JObject Content { get; set; }

        public int HttpStatusCode { get; set; }
    }
}