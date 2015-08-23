// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : HomeController.cs
// Created          : 2015-08-18  18:41
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-23  16:22
// ***********************************************************************
// <copyright file="HomeController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using Moe.AspNet.Utility;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Link.Models;
using Yuyi.Jinyinmao.Api.Link.Utils;

namespace Yuyi.Jinyinmao.Api.Link.Controllers
{
    /// <summary>
    ///     HomeController.
    /// </summary>
    public class HomeController : ApiController
    {
        /// <summary>
        ///     The link log table name
        /// </summary>
        private static readonly string LinkLogTableName = "LinkLogs";

        /// <summary>
        ///     The link table name
        /// </summary>
        private static readonly string LinkTableName = "Links";

        /// <summary>
        ///     The partition key
        /// </summary>
        private static readonly string PartitionKey = "ShortLink";

        /// <summary>
        ///     The response
        /// </summary>
        private static HttpResponseMessage response;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HomeController" /> class.
        /// </summary>
        public HomeController()
        {
            response = new HttpResponseMessage(HttpStatusCode.Found);
            response.Headers.Add("Cache-Control", "max-age=10800");
        }

        /// <summary>
        ///     Indexes this instance.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Index()
        {
            return this.Ok();
        }

        /// <summary>
        ///     Jumps the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [HttpGet]
        [Route("{url}")]
        public async Task<IHttpActionResult> Jump(string url)
        {
            //if not in cache, search in db
            Models.Link shortUrl = (Models.Link)CacheHelper.GetCache(url);

            if (shortUrl != null)
            {
                this.LogLinkHits(shortUrl);
                response.Headers.Add("Location", shortUrl.OriginalLink);
                return this.ResponseMessage(response);
            }

            //try update data from db
            Models.Link entity = await StorageHelper.FindEntityByCondition<Models.Link>(LinkTableName, PartitionKey, url);
            if (entity != null)
            {
                CacheHelper.SetCache(url, entity, 5);
                this.LogLinkHits(entity);
                response.Headers.Add("Location", entity.OriginalLink);
                return this.ResponseMessage(response);
            }

            //if not in
            return this.NotFound();
        }

        /// <summary>
        ///     Logs the link hits.
        /// </summary>
        /// <param name="link">The link.</param>
        private void LogLinkHits(Models.Link link)
        {
            LinkLog linkLog = new LinkLog
            {
                PartitionKey = link.ShortedLink,
                RowKey = Guid.NewGuid().ToString(),
                Ip = HttpUtils.GetUserHostAddress(this.Request),
                UserAgent = HttpUtils.GetUserAgent(this.Request),
                SourceUrl = link.ShortedLink,
                TargetUrl = link.OriginalLink,
                HitTime = DateTime.UtcNow.ToChinaStandardTime()
            };

            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                StorageHelper.LogLinkHitsAsync(LinkLogTableName, linkLog));
        }
    }
}
