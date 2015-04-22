// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-21  11:06 PM
// ***********************************************************************
// <copyright file="ISettlementAccount.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface ISettlementAccount
    /// </summary>
    public interface ISettlementAccount : IGrain
    {
        /// <summary>
        ///     Registers the specified settlement account register.
        /// </summary>
        /// <param name="settlementAccountRegister">The settlement account register.</param>
        /// <returns>Task.</returns>
        Task Register(SettlementAccountRegister settlementAccountRegister);
    }
}
