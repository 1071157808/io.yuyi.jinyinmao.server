// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-20  1:25 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-03  6:00 PM
// ***********************************************************************
// <copyright file="UserRegistered.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     UserRegistered.
    /// </summary>
    [Immutable]
    public class UserRegistered : Event
    {
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
        ///     邀请码(推广相关)
        /// </summary>
        public string InviteBy { get; set; }

        /// <summary>
        ///     登录名
        /// </summary>
        public List<string> LoginNames { get; set; }

        /// <summary>
        ///     金银e家代码
        /// </summary>
        public string OutletCode { get; set; }

        /// <summary>
        ///     注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        ///     用户编号
        /// </summary>
        public Guid UserId { get; set; }
    }
}