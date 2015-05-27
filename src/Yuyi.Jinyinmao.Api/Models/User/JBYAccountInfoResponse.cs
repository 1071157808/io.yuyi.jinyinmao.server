// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  1:49 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-12  1:54 AM
// ***********************************************************************
// <copyright file="JBYAccountInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     JBYAccountInfoResponse.
    /// </summary>
    public class JBYAccountInfoResponse : IResponse
    {
        /// <summary>
        ///     金包银计息金额
        /// </summary>
        [Required, JsonProperty("jBYAccrualAmount")]
        public int JBYAccrualAmount { get; set; }

        /// <summary>
        ///     金包银可取现金额
        /// </summary>
        [Required, JsonProperty("jBYWithdrawalableAmount")]
        public int JBYWithdrawalableAmount { get; set; }

        /// <summary>
        ///     当天金包银已经申请体现的总额
        /// </summary>
        [Required, JsonProperty("todayJBYWithdrawalAmount")]
        public int TodayJBYWithdrawalAmount { get; set; }
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