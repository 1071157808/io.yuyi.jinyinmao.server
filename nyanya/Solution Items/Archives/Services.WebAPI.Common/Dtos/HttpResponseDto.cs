// FileInformation: nyanya/Services.WebAPI.Common/HttpResponseDto.cs
// CreatedTime: 2014/04/15   10:46 AM
// LastUpdatedTime: 2014/04/15   2:03 PM

using Newtonsoft.Json.Linq;

namespace Services.WebAPI.Common.Dtos
{
    public class HttpResponseDto
    {
        public JObject Content { get; set; }

        public int HttpStatusCode { get; set; }
    }
}