// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  8:17 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  10:14 PM
// ***********************************************************************
// <copyright file="SagaEntity.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     SagaEntity.
    /// </summary>
    public class SagaEntity : TableEntity
    {
        /// <summary>
        ///     Gets or sets the begin time.
        /// </summary>
        /// <value>The begin time.</value>
        public DateTime BeginTime { get; set; }

        /// <summary>
        ///     Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public Dictionary<string, object> Info { get; set; }

        /// <summary>
        ///     Gets or sets the initialize data.
        /// </summary>
        /// <value>The initialize data.</value>
        public string InitData { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets the saga identifier.
        /// </summary>
        /// <value>The saga identifier.</value>
        public Guid SagaId { get; set; }

        /// <summary>
        ///     Gets or sets the type of the saga.
        /// </summary>
        /// <value>The type of the saga.</value>
        public string SagaType { get; set; }

        /// <summary>
        ///     Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public int State { get; set; }

        /// <summary>
        ///     Gets or sets the update time.
        /// </summary>
        /// <value>The update time.</value>
        public DateTime UpdateTime { get; set; }
    }
}
