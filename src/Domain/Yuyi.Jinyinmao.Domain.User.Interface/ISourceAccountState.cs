// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-04  9:10 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-04  9:21 PM
// ***********************************************************************
// <copyright file="ISourceAccountState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Moe.Actor.Model;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface ISourceAccountState
    /// </summary>
    public interface ISourceAccountState : IEntityState
    {
        /// <summary>
        ///     Gets or sets the type of the client.
        /// </summary>
        /// <value>The type of the client.</value>
        long ClientType { get; set; }

        /// <summary>
        ///     Gets or sets the contract identifier.
        /// </summary>
        /// <value>The contract identifier.</value>
        long ContractId { get; set; }

        /// <summary>
        ///     Gets or sets the identification code.
        /// </summary>
        /// <value>The identification code.</value>
        string IdentificationCode { get; set; }

        /// <summary>
        ///     Gets or sets the invite by.
        /// </summary>
        /// <value>The invite by.</value>
        string InviteBy { get; set; }

        /// <summary>
        ///     Gets or sets the ip reg.
        /// </summary>
        /// <value>The ip reg.</value>
        string IpReg { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>The note.</value>
        string Note { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        Guid UserId { get; set; }
    }
}
