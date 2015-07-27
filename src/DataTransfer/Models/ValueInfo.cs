// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : ValueInfo.cs
// Created          : 2015-07-27  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  3:41 PM
// ***********************************************************************
// <copyright file="ValueInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    [Table("ValueInfo")]
    public class ValueInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime RepaymentDeadline { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime SettleDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValueDate { get; set; }

        public int ValueDateMode { get; set; }
    }
}