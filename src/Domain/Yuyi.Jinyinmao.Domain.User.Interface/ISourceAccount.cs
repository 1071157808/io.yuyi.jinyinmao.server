// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-12  6:34 PM
// ***********************************************************************
// <copyright file="ISourceAccount.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface ISourceAccount
    /// </summary>
    public interface ISourceAccount : IGrain
    {
        /// <summary>
        ///     Registers the specified jinyinmao account register.
        /// </summary>
        /// <param name="sourceAccountRegister">The source account register.</param>
        /// <returns>Task.</returns>
        Task Register(SourceAccountRegister sourceAccountRegister);
    }
}
