// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  1:28 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-25  1:26 AM
// ***********************************************************************
// <copyright file="UserJBYController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
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
using Yuyi.Jinyinmao.Packages.Helper;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     UserJBYController.
    /// </summary>
    [RoutePrefix("User/JBY")]
    public class UserJBYController : ApiControllerBase
    {
        private readonly IUserInfoService userInfoService;
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserJBYController" /> class.
        /// </summary>
        /// <param name="userInfoService">The user information service.</param>
        /// <param name="userService">The user service.</param>
        public UserJBYController(IUserInfoService userInfoService, IUserService userService)
        {
            this.userInfoService = userInfoService;
            this.userService = userService;
        }

        /// <summary>
        ///     金包银账户信息
        /// </summary>
        /// <remarks>
        ///     必须登录
        /// </remarks>
        /// <response code="200">成功</response>
        /// <response code="400">UJI:查询不到该账户的信息</response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Info"), CookieAuthorize, ResponseType(typeof(JBYAccountInfoResponse))]
        public async Task<IHttpActionResult> Info()
        {
            JBYAccountInfo info = await this.userInfoService.GetJBYAccountInfoAsync(this.CurrentUser.Id);

            if (info == null)
            {
                return this.BadRequest("UJI:查询不到该账户的信息");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     金包银流水信息
        /// </summary>
        /// <remarks>
        ///     必须登录
        /// </remarks>
        /// <response code="200">成功</response>
        /// <response code="400">UJT1:交易流水不存在</response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Transcation/{transcationIdentifier:length(32)}"), CookieAuthorize, ResponseType(typeof(JBYTranscationInfoResponse))]
        public async Task<IHttpActionResult> Transcation(string transcationIdentifier)
        {
            Guid transcationId;
            if (!Guid.TryParseExact(transcationIdentifier, "N", out transcationId))
            {
                return this.BadRequest("UJT1:交易流水不存在");
            }

            JBYAccountTranscationInfo info = await this.userInfoService.GetJBYAccountTranscationInfoAsync(this.CurrentUser.Id, transcationId);

            if (info == null)
            {
                return this.BadRequest("UJT1:交易流水不存在");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     金包银流水信息
        /// </summary>
        /// <remarks>
        ///     必须登录，每页10条信息，页码从0开始，
        /// </remarks>
        /// <response code="200">成功</response>
        /// <response code="400">USAT1:交易流水不存在</response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Transcations/{pageIndex:int=0}"), CookieAuthorize, ResponseType(typeof(PaginatedResponse<JBYTranscationInfoResponse>))]
        public async Task<IHttpActionResult> Transcations(int pageIndex = 0)
        {
            pageIndex = pageIndex < 0 ? 0 : pageIndex;

            PaginatedList<JBYAccountTranscationInfo> infos = await this.userInfoService.GetJBYAccountTranscationInfosAsync(this.CurrentUser.Id, pageIndex, 10);

            if (infos == null)
            {
                return this.BadRequest("USAT1:交易流水不存在");
            }

            return this.Ok(infos.ToPaginated(i => i.ToResponse()).ToResponse());
        }

        /// <summary>
        ///     金包银赎回
        /// </summary>
        /// <remarks>
        ///     必须登录，且暂时不支持直接赎回到银行卡
        /// </remarks>
        /// <param name="request">
        ///     账户取现请求
        /// </param>
        /// <response code="200">成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">UJW1:请重置支付密码后再试</response>
        /// <response code="400">UJW2:支付密码错误，支付密码输入错误5次会锁定支付功能</response>
        /// <response code="400">UJW3:暂时无法赎回</response>
        /// <response code="400">UJW4:赎回金额已经达到今日上限</response>
        /// <response code="400">UJW5:赎回金额超过限制</response>
        /// <response code="400">UJW6:取现失败</response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Withdrawal"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(JBYTranscationInfoResponse))]
        public async Task<IHttpActionResult> Withdrawal([FromUri] JBYWithdrawalRequest request)
        {
            CheckPaymentPasswordResult result = await this.userService.CheckPaymentPasswordAsync(this.CurrentUser.Id, request.PaymentPassword);

            if (result.Lock)
            {
                return this.BadRequest("UJW1:请重置支付密码后再试");
            }

            if (!result.Success)
            {
                return this.BadRequest("UJW2:支付密码错误，支付密码输入错误5次会锁定支付功能");
            }

            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);

            if (userInfo == null)
            {
                this.Trace.Warn(this.Request, "Application", "UserSettleAccount-Withdrawal:Can not load user data.{0}".FormatWith(this.CurrentUser.Id));
                return this.BadRequest("UJW3:暂时无法赎回");
            }

            if (userInfo.TodayJBYWithdrawalAmount + request.Amount > VariableHelper.DailyJBYWithdrawalAmountLimit)
            {
                return this.BadRequest("UJW4:赎回金额已经达到今日上限");
            }

            if (userInfo.JBYWithdrawalableAmount < request.Amount)
            {
                return this.BadRequest("UJW5:赎回金额超过限制");
            }

            JBYAccountTranscationInfo info = await this.userService.WithdrawalAsync(new JBYWithdrawal
            {
                Amount = request.Amount,
                Args = this.BuildArgs(),
                UserId = this.CurrentUser.Id
            });

            if (info == null)
            {
                return this.BadRequest("UJW6:取现失败");
            }

            return this.Ok(info.ToResponse());
        }
    }
}