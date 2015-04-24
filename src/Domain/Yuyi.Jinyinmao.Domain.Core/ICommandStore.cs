// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  8:17 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  2:49 PM
// ***********************************************************************
// <copyright file="ICommandStore.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface ICommandStore
    /// </summary>
    public interface ICommandStore : IDictionary<Guid, string>
    {
        /// <summary>
        ///     Gets the entity identifier.
        /// </summary>
        /// <value>The entity identifier.</value>
        Guid EntityId { get; }

        /// <summary>
        ///     Stores the command record asynchronous.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>Task.</returns>
        Task StoreCommandRecordAsync(CommandRecord record);
    }
}
