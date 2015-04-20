// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-20  1:32 PM
// ***********************************************************************
// <copyright file="JBYAccount.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using Moe.Actor.Models;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class JBYAccount.
    /// </summary>
    public class JBYAccount : EntityGrain<IJBYAccountState>, IJBYAccount
    {
        #region IJBYAccount Members

        /// <summary>
        ///     Registers the specified jby account register.
        /// </summary>
        /// <param name="jbyAccountRegister">The jby account register.</param>
        /// <returns>Task.</returns>
        public Task Register(JBYAccountRegister jbyAccountRegister)
        {
            this.State.Id = Guid.NewGuid();
            this.State.UserId = jbyAccountRegister.UserId;

            return this.State.WriteStateAsync();
        }

        #endregion IJBYAccount Members
    }
}
