// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-18  1:15 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  3:22 AM
// ***********************************************************************
// <copyright file="JBYAccountInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     JBYAccountInfo.
    /// </summary>
    [Immutable]
    public class JBYAccountInfo
    {
        /// <summary>
        ///     Gets or sets the jby accrual amount.
        /// </summary>
        /// <value>The jby accrual amount.</value>
        public int JBYAccrualAmount { get; set; }

        /// <summary>
        ///     Gets or sets the jby total amount.
        /// </summary>
        /// <value>The jby total amount.</value>
        public int JBYTotalAmount { get; set; }

        /// <summary>
        ///     Gets or sets the jby total interest.
        /// </summary>
        /// <value>The jby total interest.</value>
        public int JBYTotalInterest { get; set; }

        /// <summary>
        ///     Gets or sets the jby total pricipal.
        /// </summary>
        /// <value>The jby total pricipal.</value>
        public int JBYTotalPricipal { get; set; }

        /// <summary>
        ///     Gets or sets the jby withdrawalable amount.
        /// </summary>
        /// <value>The jby withdrawalable amount.</value>
        public int JBYWithdrawalableAmount { get; set; }

        /// <summary>
        ///     Gets or sets the today jby withdrawal amount.
        /// </summary>
        /// <value>The today jby withdrawal amount.</value>
        public int TodayJBYWithdrawalAmount { get; set; }
    }
}