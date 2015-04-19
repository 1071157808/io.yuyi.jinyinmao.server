// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-12  6:46 PM
// ***********************************************************************
// <copyright file="IUserState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Moe.Actor.Model;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IUserState
    /// </summary>
    public interface IUserState : IEntityState
    {
        /// <summary>
        ///     用户手机号码
        /// </summary>
        string Cellphone { get; set; }

        /// <summary>
        ///     证件类型
        /// </summary>
        Credential Credential { get; set; }

        /// <summary>
        ///     证件编号
        /// </summary>
        string CredentialNo { get; set; }

        /// <summary>
        ///     金包银账户
        /// </summary>
        IJBYAccount JBYAccount { get; set; }

        /// <summary>
        ///     金银猫账户
        /// </summary>
        IJinyinmaoAccount JinyinmaoAccount { get; set; }

        /// <summary>
        ///     真实姓名
        /// </summary>
        string RealName { get; set; }

        /// <summary>
        ///     注册时间
        /// </summary>
        DateTime RegisterTime { get; set; }

        /// <summary>
        ///     来源账户
        /// </summary>
        ISourceAccount SourceAccount { get; set; }

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
