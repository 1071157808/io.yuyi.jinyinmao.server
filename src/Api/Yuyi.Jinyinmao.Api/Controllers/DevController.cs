// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  8:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-06  9:56 PM
// ***********************************************************************
// <copyright file="DevController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Web;
using System.Web.Http;

namespace Yuyi.Jinyinmao.Api.Controllers
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
                    Request.Content,
                    RequestContext.ClientCertificate,
                    RequestContext.IsLocal,
                    RequestContext.VirtualPathRoot,
                    HttpContext.Current.Request.Browser.Browser,
                    HttpContext.Current.Request.IsSecureConnection,
                    HttpContext.Current.Request.Browser.IsMobileDevice
                });
        }
    }
}
