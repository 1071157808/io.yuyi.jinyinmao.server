// FileInformation: nyanya/Services.WebAPI.Common/ServerTimeHandler.cs
// CreatedTime: 2014/04/24   10:14 AM
// LastUpdatedTime: 2014/04/24   2:51 PM

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Services.WebAPI.Common.MessageHandlers
{
    public class ServerTimeHandler : DelegatingHandler
    {
        private const string ServerTimeHeader = "ServerTime";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(
                task =>
                {
                    HttpResponseMessage response;
                    try
                    {
                        response = task.Result;
                    }
                    catch (Exception e)
                    {
                        response = request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                    }
                    response.Headers.Add(ServerTimeHeader, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                    return response;
                }, cancellationToken);
        }
    }
}