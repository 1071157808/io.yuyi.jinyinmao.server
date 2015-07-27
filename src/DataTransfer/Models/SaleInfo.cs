namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SaleInfo")]
    public partial class SaleInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        public int FinancingSumCount { get; set; }

        public int MaxShareCount { get; set; }

        public int MinShareCount { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
