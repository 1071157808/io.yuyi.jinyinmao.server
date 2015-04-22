// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-22  2:07 AM
// ***********************************************************************
// <copyright file="User.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moe.Actor.Models;
using Moe.Lib;
using Orleans.Providers;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Helper;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class User.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public class User : EntityGrain<IUserState>, IUser
    {
        #region IUser Members

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> GetUserInfoAsync()
        {
            return Task.FromResult(new UserInfo
            {
                Cellphone = this.State.Cellphone,
                UserId = this.State.Id
            });
        }

        /// <summary>
        ///     Determines whether [is registered] asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> IsRegisteredAsync()
        {
            return Task.FromResult(this.State.Id != Guid.Empty);
        }

        /// <summary>
        ///     Registers the specified user register.
        /// </summary>
        /// <param name="userRegister">The user register.</param>
        /// <returns>Task.</returns>
        public async Task RegisterAsync(UserRegister userRegister)
        {
            if (this.State.Id == userRegister.UserId)
            {
                return;
            }

            if (this.State.Id != Guid.Empty)
            {
                this.GetLogger().Warn(1, "Conflict user id: UserId {0}, UserRegisterCommand.UserId {1}", this.State.Id, userRegister.UserId);
                return;
            }

            DateTime registerTime = DateTime.Now;

            this.State.Id = userRegister.UserId;
            this.State.Args = userRegister.Args;
            this.State.Cellphone = userRegister.Cellphone;
            this.State.ClientType = userRegister.ClientType;
            this.State.ContractId = userRegister.ContractId;
            this.State.Credential = Credential.None;
            this.State.CredentialNo = "empty";
            this.State.EncryptedPassword = CryptographyHelper.Encrypting(userRegister.Password, userRegister.UserId.ToGuidString());
            this.State.InviteBy = userRegister.InviteBy;
            this.State.LoginNames = new List<string> { userRegister.Cellphone };
            this.State.OutletCode = userRegister.OutletCode;
            this.State.RealName = "empty";
            this.State.RegisterTime = registerTime;
            this.State.Salt = userRegister.UserId.ToGuidString();
            this.State.Verified = false;
            this.State.VerifiedTime = null;

            Guid JBYAccountId = Guid.NewGuid();
            this.State.JBYAccount = JBYAccountFactory.GetGrain(JBYAccountId);
            await this.State.JBYAccount.Register(new JBYAccountRegister
            {
                UserId = userRegister.UserId
            });

            Guid settlementAccountId = Guid.NewGuid();
            this.State.SettlementAccount = SettlementAccountFactory.GetGrain(settlementAccountId);
            await this.State.SettlementAccount.Register(new SettlementAccountRegister
            {
                UserId = userRegister.UserId
            });

            await this.StoreCommandAsync(userRegister);

            await this.StoreEventAsync(new UserRegistered(this.State.Id.ToGuidString())
            {
                Args = userRegister.Args,
                Cellphone = userRegister.Cellphone,
                ClientType = userRegister.ClientType,
                ContractId = userRegister.ContractId,
                InviteBy = userRegister.InviteBy,
                JBYAccountId = JBYAccountId,
                LoginNames = this.State.LoginNames,
                OutletCode = userRegister.OutletCode,
                RegisterTime = registerTime,
                SettlementAccountId = settlementAccountId,
                UserId = userRegister.UserId
            });

            await this.State.WriteStateAsync();
        }

        #endregion IUser Members
    }
}
