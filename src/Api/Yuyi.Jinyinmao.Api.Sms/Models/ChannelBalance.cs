// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-10  11:18 AM
// ***********************************************************************
// <copyright file="ChannelBalance.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;

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
        [Required]
        public int Balance { get; set; }

        /// <summary>
        ///     是否支付余额查询
        /// </summary>
        [Required]
        public bool SupportBalanceQuery { get; set; }
    }
}