// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-07-26  3:10 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-26  5:42 PM
// ***********************************************************************
// <copyright file="ICouponService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yuyi.Jinyinmao.Log;

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Interface ICouponService
    /// </summary>
    public interface ICouponService
    {
        /// <summary>
        ///     Gets the coupon.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;CouponInfo&gt;.</returns>
        [LogExceptionAspect]
        Task<CouponInfo> GetCouponAsync(Guid userId);

        /// <summary>
        ///     Gets the coupons.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;List&lt;CouponInfo&gt;&gt;.</returns>
        [LogExceptionAspect]
        Task<List<CouponInfo>> GetCouponsAsync(Guid userId);

        /// <summary>
        ///     Removes the coupon.
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;CouponInfo&gt;.</returns>
        [LogExceptionAspect]
        Task<CouponInfo> RemoveCouponAsync(int couponId, Guid userId);

        /// <summary>
        ///     Uses the coupon.
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;CouponInfo&gt;.</returns>
        [LogExceptionAspect]
        Task<CouponInfo> UseCouponAsync(int couponId, Guid userId);
    }
}