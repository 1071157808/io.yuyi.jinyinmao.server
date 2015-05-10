// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-08  1:54 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-09  12:37 AM
// ***********************************************************************
// <copyright file="UserOrderController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Api.Models.Order;
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
        /// <param name="categories">产品分类，默认值为100000010，详细的产品分类参考文档，可以传递数组 </param>
        /// <response code="200"></response>
        /// <response code="401">UAUTH1:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Index/{index:min(0):int=0}/{sortMode:min(1):int=1}"), CookieAuthorize, ResponseType(typeof(PaginatedResponse<OrderInfoResponse>))]
        public async Task<IHttpActionResult> Index(int index = 0, int sortMode = 1, [FromUri] long[] categories = null)
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
    }
}