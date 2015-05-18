// FileInformation: nyanya/nyanya.Cat/UserController.cs
// CreatedTime: 2015/03/05   9:54 PM
// LastUpdatedTime: 2015/03/12   6:18 PM

using Cat.Commands.Users;
using Cat.Domain.Auth.Models;
using Cat.Domain.Auth.Services.DTO;
using Cat.Domain.Auth.Services.Interfaces;
using Cat.Domain.Users.Models;
using Cat.Domain.Users.ReadModels;
using Cat.Domain.Users.Services.DTO;
using Cat.Domain.Users.Services.Interfaces;
using Cat.Events.Users;
using Domian.Bus;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Secutiry;
using Infrastructure.Lib.Utility;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Cat.Filters;
using nyanya.Cat.Models;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;

namespace nyanya.Cat.Controllers
{
    /// <summary>
    ///     UserController
    /// </summary>
    [RoutePrefix("User")]
    public class UserController : ApiControllerBase
    {
        /// <summary>
        ///     The command bus
        /// </summary>
        private readonly ICommandBus commandBus;

        /// <summary>
        ///     The event bus
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        ///     The user information service
        /// </summary>
        private readonly IExactUserInfoService userInfoService;

        /// <summary>
        ///     The user service
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        ///     The veri code service
        /// </summary>
        private readonly IVeriCodeService veriCodeService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="veriCodeService">The verification code service.</param>
        /// <param name="userInfoService">The user information service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="eventBus">The event bus.</param>
        public UserController(ICommandBus commandBus, IVeriCodeService veriCodeService, IExactUserInfoService userInfoService, IUserService userService, IEventBus eventBus)
        {
            this.commandBus = commandBus;
            this.veriCodeService = veriCodeService;
            this.userInfoService = userInfoService;
            this.userService = userService;
            this.eventBus = eventBus;
        }

        /// <summary>
        ///     添加银行卡
        /// </summary>
        /// <param name="request">
        ///     BankCardNo[string]: 银行卡号（15-19位）
        ///     BankName[string]: 银行名称
        ///     CityName[string]: 开户行所在城市名称
        ///     ClientType[long?]: 客户端标识(推广相关)
        ///     FlgMoreI1[long?]: FlgMoreI1(推广相关)
        ///     FlgMoreI2[long?]: FlgMoreI2(推广相关)
        ///     FlgMoreS1[string]: FlgMoreS1(推广相关)
        ///     FlgMoreS2[string]: FlgMoreS2(推广相关)
        /// </param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     No Content
        ///     @{h2@} HttpStatusCode:400 @{/h2@}
        ///     "无法添加银行卡"
        ///     "请先开通快捷支付功能"
        ///     "最多只能绑定10张银行卡。"
        ///     "此卡片已经被绑定"
        /// </returns>
        [HttpPost, Route("AddBankCard")]
        [TokenAuthorize]
        [EmptyParameterFilter("request", Order = 1), ValidateModelState(Order = 2)]
        public async Task<IHttpActionResult> AddBankCard(AddBankCardRequest request)
        {
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Identifier);

            if (userInfo == null)
            {
                this.Warning("Can not load user date.{0}".FormatWith(this.CurrentUser.Identifier));
                return this.BadRequest("无法添加银行卡");
            }

            if (!userInfo.Verified.GetValueOrDefault(false))
            {
                return this.BadRequest("请先开通快捷支付功能");
            }

            if (userInfo.BankCardsCount > 9)
            {
                return this.BadRequest("您不能再添加更多银行卡");
            }

            if (await this.userService.CheckBankCardAvaliableAsync(request.BankCardNo))
            {
                return this.BadRequest("此卡片已经被绑定");
            }

            if (await this.userInfoService.GetAddBankFailCountAsync(userInfo.UserIdentifier, request.BankCardNo) >= 5)
            {
                return this.BadRequest("对不起，该卡失败次数已超过5次，请您明天再试。");
            }

