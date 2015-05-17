// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  1:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-17  7:56 PM
// ***********************************************************************
// <copyright file="UserController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Tracing;
using Moe.AspNet.Filters;
using Moe.Lib;
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
        ///     实名认证（通过银联）
        /// </summary>
        /// <remarks>
        ///     实名认证必须先设置支付密码
        ///     <br />
        ///     实名认证过程会绑定一张银行卡，同时将该卡设置为默认银行卡
        /// </remarks>
        /// <param name="request">
        ///     实名认证请求
        /// </param>
        /// <response code="200">认证成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">UAA1:无法开通快捷支付功能</response>
        /// <response code="400">UAA2:请先设置支付密码</response>
        /// <response code="400">UAA3:已经通过实名认证</response>
        /// <response code="400">UAA4:正在进行实名认证，请耐心等待</response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [Route("Authenticate"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> Authenticate(AuthenticationRequest request)
        {
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);

            if (userInfo == null)
            {
                this.Trace.Warn(this.Request, "Application", "User-Authenticate:Can not load user data.{0}".FormatWith(this.CurrentUser.Id));
                return this.BadRequest("UAA1:无法开通快捷支付功能");
            }

            if (!userInfo.HasSetPaymentPassword)
            {
                return this.BadRequest("UAA2:请先设置支付密码");
            }

            if (userInfo.Verified)
            {
                return this.BadRequest("UAA3:已经通过实名认证");
            }

            if (userInfo.RealName.IsNotNullOrEmpty() && !userInfo.Verified)
            {
                return this.BadRequest("UAA4:正在进行实名认证，请耐心等待");
            }

            await this.userService.AuthenticateAsync(new Authenticate
            {
                Args = this.BuildArgs(),
                BankCardNo = request.BankCardNo,
                BankName = request.BankName,
                Cellphone = userInfo.Cellphone,
                CityName = request.CityName,
                Credential = request.Credential,
                CredentialNo = request.CredentialNo,
                RealName = request.RealName,
                UserId = this.CurrentUser.Id
            });

            return this.Ok();
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