// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  2:12 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  5:47 PM
// ***********************************************************************
// <copyright file="IEventStore.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IEventStore
    /// </summary>
    public interface IEventStore : IDictionary<Guid, string>
    {
        /// <summary>
        ///     Gets the entity identifier.
        /// </summary>
        /// <value>The entity identifier.</value>
        Guid EntityId { get; }

        /// <summary>
        ///     Stores the event record asynchronous.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>Task.</returns>
        Task StoreEventRecordAsync(EventRecord record);
    }
}
