// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  2:18 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  2:18 PM
// ***********************************************************************
// <copyright file="ICommand.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface ICommand
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        ///     Gets the command identifier.
        /// </summary>
        /// <value>The command identifier.</value>
        Guid CommandId { get; }
    }
}
