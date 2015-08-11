// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-07-26  5:58 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-26  6:36 PM
// ***********************************************************************
// <copyright file="CouponInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Models.Coupon
{
    /// <summary>
    ///     CouponInfoResponse.
    /// </summary>
    public class CouponInfoResponse
    {
        /// <summary>
        ///     添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        ///     金额
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        ///     有效时间
        /// </summary>
        public DateTime EffectiveEndTime { get; set; }

        /// <summary>
        ///     起效时间
        /// </summary>
        public DateTime EffectiveStartTime { get; set; }

        /// <summary>
        ///     代金券Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///     是否已使用
        /// </summary>
        public bool UseFlag { get; set; }

        /// <summary>
        ///     使用时间
        /// </summary>
        public DateTime UseTime { get; set; }
    }

    internal static class CouponInfoEx
    {
        internal static CouponInfoResponse ToResponse(this CouponInfo info)
        {
            return new CouponInfoResponse
            {
                AddTime = info.AddTime,
                Amount = info.Amount,
                EffectiveEndTime = info.EffectiveEndTime,
                EffectiveStartTime = info.EffectiveStartTime,
                Id = info.Id,
                Remark = info.Remark,
                UseFlag = info.UseFlag,
                UseTime = info.UseTime.GetValueOrDefault()
            };
        }
    }
}