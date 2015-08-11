// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : User_Grain.cs
// Created          : 2015-05-27  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-06  12:16 AM
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
            await this.ReadStateAsync();
            this.ReloadSettleAccountData();
            this.ReloadJBYAccountData();
            this.ReloadOrderInfosData();
            await this.SyncAsync();
        }

        /// <summary>
        ///     Synchronizes the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public override async Task SyncAsync()
        {
            await DBSyncHelper.SyncUserAsync(await this.GetUserInfoAsync());
            foreach (KeyValuePair<Guid, Order> order in this.State.Orders)
            {
                await DBSyncHelper.SyncOrderAsync(order.Value.ToInfo());
            }

            foreach (KeyValuePair<string, BankCard> bankCard in this.State.BankCards)
            {
                await DBSyncHelper.SyncBankCardAsync(bankCard.Value.ToInfo(), this.State.UserId.ToGuidString());
            }

            foreach (KeyValuePair<Guid, JBYAccountTransaction> jbyAccountTransaction in this.State.JBYAccount)
            {
                await DBSyncHelper.SyncJBYAccountTransactionAsync(jbyAccountTransaction.Value.ToInfo());
            }

            foreach (KeyValuePair<Guid, SettleAccountTransaction> settleAccountTransaction in this.State.SettleAccount)
            {
                await DBSyncHelper.SyncSettleAccountTransactionAsync(settleAccountTransaction.Value.ToInfo());
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