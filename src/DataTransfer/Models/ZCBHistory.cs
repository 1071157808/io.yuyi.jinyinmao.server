namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ZCBHistory")]
    public partial class ZCBHistory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductNo { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        public int FinancingSumCount { get; set; }

        [Required]
        [StringLength(40)]
        public string SubProductNo { get; set; }

        public decimal UnitPrice { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime NextStartSellTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime NextEndSellTime { get; set; }

        public decimal NextYield { get; set; }

        public int EnableSale { get; set; }

        [Required]
        [StringLength(80)]
        public string PledgeAgreementName { get; set; }

        [Required]
        public string PledgeAgreement { get; set; }

        [Required]
        [StringLength(80)]
        public string ConsignmentAgreementName { get; set; }

        [Required]
        public string ConsignmentAgreement { get; set; }

        public decimal PerRemainRedeemAmount { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreateTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdateTime { get; set; }
    }
}
