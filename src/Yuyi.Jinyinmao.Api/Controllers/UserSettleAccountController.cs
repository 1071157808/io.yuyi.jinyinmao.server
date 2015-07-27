// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-02  11:37 PM
// ***********************************************************************
// <copyright file="UserSettleAccountController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
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
    ///     UserSettleAccountController.
    /// </summary>
    [RoutePrefix("User/Settle")]
    public class UserSettleAccountController : ApiControllerBase
    {
        private readonly IUserInfoService userInfoService;
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserSettleAccountController" /> class.
        /// </summary>
        /// <param name="userInfoService">The user information service.</param>
        /// <param name="userService">The user service.</param>
        public UserSettleAccountController(IUserInfoService userInfoService, IUserService userService)
        {
            this.userInfoService = userInfoService;
            this.userService = userService;
        }

        /// <summary>
        ///     账户充值（Yilian）
        /// </summary>
        /// <remarks>
        ///     必须登录，并且使用的银行卡必须经过认证
        /// </remarks>
        /// <param name="request">
        ///     账户充值请求
        /// </param>
        /// <response code="200"></response>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     USAD1:请重置支付密码后再试
        ///     <br />
        ///     USAD2:支付密码错误，支付密码输入错误5次会锁定支付功能
        ///     <br />
        ///     USAD3:该银行卡不能用于易联支付
        /// </response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [Route("Deposit/Yilian"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> DepositFromYilian(DepositFromYilianRequest request)
        {
            CheckPaymentPasswordResult result = await this.userService.CheckPaymentPasswordAsync(this.CurrentUser.Id, request.PaymentPassword);

            if (result.Lock)
            {
                return this.BadRequest("USAD1:请重置支付密码后再试");
            }

            if (!result.Success)
            {
                return this.BadRequest("USAD2:支付密码错误，支付密码输入错误5次会锁定支付功能");
            }

            BankCardInfo bankCardInfo = await this.userInfoService.GetBankCardInfoAsync(this.CurrentUser.Id, request.BankCardNo);

            if (bankCardInfo == null || !bankCardInfo.Dispaly || !bankCardInfo.Verified || !bankCardInfo.VerifiedByYilian)
            {
                return this.BadRequest("USAD3:该银行卡不能用于签约支付");
            }

            await this.userService.DepositAsync(new PayByYilian
            {
                Amount = request.Amount,
                Args = this.BuildArgs(),
                BankCardNo = request.BankCardNo,
                UserId = this.CurrentUser.Id
            });

            return this.Ok();
        }

        /// <summary>
        ///     钱包账户信息
        /// </summary>
        /// <remarks>
        ///     必须登录
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="400">USAI:查询不到该账户的信息</response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Info"), CookieAuthorize, ResponseType(typeof(SettleAccountInfoResponse))]
        public async Task<IHttpActionResult> Info()
        {
            SettleAccountInfo info = await this.userInfoService.GetSettleAccountInfoAsync(this.CurrentUser.Id);

            if (info == null)
            {
                return this.BadRequest("USAI:查询不到该账户的信息");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     钱包流水信息
        /// </summary>
        /// <remarks>
        ///     必须登录
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="400">USAT1:交易流水不存在</response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Transaction/{transactionIdentifier:length(32)}"), CookieAuthorize, ResponseType(typeof(SettleAccountTransactionInfoResponse))]
        public async Task<IHttpActionResult> Transaction(string transactionIdentifier)
        {
            Guid transactionId;
            if (!Guid.TryParseExact(transactionIdentifier, "N", out transactionId))
            {
                return this.BadRequest("USAT1:交易流水不存在");
            }

            SettleAccountTransactionInfo info = await this.userInfoService.GetSettleAccountTransactionInfoAsync(this.CurrentUser.Id, transactionId);

            if (info == null)
            {
                return this.BadRequest("USAT1:交易流水不存在");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     钱包流水信息
        /// </summary>
        /// <remarks>
        ///     必须登录，每页10条信息，页码从0开始，
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="400">USAT1:交易流水不存在</response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Transactions/{pageIndex:int=0}"), CookieAuthorize, ResponseType(typeof(PaginatedResponse<SettleAccountTransactionInfoResponse>))]
        public async Task<IHttpActionResult> Transactions(int pageIndex = 0)
        {
            pageIndex = pageIndex < 0 ? 0 : pageIndex;

            PaginatedList<SettleAccountTransactionInfo> infos = await this.userInfoService.GetSettleAccountTransactionInfosAsync(this.CurrentUser.Id, pageIndex, 10);

            if (infos == null)
            {
                return this.BadRequest("USAT1:交易流水不存在");
            }

            return this.Ok(infos.ToPaginated(i => i.ToResponse()).ToResponse());
        }

        /// <summary>
        ///     账户取现
        /// </summary>
        /// <remarks>
        ///     必须登录，并且使用的银行卡必须经过认证
        /// </remarks>
        /// <param name="request">
        ///     账户取现请求
        /// </param>
        /// <response code="200"></response>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     USAW1:暂时无法取现
        ///     <br />
        ///     USAW2:取现次数已经达到今日上限
        ///     <br />
        ///     USAW3:该银行卡不能用于取现
        ///     <br />
        ///     USAW4:取现额度超过限制
        ///     <br />
        ///     USAW5:请重置支付密码后再试
        ///     <br />
        ///     USAW6:支付密码错误，支付密码输入错误5次会锁定支付功能
        ///     <br />
        ///     USAW7:取现失败
        /// </response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [Route("Withdrawal"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(SettleAccountTransactionInfoResponse))]
        public async Task<IHttpActionResult> Withdrawal(WithdrawalRequest request)
        {
            CheckPaymentPasswordResult result = await this.userService.CheckPaymentPasswordAsync(this.CurrentUser.Id, request.PaymentPassword);

            if (result.Lock)
            {
                return this.BadRequest("USAW5:请重置支付密码后再试");
            }

            if (!result.Success)
            {
                return this.BadRequest("USAW6:支付密码错误，支付密码输入错误5次会锁定支付功能");
            }

            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);

            if (userInfo == null)
            {
                this.TraceWriter.Warn(this.Request, "Application", "UserSettleAccount-Withdrawal:Can not load user data.{0}".FormatWith(this.CurrentUser.Id));
                return this.BadRequest("USAW1:暂时无法取现");
            }

            if (userInfo.TodayWithdrawalCount >= VariableHelper.DailyWithdrawalLimitCount)
            {
                return this.BadRequest("USAW2:取现次数已经达到今日上限");
            }

            int charge = 0;
            if (userInfo.MonthWithdrawalCount >= VariableHelper.MonthFreeWithrawalLimitCount)
            {
                charge = VariableHelper.WithdrawalChargeFee;
            }

            if (userInfo.Balance < request.Amount + charge)
            {
                return this.BadRequest("USAW4:取现额度超过限制");
            }

            BankCardInfo bankCardInfo = await this.userInfoService.GetBankCardInfoAsync(this.CurrentUser.Id, request.BankCardNo);

            if (bankCardInfo == null || !bankCardInfo.Dispaly || !bankCardInfo.Verified || !bankCardInfo.VerifiedByYilian)
            {
                return this.BadRequest("USAW3:该银行卡不能用于取现");
            }

            if (bankCardInfo.WithdrawAmount < request.Amount)
            {
                return this.BadRequest("USAW4:取现额度超过限制");
            }

            SettleAccountTransactionInfo info = await this.userService.WithdrawalAsync(new Withdrawal
            {
                Amount = request.Amount,
                Args = this.BuildArgs(),
                BankCardNo = request.BankCardNo,
                UserId = this.CurrentUser.Id
            });

            if (info == null)
            {
                return this.BadRequest("USAW7:取现失败");
            }

            return this.Ok(info.ToResponse());
        }
    }
}