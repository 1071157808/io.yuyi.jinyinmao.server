// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-07  2:54 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-07  10:40 AM
// ***********************************************************************
// <copyright file="SignInResult.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Class SignInResult.
    /// </summary>
    public class SignInResult
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="SignInResult" /> is successed.
        /// </summary>
        /// <value><c>true</c> if successed; otherwise, <c>false</c>.</value>
        public bool Successed { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}
