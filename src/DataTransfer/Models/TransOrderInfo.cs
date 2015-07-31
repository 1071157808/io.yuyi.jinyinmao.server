namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransOrderInfo")]
    public partial class TransOrderInfo
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(1)]
        public string AccountTransactionIdentifier { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string Args { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(15)]
        public string Cellphone { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal ExtraInterest { get; set; }

        public int? ExtraInterestRecords { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ExtraYield { get; set; }

        [Key]
        [Column(Order = 5)]
        public decimal Interest { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool IsRepaid { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string OrderId { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(20)]
        public string OrderNo { get; set; }

        [Key]
        [Column(Order = 9, TypeName = "datetime2")]
        public DateTime OrderTime { get; set; }

        [Key]
        [Column(Order = 10)]
        public decimal Principal { get; set; }

        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductCategory { get; set; }

        public int? ProductType { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(50)]
        public string ProductId { get; set; }

        public int? ProductSnapshot { get; set; }

        public int? RepaidTime { get; set; }

        public int? ResultCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ResultTime { get; set; }

        [Key]
        [Column(Order = 13, TypeName = "datetime2")]
        public DateTime SettleDate { get; set; }

        [StringLength(200)]
        public string TransDesc { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(50)]
        public string UserId { get; set; }

        public int? UserInfo { get; set; }

        [Key]
        [Column(Order = 15, TypeName = "datetime2")]
        public DateTime ValueDate { get; set; }

        [Key]
        [Column(Order = 16)]
        public decimal Yield { get; set; }
    }
}
