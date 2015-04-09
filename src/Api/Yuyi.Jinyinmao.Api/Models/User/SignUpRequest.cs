// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  2:56 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-07  2:09 AM
// ***********************************************************************
// <copyright file="SignUpRequest.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.RequestModels;
using Moe.AspNet.Validations;

namespace Yuyi.Jinyinmao.Api.Models.User
{
    /// <summary>
    ///     Class SignUpRequest.
    /// </summary>
    public class SignUpRequest : IRequest
    {
        /// <summary>
        ///     客户端标识, 900 => PC, 901 => iPhone, 902 => Android, 903 => M
        /// </summary>
        public long? ClientType { get; set; }

        /// <summary>
        ///     活动编号(推广相关)
        /// </summary>
        public long? ContractId { get; set; }

        /// <summary>
        ///     邀请码(推广相关)
        /// </summary>
        public string InviteBy { get; set; }

        /// <summary>
        ///     金银e家代码
        /// </summary>
        public string OutletCode { get; set; }

        /// <summary>
        ///     用户设置的密码（6-18位的数字、字母、一般特殊字符组合）
        /// </summary>
        [Required, StringLength(18, MinimumLength = 6), SimplePasswordFormat]
        public string Password { get; set; }

        /// <summary>
        ///     验证码口令
        /// </summary>
        [Required, StringLength(32, MinimumLength = 32)]
        public string Token { get; set; }
    }
}
