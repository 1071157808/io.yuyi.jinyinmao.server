// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : ZCBProducts.cs
// Created          : 2015-07-27  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  3:42 PM
// ***********************************************************************
// <copyright file="ZCBProducts.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace DataTransfer.Models
{
    public class ZCBProducts
    {
        [StringLength(80)]
        public string ConsignmentAgreementName { get; set; }

        public int EnableSale { get; set; }

        public int Id { get; set; }

        public decimal PerRemainRedeemAmount { get; set; }

        [StringLength(80)]
        public string PledgeAgreementName { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        [Required]
        [StringLength(40)]
        public string SubProductNo { get; set; }

        public decimal TotalInterest { get; set; }

        public decimal TotalRedeemAmount { get; set; }

        public decimal TotalRedeemInterest { get; set; }

        public decimal TotalSaleAmount { get; set; }
    }
}