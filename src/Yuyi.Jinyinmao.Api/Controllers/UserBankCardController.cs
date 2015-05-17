// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  1:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-14  5:30 PM
// ***********************************************************************
// <copyright file="UserBankCardController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
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
    ///     UserBankCardController.
    /// </summary>
    [RoutePrefix("User/BankCards")]
    public class UserBankCardController : ApiControllerBase
    {
        private readonly IUserInfoService userInfoService;
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserBankCardController" /> class.
        /// </summary>
        /// <param name="userInfoService">The user information service.</param>
        /// <param name="userService">The user service.</param>
        public UserBankCardController(IUserInfoService userInfoService, IUserService userService)
        {
            this.userInfoService = userInfoService;
            this.userService = userService;
        }

        /// <summary>
        ///     添加新银行卡
        /// </summary>
        /// <remarks>
        ///     该接口知会添加银行卡信息，不会对银行卡的信息进行校验
        /// </remarks>
        /// <param name="request">
        ///     添加银行卡请求
        /// </param>
        /// <response code="200">成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">UBCABC1:无法添加银行卡</response>
        /// <response code="400">UBCABC2:最多绑定10张银行卡</response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [Route("AddBankCard"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> AddBankCard(AddBankCardRequest request)
        {
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);

            if (userInfo == null)
            {
                this.Trace.Warn(this.Request, "Application", "User-AddBankCard:Can not load user data.{0}".FormatWith(this.CurrentUser.Id));
                return this.BadRequest("UBCABC1:无法添加银行卡");
            }

            if (userInfo.BankCardsCount >= 10)
            {
                return this.BadRequest("UBCABC2:最多绑定10张银行卡");
            }

            await this.userService.AddBankCardAsync(new AddBankCard
            {
                BankCardNo = request.BankCardNo,
                BankName = request.BankName,
                CityName = request.CityName,
                UserId = this.CurrentUser.Id,
                Args = this.BuildArgs()
            });

            return this.Ok();
        }

        /// <summary>
        ///     添加新银行卡（通过易联）
        /// </summary>
        /// <remarks>
        ///     该接口只适合已经绑定过实名信息的用户添加新银行卡，最多只能绑定10张银行卡
        /// </remarks>
        /// <param name="request">
        ///     添加银行卡请求
        /// </param>
        /// <response code="200">成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">UBCABCBY1:无法添加银行卡</response>
        /// <response code="400">UBCABCBY2:请先进行实名认证</response>
        /// <response code="400">UBCABCBY3:最多绑定10张银行卡</response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [Route("AddBankCardByYilian"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> AddBankCardByYilian(AddBankCardRequest request)
        {
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);

            if (userInfo == null)
            {
                this.Trace.Warn(this.Request, "Application", "User-AddBankCard:Can not load user data.{0}".FormatWith(this.CurrentUser.Id));
                return this.BadRequest("UBCABC1:无法添加银行卡");
            }

            if (!userInfo.Verified)
            {
                return this.BadRequest("UBCABC2:请先进行实名认证");
            }

            if (userInfo.BankCardsCount >= 10)
            {
                return this.BadRequest("UBCABC3:最多绑定10张银行卡");
            }

            await this.userService.AddBankCardAsync(new AddBankCard
            {
                BankCardNo = request.BankCardNo,
                BankName = request.BankName,
                CityName = request.CityName,
                UserId = this.CurrentUser.Id,
                Args = this.BuildArgs()
            });

            return this.Ok();
        }

        /// <summary>
        ///     获取用户银行卡信息
        /// </summary>
        /// <remarks>
        ///     会获取所有的银行卡信息，无分页功能，无排序
        /// </remarks>
        /// <response code="200">注册成功</response>
        /// <response code="400">UBCI:无法获取用户信息</response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Index"), CookieAuthorize, ResponseType(typeof(List<BankCardInfoResponse>))]
        public async Task<IHttpActionResult> Index()
        {
            List<BankCardInfo> cards = await this.userInfoService.GetBankCardInfosAsync(this.CurrentUser.Id);
            if (cards == null)
            {
                this.Trace.Warn(this.Request, "Application", "User-GetBankCards:Can not load user bank cards data.{0}".FormatWith(this.CurrentUser.Id));
                return this.BadRequest("UBCI:无法获取用户信息");
            }

            return this.Ok(cards.Select(c => c.ToResponse()).ToList());
        }

        /// <summary>
        ///     设置默认银行卡
        /// </summary>
        /// <remarks>
        ///     参数传递直接传递银行卡号，后续需要修改
        /// </remarks>
        /// <param name="bankCardNo">bankCardNo[string]:银行卡号（15-19位）</param>
        /// <response code="200">重置成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">UBCSDBC:银行卡信息错误</response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("SetDefaultBankCard/{bankCardNo}"), CookieAuthorize]
        public async Task<IHttpActionResult> SetDefaultBankCard(string bankCardNo)
        {
            BankCardInfo info = await this.userInfoService.GetBankCardInfoAsync(this.CurrentUser.Id, bankCardNo);
            if (info == null)
            {
                return this.BadRequest("UBCSDBC:银行卡信息错误");
            }

            if (!info.IsDefault)
            {
                await this.userService.SetDefaultBankCardAsync(this.CurrentUser.Id, bankCardNo);
            }

            return this.Ok();
        }
    }
}