            AddBankCard command = this.BuildAddBankCardCommand(request, userInfo);
            this.commandBus.Excute(command);
            return this.OK();
        }

        /// <summary>
        ///     手机号是否已注册
        /// </summary>
        /// <param name="cellphone">手机号[必填]</param>
        /// <returns>
        ///     Result[bool]：手机号是否存在 true or false
        ///     UserType[int]：用户类型 10金银猫用户 1兴业用户 11金银猫和兴业
        /// </returns>
        [HttpGet]
        [Route("CheckCellphone")]
        [ParameterRequire("cellphone")]
        [ParameterCellphoneFormat("cellphone", 1)]
        public async Task<IHttpActionResult> CheckCellphone(string cellphone)
        {
            CheckCellPhoneResult result = await this.userService.CheckCellPhoneAsync(cellphone);
            return this.Ok(result);
        }

        /// <summary>
        ///     重置登录密码
        /// </summary>
        /// <param name="request">
        ///     Password: 用户设置的密码（6-18位）
        ///     Token: 验证码口令
        /// </param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     No Content
        ///     @{h2@} HttpStatusCode:400 @{/h2@}
        ///     "该验证码已经使用过，请重新获取。"
        ///     "该手机号不存在或手机号错误"
        /// </returns>
        [Route("ResetLoginPassword")]
        [EmptyParameterFilter("request")]
        [ValidateModelState(Order = 1)]
        public async Task<IHttpActionResult> ResetLoginPassword(ResetPasswordRequest request)
        {
            UseVeriCodeResult result = await this.veriCodeService.UseAsync(request.Token, VeriCode.VeriCodeType.ResetLoginPassword);
            if (!result.Result)
            {
                return this.BadRequest("该验证码已经使用过，请重新获取。");
            }

            UserLoginInfo info = await this.userInfoService.GetLoginInfoAsync(result.Cellphone);

            if (info == null)
            {
                return this.BadRequest("该手机号不存在或手机号错误");
            }

            this.commandBus.Excute(new ChangeLoginPassword(info.UserIdentifier)
            {
                Password = request.Password
            });

            return this.OK();
        }

        /// <summary>
        ///     重置支付密码
        /// </summary>
        /// <param name="request">
        ///     Password: 用户设置的密码（6-18位）
        ///     Token: 验证码口令
        ///     CredentialNo: 用户证件号
        ///     BankCardNo: 默认银行卡卡号
        /// </param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     No Content
        ///     @{h2@} HttpStatusCode:400 @{/h2@}
        ///     "请输入正确的验证码"
        ///     "请先开通快捷支付。"
        ///     "您输入的信息错误！请重新输入"
        /// </returns>
        [Route("ResetPaymentPassword")]
        [EmptyParameterFilter("request")]
        [TokenAuthorize]
        [ValidateModelState(Order = 1)]
        public async Task<IHttpActionResult> ResetPaymentPassword(ResetPaymentPasswordRequest request)
        {
            UseVeriCodeResult result = await this.veriCodeService.UseAsync(request.Token, VeriCode.VeriCodeType.ResetPaymentPassword);
            if (!result.Result)
            {
                return this.BadRequest("操作超时，请重新获取验证码，重置密码");
            }

            UserInfo info = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Identifier);

            if (info == null || !info.HasDefaultBankCard || !info.HasSetPaymentPassword || !info.Verified.GetValueOrDefault(true))
            {
                return this.BadRequest("请先开通快捷支付");
            }

            if (string.Compare(info.BankCardNo, request.BankCardNo, StringComparison.OrdinalIgnoreCase) != 0 ||
                string.Compare(info.CredentialNo, request.CredentialNo, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return this.BadRequest("您输入的信息错误！请重新输入");
            }

            if (await this.userService.CompareWithLoginPassword(this.CurrentUser.Identifier, request.Password))
            {
                return this.BadRequest("支付密码不能与登录密码一致");
            }

            this.commandBus.Excute(new SetPaymentPassword(this.CurrentUser.Identifier)
            {
                PaymentPassword = request.Password
            });

            return this.OK();
        }

