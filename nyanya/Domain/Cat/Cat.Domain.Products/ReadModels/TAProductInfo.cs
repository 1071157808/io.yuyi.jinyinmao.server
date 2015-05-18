// FileInformation: nyanya/Cat.Domain.Products/TAProductInfo.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/17   12:28 PM

using Cat.Commands.Products;

namespace Cat.Domain.Products.ReadModels
{
    public class TAProductInfo : ProductInfo
    {
        public string BillNo { get; set; }

        public string ConsignmentAgreementName { get; set; }

        public string Drawee { get; set; }

        public string DraweeInfo { get; set; }

        public string EnterpriseInfo { get; set; }

        public string EnterpriseLicense { get; set; }

        public string EnterpriseName { get; set; }

        public GuaranteeMode GuaranteeMode { get; set; }

        public string PledgeAgreementName { get; set; }

        public string Securedparty { get; set; }

        public string SecuredpartyInfo { get; set; }

        public string Usage { get; set; }
    }
}