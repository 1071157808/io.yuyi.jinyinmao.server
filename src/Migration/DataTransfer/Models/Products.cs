// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Products.cs
// Created          : 2015-07-27  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  3:37 PM
// ***********************************************************************
// <copyright file="Products.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransfer.Models
{
    public class Products
    {
        public int ConsignmentAgreementId { get; set; }

        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LaunchTime { get; set; }

        public int Period { get; set; }

        public int PledgeAgreementId { get; set; }

        public int ProductCategory { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductNo { get; set; }

        public int ProductNumber { get; set; }

        public int ProductType { get; set; }

        public bool Repaid { get; set; }

        public bool SoldOut { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? SoldOutTime { get; set; }

        public decimal Yield { get; set; }
    }
}