// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  12:54 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-19  12:13 PM
// ***********************************************************************
// <copyright file="DevController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Web;
using System.Web.Http;
using Moe.AspNet.Utility;

namespace Yuyi.Jinyinmao.Api.Sms.Controllers
{
    /// <summary>
    ///     Class HomeController.
    /// </summary>
    [RoutePrefix("")]
    public class DevController : ApiController
    {
        /// <summary>
        ///     The default action of the service.
        /// </summary>
        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            return this.Ok(
                new
                {
                    Request.RequestUri,
                    Request.Headers,
                    QueryParameters = Request.GetQueryNameValuePairs(),
                    RequestProperties = Request.Properties.Keys,
                    RequestContext.ClientCertificate,
                    RequestContext.IsLocal,
                    RequestContext.VirtualPathRoot,
                    HttpContext.Current.Request.Browser.Browser,
                    HttpContext.Current.Request.IsSecureConnection,
                    HttpContext.Current.Request.Browser.IsMobileDevice,
                    UserHostAddress = HttpUtils.GetUserHostAddress(Request),
                    UserAgent = HttpUtils.GetUserAgent(Request),
                    Cookie = Request.Headers.GetCookies(),
                    Request.Content,
                    ConfigurationProperties = Configuration.Properties,
                    ServerIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString()
                });
        }
    }
}