        /// <summary>
        ///     设置默认银行卡
        /// </summary>
        /// <param name="bankCardNo">BankCardNo[string]: 银行卡号（15-19位）</param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     Empty Body
        ///     @{h2@} HttpStatusCode: 400 @{/h2@}
        ///     "银行卡信息错误"
        /// </returns>
        [HttpGet, Route("SetDefaultBankCard"), Route("SetDefaultBankCard/{bankCardNo}")]
        [ParameterRequire("bankCardNo")]
        [TokenAuthorize]
        public async Task<IHttpActionResult> SetDefaultBankCard(string bankCardNo)
        {
            if (!await this.userService.CheckBankCardAvaliableAsync(this.CurrentUser.Identifier, bankCardNo))
            {
                return this.BadRequest("银行卡信息错误");
            }

            await this.userService.SetDefaultBankCardAsync(this.CurrentUser.Identifier, bankCardNo);
            return this.OK();
        }

        /// <summary>
        ///     设定支付密码（不能与登录密码一致）
        /// </summary>
        /// <param name="request">Password[string]: 支付密码</param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     Empty Body
        ///     @{h2@} HttpStatusCode:400 @{/h2@}
        ///     "密码格式不正确"
        ///     "支付密码不能与登录密码一致"
        ///     "已设置支付密码"
        /// </returns>
        [HttpPost, Route("SetPaymentPassword")]
        [TokenAuthorize]
        [EmptyParameterFilter("request", Order = 1), ValidateModelState(Order = 2)]
        public async Task<IHttpActionResult> SetPaymentPassword(SetPaymentPasswordRequest request)
        {
            UserInfo info = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Identifier);
            if (await this.userService.CompareWithLoginPassword(this.CurrentUser.Identifier, request.Password))
            {
                return this.BadRequest("支付密码不能与登录密码一致");
            }

            if (info.HasSetPaymentPassword)
            {
                return this.BadRequest("已设置支付密码");
            }

            this.commandBus.Excute(new SetPaymentPassword(this.CurrentUser.Identifier)
            {
                PaymentPassword = request.Password
            });

