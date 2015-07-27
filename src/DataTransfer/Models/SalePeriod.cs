namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalePeriod")]
    public partial class SalePeriod
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartSellTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EndSellTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PreStartSellTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PreEndSellTime { get; set; }
    }
}
