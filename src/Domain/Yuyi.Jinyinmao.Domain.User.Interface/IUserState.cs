// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-21  11:23 PM
// ***********************************************************************
// <copyright file="IUserState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IUserState
    /// </summary>
    public interface IUserState : IEntityState
    {
        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        string Args { get; set; }

        /// <summary>
        ///     用户手机号码
        /// </summary>
        string Cellphone { get; set; }

        /// <summary>
        ///     客户端标识, 900 => PC, 901 => iPhone, 902 => Android, 903 => M
        /// </summary>
        long ClientType { get; set; }

        /// <summary>
        ///     活动编号(推广相关)
        /// </summary>
        long ContractId { get; set; }

        /// <summary>
        ///     证件类型
        /// </summary>
        Credential Credential { get; set; }

        /// <summary>
        ///     证件编号
        /// </summary>
        string CredentialNo { get; set; }

        /// <summary>
        ///     加密密码
        /// </summary>
        string EncryptedPassword { get; set; }

        /// <summary>
        ///     加密支付密码
        /// </summary>
        string EncryptedPaymentPassword { get; set; }

        /// <summary>
        ///     邀请人
        /// </summary>
        string InviteBy { get; set; }

        /// <summary>
        ///     金包银账户
        /// </summary>
        IJBYAccount JBYAccount { get; set; }

        /// <summary>
        ///     登录名
        /// </summary>
        List<string> LoginNames { get; set; }

        /// <summary>
        ///     金银e家代码
        /// </summary>
        string OutletCode { get; set; }

        /// <summary>
        /// 支付密码加密盐
        /// </summary>
        string PaymentSalt { get; set; }

        /// <summary>
        ///     真实姓名
        /// </summary>
        string RealName { get; set; }

        /// <summary>
        ///     注册时间
        /// </summary>
        DateTime RegisterTime { get; set; }

        /// <summary>
        ///     密码盐
        /// </summary>
        string Salt { get; set; }

        /// <summary>
        ///     结算账户
        /// </summary>
        ISettlementAccount SettlementAccount { get; set; }

        /// <summary>
        ///     是否通过实名认证
        /// </summary>
        bool Verified { get; set; }

        /// <summary>
        ///     实名认证时间
        /// </summary>
        DateTime? VerifiedTime { get; set; }
    }
}
