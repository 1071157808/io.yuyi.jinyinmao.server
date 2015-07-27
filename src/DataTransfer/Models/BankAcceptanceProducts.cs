namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BankAcceptanceProducts
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        [Required]
        [StringLength(100)]
        public string BankName { get; set; }

        [Required]
        [StringLength(80)]
        public string BillNo { get; set; }

        [Required]
        [StringLength(80)]
        public string BusinessLicense { get; set; }

        [Required]
        [StringLength(80)]
        public string EnterpriseName { get; set; }

        [Required]
        [StringLength(200)]
        public string Usage { get; set; }
    }
}
