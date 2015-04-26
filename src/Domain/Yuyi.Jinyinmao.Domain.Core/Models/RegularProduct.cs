// ***********************************************************************
// Assembly         : Yuyi.Jinyinmao.Domain.Core
// Author           : Siqi Lu
// Created          : 2015-04-24  11:18 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 04-24-2015
// ***********************************************************************
// <copyright file="RegularProduct.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Domain.Models
{
    /// <summary>
    /// RegularProduct.
    /// </summary>
    public class RegularProduct
    {
        /// <summary>
        /// Gets or sets the end sell time.
        /// </summary>
        /// <value>The end sell time.</value>
        public DateTime EndSellTime { get; set; }

        /// <summary>
        /// Gets or sets the financing sum count.
        /// </summary>
        /// <value>The financing sum count.</value>
        public int FinancingSumCount { get; set; }

        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string Info { get; set; }

        /// <summary>
        /// Gets or sets the issue no.
        /// </summary>
        /// <value>The issue no.</value>
        public int IssueNo { get; set; }

        /// <summary>
        /// Gets or sets the launch time.
        /// </summary>
        /// <value>The launch time.</value>
        public DateTime LaunchTime { get; set; }

        /// <summary>
        /// Gets or sets the maximum share count.
        /// </summary>
        /// <value>The maximum share count.</value>
        public int MaxShareCount { get; set; }

        /// <summary>
        /// Gets or sets the minimum share count.
        /// </summary>
        /// <value>The minimum share count.</value>
        public int MinShareCount { get; set; }

        /// <summary>
        /// Gets or sets the pledge no.
        /// </summary>
        /// <value>The pledge no.</value>
        public string PledgeNo { get; set; }

        /// <summary>
        /// Gets or sets the product category.
        /// </summary>
        /// <value>The product category.</value>
        public long ProductCategory { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>The name of the product.</value>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the product no.
        /// </summary>
        /// <value>The product no.</value>
        public string ProductNo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RegularProduct"/> is repaid.
        /// </summary>
        /// <value><c>true</c> if repaid; otherwise, <c>false</c>.</value>
        public bool Repaid { get; set; }

        /// <summary>
        /// Gets or sets the repaid time.
        /// </summary>
        /// <value>The repaid time.</value>
        public Nullable<DateTime> RepaidTime { get; set; }

        /// <summary>
        /// Gets or sets the repayment deadline.
        /// </summary>
        /// <value>The repayment deadline.</value>
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        /// Gets or sets the settle date.
        /// </summary>
        /// <value>The settle date.</value>
        public DateTime SettleDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [sold out].
        /// </summary>
        /// <value><c>true</c> if [sold out]; otherwise, <c>false</c>.</value>
        public bool SoldOut { get; set; }

        /// <summary>
        /// Gets or sets the sold out time.
        /// </summary>
        /// <value>The sold out time.</value>
        public DateTime SoldOutTime { get; set; }

        /// <summary>
        /// Gets or sets the start sell time.
        /// </summary>
        /// <value>The start sell time.</value>
        public DateTime StartSellTime { get; set; }

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        /// <value>The unit price.</value>
        public int UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the value date.
        /// </summary>
        /// <value>The value date.</value>
        public DateTime? ValueDate { get; set; }

        /// <summary>
        /// Gets or sets the value date mode.
        /// </summary>
        /// <value>The value date mode.</value>
        public int? ValueDateMode { get; set; }

        /// <summary>
        /// Gets or sets the yield.
        /// </summary>
        /// <value>The yield.</value>
        public decimal Yield { get; set; }
    }
}
