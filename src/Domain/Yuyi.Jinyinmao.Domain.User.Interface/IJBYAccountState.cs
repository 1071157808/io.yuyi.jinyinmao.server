// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-12  6:42 PM
// ***********************************************************************
// <copyright file="IJBYAccountState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Moe.Actor.Interface.Model;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IJBYAccountState
    /// </summary>
    public interface IJBYAccountState : IEntityState
    {
        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        Guid UserId { get; set; }
    }
}
