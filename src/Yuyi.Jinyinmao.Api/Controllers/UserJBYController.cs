// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : UserJBYController.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-18  14:05
// ***********************************************************************
// <copyright file="UserJBYController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
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
        /// <response code="200"></response>
        /// <response code="400">UJI:查询不到该账户的信息</response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet]
        [Route("Info")]
        [CookieAuthorize]
        [ResponseType(typeof(JBYAccountInfoResponse))]
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
        ///     金包银复投流水信息
        /// </summary>
        /// <remarks>
        ///     必须登录，每页10条信息，页码从0开始，
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="400">USAT1:交易流水不存在</response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet]
        [Route("Transactions/Reinvesting/{pageIndex:int=0}")]
        [CookieAuthorize]
        [ResponseType(typeof(PaginatedResponse<JBYTransactionInfoResponse>))]
        public async Task<IHttpActionResult> ReinvestingTransactions(int pageIndex = 0)
        {
            pageIndex = pageIndex < 0 ? 0 : pageIndex;

            PaginatedList<JBYAccountTransactionInfo> infos = await this.userInfoService.GetJBYAccountReinvestingTransactionInfosAsync(this.CurrentUser.Id, pageIndex, 10);

            if (infos == null)
            {
                return this.BadRequest("USAT1:交易流水不存在");
            }

            return this.Ok(infos.ToPaginated(i => i.ToResponse()).ToResponse());
        }

        /// <summary>
        ///     金包银流水信息
        /// </summary>
        /// <remarks>
        ///     必须登录
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="400">UJT1:交易流水不存在</response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet]
        [Route("Transaction/{transactionIdentifier:length(32)}")]
        [CookieAuthorize]
        [ResponseType(typeof(JBYTransactionInfoResponse))]
        public async Task<IHttpActionResult> Transaction(string transactionIdentifier)
        {
            Guid transactionId;
            if (!Guid.TryParseExact(transactionIdentifier, "N", out transactionId))
            {
                return this.BadRequest("UJT1:交易流水不存在");
            }

            JBYAccountTransactionInfo info = await this.userInfoService.GetJBYAccountTransactionInfoAsync(this.CurrentUser.Id, transactionId);

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
        /// <response code="200"></response>
        /// <response code="400">USAT1:交易流水不存在</response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet]
        [Route("Transactions/{pageIndex:int=0}")]
        [CookieAuthorize]
        [ResponseType(typeof(PaginatedResponse<JBYTransactionInfoResponse>))]
        public async Task<IHttpActionResult> Transactions(int pageIndex = 0)
        {
            pageIndex = pageIndex < 0 ? 0 : pageIndex;

            PaginatedList<JBYAccountTransactionInfo> infos = await this.userInfoService.GetJBYAccountTransactionInfosAsync(this.CurrentUser.Id, pageIndex, 10);

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
        /// <response code="200"></response>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     UJW1:请重置支付密码后再试
        ///     <br />
        ///     UJW2:支付密码错误&lt;br&gt;错误5次会锁定支付功能
        ///     <br />
        ///     UJW3:暂时无法赎回
        ///     <br />
        ///     UJW4:赎回金额已经达到今日上限
        ///     <br />
        ///     UJW5:赎回金额超过限制
        ///     <br />
        ///     UJW6:取现失败
        /// </response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [Route("Withdrawal")]
        [CookieAuthorize]
        [ActionParameterRequired]
        [ActionParameterValidate(Order = 1)]
        [ResponseType(typeof(JBYTransactionInfoResponse))]
        public async Task<IHttpActionResult> Withdrawal(JBYWithdrawalRequest request)
        {
            CheckPaymentPasswordResult result = await this.userService.CheckPaymentPasswordAsync(this.CurrentUser.Id, request.PaymentPassword);

            if (result.Lock)
            {
                return this.BadRequest("UJW1:请重置支付密码后再试");
            }

            if (!result.Success)
            {
                return this.BadRequest("UJW2:支付密码错误<br>错误5次会锁定支付功能");
            }

            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);
            if (userInfo == null)
            {
                return this.BadRequest("USERNOTFOUND:用户信息加载错误，请重新登陆");
            }

            if (userInfo.Closed)
            {
                return this.BadRequest("UC:该账户已经被锁定，请联系金银猫客服");
            }

            if (userInfo.TodayJBYWithdrawalAmount + request.Amount > VariableHelper.DailyJBYWithdrawalAmountLimit)
            {
                return this.BadRequest("UJW4:赎回金额已经达到今日上限");
            }

            if (userInfo.JBYWithdrawalableAmount < request.Amount)
            {
                return this.BadRequest("UJW5:赎回金额超过限制");
            }

            JBYAccountTransactionInfo info = await this.userService.JBYWithdrawalAsync(this.BuildJBYWithdrawalCommand(request));

            if (info == null)
            {
                return this.BadRequest("UJW6:取现失败");
            }

            return this.Ok(info.ToResponse());
        }

        private JBYWithdrawal BuildJBYWithdrawalCommand(JBYWithdrawalRequest request)
        {
            return new JBYWithdrawal
            {
                EntityId = this.CurrentUser.Id,
                Amount = request.Amount,
                Args = this.BuildArgs(),
                UserId = this.CurrentUser.Id
            };
        }
    }
}