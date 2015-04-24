// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-21  11:03 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-25  12:51 AM
// ***********************************************************************
// <copyright file="SettlementAccount.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
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
        public async Task Register(SettlementAccountRegister settlementAccountRegister)
        {
            if (this.State.UserId == settlementAccountRegister.UserId)
            {
                return;
            }

            if (this.State.UserId != Guid.Empty)
            {
                this.GetLogger().Warn(1, "Conflict user id: UserId {0}, SettlementAccountRegister.UserId {1}", this.State.UserId, settlementAccountRegister.UserId);
                return;
            }

            this.State.Id = this.GetPrimaryKey();
            this.State.UserId = settlementAccountRegister.UserId;

            await this.State.WriteStateAsync();
        }

        #endregion ISettlementAccount Members
    }
}
