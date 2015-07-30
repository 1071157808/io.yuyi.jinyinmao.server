// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SagaStateRecordResult.cs
// Created          : 2015-07-27  6:28 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-29  12:52 PM
// ***********************************************************************
// <copyright file="SagaStateRecordResult.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace SagasTransfer
{
    /// <summary>
    ///     SagaStateRecordResult.
    /// </summary>
    public class SagaStateRecordResult
    {
        /// <summary>
        ///     Gets or sets the begin time.
        /// </summary>
        /// <value>The begin time.</value>
        public DateTime BeginTime { get; set; }

        /// <summary>
        ///     Gets or sets the current processing status.
        /// </summary>
        /// <value>The current processing status.</value>
        public int CurrentProcessingStatus { get; set; }

        /// <summary>
        ///     Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public string Info { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets the partition key.
        /// </summary>
        /// <value>The partition key.</value>
        public string PartitionKey { get; set; }

        /// <summary>
        ///     Gets or sets the row key.
        /// </summary>
        /// <value>The row key.</value>
        public string RowKey { get; set; }

        /// <summary>
        ///     Gets or sets the saga identifier.
        /// </summary>
        /// <value>The saga identifier.</value>
        public Guid SagaId { get; set; }

        /// <summary>
        ///     Gets or sets the state of the saga.
        /// </summary>
        /// <value>The state of the saga.</value>
        public string SagaState { get; set; }

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