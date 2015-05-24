// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-22  5:47 PM
// ***********************************************************************
// <copyright file="EventRecord.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     EventEx.
    /// </summary>
    public static class EventEx
    {
        /// <summary>
        ///     To the record.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>EventRecord.</returns>
        public static EventRecord ToRecord(this IEvent @event)
        {
            return new EventRecord
            {
                Event = @event.ToJson(),
                EventId = @event.EventId,
                EventName = @event.GetType().Name
            };
        }
    }

    /// <summary>
    ///     EventRecord.
    /// </summary>
    public class EventRecord
    {
        /// <summary>
        ///     Gets or sets the event.
        /// </summary>
        /// <value>The event.</value>
        public string Event { get; set; }

        /// <summary>
        ///     Gets or sets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public Guid EventId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the event.
        /// </summary>
        /// <value>The name of the event.</value>
        public string EventName { get; set; }
    }
}