// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-13  12:59 AM
// ***********************************************************************
// <copyright file="IJBYAccount.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IJBYAccount
    /// </summary>
    public interface IJBYAccount : IGrain
    {
        /// <summary>
        ///     Registers the specified jby account register.
        /// </summary>
        /// <param name="jbyAccountRegister">The jby account register.</param>
        /// <returns>Task.</returns>
        Task Register(JBYAccountRegister jbyAccountRegister);
    }
}
