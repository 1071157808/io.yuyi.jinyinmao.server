// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-30  2:06 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-08  1:11 PM
// ***********************************************************************
// <copyright file="OrderInfo.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     OrderEx.
    /// </summary>
    public static class OrderEx
    {
        /// <summary>
        ///     To the information.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>OrderInfo.</returns>
        public static OrderInfo ToInfo(this Order order)
        {
            return new OrderInfo
            {
                AccountTranscationId = order.AccountTranscationId,
                Cellphone = order.Cellphone,
                ExtraInterest = order.ExtraInterest,
                ExtraYield = order.ExtraYield,
                Info = order.Info,
                Interest = order.Interest,
                IsRepaid = order.IsRepaid,
                OrderId = order.OrderId,
                OrderNo = order.OrderNo,
                OrderTime = order.OrderTime,
                Principal = order.Principal,
                ProductCategory = order.ProductCategory,
                ProductId = order.ProductId,
                ProductSnapshot = order.ProductSnapshot,
                RepaidTime = order.RepaidTime,
                ResultCode = order.ResultCode,
                ResultTime = order.ResultTime,
                SettleDate = order.SettleDate,
                TransDesc = order.TransDesc,
                UserId = order.UserId,
                UserInfo = order.UserInfo,
                ValueDate = order.ValueDate,
                Yield = order.Yield
            };
        }
    }

    /// <summary>
    ///     OrderInfo.
    /// </summary>
    public class OrderInfo
    {
        /// <summary>
        ///     Gets or sets the account transcation identifier.
        /// </summary>
        /// <value>The account transcation identifier.</value>
        public Guid AccountTranscationId { get; set; }

        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the extra interest.
        /// </summary>
        /// <value>The extra interest.</value>
        public int ExtraInterest { get; set; }

        /// <summary>
        ///     Gets or sets the extra yield.
        /// </summary>
        /// <value>The extra yield.</value>
        public int ExtraYield { get; set; }

        /// <summary>
        ///     Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public Dictionary<string, object> Info { get; set; }

        /// <summary>
        ///     Gets or sets the interest.
        /// </summary>
        /// <value>The interest.</value>
        public int Interest { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is repaid.
        /// </summary>
        /// <value><c>true</c> if this instance is repaid; otherwise, <c>false</c>.</value>
        public bool IsRepaid { get; set; }

        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        public Guid OrderId { get; set; }

        /// <summary>
        ///     Gets or sets the order no.
        /// </summary>
        /// <value>The order no.</value>
        public string OrderNo { get; set; }

        /// <summary>
        ///     Gets or sets the order time.
        /// </summary>
        /// <value>The order time.</value>
        public DateTime OrderTime { get; set; }

        /// <summary>
        ///     Gets or sets the principal.
        /// </summary>
        /// <value>The principal.</value>
        public int Principal { get; set; }

        /// <summary>
        ///     Gets or sets the product category.
        /// </summary>
        /// <value>The product category.</value>
        public long ProductCategory { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public Guid ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the product snapshot.
        /// </summary>
        /// <value>The product snapshot.</value>
        public RegularProductInfo ProductSnapshot { get; set; }

        /// <summary>
        ///     Gets or sets the repaid time.
        /// </summary>
        /// <value>The repaid time.</value>
        public DateTime? RepaidTime { get; set; }

        /// <summary>
        ///     Gets or sets the result code.
        /// </summary>
        /// <value>The result code.</value>
        public int ResultCode { get; set; }

        /// <summary>
        ///     Gets or sets the result time.
        /// </summary>
        /// <value>The result time.</value>
        public DateTime? ResultTime { get; set; }

        /// <summary>
        ///     Gets or sets the settle date.
        /// </summary>
        /// <value>The settle date.</value>
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     Gets or sets the trans desc.
        /// </summary>
        /// <value>The trans desc.</value>
        public string TransDesc { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        ///     Gets or sets the value date.
        /// </summary>
        /// <value>The value date.</value>
        public DateTime ValueDate { get; set; }

        /// <summary>
        ///     Gets or sets the yield.
        /// </summary>
        /// <value>The yield.</value>
        public int Yield { get; set; }
    }
}