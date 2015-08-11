// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : CellphoneInfoResponse.cs
// Created          : 2015-08-11  4:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-11  6:29 PM
// ***********************************************************************
// <copyright file="CellphoneInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     CellphoneInfoResponse.
    /// </summary>
    public class CellphoneInfoResponse : IResponse
    {
        /// <summary>
        ///     手机号
        /// </summary>
        [Required]
        public string Cellphone { get; set; }

        /// <summary>
        ///     是否已注册
        /// </summary>
        [Required]
        public bool Registered { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        [Required]
        public string UserIdentifier { get; set; }
    }

    internal static class CellphoneInfoEx
    {
        internal static CellphoneInfoResponse ToResponse(this CellphoneInfo info)
        {
            return new CellphoneInfoResponse
            {
                Cellphone = info.Cellphone,
                Registered = info.Registered,
                UserIdentifier = info.UserId.ToGuidString()
            };
        }
    }
}