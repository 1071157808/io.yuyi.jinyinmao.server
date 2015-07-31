namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransJbyOrderInfo")]
    public partial class TransJbyOrderInfo
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(1)]
        public string AccountTransactionIdentifier { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string Args { get; set; }

        [StringLength(20)]
        public string Cellphone { get; set; }

        public int? ExtraInterest { get; set; }

        public int? ExtraInterestRecords { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ExtraYield { get; set; }

        public int? Interest { get; set; }

        public bool? IsRepaid { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string OrderId { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string OrderNo { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "datetime2")]
        public DateTime OrderTime { get; set; }

        [Key]
        [Column(Order = 6)]
        public decimal Principal { get; set; }

        public int? ProductCategory { get; set; }

        public int? ProductType { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string ProductId { get; set; }

        public int? ProductSnapshot { get; set; }

        public int? RepaidTime { get; set; }

        public int? ResultCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ResultTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? SettleDate { get; set; }

        [StringLength(200)]
        public string TransDesc { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(50)]
        public string UserId { get; set; }

        public int? UserInfo { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValueDate { get; set; }

        public int? Yield { get; set; }

        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Type { get; set; }
    }
}
