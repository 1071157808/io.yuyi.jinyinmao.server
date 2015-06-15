// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-15  7:05 PM
// ***********************************************************************
// <copyright file="SettleAccountInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Moe.AspNet.Models;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     SettleAccountInfoResponse.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class SettleAccountInfoResponse : IResponse
    {
        /// <summary>
        ///     账户余额，以“分”为单位
        /// </summary>
        [Required, JsonProperty("balance")]
        public long Balance { get; set; }

        /// <summary>
        ///     在途的出项金额，以“分”为单位
        /// </summary>
        [Required, JsonProperty("crediting")]
        public long Crediting { get; set; }

        /// <summary>
        ///     在途的进项金额，以“分”为单位
        /// </summary>
        [Required, JsonProperty("debiting")]
        public long Debiting { get; set; }

        /// <summary>
        ///     今天的提现次数
        /// </summary>
        [Required, JsonProperty("monthWithdrawalCount")]
        public int MonthWithdrawalCount { get; set; }

        /// <summary>
        ///     当月的提现次数
        /// </summary>
        [Required, JsonProperty("todayWithdrawalCount")]
        public int TodayWithdrawalCount { get; set; }
    }

    internal static class SettleAccountInfoEx
    {
        internal static SettleAccountInfoResponse ToResponse(this SettleAccountInfo info) => new SettleAccountInfoResponse
        {
            Balance = info.Balance,
            Crediting = info.Crediting,
            Debiting = info.Debiting,
            MonthWithdrawalCount = info.MonthWithdrawalCount,
            TodayWithdrawalCount = info.TodayWithdrawalCount
        };
    }
}