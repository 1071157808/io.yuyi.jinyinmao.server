// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : TransSettleAccountTransaction.cs
// Created          : 2015-07-31  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-31  7:42 PM
// ***********************************************************************
// <copyright file="TransSettleAccountTransaction.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    [Table("TransSettleAccountTransaction")]
    public class TransSettleAccountTransaction
    {
        [StringLength(30)]
        public string BankCardNo { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CallbackTime { get; set; }

        [Key]
        [StringLength(50)]
        public string OrderId { get; set; }
    }
}