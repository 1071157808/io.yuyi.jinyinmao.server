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

        public Guid UserId { get; set; }

        public string Data { get; set; }

        public Guid OrderId { get; set; }

    }
}
