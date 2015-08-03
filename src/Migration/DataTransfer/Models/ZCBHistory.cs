// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : ZCBHistory.cs
// Created          : 2015-07-27  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  3:41 PM
// ***********************************************************************
// <copyright file="ZCBHistory.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    [Table("ZCBHistory")]
    public class ZCBHistory
    {
        [Required]
        public string ConsignmentAgreement { get; set; }

        [Required]
        [StringLength(80)]
        public string ConsignmentAgreementName { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreateTime { get; set; }

        public int EnableSale { get; set; }

        public int FinancingSumCount { get; set; }

        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime NextEndSellTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime NextStartSellTime { get; set; }

        public decimal NextYield { get; set; }

        public decimal PerRemainRedeemAmount { get; set; }

        [Required]
        public string PledgeAgreement { get; set; }

        [Required]
        [StringLength(80)]
        public string PledgeAgreementName { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductNo { get; set; }

        [Required]
        [StringLength(40)]
        public string SubProductNo { get; set; }

        public decimal UnitPrice { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdateTime { get; set; }
    }
}