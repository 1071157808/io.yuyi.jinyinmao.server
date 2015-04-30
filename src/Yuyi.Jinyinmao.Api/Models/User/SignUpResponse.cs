// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  11:36 PM
// ***********************************************************************
// <copyright file="SignUpResponse.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     Class SignUpResponse.
    /// </summary>
    public class SignUpResponse : IResponse
    {
        /// <summary>
        ///     用户的手机号码
        /// </summary>
        /// <value>The cellphone.</value>
        [Required, JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     用户编号
        /// </summary>
        /// <value>The user identifier.</value>
        [Required, JsonProperty("userId")]
        public Guid UserId { get; set; }
    }

    /// <summary>
    ///     Class UserInfoEx.
    /// </summary>
    internal static partial class UserInfoEx
    {
        /// <summary>
        ///     To the response.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>SignUpResponse.</returns>
        internal static SignUpResponse ToSignUpResponse(this UserInfo info)
        {
            return new SignUpResponse
            {
                Cellphone = info.Cellphone,
                UserId = info.UserId
            };
        }
    }
}
