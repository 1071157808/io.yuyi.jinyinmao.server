// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : RepayOrderRequest.cs
// Created          : 2015-08-18  17:55
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-19  18:37
// ***********************************************************************
// <copyright file="RepayOrderRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models.Dev
{
    /// <summary>
    ///     RepayOrderRequest.
    /// </summary>
    public class RepayOrderRequest : IRequest
    {
        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        [Required]
        [StringLength(32, MinimumLength = 32)]
        [JsonProperty("orderIdentifier")]
        public string OrderIdentifier { get; set; }

        /// <summary>
        ///     Gets or sets the repay time.
        /// </summary>
        [Required]
        [JsonProperty("repayTime")]
        public DateTime RepayTime { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        [Required]
        [StringLength(32, MinimumLength = 32)]
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}