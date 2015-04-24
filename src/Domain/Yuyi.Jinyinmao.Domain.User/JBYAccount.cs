// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-25  12:50 AM
// ***********************************************************************
// <copyright file="JBYAccount.cs" company="Shanghai Yuyi">
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
    ///     Class JBYAccount.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public class JBYAccount : Grain<IJBYAccountState>, IJBYAccount
    {
        #region IJBYAccount Members

        /// <summary>
        ///     Registers the specified jby account register.
        /// </summary>
        /// <param name="jbyAccountRegister">The jby account register.</param>
        /// <returns>Task.</returns>
        public async Task Register(JBYAccountRegister jbyAccountRegister)
        {
            if (this.State.UserId == jbyAccountRegister.UserId)
            {
                return;
            }

            if (this.State.UserId != Guid.Empty)
            {
                this.GetLogger().Warn(1, "Conflict user id: UserId {0}, JBYAccountRegister.UserId {1}", this.State.UserId, jbyAccountRegister.UserId);
                return;
            }

            this.State.Id = this.GetPrimaryKey();
            this.State.UserId = jbyAccountRegister.UserId;

            await this.State.WriteStateAsync();
        }

        #endregion IJBYAccount Members
    }
}
