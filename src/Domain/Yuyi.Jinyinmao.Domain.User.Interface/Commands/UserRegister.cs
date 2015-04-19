// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-12  6:15 PM
// ***********************************************************************
// <copyright file="UserRegister.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Moe.Actor.Commands;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     Class UserRegister.
    /// </summary>
    [Immutable]
    public class UserRegister : Command
    {
        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string Args { get; set; }

        /// <summary>
        ///     用户手机号
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        ///     客户端标识, 900 => PC, 901 => iPhone, 902 => Android, 903 => M
        /// </summary>
        public long ClientType { get; set; }

        /// <summary>
        ///     活动编号(推广相关)
        /// </summary>
        public long ContractId { get; set; }

        /// <summary>
        ///     Gets the handler identifier.
        /// </summary>
        public override Guid HandlerId
        {
            get { return this.UserId; }
        }

        /// <summary>
        ///     邀请码(推广相关)
        /// </summary>
        public string InviteBy { get; set; }

        /// <summary>
        ///     金银e家代码
        /// </summary>
        public string OutletCode { get; set; }

        /// <summary>
        ///     用户设置的密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     用户编号
        /// </summary>
        public Guid UserId { get; set; }
    }
}
