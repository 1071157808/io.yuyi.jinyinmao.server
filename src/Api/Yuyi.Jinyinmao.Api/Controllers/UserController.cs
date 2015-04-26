// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  11:02 PM
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
using System.Web.Http.Tracing;
using System.Web.Security;
using Moe.AspNet.Filters;
using Moe.AspNet.Utility;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Filters;
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
        private readonly ISmsService smsService;
        private readonly IUserInfoService userInfoService;
        private readonly IUserService userService;
        private readonly IVeriCodeService veriCodeService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        /// <param name="smsService">The SMS service.</param>
        /// <param name="userInfoService">The user information service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="veriCodeService">The veri code service.</param>
        public UserController(ISmsService smsService, IUserInfoService userInfoService, IUserService userService, IVeriCodeService veriCodeService)
        {
            this.smsService = smsService;
            this.userInfoService = userInfoService;
            this.userService = userService;
            this.veriCodeService = veriCodeService;
        }

        /// <summary>
        ///     添加新银行卡
        /// </summary>
        /// <remarks>
        ///     该接口只适合已经绑定过实名信息的用户添加新银行卡
        /// </remarks>
        /// <param name="request">
        ///     添加银行卡请求
        /// </param>
        /// <response code="200">成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">UABC1:无法添加银行卡</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="500"></response>
        [Route("AddBankCard"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> AddBankCard(AddBankCardRequest request)
        {
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);

            if (userInfo == null)
            {
                this.Trace.Warn(this.Request, "Application", "User-AddBankCard: Can not load user date.{0}".FormatWith(this.CurrentUser.Id));
                return this.BadRequest("UABC1:无法添加银行卡");
            }

            if (!userInfo.Verified)
            {
                return this.BadRequest("UABC2:请先进行实名认证");
            }

            await this.userService.AddBankCardAsync(new AddBankCard
            {
                BankCardNo = request.BankCardNo,
                BankName = request.BankName,
                CityName = request.CityName,
                CommandId = Guid.NewGuid(),
                UserId = this.CurrentUser.Id
            });
            return this.Ok();
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
        /// <response code="500"></response>
        [HttpGet, Route("CheckCellphone"), ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(CheckCellphoneResult))]
        public async Task<IHttpActionResult> CheckCellphone(string cellphone)
        {
            cellphone = cellphone ?? "";
            Match match = RegexUtility.CellphoneRegex.Match(cellphone);
            if (!match.Success || match.Index != 0 || match.Length != cellphone.Length)
            {
                return this.BadRequest("UCC:手机号格式不正确");
            }

            CheckCellphoneResult result = await this.userService.CheckCellphoneAsync(cellphone);
            return this.Ok(result);
        }

        /// <summary>
        ///     重置登录密码
        /// </summary>
        /// <remarks>
        ///     重置密码前，必须要认证手机号，并且获得认证手机号的token
        /// </remarks>
        /// <param name="request">
        ///     重置登录密码请求
        /// </param>
        /// <response code="200">重置成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="500"></response>
        [Route("SignIn"), ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> ResetLoginPassword(ResetPasswordRequest request)
        {
            UseVeriCodeResult veriCodeResult = await this.veriCodeService.UseAsync(request.Token, VeriCodeType.ResetLoginPassword);
            if (!veriCodeResult.Result)
            {
                return this.BadRequest("URLP1:该验证码已经失效，请重新获取验证码");
            }

            SignUpUserIdInfo info = await this.userService.GetSignUpUserIdInfoAsync(veriCodeResult.Cellphone);
            if (!info.Registered)
            {
                return this.BadRequest("URLP2:手机号码不存在，密码修改失败");
            }
            await this.userService.ResetLoginPasswordAsync(new ResetLoginPassword
            {
                CommandId = Guid.NewGuid(),
                Password = request.Password,
                Salt = info.UserId.ToGuidString(),
                UserId = info.UserId
            });

            return this.Ok();
        }

        /// <summary>
        ///     重置支付密码
        /// </summary>
        /// <remarks>
        ///     如果已经通过实名认证，需要验证手机号、用户姓名、身份证号3要素进行重置支付密码
        ///     <br />
        ///     如果没有实名认证过，只需要验证手机号即可进行实名认证
        /// </remarks>
        /// <param name="request">
        ///     重置支付密码请求
        /// </param>
        /// <response code="200">重置成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="500"></response>
        [Route("ResetPaymentPassword"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> ResetPaymentPassword(ResetPaymentPasswordRequest request)
        {
            UseVeriCodeResult result = await this.veriCodeService.UseAsync(request.Token, VeriCodeType.ResetPaymentPassword);
            if (!result.Result)
            {
                return this.BadRequest("URPP1:该验证码已经失效，请重新获取验证码");
            }

            UserInfo info = await this.userService.GetUserInfoAsync(this.CurrentUser.Id);

            if (info == null || !info.HaSetPaymentPassword ||
                (info.Verified && info.RealName == request.UserRealName && info.CredentialNo.ToUpperInvariant() == request.CredentialNo.ToUpperInvariant()))
            {
                return this.BadRequest("URPP2:您输入的身份信息错误！请重新输入");
            }

            if (await this.userService.CheckPasswordAsync(this.CurrentUser.Id, request.Password))
            {
                return this.BadRequest("URPP3: 支付密码不能与登录密码一致，请选择新的支付密码");
            }

            await this.userService.SetPaymentPasswordAsync(new SetPaymentPassword
            {
                CommandId = Guid.NewGuid(),
                Override = true,
                PaymentPassword = request.Password,
                Salt = this.CurrentUser.Id.ToGuidString(),
                UserId = this.CurrentUser.Id
            });

            return this.Ok();
        }

        /// <summary>
        ///     设置支付密码
        /// </summary>
        /// <remarks>
        ///     支付密码必须包含一位字母或者一般特殊字符，长度为8到18位之间
        /// </remarks>
        /// <param name="request">
        ///     设置支付密码请求
        /// </param>
        /// <response code="200">重置成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="500"></response>
        [Route("SetPaymentPassword"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> SetPaymentPassword(SetPaymentPasswordRequest request)
        {
            if (await this.userService.CheckPasswordAsync(this.CurrentUser.Id, request.Password))
            {
                return this.BadRequest("USPP1: 支付密码不能与登录密码一致，请选择新的支付密码");
            }

            await this.userService.SetPaymentPasswordAsync(new SetPaymentPassword
            {
                CommandId = Guid.NewGuid(),
                Override = false,
                PaymentPassword = request.Password,
                Salt = this.CurrentUser.Id.ToGuidString(),
                UserId = this.CurrentUser.Id
            });

            return this.Ok();
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
        /// <response code="500"></response>
        [Route("SignIn"), ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(SignInResponse))]
        public async Task<IHttpActionResult> SignIn(SignInRequest request)
        {
            SignInResult signInResult = await this.userService.CheckPasswordViaCellphoneAsync(request.LoginName, request.Password);

            if (signInResult.Success)
            {
                this.SetCookie(signInResult.UserId, signInResult.Cellphone);
            }

            return this.Ok(signInResult.ToResponse());
        }

        /// <summary>
        ///     金银猫客户端注销接口
        /// </summary>
        /// <remarks>
        ///     客户端可以通过直接清除Cookie MA的值实现注销
        /// </remarks>
        /// <response code="200">注销成功</response>
        [Route("SignOut")]
        public IHttpActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return this.Ok();
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
        /// <response code="500"></response>
        [Route("SignUp"), ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(SignUpResponse))]
        public async Task<IHttpActionResult> SignUp(SignUpRequest request)
        {
            UseVeriCodeResult result = await this.veriCodeService.UseAsync(request.Token, VeriCodeType.SignUp);

            if (!result.Result)
            {
                return this.BadRequest("USU1:请输入正确的验证码");
            }

            SignUpUserIdInfo info = await this.userService.GetSignUpUserIdInfoAsync(result.Cellphone);

            if (info.Registered)
            {
                return this.BadRequest("USU2:此号码已注册，请直接登录");
            }

            UserInfo userInfo = await this.userService.RegisterUserAsync(new UserRegister
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
            this.SetCookie(userInfo.UserId, userInfo.Cellphone);

            return this.Ok(userInfo.ToSignUpResponse());
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
