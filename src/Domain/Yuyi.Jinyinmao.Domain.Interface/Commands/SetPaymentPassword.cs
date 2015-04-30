// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-25  3:04 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  12:12 AM
// ***********************************************************************
// <copyright file="SetPaymentPassword.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     SetPaymentPassword.
    /// </summary>
    public class SetPaymentPassword : Command
    {
        /// <summary>
        ///     是否直接覆盖原密码
        /// </summary>
        public bool Override { get; set; }

        /// <summary>
        ///     支付密码
        /// </summary>
        public string PaymentPassword { get; set; }

        /// <summary>
        ///     密码加密盐
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        ///     用户唯一标示符
        /// </summary>
        public Guid UserId { get; set; }
    }
}
