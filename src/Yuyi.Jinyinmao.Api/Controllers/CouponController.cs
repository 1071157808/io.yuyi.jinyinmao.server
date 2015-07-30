// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : CouponController.cs
// Created          : 2015-07-26  5:57 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-30  1:47 AM
// ***********************************************************************
// <copyright file="CouponController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models.Coupon;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     CouponController.
    /// </summary>
    [RoutePrefix("Coupon")]
    public class CouponController : ApiControllerBase
    {
        /// <summary>
        ///     The coupon service
        /// </summary>
        private readonly ICouponService couponService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CouponController" /> class.
        /// </summary>
        /// <param name="couponService">The coupon service.</param>
        public CouponController(ICouponService couponService)
        {
            this.couponService = couponService;
        }

        /// <summary>
        ///     获取直接使用的本金券
        /// </summary>
        /// <remarks>
        ///     会获取可以使用的一张本金券，如果没有可用的本金券，则返回空body
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route(""), CookieAuthorize, ResponseType(typeof(CouponInfoResponse))]
        public async Task<IHttpActionResult> GetCoupon()
        {
            CouponInfo coupon = await this.couponService.GetCouponAsync(this.CurrentUser.Id);

            return coupon == null ? this.Ok() : this.Ok(coupon.ToResponse());
        }

        /// <summary>
        ///     获取本金券列表
        /// </summary>
        /// <remarks>
        ///     会获取可以使用的本金券列表
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Index"), CookieAuthorize, ResponseType(typeof(List<CouponInfoResponse>))]
        public async Task<IHttpActionResult> GetCoupons()
        {
            List<CouponInfo> coupons = await this.couponService.GetCouponsAsync(this.CurrentUser.Id);
            return this.Ok(coupons.Select(c => c.ToResponse()).ToList());
        }

        /// <summary>
        ///     删除本金券
        /// </summary>
        /// <remarks>
        ///     删除本金券
        /// </remarks>
        /// <param name="couponId">代金券Id</param>
        /// <response code="200"></response>
        /// <response code="400">
        ///     CRC1:未找到指定的本金券
        /// </response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [HttpGet, Route("Remove/{couponId:int:min(1)}"), CookieAuthorize, ResponseType(typeof(List<CouponInfoResponse>))]
        public async Task<IHttpActionResult> RemoveCoupon(int couponId)
        {
            CouponInfo coupon = await this.couponService.RemoveCouponAsync(couponId, this.CurrentUser.Id);

            if (coupon == null)
            {
                return this.BadRequest("CRC1:未找到指定的本金券");
            }

            return this.Ok(coupon.ToResponse());
        }
    }
}