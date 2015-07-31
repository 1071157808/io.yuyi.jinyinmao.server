// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : PayCommand.cs
// Created          : 2015-05-27  7:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-31  7:26 PM
// ***********************************************************************
// <copyright file="PayCommand.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     PayCommand.
    /// </summary>
    public abstract class PayCommand : Command
    {
        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public long Amount { get; set; }

        /// <summary>
        ///     Gets or sets the bank card no.
        /// </summary>
        /// <value>The bank card no.</value>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }
    }
}