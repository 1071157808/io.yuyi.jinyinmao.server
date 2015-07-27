// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : OrderEntity.cs
// Created          : 2015-07-27  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  4:00 PM
// ***********************************************************************
// <copyright file="OrderEntity.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace DataTransfer.Models.Entity
{
    internal class OrderEntity : TableEntity
    {
        public Guid AccountTransactionId { get; set; }

        public Dictionary<string, object> Args { get; set; }

        public string Cellphone { get; set; }

        public long ExtraInterest { get; set; }

        public List<ExtraInterestRecord> ExtraInterestRecords { get; set; }

        public int ExtraYield { get; set; }

        public long Interest { get; set; }

        public bool IsRepaid { get; set; }

        public Guid OrderId { get; set; }

        public string OrderNo { get; set; }

        public DateTime OrderTime { get; set; }

        public long Principal { get; set; }

        public long ProductCategory { get; set; }

        public Guid ProductId { get; set; }

        public RegularProductInfo ProductSnapshot { get; set; }

        public DateTime? RepaidTime { get; set; }

        public int ResultCode { get; set; }

        public DateTime? ResultTime { get; set; }

        public DateTime SettleDate { get; set; }

        public string TransDesc { get; set; }

        public Guid UserId { get; set; }

        public UserInfo UserInfo { get; set; }

        public DateTime ValueDate { get; set; }

        public int Yield { get; set; }
    }
}