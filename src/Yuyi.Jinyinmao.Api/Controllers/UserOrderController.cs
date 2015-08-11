// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : UserOrderController.cs
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-31  12:39 PM
// ***********************************************************************
// <copyright file="UserOrderController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     UserOrderController.
    /// </summary>
    [RoutePrefix("User/Order")]
    public class UserOrderController : ApiControllerBase
    {
        private readonly IUserInfoService userInfoService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserOrderController" /> class.
        /// </summary>
        /// <param name="userInfoService">The user information service.</param>
        public UserOrderController(IUserInfoService userInfoService)
        {
            this.userInfoService = userInfoService;
        }

        /// <summary>
        ///     订单列表
        /// </summary>
        /// <remarks>
        ///     每页数量为10个，页数从0开始。
        /// </remarks>
        /// <param name="index">页码，从0开始，最小为0</param>
        /// <param name="sortMode">排序规则 1 => 按下单时间排序，2 => 按结息日期排序</param>
        /// <response code="200"></response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Index/{index:min(0):int=0}/{sortMode:min(1):int=1}"), CookieAuthorize, ResponseType(typeof(PaginatedResponse<OrderInfoResponse>))]
        public async Task<IHttpActionResult> Index(int index = 0, int sortMode = 1)
        {
            index = index < 0 ? 0 : index;

            OrdersSortMode ordersSortMode = (OrdersSortMode)Enum.ToObject(typeof(OrdersSortMode), sortMode);

            PaginatedList<OrderInfo> infos = await this.userInfoService.GetOrderInfosAsync(this.CurrentUser.Id, index, 10, ordersSortMode);

            return this.Ok(infos.ToPaginated(t => t.ToResponse()).ToResponse());
        }

        /// <summary>
        ///     订单列表
        /// </summary>
        /// <remarks>
        ///     根据产品分类查询登录用户订单，默认查询所有订单
        ///     每页数量为10个，页数从0开始。
        /// </remarks>
        /// <param name="index">页码，从0开始，最小为0</param>
        /// <param name="sortMode">排序规则 1 => 按下单时间排序，2 => 按结息日期排序</param>
        /// <param name="categories">产品分类，默认值为100000010，详细的产品分类参考文档，可以传递数组 </param>
        /// <response code="200"></response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("IndexByCategory/{index:min(0):int=0}/{sortMode:min(1):int=1}"), CookieAuthorize, ResponseType(typeof(PaginatedResponse<OrderInfoResponse>))]
        public async Task<IHttpActionResult> IndexByCategory(int index = 0, int sortMode = 1, [FromUri] long[] categories = null)
        {
            index = index < 0 ? 0 : index;

            if (categories == null)
            {
                categories = new long[] { 100000010 };
            }

            OrdersSortMode ordersSortMode = (OrdersSortMode)Enum.ToObject(typeof(OrdersSortMode), sortMode);

            PaginatedList<OrderInfo> infos = await this.userInfoService.GetOrderInfosAsync(this.CurrentUser.Id, index, 10, ordersSortMode, categories);

            return this.Ok(infos.ToPaginated(t => t.ToResponse()).ToResponse());
        }

        /// <summary>
        ///     订单详情
        /// </summary>
        /// <remarks>
        ///     订单详情。订单详情数据会有1分钟缓存时间。
        /// </remarks>
        /// <param name="orderIdentifier">订单唯一标识</param>
        /// <response code="200"></response>
        /// <response code="400">UOI:订单不存在</response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Info/{orderIdentifier:length(32)}"), CookieAuthorize, ResponseType(typeof(OrderInfoResponse))]
        public async Task<IHttpActionResult> Info(string orderIdentifier)
        {
            Guid orderId;
            if (!Guid.TryParseExact(orderIdentifier, "N", out orderId))
            {
                return this.BadRequest("UOI:订单不存在");
            }

            OrderInfo info = await this.userInfoService.GetOrderInfoAsync(this.CurrentUser.Id, orderId);
            if (info == null)
            {
                return this.BadRequest("UOI:订单不存在");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     即将结息的订单列表
        /// </summary>
        /// <remarks>
        ///     默认数量为5
        /// </remarks>
        /// <param name="count">订单数量</param>
        /// <response code="200"></response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Settling/{count:min(0):int=5}"), CookieAuthorize, ResponseType(typeof(List<OrderInfoResponse>))]
        public async Task<IHttpActionResult> Settling(int count = 5)
        {
            count = count < 0 ? 1 : count;

            List<OrderInfo> infos = await this.userInfoService.GetSettlingOrderInfosAsync(this.CurrentUser.Id, count);

            return this.Ok(infos.Select(o => o.ToResponse()).ToList());
        }
    }
}