            return this.OK();
        }

        /// <summary>
        ///     登录
        /// </summary>
        /// <param name="request">
        ///     Name[必填]: 用户名，现在就是手机号码
        ///     Password[必填]: 用户设置的密码（6-18位）
        /// </param>
        /// <returns>
        ///     RemainCount: 今天剩余尝试次数，每天（自然天）只能登录失败5次，重置登录密码后可以重置失败次数
        ///     Successful: 登录结果
        ///     UserExist: 用户是否存在
        ///     Lock: 账户是否被锁定
        /// </returns>
        [Route("SignIn")]
        [EmptyParameterFilter("request")]
        [ValidateModelState(Order = 1)]
        [ResponseType(typeof(SignInResponse))]
        public async Task<IHttpActionResult> SignIn(SignInRequest request)
        {
            if (this.CurrentUser.Cellphone == request.Name && this.CurrentUser.ExpiryTime > DateTime.Now.AddMinutes(5))
            {
                var rCode = DesHelper.DesEncrypt(string.Format("{0},{1}", CurrentUser.Identifier, DateTime.Now.AddMinutes(1).ToBinary()));
                return this.Ok(new SignInResponse { RemainCount = 5, Successful = true, UserExist = true, RCode = rCode });
            }
            SignInResult signInResult = await this.SignInAsync(request.Name, request.Password);
            //RequestContext.

            //resp.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            HttpContext.Current.Response.AddHeader("P3P", "CP=CAO PSA OUR");
            return this.Ok(signInResult.ToSignInResponseModel());

            // 为移动端新注册用户，派发本金券
            //if (signInResult.Successful)
            //{
            //    CurrentUser user = await this.userInfoService.GetCurrentUser(request.Name);
            //    if (!String.IsNullOrEmpty(user.Identifier))
            //    {
            //        // ReSharper disable once UnusedVariable
            //        Task t = this.itemService.CollectActivitisItems(user.Identifier, DateTime.Now.Date.AddDays(ApplicationConfig.Item.OHPItem.ValidityDuration));
            //    }
            //}
        }

        /// <summary>
        ///     登出
        /// </summary>
        /// <returns>
        ///     200
        /// </returns>
        [Route("SignOut")]
        [HttpGet]
        public IHttpActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            this.SetSignOutAtOld();
            return this.OK();
        }

        /// <summary>
        ///     注册
        /// </summary>
        /// <param name="request">
        ///     Password[string]: 用户设置的密码（6-18位）
        ///     Token[string]: 验证码口令
        ///     Code[string]: 识别码
        ///     ClientType[long?]: 客户端标识(推广相关)
        ///     ContractId[long?]: 活动标识(推广相关)
        ///     FlgMoreI1[long?]: FlgMoreI1(推广相关)
        ///     FlgMoreI2[long?]: FlgMoreI2(推广相关)
        ///     FlgMoreS1[string]: FlgMoreS1(推广相关)
        ///     FlgMoreS2[string]: FlgMoreS2(推广相关)
        ///     InviteBy[string]: 邀请码(推广相关)
        /// </param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     No Content
        ///     @{h2@} HttpStatusCode:400 @{/h2@}
        ///     "请输入正确的验证码"
        ///     "此号码已注册，请选择登录，如果您忘记密码，请选择重置密码"
        /// </returns>
        [Route("SignUp")]
        [EmptyParameterFilter("request")]
        [ValidateModelState(Order = 1)]
        public async Task<IHttpActionResult> SignUp(SignUpRequest request)
        {
            UseVeriCodeResult result = await this.veriCodeService.UseAsync(request.Token, VeriCode.VeriCodeType.SignUp);

            if (!result.Result)
            {
                return this.BadRequest("请输入正确的验证码");
            }
            string userId = "";
            try
            {
                userId = await this.userService.GetSignUpUserId(result.Cellphone);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (userId.Equals(""))
            {
                return this.BadRequest("此号码已注册，请选择登录，如果您忘记密码，请选择重置密码");
            }

            this.commandBus.Excute(new RegisterANewUser(userId)
            {
                Cellphone = result.Cellphone,
                Password = request.Password,
                IdentificionCode = request.Code,
                UserType = 10,
                ClientType = request.ClientType,
                ContractId = request.ContractId,
                FlgMoreS1 = request.FlgMoreS1.ToStringIncludeNull(),
                FlgMoreS2 = request.FlgMoreS2.ToStringIncludeNull(),
                FlgMoreI1 = request.FlgMoreI1,
                FlgMoreI2 = request.FlgMoreI2,
                IpReg = HttpContext.Current.Request.UserHostAddress,
                InviteBy = request.InviteBy.ToStringIncludeNull()
            });

            // 自动登陆
            await this.SignInAsync(result.Cellphone, request.Password);

            return this.OK();

            //await this.authenticationService.SignUp(info.Cellphone, request.Password);

            // 为移动端新注册用户，派发本金券
            //Domian.Passport.Services.CurrentUser user = await this.userInfoService.GetCurrentUser(info.Cellphone);
            //if (!String.IsNullOrEmpty(user.Identifier))
            //{
            //    // ReSharper disable once UnusedVariable
            //    Task t = this.itemService.CollectActivitisItems(user.Identifier, DateTime.Now.Date.AddDays(ApplicationConfig.Item.OHPItem.ValidityDuration));
            //}

            // ReSharper disable once UnusedVariable
            // 为用户添加来源信息
            //Task addSourceInfoTask = this.userService.AddSourceInfo(info.Cellphone, this.Request.Headers);
        }

        /// <summary>
        ///     开通快捷支付，并且绑定第一张借记卡
        /// </summary>
        /// <param name="request">
        ///     BankCardNo[string]: 银行卡号（15-19位）
        ///     BankName[string]: 银行名称
        ///     CityName[string]: 开户行所在城市名称
        ///     Credential[int]: 证件类型 0 =&gt; 身份证, 1 =&gt; 护照, 2 =&gt; 台湾, 3 =&gt; 军官
        ///     CredentialNo[string]: 证件号码
        ///     RealName[string]: 真实姓名
        ///     ClientType[long?]: 客户端标识(推广相关)
        ///     FlgMoreI1[long?]: FlgMoreI1(推广相关)
        ///     FlgMoreI2[long?]: FlgMoreI2(推广相关)
        ///     FlgMoreS1[string]: FlgMoreS1(推广相关)
        ///     FlgMoreS2[string]: FlgMoreS2(推广相关)
        /// </param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     Empty Body
        ///     @{h2@} HttpStatusCode: 400 @{/h2@}
        ///     "无法开通快捷支付功能"
        ///     "请先设定支付密码"
        ///     "已经开通快捷支付功能"
        ///     "正在开通快捷支付功能，请耐心等待"
        ///     "该银行卡已经被绑定"
        ///     "该证件已经被绑定"
        /// </returns>
        [HttpPost, Route("SignUpPayment")]
        [TokenAuthorize]
        [EmptyParameterFilter("request", Order = 1), ValidateModelState(Order = 2)]
        public async Task<IHttpActionResult> SignUpPayment(SignUpPaymentRequest request)
        {
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Identifier);

            if (userInfo == null)
            {
                this.Warning("Can not load user date.{0}".FormatWith(this.CurrentUser.Identifier));
                return this.BadRequest("无法开通快捷支付功能");
            }

            if (!userInfo.HasSetPaymentPassword)
            {
                return this.BadRequest("请先设定支付密码");
            }

            if (userInfo.Verified.HasValue && userInfo.Verified.Value)
            {
                return this.BadRequest("已经开通快捷支付功能");
            }

            if (userInfo.Verified.HasValue && !userInfo.Verified.Value)
            {
                return this.BadRequest("正在开通快捷支付功能，请耐心等待");
            }

            if (await this.userService.CheckBankCardAvaliableAsync(request.BankCardNo))
            {
                return this.BadRequest("该银行卡已经被绑定");
            }

            if (!await this.userService.CheckCredentialNoAvaliableAsync(request.CredentialNo))
            {
                return this.BadRequest("该证件已经被绑定");
            }

            if (await this.userInfoService.GetAddBankFailCountAsync(userInfo.UserIdentifier, request.BankCardNo) >= 5)
            {
                return this.BadRequest("对不起，该卡失败次数已超过5次，请您明天再试。");
            }

            SignUpPayment command = this.BuildSignUpPaymentCommand(request, userInfo);
            this.commandBus.Excute(command);
            return this.OK();
        }

        /// <summary>
        ///     Builds the add bank card command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        private AddBankCard BuildAddBankCardCommand(AddBankCardRequest request, UserInfo info)
        {
            AddBankCard command = new AddBankCard(this.CurrentUser.Identifier)
            {
                BankCardNo = request.BankCardNo,
                BankName = request.BankName,
                Cellphone = info.Cellphone,
                CityName = request.CityName,
                Credential = info.Credential.GetValueOrDefault(),
                CredentialNo = info.CredentialNo,
                RealName = info.RealName,
                ClientType = request.ClientType,
                FlgMoreI1 = request.FlgMoreI1,
                FlgMoreI2 = request.FlgMoreI2,
                FlgMoreS1 = request.FlgMoreS1.ToStringIncludeNull(),
                FlgMoreS2 = request.FlgMoreS2.ToStringIncludeNull(),
                IpClient = HttpContext.Current.Request.UserHostAddress
            };

            return command;
        }

        /// <summary>
        ///     Builds the sign up payment command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        private SignUpPayment BuildSignUpPaymentCommand(SignUpPaymentRequest request, UserInfo info)
        {
            SignUpPayment command = new SignUpPayment(this.CurrentUser.Identifier)
            {
                BankCardNo = request.BankCardNo,
                BankName = request.BankName,
                Cellphone = info.Cellphone,
                CityName = request.CityName,
                Credential = request.Credential,
                CredentialNo = request.CredentialNo,
                RealName = request.RealName,
                ClientType = request.ClientType,
                FlgMoreI1 = request.FlgMoreI1,
                FlgMoreI2 = request.FlgMoreI2,
                FlgMoreS1 = request.FlgMoreS1.ToStringIncludeNull(),
                FlgMoreS2 = request.FlgMoreS2.ToStringIncludeNull(),
                IpClient = HttpContext.Current.Request.UserHostAddress
            };

            if (info.HasYSBInfo)
            {
                command.RealName = info.YSBRealName;
                command.Credential = Credential.IdCard;
                command.CredentialNo = info.YSBIdCard;
            }

            return command;
        }

        /// <summary>
        ///     Sets the cookie in HTTP context.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="expires">The expires.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="path">The path.</param>
        /// <param name="secure">if set to <c>true</c> [secure].</param>
        private void SetCookieInHttpContext(string name, string value, DateTime expires, string domain, string path = "/", bool secure = false)
        {
            HttpCookie cookie = new HttpCookie(name, value);
            cookie.Expires = expires;
            cookie.Domain = domain;
            cookie.Path = path;
            cookie.Secure = secure;
            HttpContext.Current.Response.SetCookie(cookie);
        }

        /// <summary>
        ///     Sets the sign in at old.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        private void SetSignInAtOld(string userIdentifier)
        {
            UserSignInSucceeded userSignInSucceeded = new UserSignInSucceeded(userIdentifier, typeof(User));
            userSignInSucceeded.AmpAuthToken = GuidUtils.NewGuidString();
            userSignInSucceeded.GoldCatAuthToken = GuidUtils.NewGuidString();
            this.SetCookieInHttpContext("AmpAuthToken", userSignInSucceeded.AmpAuthToken, DateTime.Now.AddMonths(3), ".jinyinmao.com.cn");
            this.eventBus.Publish(userSignInSucceeded);
        }

        private void SetSignOutAtOld()
        {
            this.SetCookieInHttpContext("AmpAuthToken", "", DateTime.Now.AddMonths(-3), ".jinyinmao.com.cn");
        }

        /// <summary>
        ///     Signs the in asynchronous.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        private async Task<SignInResult> SignInAsync(string loginName, string password)
        {
            SignInResult signInResult = await this.userService.SignInAsync(loginName, password);

            if (signInResult.Successful)
            {
                bool isMobileDevice = HttpUtils.IsMobileDevice(this.Request);
                UserLoginInfo user = signInResult.UserLoginInfo;
                DateTime expiry = isMobileDevice ? DateTime.Now.AddDays(14) : DateTime.Now.AddMinutes(30);
                if (!isMobileDevice)
                {
                    this.SetSignInAtOld(signInResult.UserLoginInfo.UserIdentifier);
                }
                FormsAuthentication.SetAuthCookie(string.Format("{0},{1},{2}", user.UserIdentifier, user.LoginName, expiry.ToBinary()), true);
                //生成兴业联合登陆加密串
                signInResult.RCode = DesHelper.DesEncrypt(string.Format("{0},{1}", user.UserIdentifier, DateTime.Now.AddMinutes(1).ToBinary()));
            }
            return signInResult;
        }
    }
}
