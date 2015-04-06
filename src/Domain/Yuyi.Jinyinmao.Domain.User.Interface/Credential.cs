// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-04  6:07 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-04  6:09 PM
// ***********************************************************************
// <copyright file="Credential.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     用户证件类型
    /// </summary>
    public enum Credential
    {
        /// <summary>
        ///     身份证
        /// </summary>
        IdCard = 10,

        /// <summary>
        ///     护照
        /// </summary>
        Passport = 20,

        /// <summary>
        ///     台湾
        /// </summary>
        Taiwan = 30,

        /// <summary>
        ///     军官证
        /// </summary>
        Junguan = 40
    }
}
