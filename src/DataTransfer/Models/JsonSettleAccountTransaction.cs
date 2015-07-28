namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("JsonSettleAccountTransaction")]
    public partial class JsonSettleAccountTransaction
    {
        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string UserId { get; set; }

        [Required]
        public string Data { get; set; }

        [Required]
        [StringLength(32)]
        public string OrderId { get; set; }
    }
}
