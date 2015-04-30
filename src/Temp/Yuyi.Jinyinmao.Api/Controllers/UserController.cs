// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-28  10:51 AM
// ***********************************************************************
// <copyright file="UserController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Tracing;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     Class UserController.
    /// </summary>
    [RoutePrefix("User")]
    public class UserController : ApiControllerBase
    {
        private readonly IUserInfoService userInfoService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        /// <param name="userInfoService">The user information service.</param>
        public UserController(IUserInfoService userInfoService)
        {
            this.userInfoService = userInfoService;
        }

        /// <summary>
        ///     获取用户信息
        /// </summary>
        /// <remarks>
        ///     用户未登录会返回401
        /// </remarks>
        /// <response code="200">认证成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">UG:无法获取用户信息</response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route(""), CookieAuthorize, ResponseType(typeof(UserInfoResponse))]
        public async Task<IHttpActionResult> Get()
        {
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Id);
            if (userInfo == null)
            {
                this.Trace.Warn(this.Request, "Application", "User-GetBankCards:Can not load user data.{0}".FormatWith(this.CurrentUser.Id));
                return this.BadRequest("UG:无法获取用户信息");
            }

            return this.Ok(userInfo.ToResponse());
        }

        /// <summary>
        ///     获取用户账户信息（合并金银猫、兴业、富滇）
        /// </summary>
        /// <returns>
        ///     TotalInterest[decimal]: 总收益
        ///     ExpectedInterest[decimal]: 预期收益
        ///     InvestingPrincipal[decimal]: 在投金额
        ///     IncomePerMinute[decimal]: 以分钟为单位的赚钱速度
        ///     DefeatedPercent[int]: 打败的百分比
        /// </returns>
        [HttpGet, Route("InvestingInfo")]
        //[ResponseType(typeof(UserInvestingInfoResponse))]
        public async Task<IHttpActionResult> GetInvestingInfo()
        {
            throw new NotImplementedException();
            //            //金银猫站点
            //            decimal maxIncomeSpeed = await this.orderInfoService.GetMaxIncomeSpeedAsync();
            //            decimal userIncomeSpeed = await this.orderInfoService.GetTheUserIncomeSpeedAsync(this.CurrentUser.Identifier);
            //            InvestingInfo investingInfo = await this.orderInfoService.GetInvestingInfoAsync(this.CurrentUser.Identifier);
            //
            //            //附加上兴业站点
            //            decimal xyMaxIncomeSpeed = await this.xyOrderInfoService.GetMaxIncomeSpeedAsync();
            //            maxIncomeSpeed = maxIncomeSpeed >= xyMaxIncomeSpeed ? maxIncomeSpeed : xyMaxIncomeSpeed;
            //            userIncomeSpeed += await this.xyOrderInfoService.GetTheUserIncomeSpeedAsync(this.CurrentUser.Identifier);
            //            Xingye.Domain.Orders.Services.DTO.InvestingInfo xyInvestingInfo = await this.xyOrderInfoService.GetInvestingInfoAsync(this.CurrentUser.Identifier);
            //            var allInvestingInfo = new InvestingInfo
            //            {
            //                Interest = investingInfo.Interest + xyInvestingInfo.Interest,
            //                Principal = investingInfo.Principal + xyInvestingInfo.Principal,
            //                TotalInterest = investingInfo.TotalInterest + xyInvestingInfo.TotalInterest
            //            };
            //            UserInvestingInfoResponse response = new UserInvestingInfoResponse(maxIncomeSpeed, userIncomeSpeed, allInvestingInfo);
            //            return this.Ok(response);
        }
    }
}
