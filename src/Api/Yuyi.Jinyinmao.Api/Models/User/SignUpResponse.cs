// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-13  12:33 AM
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

namespace Yuyi.Jinyinmao.Api.Models.User
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
        [Required, JsonProperty(PropertyName = "cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     用户编号
        /// </summary>
        /// <value>The user identifier.</value>
        [Required, JsonProperty(PropertyName = "userId")]
        public Guid UserId { get; set; }
    }

    /// <summary>
    ///     Class UserInfoEx.
    /// </summary>
    internal static class UserInfoEx
    {
        /// <summary>
        ///     To the response.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>SignUpResponse.</returns>
        internal static SignUpResponse ToResponse(this UserInfo info)
        {
            return new SignUpResponse
            {
                Cellphone = info.Cellphone,
                UserId = info.UserId
            };
        }
    }
}
