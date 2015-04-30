// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-27  4:01 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  4:10 PM
// ***********************************************************************
// <copyright file="Authenticate.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     Authenticate.
    /// </summary>
    [Immutable]
    public class Authenticate : Command
    {
        /// <summary>
        ///     银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        ///     用户手机号
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        ///     银行卡所在城市
        /// </summary>
        public string CityName { get; set; }

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
        ///     用户唯一标示符
        /// </summary>
        public Guid UserId { get; set; }
    }
}
