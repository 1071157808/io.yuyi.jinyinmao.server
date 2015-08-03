// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SalePeriod.cs
// Created          : 2015-07-27  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  3:38 PM
// ***********************************************************************
// <copyright file="SalePeriod.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    [Table("SalePeriod")]
    public class SalePeriod
    {
        [Column(TypeName = "datetime2")]
        public DateTime EndSellTime { get; set; }

        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PreEndSellTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PreStartSellTime { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartSellTime { get; set; }
    }
}