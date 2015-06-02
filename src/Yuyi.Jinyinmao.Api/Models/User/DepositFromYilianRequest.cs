// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-03  3:45 AM
// ***********************************************************************
// <copyright file="DepositFromYilianRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     DepositFromYilianRequest.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class DepositFromYilianRequest : IRequest
    {
        /// <summary>
        ///     支付金额，以“分”为单位
        /// </summary>
        /// <value>The amount.</value>
        [Required, Range(100, 200000000), JsonProperty("amount")]
        public int Amount { get; set; }

        /// <summary>
        ///     银行卡号，15到19位
        /// </summary>
        [Required, RegularExpression(@"^\d{15,19}$"), JsonProperty("bankCardNo")]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     支付密码
        /// </summary>
        [Required, StringLength(18, MinimumLength = 6), JsonProperty("paymentPassword")]
        public string PaymentPassword { get; set; }
    }
}