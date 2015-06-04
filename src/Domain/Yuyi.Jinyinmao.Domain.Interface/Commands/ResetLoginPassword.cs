// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-25  2:26 AM
// 
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-04  3:49 PM
// ***********************************************************************
// <copyright file="ResetLoginPassword.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     ResetLoginPassword.
    /// </summary>
    [Immutable]
    public class ResetLoginPassword : Command
    {
        /// <summary>
        ///     登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     加密码
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        ///     用户唯一标示符
        /// </summary>
        public Guid UserId { get; set; }
    }
}