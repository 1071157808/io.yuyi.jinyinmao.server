// FileInformation: nyanya/Xingye.Commands.Products/LaunchTAProduct.cs
// CreatedTime: 2014/09/01   5:52 PM
// LastUpdatedTime: 2014/09/02   10:04 AM

using Domian.Commands;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;

namespace Xingye.Commands.Products
{
    [Route("/ProductMaterial/TA")]
    public class LaunchTAProduct : Command
    {
        public LaunchTAProduct()
            : base("JB")
        {
        }

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

        public string SecuredpartyInfo { get; set; }

        public DateTime SettleDate { get; set; }

        public DateTime StartSellTime { get; set; }

        public int UnitPrice { get; set; }

        public string Usage { get; set; }

        public DateTime? ValueDate { get; set; }

        public ValueDateMode ValueDateMode { get; set; }

        public decimal Yield { get; set; }
    }

    public class LaunchTAProductValidator : AbstractValidator<LaunchTAProduct>
    {
        public LaunchTAProductValidator()
        {
            this.RuleFor(c => c.BillNo).NotNull();
            this.RuleFor(c => c.BillNo).NotEmpty();

            this.RuleFor(c => c.ConsignmentAgreement).NotNull();
            this.RuleFor(c => c.ConsignmentAgreement).NotEmpty();

            this.RuleFor(c => c.ConsignmentAgreementName).NotNull();
            this.RuleFor(c => c.ConsignmentAgreementName).NotEmpty();

            this.RuleFor(c => c.Drawee).NotNull();
            this.RuleFor(c => c.Drawee).NotEmpty();

            this.RuleFor(c => c.DraweeInfo).NotNull();
            this.RuleFor(c => c.DraweeInfo).NotEmpty();

            this.RuleFor(c => c.EndorseImageLink).NotNull();
            this.RuleFor(c => c.EndorseImageLink).NotEmpty();

            this.RuleFor(c => c.EndorseImageThumbnailLink).NotNull();
            this.RuleFor(c => c.EndorseImageThumbnailLink).NotEmpty();

            this.RuleFor(c => c.EnterpriseName).NotNull();
            this.RuleFor(c => c.EnterpriseName).NotEmpty();

            this.RuleFor(c => c.EnterpriseInfo).NotNull();
            this.RuleFor(c => c.EnterpriseInfo).NotEmpty();

            this.RuleFor(c => c.EnterpriseLicense).NotNull();
            this.RuleFor(c => c.EnterpriseLicense).NotEmpty();

            this.RuleFor(c => c.PledgeAgreementName).NotNull();
            this.RuleFor(c => c.PledgeAgreementName).NotEmpty();

            this.RuleFor(c => c.PledgeAgreement).NotNull();
            this.RuleFor(c => c.PledgeAgreement).NotEmpty();

            this.RuleFor(c => c.ProductName).NotNull();
            this.RuleFor(c => c.ProductName).NotEmpty();

            this.RuleFor(c => c.ProductNo).NotNull();
            this.RuleFor(c => c.ProductNo).NotEmpty();

            this.RuleFor(c => c.Securedparty).NotNull();
            this.RuleFor(c => c.Securedparty).NotEmpty();

            this.RuleFor(c => c.SecuredpartyInfo).NotNull();
            this.RuleFor(c => c.SecuredpartyInfo).NotEmpty();

            this.RuleFor(c => c.Usage).NotNull();
            this.RuleFor(c => c.Usage).NotEmpty();
        }
    }
}