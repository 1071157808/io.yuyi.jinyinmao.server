namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransRegularProductState")]
    public partial class TransRegularProductState
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Agreement1 { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Agreement2 { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string ProductId { get; set; }

        public int? Args { get; set; }

        [StringLength(100)]
        public string BankName { get; set; }

        [StringLength(300)]
        public string Drawee { get; set; }

        [StringLength(1000)]
        public string DraweeInfo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(300)]
        public string EndorseImageLink { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "datetime2")]
        public DateTime EndSellTime { get; set; }

        [StringLength(1000)]
        public string EnterpriseInfo { get; set; }

        [StringLength(80)]
        public string EnterpriseLicense { get; set; }

        [StringLength(300)]
        public string EnterpriseName { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FinancingSumAmount { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IssueNo { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "datetime2")]
        public DateTime IssueTime { get; set; }

        public int? Orders { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Period { get; set; }

        [StringLength(80)]
        public string PledgeNo { get; set; }

        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductCategory { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductType { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(40)]
        public string ProductNo { get; set; }

        [Key]
        [Column(Order = 13)]
        public bool Repaid { get; set; }

        public int? RepaidTime { get; set; }

        [Key]
        [Column(Order = 14, TypeName = "datetime2")]
        public DateTime RepaymentDeadline { get; set; }

        [StringLength(300)]
        public string RiskManagement { get; set; }

        [StringLength(1000)]
        public string RiskManagementInfo { get; set; }

        public int? RiskManagementMode { get; set; }

        [Key]
        [Column(Order = 15, TypeName = "datetime2")]
        public DateTime SettleDate { get; set; }

        [Key]
        [Column(Order = 16)]
        public bool SoldOut { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? SoldOutTime { get; set; }

        [Key]
        [Column(Order = 17, TypeName = "datetime2")]
        public DateTime StartSellTime { get; set; }

        [Key]
        [Column(Order = 18)]
        public decimal UnitPrice { get; set; }

        [StringLength(1000)]
        public string Usage { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValueDate { get; set; }

        [Key]
        [Column(Order = 19)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ValueDateMode { get; set; }

        [Key]
        [Column(Order = 20)]
        public decimal Yield { get; set; }
    }
}
