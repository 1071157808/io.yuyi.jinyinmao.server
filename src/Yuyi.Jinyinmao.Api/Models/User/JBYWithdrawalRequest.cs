// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  3:10 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-12  3:11 AM
// ***********************************************************************
// <copyright file="JBYWithdrawalRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     JBYWithdrawalRequest.
    /// </summary>
    public class JBYWithdrawalRequest : IRequest
    {
        /// <summary>
        ///     取现额度，以“分”为单位，1分到100万
        /// </summary>
        /// <value>The amount.</value>
        [Required, Range(1, 100000000), JsonProperty("amount")]
        public int Amount { get; set; }

        /// <summary>
        ///     支付密码
        /// </summary>
        [Required, StringLength(18, MinimumLength = 6), JsonProperty("paymentPassword")]
        public string PaymentPassword { get; set; }
    }
}