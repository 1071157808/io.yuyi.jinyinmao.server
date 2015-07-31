namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("JsonJBYAccountTransaction")]
    public partial class JsonJBYAccountTransaction
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        [Required]
        public string Data { get; set; }

        public Guid OrderId { get; set; }
    }
}
