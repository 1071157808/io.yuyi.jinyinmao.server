// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  1:27 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-07  3:01 AM
// ***********************************************************************
// <copyright file="UserController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using Moe.Actor.Commands;
using Moe.AspNet.Filters;
using Moe.AspNet.Utility;
using Yuyi.Jinyinmao.Api.Models.User;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Service.Interface;
using Yuyi.Jinyinmao.Service.Misc.Interface;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     Class UserController.
    /// </summary>
    [RoutePrefix("User")]
    public class UserController : ApiController
    {
        private readonly IUserService userService;
        private readonly IVeriCodeService veriCodeService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        /// <param name="veriCodeService">The veri code service.</param>
        /// <param name="userService">The user service.</param>
        public UserController(IVeriCodeService veriCodeService, IUserService userService)
        {
            this.veriCodeService = veriCodeService;
            this.userService = userService;
        }

        /// <summary>
        ///     金银猫客户端注册接口
        /// </summary>
        /// <remarks>
        ///     在金银猫的客户端注册，包括PC网页、M版网页、iPhone、Android
        ///     &lt;br /&gt;
        ///     前置条件：已经通过验证码验证手机号码的真实性
        /// </remarks>
        /// <param name="request">
        ///     注册请求
        /// </param>
        /// <response code="200">注册成功</response>
        /// <response code="400">US01:请输入正确的验证码</response>
        /// <response code="400">US02:此号码已注册，请直接登录</response>
        [Route("SignUp"), ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(SignUpResponse))]
        public async Task<IHttpActionResult> SignUpAsync(SignUpRequest request)
        {
            UseVeriCodeResult result = await this.veriCodeService.UseAsync(request.Token, VeriCodeType.SignUp);

            if (!result.Result)
            {
                return this.BadRequest("US01:请输入正确的验证码");
            }

            SignUpUserIdInfo info = await this.userService.GetSignUpUserIdInfoAsync(result.Cellphone);

            if (info.Registered)
            {
                return this.BadRequest("US02:此号码已注册，请直接登录");
            }

            ICommandHanderResult<UserInfo> commandResult = await this.userService.ExcuteCommand(new UserRegister
            {
                Cellphone = info.Cellphone,
                RegisterTime = DateTime.Now,
                UserId = info.UserId
            });

            // 自动登陆
            this.SetCookie(commandResult.Result.UserId, commandResult.Result.Cellphone);

            return this.Ok(new SignUpResponse { Cellphone = commandResult.Result.Cellphone, UserId = commandResult.Result.UserId });
        }

        /// <summary>
        ///     Sets the cookie.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cellphone">The cellphone.</param>
        private void SetCookie(Guid userId, string cellphone)
        {
            bool isMobileDevice = HttpUtils.IsFromMobileDevice(this.Request);
            DateTime expiry = isMobileDevice ? DateTime.Now.AddDays(30) : DateTime.Now.AddMinutes(30);
            FormsAuthentication.SetAuthCookie(string.Format("{0},{1},{2}", userId, cellphone, expiry.ToBinary()), true);
        }
    }
}
