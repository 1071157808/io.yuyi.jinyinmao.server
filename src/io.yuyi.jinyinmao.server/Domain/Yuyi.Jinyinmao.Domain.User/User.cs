// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-02  12:13 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-04  5:59 PM
// ***********************************************************************
// <copyright file="User.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Orleans.Model;

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
        public Task Register(UserRegister userRegister)
        {
            throw new NotImplementedException();
        }

        #endregion IUser Members
    }
}
