// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-20  1:25 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-21  11:26 PM
// ***********************************************************************
// <copyright file="UserRegistered.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Moe.Actor.Interface.Events;
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
        ///     Initializes a new instance of the <c>Event</c> class.
        /// </summary>
        /// <param name="sourceId">The source identifier.</param>
        public UserRegistered(string sourceId) : base(sourceId, typeof(IUser))
        {
        }

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
        ///     邀请码(推广相关)
        /// </summary>
        public string InviteBy { get; set; }

        /// <summary>
        ///     金包银账户
        /// </summary>
        public Guid JBYAccountId { get; set; }

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
        ///     结算账户
        /// </summary>
        public Guid SettlementAccountId { get; set; }

        /// <summary>
        ///     用户编号
        /// </summary>
        public Guid UserId { get; set; }
    }
}
