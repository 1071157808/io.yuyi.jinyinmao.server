namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ZCBProducts
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        public int EnableSale { get; set; }

        public decimal TotalSaleAmount { get; set; }

        public decimal TotalInterest { get; set; }

        public decimal TotalRedeemAmount { get; set; }

        public decimal TotalRedeemInterest { get; set; }

        [Required]
        [StringLength(40)]
        public string SubProductNo { get; set; }

        public decimal PerRemainRedeemAmount { get; set; }

        [StringLength(80)]
        public string ConsignmentAgreementName { get; set; }

        [StringLength(80)]
        public string PledgeAgreementName { get; set; }
    }
}
