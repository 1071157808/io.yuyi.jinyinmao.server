// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-05  1:48 AM
// ***********************************************************************
// <copyright file="User_Grain.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     User.
    /// </summary>
    public partial class User
    {
        #region IUser Members

        /// <summary>
        ///     Reload state data as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public override async Task ReloadAsync()
        {
            await this.State.ReadStateAsync();
            this.ReloadSettleAccountData();
            this.ReloadJBYAccountData();
            this.ReloadOrderInfosData();
            await this.SyncAsync();
        }

        /// <summary>
        ///     Synchronizes the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SyncAsync()
        {
            await DBSyncHelper.SyncUser(await this.GetUserInfoAsync());
            foreach (KeyValuePair<Guid, Order> order in this.State.Orders)
            {
                await DBSyncHelper.SyncOrder(order.Value.ToInfo());
            }

            foreach (KeyValuePair<string, BankCard> bankCard in this.State.BankCards)
            {
                await DBSyncHelper.SyncBankCard(bankCard.Value.ToInfo(), this.State.Id.ToGuidString());
            }

            foreach (KeyValuePair<Guid, JBYAccountTranscation> jbyAccountTranscation in this.State.JBYAccount)
            {
                await DBSyncHelper.SyncJBYAccountTranscation(jbyAccountTranscation.Value.ToInfo());
            }

            foreach (KeyValuePair<Guid, SettleAccountTranscation> settleAccountTranscation in this.State.SettleAccount)
            {
                await DBSyncHelper.SyncSettleAccountTranscation(settleAccountTranscation.Value.ToInfo());
            }
        }

        #endregion IUser Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        /// <returns>Task.</returns>
        public override Task OnActivateAsync()
        {
            this.ReloadSettleAccountData();
            this.ReloadJBYAccountData();
            this.ReloadOrderInfosData();
            return base.OnActivateAsync();
        }
    }
}