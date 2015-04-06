// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-02  12:13 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-05  7:13 PM
// ***********************************************************************
// <copyright file="User.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Actor.Model;
using Moe.Lib;
using Orleans;
using Yuyi.Jinyinmao.Domain.Commands;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class User.
    /// </summary>
    public class User : EntityGrain<IUserState>, IUser
    {
        #region IUser Members

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
            this.State.JinyinmaoAccount = JinyinmaoAccountFactory.GetGrain(GuidUtility.NewSequentialGuid());
            this.State.SourceAccount = SourceAccountFactory.GetGrain(GuidUtility.NewSequentialGuid());

            return this.State.WriteStateAsync();
        }

        #endregion IUser Members
    }
}
