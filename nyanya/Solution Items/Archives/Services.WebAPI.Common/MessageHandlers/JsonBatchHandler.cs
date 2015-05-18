// FileInformation: nyanya/Services.WebAPI.Common/JsonBatchHandler.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/07/26   6:34 PM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Batch;
using Infrastructure.Lib.Utility;

namespace Services.WebAPI.Common.MessageHandlers
{
    public class JsonBatchHandler : DefaultHttpBatchHandler
    {
        public JsonBatchHandler(HttpServer server)
            : base(server)
        {
            this.SupportedContentTypes.Add("text/json");
            this.SupportedContentTypes.Add("application/json");
            this.ExecutionOrder = BatchExecutionOrder.NonSequential;
        }

        public override async Task<HttpResponseMessage> CreateResponseMessageAsync(IList<HttpResponseMessage> responses,
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            List<JsonResponseMessage> jsonResponses = new List<JsonResponseMessage>();
            foreach (HttpResponseMessage subResponse in responses)
            {
                JsonResponseMessage jsonResponse = new JsonResponseMessage
                {
                    Code = (int)subResponse.StatusCode
                };
                // only add cookie to the header
                foreach (KeyValuePair<string, IEnumerable<string>> header in subResponse.Headers)
                {
                    jsonResponse.Headers.Add(header.Key, String.Join(",", header.Value));
                }
                if (subResponse.Content != null)
                {
                    jsonResponse.Body = await subResponse.Content.ReadAsStringAsync();
                    foreach (KeyValuePair<string, IEnumerable<string>> header in subResponse.Content.Headers)
                    {
                        jsonResponse.Headers.Add(header.Key, String.Join(",", header.Value));
                    }
                }
                jsonResponses.Add(jsonResponse);
            }

            return request.CreateResponse(HttpStatusCode.OK, jsonResponses);
        }

        public override async Task<IList<HttpRequestMessage>> ParseBatchRequestsAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            JsonRequestMessage[] jsonSubRequests = await request.Content.ReadAsAsync<JsonRequestMessage[]>(cancellationToken);

            // Only support for get for the moment
            IEnumerable<HttpRequestMessage> subRequests = jsonSubRequests.Where(r => r.Method.ToUpper() == "GET").Select(r =>
            {
                Uri subRequestUri = new Uri(request.RequestUri, "/" + r.RelativeUrl);
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod(r.Method), subRequestUri);

                // copy all http headers from request, and do not copy the content headers (do not support POST | PUT)
                HttpUtils.CopyRequestHeaders(request.Headers, httpRequestMessage.Headers);
                return httpRequestMessage;
            });
            return subRequests.ToList();
        }
    }
}