// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : CellphoneInfoResponse.cs
// Created          : 2015-08-04  4:25 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-04  4:30 PM
// ***********************************************************************
// <copyright file="CellphoneInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Moe.AspNet.Models;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Api.Models.User
{
    /// <summary>
    ///     CellphoneInfoResponse.
    /// </summary>
    public class CellphoneInfoResponse : IResponse
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this cellphone is registered.
        /// </summary>
        /// <value><c>true</c> if registered; otherwise, <c>false</c>.</value>
        public bool Registered { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
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