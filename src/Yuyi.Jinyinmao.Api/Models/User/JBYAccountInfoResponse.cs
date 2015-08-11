// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-15  7:06 PM
// ***********************************************************************
// <copyright file="JBYAccountInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
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
    ///     JBYAccountInfoResponse.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class JBYAccountInfoResponse : IResponse
    {
        /// <summary>
        ///     金包银计息金额
        /// </summary>
        [Required, JsonProperty("jBYAccrualAmount")]
        public long JBYAccrualAmount { get; set; }

        /// <summary>
        ///     金包银可取现金额
        /// </summary>
        [Required, JsonProperty("jBYWithdrawalableAmount")]
        public long JBYWithdrawalableAmount { get; set; }

        /// <summary>
        ///     当天金包银已经申请提现的总额
        /// </summary>
        [Required, JsonProperty("todayJBYWithdrawalAmount")]
        public long TodayJBYWithdrawalAmount { get; set; }
    }

    internal static class JBYAccountInfoEx
    {
        internal static JBYAccountInfoResponse ToResponse(this JBYAccountInfo info) => new JBYAccountInfoResponse
        {
            JBYAccrualAmount = info.JBYAccrualAmount,
            JBYWithdrawalableAmount = info.JBYWithdrawalableAmount,
            TodayJBYWithdrawalAmount = info.TodayJBYWithdrawalAmount
        };
    }
}