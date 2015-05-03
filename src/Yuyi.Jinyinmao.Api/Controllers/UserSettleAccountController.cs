// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  3:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  1:34 AM
// ***********************************************************************
// <copyright file="UserSettleAccountController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using System.Web.Http;
using Moe.AspNet.Filters;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
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
        /// <response code="200">成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">USAD1:该银行卡不能用于易联支付</response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [Route("Deposit/Yilian"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> Deposit(DepositFromYilianRequest request)
        {
            BankCardInfo bankCardInfo = await this.userInfoService.GetBankCardInfoAsync(this.CurrentUser.Id, request.BankCardNo);

            if (bankCardInfo == null || !bankCardInfo.Verified || !bankCardInfo.CanBeUsedForYilian)
            {
                return this.BadRequest("USAD1:该银行卡不能用于易联支付");
            }

            await this.userService.DepositAsync(new DepositFromYilian
            {
                Amount = request.Amount,
                Args = this.BuildArgs(),
                BankCardNo = request.BankCardNo,
                UserId = this.CurrentUser.Id
            });

            return this.Ok();
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
        /// <response code="200">成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">USAD1:该银行卡未经过认证</response>
        /// <response code="400">USAD2:取现额度超过限制</response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [Route("Withdrawal"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> Withdrawal(WithdrawalRequest request)
        {
            BankCardInfo bankCardInfo = await this.userInfoService.GetBankCardInfoAsync(this.CurrentUser.Id, request.BankCardNo);

            if (bankCardInfo == null || !bankCardInfo.Verified)
            {
                return this.BadRequest("USAD2:该银行卡未经过认证");
            }

            if (bankCardInfo.WithdrawAmount < request.Amount)
            {
                return this.BadRequest("USAD2:取现额度超过限制");
            }

            await this.userService.WithdrawalAsync(new Withdrawal
            {
                Amount = request.Amount,
                Args = this.BuildArgs(),
                BankCardNo = request.BankCardNo,
                UserId = this.CurrentUser.Id
            });

            return this.Ok();
        }
    }
}