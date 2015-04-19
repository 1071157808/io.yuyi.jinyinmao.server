// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  11:54 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-19  11:55 AM
// ***********************************************************************
// <copyright file="ChannelBalance.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Moe.AspNet.Models;

namespace Yuyi.Jinyinmao.Api.Sms.Models
{
    /// <summary>
    ///     ChannelBalance.
    /// </summary>
    public class ChannelBalance : IResponse
    {
        /// <summary>
        ///     Gets or sets the balance.
        /// </summary>
        /// <value>The balance.</value>
        public int Balance { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [support balance query].
        /// </summary>
        /// <value><c>true</c> if [support balance query]; otherwise, <c>false</c>.</value>
        public bool SupportBalanceQuery { get; set; }
    }
}
