
using System;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using Moe.AspNet.Utility;
using Yuyi.Jinyinmao.Api.Link.Models;
using Yuyi.Jinyinmao.Api.Link.Utils;
using Moe.AspNet.Utility;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Api.Link.Controllers
{
    /// <summary>
    /// HomeController.
    /// </summary>
    public class HomeController : ApiController
    {
        /// <summary>
        /// The link log table name
        /// </summary>
        private static readonly string LinkLogTableName = "LinkLogs";

        /// <summary>
        /// The link table name
        /// </summary>
        private static readonly string LinkTableName = "Links";

        /// <summary>
        /// The partition key
        /// </summary>
        private static readonly string PartitionKey = "ShortLink";

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Index()
        {
            return this.Ok();
        }

        /// <summary>
        /// Jumps the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [HttpGet]
        [Route("{url}")]
        public async Task<IHttpActionResult> Jump(string url)
        {
            //if not in dic, search in db
            Models.Link shortUrl = (Models.Link)CacheHelper.GetCache(url);

            if (shortUrl != null)
            {
                this.LogLinkHits(shortUrl);
                return this.Redirect(shortUrl.OriginalLink);
            }

            //try update data from db
            Models.Link entity = await StorageHelper.FindByCondition<Models.Link>(LinkTableName, PartitionKey, url);
            if (entity != null)
            {
                CacheHelper.SetCache(url, entity, 5);
                this.LogLinkHits(entity);
                return this.Redirect(entity.OriginalLink);
            }

            //if not in
            return this.NotFound();
        }


        /// <summary>
        /// Logs the link hits.
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
