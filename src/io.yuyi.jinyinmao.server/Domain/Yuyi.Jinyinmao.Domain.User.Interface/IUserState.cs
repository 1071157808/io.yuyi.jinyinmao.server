// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-03  6:32 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-04  5:51 PM
// ***********************************************************************
// <copyright file="IUserState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Moe.Orleans.Model;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IUserState
    /// </summary>
    public interface IUserState : IEntityState
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the name of the real.
        /// </summary>
        /// <value>The name of the real.</value>
        string RealName { get; set; }
    }
}
