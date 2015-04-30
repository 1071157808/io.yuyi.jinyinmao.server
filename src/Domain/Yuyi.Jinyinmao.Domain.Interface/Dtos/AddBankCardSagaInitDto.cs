// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  2:13 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  4:42 PM
// ***********************************************************************
// <copyright file="AddBankCardSagaInitDto.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Commands;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     AddBankCardSagaInitDto.
    /// </summary>
    [Immutable]
    public class AddBankCardSagaInitDto
    {
        /// <summary>
        ///     Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        public AddBankCard Command { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}
