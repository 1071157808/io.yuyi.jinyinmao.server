// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-06  12:03 AM
// ***********************************************************************
// <copyright file="SagaEntity.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Newtonsoft.Json;

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
        public string Info { get; set; }

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
        ///     Gets or sets the update time. 0 => Processing, -1 => Error, 1 => Complete
        /// </summary>
        /// <value>The update time.</value>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        ///     Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, object value)
        {
            Dictionary<string, object> info = JsonConvert.DeserializeObject<Dictionary<string, object>>(this.Info);
            if (info.ContainsKey(key))
            {
                info[key] = value;
            }
            else
            {
                info.Add(key, value);
            }

            this.Info = info.ToJson();
        }
    }
}