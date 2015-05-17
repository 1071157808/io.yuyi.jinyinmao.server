// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  3:19 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-12  3:20 AM
// ***********************************************************************
// <copyright file="JBYWithdrawal.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     JBYWithdrawal.
    /// </summary>
    public class JBYWithdrawal : Command
    {
        /// <summary>
        ///     取现金额，以“分”为单位
        /// </summary>
        /// <value>The amount.</value>
        public int Amount { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }
    }
}