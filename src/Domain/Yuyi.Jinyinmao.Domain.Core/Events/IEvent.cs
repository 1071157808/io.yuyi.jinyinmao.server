// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  2:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  11:22 PM
// ***********************************************************************
// <copyright file="IEvent.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IEvent
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        ///     Gets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        Guid EventId { get; }

        /// <summary>
        ///     Gets the source identifier.
        /// </summary>
        /// <value>The source identifier.</value>
        string SourceId { get; }

        /// <summary>
        ///     Gets or sets the type of the source.
        /// </summary>
        /// <value>The type of the source.</value>
        string SourceType { get; set; }
    }
}
