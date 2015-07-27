// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SagaStateRecordResult.cs
// Created          : 2015-07-27  6:28 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  6:37 PM
// ***********************************************************************
// <copyright file="SagaStateRecordResult.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace SagasTransfer
{
    public class SagaStateRecordResult
    {
        public DateTime BeginTime { get; set; }

        public int CurrentProcessingStatus { get; set; }

        public string Info { get; set; }

        public string Message { get; set; }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public Guid SagaId { get; set; }

        public string SagaState { get; set; }

        public string SagaType { get; set; }

        public int State { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}