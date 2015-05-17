// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-27  12:10 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-15  2:01 PM
// ***********************************************************************
// <copyright file="Event.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     Event.
    /// </summary>
    public abstract class Event : IEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Event" /> class.
        /// </summary>
        protected Event()
        {
            this.EventId = Guid.NewGuid();
        }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public Dictionary<string, object> Args { get; set; }

        #region IEvent Members

        /// <summary>
        ///     Gets or sets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        ///     Gets or sets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public Guid EventId { get; set; }

        /// <summary>
        ///     Gets or sets the source identifier.
        /// </summary>
        /// <value>The source identifier.</value>
        public string SourceId { get; set; }

        /// <summary>
        ///     Gets or sets the type of the source.
        /// </summary>
        /// <value>The type of the source.</value>
        public string SourceType { get; set; }

        #endregion IEvent Members
    }
}