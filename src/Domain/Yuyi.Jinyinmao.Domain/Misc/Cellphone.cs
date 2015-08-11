// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Cellphone.cs
// Created          : 2015-04-28  11:28 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-12  2:39 AM
// ***********************************************************************
// <copyright file="Cellphone.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Lib;
using Orleans;
using Orleans.Providers;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class Cellphone.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public class Cellphone : Grain<ICellphoneState>, ICellphone
    {
        #region ICellphone Members

        /// <summary>
        ///     Gets the cellphone information.
        /// </summary>
        /// <returns>Task&lt;CellphoneInfo&gt;.</returns>
        public Task<CellphoneInfo> GetCellphoneInfoAsync()
        {
            return Task.FromResult(new CellphoneInfo
            {
                Cellphone = this.State.Cellphone,
                Registered = this.State.Registered,
                UserId = this.State.UserId.GetValueOrDefault()
            });
        }

        /// <summary>
        ///     Registers this instance.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.Threading.Tasks.Task.</returns>
        public async Task RegisterAsync(Guid userId)
        {
            this.State.Registered = true;
            this.State.UserId = userId;
            await this.WriteStateAsync();
        }

        /// <summary>
        ///     Reload cellphone as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task<CellphoneInfo> ReloadAsync()
        {
            await this.ReadStateAsync();
            return await this.GetCellphoneInfoAsync();
        }

        /// <summary>
        ///     Unregisters this instance.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task UnregisterAsync()
        {
            this.State.Registered = false;
            this.State.UserId = Guid.NewGuid();
            await this.WriteStateAsync();
        }

        #endregion ICellphone Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override async Task OnActivateAsync()
        {
            if (this.State.Cellphone.IsNullOrEmpty())
            {
                this.State.Cellphone = this.GetPrimaryKeyLong().ToString().Substring(7);
                this.State.UserId = Guid.NewGuid();
            }

            await base.OnActivateAsync();
        }
    }
}