// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-22  2:13 AM
// ***********************************************************************
// <copyright file="UserController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using Moe.AspNet.Filters;
using Moe.AspNet.Utility;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Models.User;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Dtos;
using Yuyi.Jinyinmao.Service.Interface;
using Yuyi.Jinyinmao.Service.Misc.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     Class UserController.
    /// </summary>
    [RoutePrefix("User")]
    public class UserController : ApiControllerBase
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
        ///     手机号是否已注册
        /// </summary>
        /// <remarks>
        ///     如果手机号已经注册过，则不能再用于注册
        /// </remarks>
        /// <param name="cellphone">
        ///     手机号[Required]
        /// </param>
        /// <response code="200">注册成功</response>
        /// <response code="400">US01:手机号格式不正确</response>
        [HttpGet, Route("CheckCellphone"), ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(CheckCellphoneResult))]
        public async Task<IHttpActionResult> CheckCellphone(string cellphone)
        {
            cellphone = cellphone ?? "";
            Match match = RegexUtility.CellphoneRegex.Match(cellphone);
            if (!match.Success || match.Index != 0 || match.Length != cellphone.Length)
            {
                return this.BadRequest("US01:手机号格式不正确");
            }

            CheckCellphoneResult result = await this.userService.CheckCellphoneAsync(cellphone);
            return this.Ok(result);
        }

        /// <summary>
        ///     登录
        /// </summary>
        /// <remarks>
        ///     通过账户名和密码登录，现在账户名即为用户的手机号
        /// </remarks>
        /// <param name="request">
        ///     登录请求
        /// </param>
        /// <response code="200">登录成功</response>
        /// <response code="400">请求格式不合法</response>
        [Route("SignIn"), ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(SignInResponse))]
        public async Task<IHttpActionResult> SignIn(SignInRequest request)
        {
            SignInResult signInResult = await this.userService.CheckPasswordAsync(request.LoginName, request.Password);

            if (signInResult.Successed)
            {
                this.SetCookie(signInResult.UserId, signInResult.Cellphone);
            }

            return this.Ok(signInResult.ToResponse());
        }

        /// <summary>
        ///     金银猫客户端注册接口
        /// </summary>
        /// <remarks>
        ///     在金银猫的客户端注册，包括PC网页、M版网页、iPhone、Android
        ///     <br />
        ///     前置条件：已经通过验证码验证手机号码的真实性
        /// </remarks>
        /// <param name="request">
        ///     注册请求
        /// </param>
        /// <response code="200">注册成功</response>
        /// <response code="400">请求格式不合法</response>
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

            UserInfo commandResult = await this.userService.ExcuteCommand(new UserRegister
            {
                Cellphone = info.Cellphone,
                Password = request.Password,
                UserId = info.UserId,
                ClientType = request.ClientType.GetValueOrDefault(),
                ContractId = request.ContractId.GetValueOrDefault(),
                InviteBy = request.InviteBy ?? "JYM",
                OutletCode = request.OutletCode ?? "JYM",
                Args = this.BuildArgs()
            });

            // 自动登陆
            this.SetCookie(commandResult.UserId, commandResult.Cellphone);

            return this.Ok(commandResult.ToResponse());
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
