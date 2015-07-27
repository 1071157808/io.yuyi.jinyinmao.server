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
        ///     Gets or sets the add time.
        /// </summary>
        /// <value>The add time.</value>
        public DateTime AddTime { get; set; }

        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public long Amount { get; set; }

        /// <summary>
        ///     Gets or sets the effective end time.
        /// </summary>
        /// <value>The effective end time.</value>
        public DateTime EffectiveEndTime { get; set; }

        /// <summary>
        ///     Gets or sets the effective start time.
        /// </summary>
        /// <value>The effective start time.</value>
        public DateTime EffectiveStartTime { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the remark.
        /// </summary>
        /// <value>The remark.</value>
        public string Remark { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [use flag].
        /// </summary>
        /// <value><c>true</c> if [use flag]; otherwise, <c>false</c>.</value>
        public bool UseFlag { get; set; }

        /// <summary>
        ///     Gets or sets the use time.
        /// </summary>
        /// <value>The use time.</value>
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