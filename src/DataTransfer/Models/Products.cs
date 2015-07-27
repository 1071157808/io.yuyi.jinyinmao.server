namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Products
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        public int ConsignmentAgreementId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LaunchTime { get; set; }

        public int Period { get; set; }

        public int PledgeAgreementId { get; set; }

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

        public int ProductCategory { get; set; }
    }
}
