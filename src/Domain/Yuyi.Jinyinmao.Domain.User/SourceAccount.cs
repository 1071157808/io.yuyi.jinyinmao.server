// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-12  5:42 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-12  6:34 PM
// ***********************************************************************
// <copyright file="SourceAccount.cs" company="Shanghai Yuyi">
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
    ///     Class SourceAccount.
    /// </summary>
    public class SourceAccount : EntityGrain<ISourceAccountState>, ISourceAccount
    {
        #region ISourceAccount Members

        /// <summary>
        ///     Registers the specified jinyinmao account register.
        /// </summary>
        /// <param name="sourceAccountRegister">The source account register.</param>
        /// <returns>Task.</returns>
        public async Task Register(SourceAccountRegister sourceAccountRegister)
        {
            this.State.Id = Guid.NewGuid();
            this.State.ClientType = sourceAccountRegister.ClientType;
            this.State.ContractId = sourceAccountRegister.ContractId;
            this.State.InviteBy = sourceAccountRegister.InviteBy;
            this.State.OutletCode = sourceAccountRegister.OutletCode;
            this.State.UserId = sourceAccountRegister.UserId;
            this.State.Args = sourceAccountRegister.Args;

            await this.State.WriteStateAsync();
        }

        #endregion ISourceAccount Members
    }
}
