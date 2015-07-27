namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ValueInfo")]
    public partial class ValueInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime RepaymentDeadline { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime SettleDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValueDate { get; set; }

        public int ValueDateMode { get; set; }
    }
}
