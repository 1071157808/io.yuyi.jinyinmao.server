// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  2:45 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  5:46 PM
// ***********************************************************************
// <copyright file="EventRecord.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     EventRecord.
    /// </summary>
    public class EventRecord : TableEntity
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

        public string EventName { get; set; }

        /// <summary>
        ///     Gets or sets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public long TimeStamp { get; set; }
    }
}