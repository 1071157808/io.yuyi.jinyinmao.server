namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TradeAcceptanceProducts
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        [Required]
        [StringLength(80)]
        public string BillNo { get; set; }

        [Required]
        [StringLength(80)]
        public string ConsignmentAgreementName { get; set; }

        [Required]
        [StringLength(300)]
        public string Drawee { get; set; }

        [Required]
        [StringLength(1000)]
        public string DraweeInfo { get; set; }

        [Required]
        [StringLength(1000)]
        public string EnterpriseInfo { get; set; }

        [Required]
        [StringLength(80)]
        public string EnterpriseLicense { get; set; }

        [Required]
        [StringLength(300)]
        public string EnterpriseName { get; set; }

        [Required]
        [StringLength(80)]
        public string PledgeAgreementName { get; set; }

        [Required]
        [StringLength(300)]
        public string Securedparty { get; set; }

        [Required]
        [StringLength(1000)]
        public string SecuredpartyInfo { get; set; }

        [Required]
        [StringLength(1000)]
        public string Usage { get; set; }

        public int GuaranteeMode { get; set; }
    }
}
