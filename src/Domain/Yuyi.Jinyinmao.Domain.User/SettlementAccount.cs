// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-21  11:03 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-22  12:57 AM
// ***********************************************************************
// <copyright file="SettlementAccount.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     SettlementAccount.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public class SettlementAccount : Grain<ISettlementAccountState>, ISettlementAccount
    {
        #region ISettlementAccount Members

        /// <summary>
        ///     Registers the specified settlement account register.
        /// </summary>
        /// <param name="settlementAccountRegister">The settlement account register.</param>
        /// <returns>Task.</returns>
        public Task Register(SettlementAccountRegister settlementAccountRegister)
        {
            this.State.Id = this.GetPrimaryKey();
            this.State.UserId = settlementAccountRegister.UserId;

            return this.State.WriteStateAsync();
        }

        #endregion ISettlementAccount Members
    }
}
