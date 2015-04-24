// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-25  2:26 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-25  2:27 AM
// ***********************************************************************
// <copyright file="ResetLoginPassword.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
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
    public class ResetLoginPassword : ICommand
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

        #region ICommand Members

        /// <summary>
        ///     Gets or sets the command identifier.
        /// </summary>
        public Guid CommandId { get; set; }

        #endregion ICommand Members
    }
}
