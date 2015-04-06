// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-04  6:47 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-04  6:51 PM
// ***********************************************************************
// <copyright file="IJinyinmaoAccount.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IJinyinmaoAccount
    /// </summary>
    public interface IJinyinmaoAccount : IGrain
    {
        /// <summary>
        ///     Registers the specified jinyinmao account register.
        /// </summary>
        /// <param name="jinyinmaoAccountRegister">The jinyinmao account register.</param>
        /// <returns>Task.</returns>
        Task Register(JinyinmaoAccountRegister jinyinmaoAccountRegister);
    }
}
