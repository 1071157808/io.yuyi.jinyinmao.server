// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  1:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-08  2:44 PM
// ***********************************************************************
// <copyright file="UserController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Tracing;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     Class UserController.
    /// </summary>
    [RoutePrefix("User")]
    public class UserController : ApiControllerBase
    {
        private readonly IUserInfoService userInfoService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        /// <param name="userInfoService">The user information service.</param>
        public UserController(IUserInfoService userInfoService)
        {
            this.userInfoService = userInfoService;
        }

        /// <summary>
        ///     获取用户信息
        /// </summary>
        /// <remarks>
        ///     用户未登录会返回401
        /// </remarks>
        /// <response code="200">认证成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">UG:无法获取用户信息</response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route(""), CookieAuthorize, ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> Get()
        {
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);
            if (userInfo == null)
            {
                this.Trace.Warn(this.Request, "Application", "User-GetBankCards:Can not load user data.{0}".FormatWith(this.CurrentUser.Id));
                return this.BadRequest("UG:无法获取用户信息");
            }

            return this.Ok(userInfo.ToResponse());
        }
    }
}