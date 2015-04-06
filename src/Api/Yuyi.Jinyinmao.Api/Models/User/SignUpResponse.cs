// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  2:57 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-06  2:59 AM
// ***********************************************************************
// <copyright file="SignUpResponse.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Moe.AspNet.ResponseModels;

namespace Yuyi.Jinyinmao.Api.Models.User
{
    /// <summary>
    ///     Class SignUpResponse.
    /// </summary>
    public class SignUpResponse : IResponse
    {
        /// <summary>
        /// 用户的手机号码
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }
    }
}
