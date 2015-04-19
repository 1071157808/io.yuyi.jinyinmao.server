// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-12  6:46 PM
// ***********************************************************************
// <copyright file="JinyinmaoAccount.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Actor.Model;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Helper;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class JinyinmaoAccountRegister.
    /// </summary>
    public class JinyinmaoAccount : EntityGrain<IJinyinmaoAccountState>, IJinyinmaoAccount
    {
        #region IJinyinmaoAccount Members

        /// <summary>
        ///     Registers the specified jinyinmao account register.
        /// </summary>
        /// <param name="jinyinmaoAccountRegister">The jinyinmao account register.</param>
        /// <returns>Task.</returns>
        public Task Register(JinyinmaoAccountRegister jinyinmaoAccountRegister)
        {
            this.State.Id = Guid.NewGuid();
            this.State.UserId = jinyinmaoAccountRegister.UserId;
            this.State.LoginNames = jinyinmaoAccountRegister.LoginNames;
            this.State.Salt = jinyinmaoAccountRegister.Salt;
            this.State.EncryptedPassword = CryptographyHelper.Encrypting(jinyinmaoAccountRegister.Password, jinyinmaoAccountRegister.Salt);

            return this.State.WriteStateAsync();
        }

        #endregion IJinyinmaoAccount Members
    }
}
