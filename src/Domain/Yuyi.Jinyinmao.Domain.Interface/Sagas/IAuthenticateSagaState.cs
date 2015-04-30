// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-27  4:49 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  4:50 PM
// ***********************************************************************
// <copyright file="IAuthenticateSagaState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Sagas
{
    /// <summary>
    ///     Interface IAuthenticateSagaState
    /// </summary>
    public interface IAuthenticateSagaState : ISagaState
    {
        /// <summary>
        ///     Gets or sets the initialize data.
        /// </summary>
        /// <value>The initialize data.</value>
        AuthenticateSagaInitDto InitData { get; set; }
    }
}
