// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-07  10:56 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-07  11:39 AM
// ***********************************************************************
// <copyright file="ICellphoneState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface ICellphoneState
    /// </summary>
    public interface ICellphoneState : IGrainState
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this cellphone is registered.
        /// </summary>
        /// <value><c>true</c> if registered; otherwise, <c>false</c>.</value>
        bool Registered { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        Guid? UserId { get; set; }
    }
}
