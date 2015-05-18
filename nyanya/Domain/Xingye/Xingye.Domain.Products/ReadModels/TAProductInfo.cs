namespace Xingye.Domain.Products.ReadModels
{
    public class TAProductInfo : ProductInfo
    {
        #region Public Properties

        public string BillNo { get; set; }

        public string ConsignmentAgreementName { get; set; }

        public string Drawee { get; set; }

        public string DraweeInfo { get; set; }

        public string EnterpriseInfo { get; set; }

        public string EnterpriseLicense { get; set; }

        public string EnterpriseName { get; set; }

        public string PledgeAgreementName { get; set; }

        public string Securedparty { get; set; }

        public string SecuredpartyInfo { get; set; }

        public string Usage { get; set; }

        #endregion Public Properties
    }
}