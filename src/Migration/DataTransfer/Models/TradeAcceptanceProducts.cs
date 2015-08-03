// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : TradeAcceptanceProducts.cs
// Created          : 2015-07-27  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  3:39 PM
// ***********************************************************************
// <copyright file="TradeAcceptanceProducts.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace DataTransfer.Models
{
    public class TradeAcceptanceProducts
    {
        [Required]
        [StringLength(80)]
        public string BillNo { get; set; }

        [Required]
        [StringLength(80)]
        public string ConsignmentAgreementName { get; set; }

        [Required]
        [StringLength(300)]
        public string Drawee { get; set; }

        [Required]
        [StringLength(1000)]
        public string DraweeInfo { get; set; }

        [Required]
        [StringLength(1000)]
        public string EnterpriseInfo { get; set; }

        [Required]
        [StringLength(80)]
        public string EnterpriseLicense { get; set; }

        [Required]
        [StringLength(300)]
        public string EnterpriseName { get; set; }

        public int GuaranteeMode { get; set; }

        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string PledgeAgreementName { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        [Required]
        [StringLength(300)]
        public string Securedparty { get; set; }

        [Required]
        [StringLength(1000)]
        public string SecuredpartyInfo { get; set; }

        [Required]
        [StringLength(1000)]
        public string Usage { get; set; }
    }
}