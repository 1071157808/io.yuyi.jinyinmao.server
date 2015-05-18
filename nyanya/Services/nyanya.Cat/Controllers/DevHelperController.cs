// FileInformation: nyanya/nyanya.Cat/DevHelperController.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   10:54 AM

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Tracing;
using Infrastructure.Lib.Utility;

namespace nyanya.Cat.Controllers
{
    /// <summary>
    ///     开发辅助接口(非业务接口，用于辅助开发)
    /// </summary>
    [RoutePrefix("DevHelper")]
    public class DevHelperController : ApiController
    {
        /// <summary>
        ///     回声
        /// </summary>
        [HttpGet]
        [Route("Echo")]
        public IHttpActionResult Echo()
        {
            ITraceWriter writer = this.Configuration.Services.GetTraceWriter();
            writer.Warn(this.Request, "Warn", "DevHelper Echo");

            return this.Ok(new
            {
                DateTime.Now,
                UserHostAddress = HttpUtils.GetUserHostAddress(this.Request),
                UserAgent = HttpUtils.GetUserAgent(this.Request),
                this.Request.Method.Method,
                this.Request.RequestUri,
                QueryParameters = this.Request.GetQueryNameValuePairs(),
                this.Request.Headers,
                this.Request.Properties.Keys,
                Cookie = this.Request.Headers.GetCookies(),
                this.Request.Version,
                this.Configuration.Properties
            });
        }

        /// <summary>
        ///     服务器IP
        /// </summary>
        [HttpGet]
        [Route("IP")]
        public string Ip()
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            return String.Join<IPAddress>("|", ipEntry.AddressList);
        }

        /// <summary>
        ///     服务器状态
        /// </summary>
        [HttpGet]
        [Route("Status")]
        public string Status()
        {
            return "ok";
        }
    }
}