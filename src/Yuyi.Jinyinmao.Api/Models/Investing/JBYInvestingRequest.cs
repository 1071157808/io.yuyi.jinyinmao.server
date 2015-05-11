// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  3:24 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  3:37 AM
// ***********************************************************************
// <copyright file="JBYInvestingRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models.Investing
{
    /// <summary>
    ///     JBYInvestingRequest.
    /// </summary>
    public class JBYInvestingRequest : IRequest
    {
        /// <summary>
        ///     投资金额，以“分”为单位
        /// </summary>
        [Required, Range(0, 10000000000), JsonProperty("amount")]
        public int Amount { get; set; }

        /// <summary>
        ///     支付密码
        /// </summary>
        [Required, StringLength(18, MinimumLength = 6), JsonProperty("paymentPassword")]
        public string PaymentPassword { get; set; }

        /// <summary>
        ///     产品类别
        /// </summary>
        [Required, JsonProperty("productCategory")]
        public long ProductCategory { get; set; }
    }
}