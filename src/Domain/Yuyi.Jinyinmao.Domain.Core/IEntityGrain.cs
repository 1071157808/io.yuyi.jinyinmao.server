// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  8:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  5:47 PM
// ***********************************************************************
// <copyright file="IEntityGrain.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IEntityState
    /// </summary>
    public interface IEntityState : IGrainState
    {
        /// <summary>
        ///     Gets or sets the command store.
        /// </summary>
        /// <value>The command store.</value>
        ICommandStore CommandStore { get; set; }

        /// <summary>
        ///     Gets or sets the event store.
        /// </summary>
        /// <value>The event store.</value>
        IEventStore EventStore { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        Guid Id { get; set; }
    }
}
