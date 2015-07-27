// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : TransRegularProductInfo.cs
// Created          : 2015-07-27  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  3:40 PM
// ***********************************************************************
// <copyright file="TransRegularProductInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    [Table("TransRegularProductInfo")]
    public class TransRegularProductInfo
    {
        public int? Args { get; set; }

        [StringLength(100)]
        public string BankName { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(300)]
        public string Drawee { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1000)]
        public string DraweeInfo { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(300)]
        public string EndorseImageLink { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "datetime2")]
        public DateTime EndSellTime { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(1000)]
        public string EnterpriseInfo { get; set; }

        [StringLength(80)]
        public string EnterpriseLicense { get; set; }

        [StringLength(80)]
        public string EnterpriseName { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FinancingSumAmount { get; set; }

        public int? IssueNo { get; set; }

        public int? IssueTime { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Period { get; set; }

        public int? PledgeNo { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductCategory { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(50)]
        public string ProductId { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(40)]
        public string ProductNo { get; set; }

        [Key]
        [Column(Order = 11)]
        public bool Repaid { get; set; }

        public int? RepaidTime { get; set; }

        [Key]
        [Column(Order = 12, TypeName = "datetime2")]
        public DateTime RepaymentDeadline { get; set; }

        [StringLength(300)]
        public string RiskManagement { get; set; }

        [StringLength(1000)]
        public string RiskManagementInfo { get; set; }

        public int? RiskManagementMode { get; set; }

        [Key]
        [Column(Order = 13, TypeName = "datetime2")]
        public DateTime SettleDate { get; set; }

        [Key]
        [Column(Order = 14)]
        public bool SoldOut { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? SoldOutTime { get; set; }

        [Key]
        [Column(Order = 15, TypeName = "datetime2")]
        public DateTime StartSellTime { get; set; }

        [Key]
        [Column(Order = 16)]
        public decimal UnitPrice { get; set; }

        [StringLength(200)]
        public string Usage { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValueDate { get; set; }

        [Key]
        [Column(Order = 17)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ValueDateMode { get; set; }

        [Key]
        [Column(Order = 18)]
        public decimal Yield { get; set; }
    }
}