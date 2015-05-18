// FileInformation: nyanya/Cat.Domain.Products/BAProductMaterial.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:26 PM

using System;
using Cat.Commands.Products;

namespace Cat.Domain.Products.EventHandlers.DTO
{
    internal class BAProductMaterial
    {
        public string BankName { get; set; }

        public string BillNo { get; set; }

        public string BusinessLicense { get; set; }

        public string ConsignmentAgreement { get; set; }

        public string EndorseImageLink { get; set; }

        public string EndorseImageThumbnailLink { get; set; }

        public DateTime EndSellTime { get; set; }

        public string EnterpriseName { get; set; }

        public int FinancingSumCount { get; set; }

        public int MaxShareCount { get; set; }

        public int MinShareCount { get; set; }

        public int Period { get; set; }

        public string PledgeAgreement { get; set; }

        public DateTime? PreEndSellTime { get; set; }

        public DateTime? PreStartSellTime { get; set; }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        public int ProductNumber { get; set; }

        public DateTime RepaymentDeadline { get; set; }

        public DateTime SettleDate { get; set; }

        public DateTime StartSellTime { get; set; }

        public int UnitPrice { get; set; }

        public string Usage { get; set; }

        public DateTime? ValueDate { get; set; }

        public ValueDateMode ValueDateMode { get; set; }

        public decimal Yield { get; set; }
    }
}