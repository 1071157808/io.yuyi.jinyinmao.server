// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : CouponService.cs
// Created          : 2015-07-26  3:10 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-11  9:44 AM
// ***********************************************************************
// <copyright file="CouponService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Models;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     CouponService.
    /// </summary>
    public class CouponService : ICouponService
    {
        #region ICouponService Members

        /// <summary>
        ///     get coupon as an asynchronous operation.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;CouponInfo&gt;.</returns>
        public async Task<CouponInfo> GetCouponAsync(Guid userId)
        {
            string userIdentifier = userId.ToGuidString();
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            using (JYMDBContext db = new JYMDBContext())
            {
                PrincipalCoupon coupon = await db.ReadonlyQuery<PrincipalCoupon>()
                    .Where(c => c.UserIdentifier == userIdentifier && !c.UseFlag && c.EffectiveStartTime < now && c.EffectiveEndTime > now)
                    .OrderBy(c => c.EffectiveEndTime).FirstOrDefaultAsync();

                if (coupon == null)
                {
                    return null;
                }

                return new CouponInfo
                {
                    AddTime = coupon.AddTime,
                    Amount = coupon.Amount,
                    EffectiveEndTime = coupon.EffectiveEndTime,
                    EffectiveStartTime = coupon.EffectiveStartTime,
                    Id = coupon.Id,
                    OrderNo = coupon.OrderNo,
                    Remark = coupon.Remark,
                    UseFlag = coupon.UseFlag,
                    UserIdentifier = coupon.UserIdentifier,
                    UseTime = coupon.UseTime
                };
            }
        }

        /// <summary>
        ///     get coupons as an asynchronous operation.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;List&lt;CouponInfo&gt;&gt;.</returns>
        public async Task<List<CouponInfo>> GetCouponsAsync(Guid userId)
        {
            string userIdentifier = userId.ToGuidString();
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            using (JYMDBContext db = new JYMDBContext())
            {
                List<PrincipalCoupon> coupons = await db.ReadonlyQuery<PrincipalCoupon>()
                    .Where(c => c.UserIdentifier == userIdentifier && !c.UseFlag && c.EffectiveStartTime < now && c.EffectiveEndTime > now)
                    .OrderBy(c => c.EffectiveEndTime).ToListAsync();

                return coupons.Select(c => new CouponInfo
                {
                    AddTime = c.AddTime,
                    Amount = c.Amount,
                    EffectiveEndTime = c.EffectiveEndTime,
                    EffectiveStartTime = c.EffectiveStartTime,
                    Id = c.Id,
                    OrderNo = c.OrderNo,
                    Remark = c.Remark,
                    UseFlag = c.UseFlag,
                    UserIdentifier = c.UserIdentifier,
                    UseTime = c.UseTime
                }).ToList();
            }
        }

        /// <summary>
        ///     remove coupon as an asynchronous operation.
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;CouponInfo&gt;.</returns>
        public async Task<CouponInfo> RemoveCouponAsync(int couponId, Guid userId)
        {
            string userIdentifier = userId.ToGuidString();
            using (JYMDBContext db = new JYMDBContext())
            {
                PrincipalCoupon coupon = await db.Query<PrincipalCoupon>().FirstOrDefaultAsync(c => c.Id == couponId && c.UserIdentifier == userIdentifier);
                if (coupon == null)
                {
                    return null;
                }

                CouponInfo info = new CouponInfo
                {
                    AddTime = coupon.AddTime,
                    Amount = coupon.Amount,
                    EffectiveEndTime = coupon.EffectiveEndTime,
                    EffectiveStartTime = coupon.EffectiveStartTime,
                    Id = coupon.Id,
                    OrderNo = coupon.OrderNo,
                    Remark = coupon.Remark,
                    UseFlag = coupon.UseFlag,
                    UserIdentifier = coupon.UserIdentifier,
                    UseTime = coupon.UseTime
                };

                await db.RemoveAsync(coupon);

                return info;
            }
        }

        /// <summary>
        ///     use coupon as an asynchronous operation.
        /// </summary>
        /// <param name="couponId">The coupon identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;CouponInfo&gt;.</returns>
        public async Task<CouponInfo> UseCouponAsync(int couponId, Guid userId)
        {
            string userIdentifier = userId.ToGuidString();
            using (JYMDBContext db = new JYMDBContext())
            {
                PrincipalCoupon coupon = await db.Query<PrincipalCoupon>().FirstOrDefaultAsync(c => c.Id == couponId && c.UserIdentifier == userIdentifier);
                if (coupon == null)
                {
                    return null;
                }

                CouponInfo info = new CouponInfo
                {
                    AddTime = coupon.AddTime,
                    Amount = coupon.Amount,
                    EffectiveEndTime = coupon.EffectiveEndTime,
                    EffectiveStartTime = coupon.EffectiveStartTime,
                    Id = coupon.Id,
                    OrderNo = coupon.OrderNo,
                    Remark = coupon.Remark,
                    UseFlag = coupon.UseFlag,
                    UserIdentifier = coupon.UserIdentifier,
                    UseTime = coupon.UseTime
                };

                coupon.UseFlag = true;
                coupon.UseTime = DateTime.UtcNow.ToChinaStandardTime();

                await db.ExecuteSaveChangesAsync();

                return info;
            }
        }

        #endregion ICouponService Members
    }
}