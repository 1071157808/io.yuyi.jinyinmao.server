// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  2:34 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-08  1:20 PM
// ***********************************************************************
// <copyright file="InvestingRequest.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models.Investing
{
    /// <summary>
    ///     InvestingRequest.
    /// </summary>
    public class InvestingRequest : IRequest
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

        /// <summary>
        ///     产品唯一标识
        /// </summary>
        [Required, StringLength(32, MinimumLength = 32), JsonProperty("productIdentifier")]
        public string ProductIdentifier { get; set; }
    }
}