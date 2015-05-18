// FileInformation: nyanya/Xingye.Domain.Products/TAProductMaterial.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:30 PM

using System;
using Xingye.Commands.Products;

namespace Xingye.Domain.Products.EventHandlers.DTO
{
    public class TAProductMaterial
    {
        public string BillNo { get; set; }

        public string ConsignmentAgreement { get; set; }

        public string ConsignmentAgreementName { get; set; }

        public string Drawee { get; set; }

        public string DraweeInfo { get; set; }

        public string EndorseImageLink { get; set; }

        public string EndorseImageThumbnailLink { get; set; }

        public DateTime EndSellTime { get; set; }

        public string EnterpriseInfo { get; set; }

        public string EnterpriseLicense { get; set; }

        public string EnterpriseName { get; set; }

        public int FinancingSumCount { get; set; }

        public int MaxShareCount { get; set; }

        public int MinShareCount { get; set; }

        public int Period { get; set; }

        public string PledgeAgreement { get; set; }

        public string PledgeAgreementName { get; set; }

        public DateTime? PreEndSellTime { get; set; }

        public DateTime? PreStartSellTime { get; set; }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        public int ProductNumber { get; set; }

        public DateTime RepaymentDeadline { get; set; }

        public string Securedparty { get; set; }

        public DateTime SettleDate { get; set; }

        public DateTime StartSellTime { get; set; }

        public int UnitPrice { get; set; }

        public string Usage { get; set; }

        public DateTime? ValueDate { get; set; }

        public ValueDateMode ValueDateMode { get; set; }

        public decimal Yield { get; set; }
    }
}