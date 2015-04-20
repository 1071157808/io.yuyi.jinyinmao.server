// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-20  2:07 PM
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
            this.State.Cellphone = userRegister.Cellphone;
            this.State.RegisterTime = registerTime;

            this.State.JinyinmaoAccount = JinyinmaoAccountFactory.GetGrain(Guid.NewGuid());
            await this.State.JinyinmaoAccount.Register(new JinyinmaoAccountRegister
            {
                LoginNames = new List<string> { userRegister.Cellphone },
                Password = userRegister.Password,
                Salt = this.State.Id.ToGuidString(),
                UserId = this.State.Id
            });

            this.State.SourceAccount = SourceAccountFactory.GetGrain(Guid.NewGuid());
            await this.State.SourceAccount.Register(new SourceAccountRegister
            {
                ClientType = userRegister.ClientType,
                ContractId = userRegister.ContractId,
                InviteBy = userRegister.InviteBy,
                OutletCode = userRegister.OutletCode,
                UserId = this.State.Id,
                Args = userRegister.Args
            });

            this.State.JBYAccount = JBYAccountFactory.GetGrain(Guid.NewGuid());

            await this.State.WriteStateAsync();

#pragma warning disable 4014
            this.StoreCommandAsync(userRegister);

            this.StoreEventAsync(new UserRegistered(this.State.Id.ToGuidString())
            {
                Args = userRegister.Args,
                Cellphone = userRegister.Cellphone,
                ClientType = userRegister.ClientType,
                ContractId = userRegister.ContractId,
                InviteBy = userRegister.InviteBy,
                OutletCode = userRegister.OutletCode,
                UserId = userRegister.UserId
            });
#pragma warning restore 4014
        }

        #endregion IUser Members
    }
}
