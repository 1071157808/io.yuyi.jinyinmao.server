// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  2:07 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  10:16 PM
// ***********************************************************************
// <copyright file="IAddBankCardSagaState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Sagas
{
    /// <summary>
    ///     Interface IAddBankCardSagaState
    /// </summary>
    public interface IAddBankCardSagaState : ISagaState
    {
        /// <summary>
        ///     Gets or sets the initialize data.
        /// </summary>
        /// <value>The initialize data.</value>
        AddBankCardSageInitDto InitData { get; set; }
    }
}
