namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransSettleAccountTransaction")]
    public partial class TransSettleAccountTransaction
    {
        [Key]
        [StringLength(50)]
        public string OrderId { get; set; }

        [StringLength(30)]
        public string BankCardNo { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CallbackTime { get; set; }
    }
}
