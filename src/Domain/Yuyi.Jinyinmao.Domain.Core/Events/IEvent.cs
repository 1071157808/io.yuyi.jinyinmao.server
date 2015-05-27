// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-15  2:00 PM
// ***********************************************************************
// <copyright file="IEvent.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
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

        /// <summary>
        ///     Gets or sets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        DateTime TimeStamp { get; set; }
    }
}