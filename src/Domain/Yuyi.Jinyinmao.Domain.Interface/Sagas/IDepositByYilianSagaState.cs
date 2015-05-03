// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  6:42 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-03  6:58 PM
// ***********************************************************************
// <copyright file="IDepositByYilianSagaState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Sagas
{
    /// <summary>
    ///     Interface IDepositByYilianSagaState
    /// </summary>
    public interface IDepositByYilianSagaState : ISagaState
    {
        /// <summary>
        ///     Gets or sets the initialize data.
        /// </summary>
        /// <value>The initialize data.</value>
        DepositFromYilianSagaInitDto InitData { get; set; }
    }
}