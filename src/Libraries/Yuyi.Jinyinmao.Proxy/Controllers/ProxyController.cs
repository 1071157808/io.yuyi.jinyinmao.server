// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : ProxyController.cs
// Created          : 2015-07-02  11:00 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-29  4:21 PM
// ***********************************************************************
// <copyright file="ProxyController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Moe.AspNet.Utility;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Proxy.Controllers
{
    /// <summary>
    ///     ProxyController.
    /// </summary>
    public class ProxyController : ApiController
    {
        private static readonly string[] IgnoredRequestHeaders = { "Connection", "Content-Length", "Host" };

        /// <summary>
        ///     Proxies the specified target URL.
        /// </summary>
        /// <param name="targetUrl">The target URL.</param>
        /// <returns>System.Threading.Tasks.Task&lt;System.Web.Http.IHttpActionResult&gt;.</returns>
        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpOptions, Route("")]
        public async Task<IHttpActionResult> Proxy([FromUri] string targetUrl)
        {
            if (targetUrl.IsNullOrEmpty())
            {
                return this.BadRequest("Target url was not specified.");
            }

            string url = "";
            try
            {
                url = this.Request.Headers.GetValues("X-JYM-CorsProxy-Url")?.FirstOrDefault();
            }
            catch (Exception)
            {
                //ignore
            }
            if (url.IsNullOrEmpty())
            {
                return this.BadRequest("X-JYM-CorsProxy-Url was not specified.");
            }

            string ipHeader = HttpUtils.GetUserHostAddress(this.Request);
            string userAgentHeader = HttpUtils.GetUserAgent(this.Request);

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = this.Request.Method,
                Version = this.Request.Version,
                RequestUri = new Uri(targetUrl + this.Request.RequestUri.Query, UriKind.Relative)
            };

            if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
            {
                await this.Request.Content.LoadIntoBufferAsync();
                Stream contentStream = await this.Request.Content.ReadAsStreamAsync();
                contentStream.Position = 0;
                request.Content = new StreamContent(contentStream);

                foreach (KeyValuePair<string, IEnumerable<string>> header in this.Request.Content.Headers.Where(header => !IgnoredRequestHeaders.Contains(header.Key)))
                {
                    request.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            foreach (KeyValuePair<string, object> prop in request.Properties)
            {
                request.Properties.Add(prop);
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in this.Request.Headers.Where(header => !IgnoredRequestHeaders.Contains(header.Key)))
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            request.Headers.TryAddWithoutValidation("X-JYM-IP", ipHeader);
            request.Headers.TryAddWithoutValidation("X-JYM-UserAgent", userAgentHeader);

            using (HttpClientHandler handler = new HttpClientHandler { UseCookies = false })
            using (HttpClient client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                try
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                    response.Headers.Remove("Server");
                    response.Headers.Remove("X-AspNet-Version");
                    response.Headers.Remove("X-Powered-By");
                    response.Headers.TryAddWithoutValidation("X-Date", response.Headers.Date.GetValueOrDefault(new DateTimeOffset(DateTime.UtcNow)).ToString("R"));
                    return this.ResponseMessage(response);
                }
                catch (Exception e)
                {
                    return this.InternalServerError(e);
                }
            }
        }
    }
}