namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("JsonProduct")]
    public partial class JsonProduct
    {
        public int Id { get; set; }

        [Required]
        public string Data { get; set; }

        public Guid ProductId { get; set; }
    }
}
