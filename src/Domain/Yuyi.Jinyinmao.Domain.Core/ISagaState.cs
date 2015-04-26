// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  8:28 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  9:41 PM
// ***********************************************************************
// <copyright file="ISagaState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans;

namespace Yuyi.Jinyinmao.Domain.Sagas
{
    /// <summary>
    ///     Interface ISagaState
    /// </summary>
    public interface ISagaState : IGrainState
    {
        /// <summary>
        ///     Gets or sets the saga identifier.
        /// </summary>
        /// <value>The saga identifier.</value>
        Guid SagaId { get; set; }

        /// <summary>
        ///     Gets or sets the type of the saga.
        /// </summary>
        /// <value>The type of the saga.</value>
        string SagaType { get; set; }
    }
}
