// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-27  5:03 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  6:30 PM
// ***********************************************************************
// <copyright file="AuthenticateResulted.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     AuthenticateResulted.
    /// </summary>
    public class AuthenticateResulted : Event
    {
        /// <summary>
        ///     用户手机号
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        ///     证件号类型
        /// </summary>
        public Credential Credential { get; set; }

        /// <summary>
        ///     证件号码
        /// </summary>
        public string CredentialNo { get; set; }

        /// <summary>
        ///     用户真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="AddBankCardResulted" /> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }

        /// <summary>
        /// Gets or sets the tran desc.
        /// </summary>
        /// <value>The tran desc.</value>
        public string TranDesc { get; set; }

        /// <summary>
        ///     用户唯一标示符
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="AddBankCardResulted" /> is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        public bool Verified { get; set; }

        /// <summary>
        ///     Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        public DateTime VerifiedTime { get; set; }
    }
}