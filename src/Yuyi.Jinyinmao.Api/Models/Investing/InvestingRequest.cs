// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-15  4:09 PM
// ***********************************************************************
// <copyright file="InvestingRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models.Investing
{
    /// <summary>
    ///     InvestingRequest.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class InvestingRequest : IRequest
    {
        /// <summary>
        ///     投资金额，以“分”为单位
        /// </summary>
        [Required, Range(1, 10000000000), JsonProperty("amount")]
        public int Amount { get; set; }

        /// <summary>
        ///     支付密码
        /// </summary>
        [Required, StringLength(18, MinimumLength = 6), JsonProperty("paymentPassword")]
        public string PaymentPassword { get; set; }

        /// <summary>
        ///     产品唯一标识
        /// </summary>
        [Required, StringLength(32, MinimumLength = 32), JsonProperty("productIdentifier")]
        public string ProductIdentifier { get; set; }
    }
}