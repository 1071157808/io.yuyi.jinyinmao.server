// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:28 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-15  3:53 PM
// ***********************************************************************
// <copyright file="Cellphone.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
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
        /// <returns>System.Threading.Tasks.Task.</returns>
        public async Task Register()
        {
            this.State.Registered = true;
            await this.State.WriteStateAsync();
        }

        /// <summary>
        ///     Unregisters this instance.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Unregister()
        {
            this.State.Registered = false;
            await this.State.WriteStateAsync();
        }

        #endregion ICellphone Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override async Task OnActivateAsync()
        {
            this.State.Cellphone = this.GetPrimaryKeyLong().ToString().Substring(7);
            if (!this.State.UserId.HasValue)
            {
                this.State.UserId = Guid.NewGuid();
                await this.State.WriteStateAsync();
            }
            await base.OnActivateAsync();
        }
    }
}