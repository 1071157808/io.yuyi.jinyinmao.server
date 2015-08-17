// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : UserController.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  20:01
// ***********************************************************************
// <copyright file="UserController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Domain.Commands;
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
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        /// <param name="userInfoService">The user information service.</param>
        /// <param name="userService">The user service.</param>
        public UserController(IUserInfoService userInfoService, IUserService userService)
        {
            this.userInfoService = userInfoService;
            this.userService = userService;
        }

        /// <summary>
        ///     获取用户信息
        /// </summary>
        /// <remarks>
        ///     用户未登录会返回401
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     UG:无法获取用户信息
        /// </response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet]
        [Route("")]
        [CookieAuthorize]
        [ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> Get()
        {
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);
            return this.Ok(userInfo.ToResponse());
        }

        /// <summary>
        ///     签到
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400">
        ///     US1:无法获取用户信息
        /// </response>
        /// <response code="401"></response>
        /// <response code="500"></response>
        [HttpGet]
        [Route("Sign")]
        [CookieAuthorize]
        [ResponseType(typeof(SettleAccountTransactionInfoResponse))]
        public async Task<IHttpActionResult> Sign()
        {
            SettleAccountTransactionInfo settleAccountTransactionInfo = await this.userService.SignAsync(this.BuildSignCommand());

            return this.Ok(settleAccountTransactionInfo.ToResponse());
        }

        private Sign BuildSignCommand()
        {
            return new Sign
            {
                EntityId = this.CurrentUser.Id,
                Args = this.BuildArgs(),
                UserIdentifier = this.CurrentUser.Id
            };
        }
    }
}