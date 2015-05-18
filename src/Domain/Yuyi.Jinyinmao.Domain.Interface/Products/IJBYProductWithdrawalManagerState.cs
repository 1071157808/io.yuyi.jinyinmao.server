// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  12:56 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  1:54 PM
// ***********************************************************************
// <copyright file="IJBYProductWithdrawalManagerState.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Packages.Helper;

namespace Yuyi.Jinyinmao.Domain.Products
{
    /// <summary>
    ///     Interface IJBYProductWithdrawalManagerState
    /// </summary>
    public interface IJBYProductWithdrawalManagerState : IEntityState
    {
        /// <summary>
        ///     Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        DateTime Date { get; set; }

        /// <summary>
        ///     Gets or sets the date string.
        /// </summary>
        /// <value>The date string.</value>
        string DateString { get; set; }

        /// <summary>
        ///     Gets or sets the last work day configuration.
        /// </summary>
        /// <value>The last work day configuration.</value>
        DailyConfig LastWorkDayConfig { get; set; }

        /// <summary>
        ///     Gets or sets the today configuration.
        /// </summary>
        /// <value>The today configuration.</value>
        DailyConfig TodayConfig { get; set; }

        /// <summary>
        ///     Gets or sets the withdrawal transcations.
        /// </summary>
        /// <value>The withdrawal transcations.</value>
        Dictionary<Guid, Tuple<int, JBYAccountTranscationInfo>> WithdrawalTranscations { get; set; }
    }
}