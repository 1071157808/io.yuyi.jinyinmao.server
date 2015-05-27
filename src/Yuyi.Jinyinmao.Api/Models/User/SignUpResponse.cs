// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  1:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-10  9:25 AM
// ***********************************************************************
// <copyright file="SignUpResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Moe.Lib;
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
        ///     用户唯一标识
        /// </summary>
        /// <value>The user identifier.</value>
        [Required, JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
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
        internal static SignUpResponse ToSignUpResponse(this UserInfo info) => new SignUpResponse
        {
            Cellphone = info.Cellphone,
            UserIdentifier = info.UserId.ToGuidString()
        };
    }
}