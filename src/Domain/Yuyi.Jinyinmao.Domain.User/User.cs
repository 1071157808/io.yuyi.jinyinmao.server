// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-02  12:13 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-11  2:11 AM
// ***********************************************************************
// <copyright file="User.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Actor.Model;
using Orleans;
using Orleans.Providers;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;

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
        public Task RegisterAsync(UserRegister userRegister)
        {
            if (this.State.Id == userRegister.UserId)
            {
                return TaskDone.Done;
            }

            if (this.State.Id != Guid.Empty)
            {
                // TODO: warning
                return TaskDone.Done;
            }

            this.State.Id = userRegister.UserId;
            this.State.Cellphone = userRegister.Cellphone;
            //this.State.JinyinmaoAccount = JinyinmaoAccountFactory.GetGrain(GuidUtility.NewSequentialGuid());
            //this.State.SourceAccount = SourceAccountFactory.GetGrain(GuidUtility.NewSequentialGuid());

            return this.State.WriteStateAsync();
        }

        #endregion IUser Members
    }
}
