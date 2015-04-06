// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-05  7:57 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-05  8:03 PM
// ***********************************************************************
// <copyright file="IClusterConfigState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IClusterConfigState
    /// </summary>
    public interface IClusterConfigState : IGrainState
    {
        /// <summary>
        ///     Gets or sets the user cellphone manager identifier.
        /// </summary>
        /// <value>The user cellphone manager identifier.</value>
        Guid UserCellphoneManagerId { get; set; }
    }
}
