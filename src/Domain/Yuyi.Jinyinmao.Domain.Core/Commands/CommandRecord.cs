// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  8:18 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  5:56 PM
// ***********************************************************************
// <copyright file="CommandRecord.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     CommandRecord.
    /// </summary>
    public class CommandRecord : TableEntity
    {
        /// <summary>
        ///     Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        public ICommand Command { get; set; }

        /// <summary>
        ///     Gets or sets the command identifier.
        /// </summary>
        /// <value>The command identifier.</value>
        public Guid CommandId { get; set; }

        /// <summary>
        ///     Gets or sets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public long TimeStamp { get; set; }
    }
}
