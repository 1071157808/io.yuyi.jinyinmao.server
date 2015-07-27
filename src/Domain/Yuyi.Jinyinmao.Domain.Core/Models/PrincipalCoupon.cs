// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : PrincipalCoupon.cs
// Created          : 2015-07-26  2:07 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  3:53 PM
// ***********************************************************************
// <copyright file="PrincipalCoupon.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Domain.Models
{
    /// <summary>
    ///     AccountTransaction.
    /// </summary>
    public class PrincipalCoupon
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
        ///     Gets or sets the order no.
        /// </summary>
        /// <value>The order no.</value>
        public string OrderNo { get; set; }

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
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserIdentifier { get; set; }

        /// <summary>
        ///     Gets or sets the use time.
        /// </summary>
        /// <value>The use time.</value>
        public DateTime? UseTime { get; set; }
    }
}