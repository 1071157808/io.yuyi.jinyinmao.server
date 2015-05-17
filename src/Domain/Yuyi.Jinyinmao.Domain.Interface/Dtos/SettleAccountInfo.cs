// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-18  2:06 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  2:07 AM
// ***********************************************************************
// <copyright file="SettleAccountInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     SettleAccountInfo.
    /// </summary>
    [Immutable]
    public class SettleAccountInfo
    {
        /// <summary>
        ///     Gets or sets the balance.
        /// </summary>
        /// <value>The balance.</value>
        public int Balance { get; set; }

        /// <summary>
        ///     Gets or sets the crediting.
        /// </summary>
        /// <value>The crediting.</value>
        public int Crediting { get; set; }

        /// <summary>
        ///     Gets or sets the debiting.
        /// </summary>
        /// <value>The debiting.</value>
        public int Debiting { get; set; }

        /// <summary>
        ///     Gets or sets the month withdrawal count.
        /// </summary>
        /// <value>The month withdrawal count.</value>
        public int MonthWithdrawalCount { get; set; }

        /// <summary>
        ///     Gets or sets the today withdrawal count.
        /// </summary>
        /// <value>The today withdrawal count.</value>
        public int TodayWithdrawalCount { get; set; }
    }
}