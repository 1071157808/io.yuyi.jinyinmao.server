// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-20  12:03 PM
// ***********************************************************************
// <copyright file="ChannelBalance.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Sms.Models
{
    /// <summary>
    ///     ChannelBalance.
    /// </summary>
    public class ChannelBalance : IResponse
    {
        /// <summary>
        ///     余额
        /// </summary>
        [Required, JsonProperty(PropertyName = "balance")]
        public int Balance { get; set; }

        /// <summary>
        ///     是否支付余额查询
        /// </summary>
        [Required, JsonProperty(PropertyName = "supportBalanceQuery")]
        public bool SupportBalanceQuery { get; set; }
    }
}
