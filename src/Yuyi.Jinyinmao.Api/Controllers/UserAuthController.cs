// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  1:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-25  1:04 AM
// ***********************************************************************
// <copyright file="UserAuthController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
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
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Api.Models.User;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Dtos;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     UserAuthController.
    /// </summary>
    [RoutePrefix("User/Auth")]
    public class UserAuthController : ApiControllerBase
    {
        private readonly IUserInfoService userInfoService;
        private readonly IUserService userService;
        private readonly IVeriCodeService veriCodeService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserAuthController" /> class.
        /// </summary>
        /// <param name="userInfoService">The user information service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="veriCodeService">The veri code service.</param>
        public UserAuthController(IUserInfoService userInfoService, IUserService userService, IVeriCodeService veriCodeService)
        {
            this.userInfoService = userInfoService;
            this.userService = userService;
            this.veriCodeService = veriCodeService;
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
        /// <response code="400">请求格式不合法
        /// <br />
        /// UAA1:无法开通快捷支付功能
        /// <br />
        /// UAA2:请先设置支付密码
        /// <br />
        /// UAA3:已经通过实名认证
        /// </response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Authenticate"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> Authenticate([FromUri] AuthenticationRequest request)
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

            AddBankCard addBankCardCommand = new AddBankCard
            {
                Args = this.BuildArgs(),
                BankCardNo = request.BankCardNo,
                BankName = request.BankName,
                CityName = request.CityName,
                UserId = this.CurrentUser.Id
            };

            Authenticate authenticateCommand = new Authenticate
            {
                Args = this.BuildArgs(),
                BankCardNo = request.BankCardNo,
                BankName = request.BankName,
                Cellphone = this.CurrentUser.Cellphone,
                CityName = request.CityName,
                Credential = request.Credential,
                CredentialNo = request.CredentialNo,
                RealName = request.RealName,
                UserId = this.CurrentUser.Id
            };

            await this.userService.AuthenticateAsync(addBankCardCommand, authenticateCommand);

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
        /// <response code="400">UACC:手机号格式不正确</response>
        /// <response code="500"></response>
        [HttpGet, Route("CheckCellphone"), ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(CheckCellphoneResult))]
        public async Task<IHttpActionResult> CheckCellphone(string cellphone)
        {
            cellphone = cellphone ?? "";
            Match match = RegexUtility.CellphoneRegex.Match(cellphone);
            if (!match.Success || match.Index != 0 || match.Length != cellphone.Length)
            {
                return this.BadRequest("UACC:手机号格式不正确");
            }

            CheckCellphoneResult result = await this.userService.CheckCellphoneAsync(cellphone);
            return this.Ok(result);
        }

        /// <summary>
        ///     检验支付密码
        /// </summary>
        /// <remarks>
        ///     必须登录
        /// </remarks>
        /// <param name="request">
        ///     请求
        /// </param>
        /// <response code="200">密码正确</response>
        /// <response code="400">请求格式错误
        /// <br />
        /// UACPP1:请重置支付密码后再试
        /// <br />
        /// UACPP1:支付密码错误，支付密码输入错误5次会锁定支付功能
        /// </response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("CheckPaymentPassword"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> CheckPaymentPassword([FromUri] CheckPaymentPasswordRequest request)
        {
            CheckPaymentPasswordResult result = await this.userService.CheckPaymentPasswordAsync(this.CurrentUser.Id, request.Password);

            if (result.Lock)
            {
                return this.BadRequest("UACPP1:请重置支付密码后再试");
            }

            return !result.Success ? this.BadRequest("UACPP1:支付密码错误，支付密码输入错误5次会锁定支付功能")
                : this.Ok();
        }

        /// <summary>
        ///     清楚未认证成功的身份信息
        /// </summary>
        /// <remarks>
        ///     调用该接口会清楚未认证的身份信息和银行卡信息，清楚了的信心之后后收到的认证通过结果也会被忽略
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("ClearUnauthenticatedInfo"), CookieAuthorize]
        public async Task<IHttpActionResult> ClearUnauthenticatedInfo()
        {
            await this.userService.ClearUnauthenticatedInfo(this.CurrentUser.Id);

            return this.Ok();
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
        /// <response code="400">请求格式不合法
        /// <br />
        /// UARLP1:该验证码已经失效，请重新获取验证码
        /// <br />
        /// UARLP2:手机号码不存在，密码修改失败
        /// </response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("ResetLoginPassword"), ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> ResetLoginPassword([FromUri] ResetPasswordRequest request)
        {
            UseVeriCodeResult veriCodeResult = await this.veriCodeService.UseAsync(request.Token, VeriCodeType.ResetLoginPassword);
            if (!veriCodeResult.Result)
            {
                return this.BadRequest("UARLP1:该验证码已经失效，请重新获取验证码");
            }

            SignUpUserIdInfo info = await this.userService.GetSignUpUserIdInfoAsync(veriCodeResult.Cellphone);
            if (!info.Registered)
            {
                return this.BadRequest("UARLP2:手机号码不存在，密码修改失败");
            }
            await this.userService.ResetLoginPasswordAsync(new ResetLoginPassword
            {
                Password = request.Password,
                Salt = info.UserId.ToGuidString(),
                UserId = info.UserId,
                Args = this.BuildArgs()
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
        /// <response code="400">请求格式不合法
        /// <br />
        /// UARPP1:该验证码已经失效，请重新获取验证码
        /// <br />
        /// UARPP2:您输入的身份信息错误！请重新输入
        /// <br />
        /// UARPP3:支付密码不能与登录密码一致，请选择新的支付密码
        /// </response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("ResetPaymentPassword"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> ResetPaymentPassword([FromUri] ResetPaymentPasswordRequest request)
        {
            UseVeriCodeResult result = await this.veriCodeService.UseAsync(request.Token, VeriCodeType.ResetPaymentPassword);
            if (!result.Result)
            {
                return this.BadRequest("UARPP1:该验证码已经失效，请重新获取验证码");
            }

            UserInfo info = await this.userService.GetUserInfoAsync(this.CurrentUser.Id);

            if (info == null || !info.HasSetPaymentPassword ||
                (info.Verified && (info.RealName != request.UserRealName || !string.Equals(info.CredentialNo, request.CredentialNo, StringComparison.InvariantCultureIgnoreCase))))
            {
                return this.BadRequest("UARPP2:您输入的身份信息错误！请重新输入");
            }

            if (await this.userService.CheckPasswordAsync(this.CurrentUser.Id, request.Password))
            {
                return this.BadRequest("UARPP3:支付密码不能与登录密码一致，请选择新的支付密码");
            }

            await this.userService.SetPaymentPasswordAsync(new SetPaymentPassword
            {
                Override = true,
                PaymentPassword = request.Password,
                Salt = this.CurrentUser.Id.ToGuidString(),
                UserId = this.CurrentUser.Id,
                Args = this.BuildArgs()
            });

            return this.Ok();
        }

        /// <summary>
        ///     设置支付密码
        /// </summary>
        /// <remarks>
        ///     支付密码必须包含一位字母或者一般特殊字符，长度为8到18位之间,并且不能与登录密码一致
        /// </remarks>
        /// <param name="request">
        ///     设置支付密码请求
        /// </param>
        /// <response code="200">重置成功</response>
        /// <response code="400">请求格式不合法
        /// <br />
        /// UASPP1:支付密码不能与登录密码一致，请选择新的支付密码
        /// <br />
        /// UASPP2: 支付密码已经设置，请直接使用
        /// </response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("SetPaymentPassword"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> SetPaymentPassword([FromUri] SetPaymentPasswordRequest request)
        {
            if (await this.userService.CheckPasswordAsync(this.CurrentUser.Id, request.Password))
            {
                return this.BadRequest("UASPP1:支付密码不能与登录密码一致，请选择新的支付密码");
            }

            UserInfo info = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);

            if (info.HasSetPaymentPassword)
            {
                return this.BadRequest("UASPP2: 支付密码已经设置，请直接使用");
            }

            await this.userService.SetPaymentPasswordAsync(new SetPaymentPassword
            {
                Override = false,
                PaymentPassword = request.Password,
                Salt = this.CurrentUser.Id.ToGuidString(),
                UserId = this.CurrentUser.Id,
                Args = this.BuildArgs()
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
        [HttpGet, Route("SignIn"), ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(SignInResponse))]
        public async Task<IHttpActionResult> SignIn([FromUri] SignInRequest request)
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
        [HttpGet, Route("SignOut")]
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
        /// <response code="400">请求格式不合法
        /// <br />
        /// UAS01:请输入正确的验证码
        /// <br />
        /// UAS02:此号码已注册，请直接登录
        /// </response>
        /// <response code="500"></response>
        [HttpGet, Route("SignUp"), ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(SignUpResponse))]
        public async Task<IHttpActionResult> SignUp([FromUri] SignUpRequest request)
        {
            UseVeriCodeResult result = await this.veriCodeService.UseAsync(request.Token, VeriCodeType.SignUp);

            if (!result.Result)
            {
                return this.BadRequest("UASU1:请输入正确的验证码");
            }

            SignUpUserIdInfo info = await this.userService.GetSignUpUserIdInfoAsync(result.Cellphone);

            if (info.Registered)
            {
                return this.BadRequest("UASU2:此号码已注册，请直接登录");
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
            DateTime expiry = isMobileDevice ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(1);
            FormsAuthentication.SetAuthCookie($"{userId},{cellphone},{expiry.ToBinary()}", true);
        }
    }
}