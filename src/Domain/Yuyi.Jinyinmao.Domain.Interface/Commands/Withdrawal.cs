// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  12:33 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  12:33 AM
// ***********************************************************************
// <copyright file="Withdrawal.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     Withdrawal.
    /// </summary>
    public class Withdrawal : Command
    {
        /// <summary>
        ///     取现金额，以“分”为单位
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        ///     银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        public Guid UserId { get; set; }
    }
}