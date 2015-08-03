// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SaleInfo.cs
// Created          : 2015-07-27  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  3:38 PM
// ***********************************************************************
// <copyright file="SaleInfo.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    [Table("SaleInfo")]
    public class SaleInfo
    {
        public int FinancingSumCount { get; set; }

        public int Id { get; set; }

        public int MaxShareCount { get; set; }

        public int MinShareCount { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        public decimal UnitPrice { get; set; }
    }
}