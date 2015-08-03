// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : BankAcceptanceProducts.cs
// Created          : 2015-07-27  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  4:00 PM
// ***********************************************************************
// <copyright file="BankAcceptanceProducts.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace DataTransfer.Models
{
    public class BankAcceptanceProducts
    {
        [Required]
        [StringLength(100)]
        public string BankName { get; set; }

        [Required]
        [StringLength(80)]
        public string BillNo { get; set; }

        [Required]
        [StringLength(80)]
        public string BusinessLicense { get; set; }

        [Required]
        [StringLength(80)]
        public string EnterpriseName { get; set; }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        [Required]
        [StringLength(200)]
        public string Usage { get; set; }
    }
}