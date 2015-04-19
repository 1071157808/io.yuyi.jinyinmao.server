// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-12  6:03 PM
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
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        string Args { get; set; }

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
        ///     Gets or sets the invite by.
        /// </summary>
        /// <value>The invite by.</value>
        string InviteBy { get; set; }

        /// <summary>
        ///     Gets or sets the identification code.
        /// </summary>
        /// <value>The identification code.</value>
        string OutletCode { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        Guid UserId { get; set; }
    }
}
