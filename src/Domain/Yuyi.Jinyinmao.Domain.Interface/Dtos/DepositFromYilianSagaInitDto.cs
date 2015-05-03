// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  6:37 PM
// 
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  12:17 AM
// ***********************************************************************
// <copyright file="DepositFromYilianSagaInitDto.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Commands;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     DepositFromYilianSagaInitDto.
    /// </summary>
    [Immutable]
    public class DepositFromYilianSagaInitDto
    {
        /// <summary>
        ///     Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        public DepositFromYilian Command { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        ///     Gets or sets the back card information.
        /// </summary>
        /// <value>The back card information.</value>
        public BankCardInfo BackCardInfo { get; set; }

        /// <summary>
        ///     Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        public TranscationInfo TranscationInfo { get; set; }
    }
}