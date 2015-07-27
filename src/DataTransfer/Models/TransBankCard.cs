// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : TransBankCard.cs
// Created          : 2015-07-27  3:33 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  3:40 PM
// ***********************************************************************
// <copyright file="TransBankCard.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    [Table("TransBankCard")]
    public class TransBankCard
    {
        public DateTime? AddingTime { get; set; }

        public int? Args { get; set; }

        [StringLength(25)]
        public string BankCardNo { get; set; }

        [StringLength(20)]
        public string BankName { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string Cellphone { get; set; }

        [StringLength(20)]
        public string CityName { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Dispaly { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string UserId { get; set; }

        public int? Verified { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VerifiedByYilian { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? VerifiedTime { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WithdrawAmount { get; set; }
    }
}