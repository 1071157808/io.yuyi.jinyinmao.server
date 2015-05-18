// FileInformation: nyanya/Xingye.Commands.Products/LaunchBAProduct.cs
// CreatedTime: 2014/09/01   5:52 PM
// LastUpdatedTime: 2014/09/02   10:04 AM

using Domian.Commands;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;

namespace Xingye.Commands.Products
{
    public enum AgreementType
    {
        /// <summary>
        ///     担保公司
        /// </summary>
        Suretycompany = 10,

        /// <summary>
        ///     群体担保
        /// </summary>
        Groupguarantee = 20
    }

    public enum ProductType
    {
        /// <summary>
        ///     银承
        /// </summary>
        BankAcceptance = 10,

        /// <summary>
        ///     商承
        /// </summary>
        TradeAcceptance = 20,
    }

    public enum ValueDateMode
    {
        T0 = 10,
        T1 = 20,
        FixedDate = 30
    }

    [Route("/BA/ProductMaterial")]
    public class LaunchBAProduct : Command
    {
        public LaunchBAProduct()
            : base("JB")
        {
        }

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

    public class LaunchBAProductValidator : AbstractValidator<LaunchBAProduct>
    {
        public LaunchBAProductValidator()
        {
            this.RuleFor(c => c.BankName).NotNull();
            this.RuleFor(c => c.BankName).NotEmpty();

            this.RuleFor(c => c.BillNo).NotNull();
            this.RuleFor(c => c.BillNo).NotEmpty();

            this.RuleFor(c => c.BusinessLicense).NotNull();
            this.RuleFor(c => c.BusinessLicense).NotEmpty();

            this.RuleFor(c => c.ConsignmentAgreement).NotNull();
            this.RuleFor(c => c.ConsignmentAgreement).NotEmpty();

            this.RuleFor(c => c.EndorseImageLink).NotNull();
            this.RuleFor(c => c.EndorseImageLink).NotEmpty();

            this.RuleFor(c => c.EndorseImageThumbnailLink).NotNull();
            this.RuleFor(c => c.EndorseImageThumbnailLink).NotEmpty();

            this.RuleFor(c => c.EnterpriseName).NotNull();
            this.RuleFor(c => c.EnterpriseName).NotEmpty();

            this.RuleFor(c => c.PledgeAgreement).NotNull();
            this.RuleFor(c => c.PledgeAgreement).NotEmpty();

            this.RuleFor(c => c.ProductName).NotNull();
            this.RuleFor(c => c.ProductName).NotEmpty();

            this.RuleFor(c => c.ProductNo).NotNull();
            this.RuleFor(c => c.ProductNo).NotEmpty();

            this.RuleFor(c => c.Usage).NotNull();
            this.RuleFor(c => c.Usage).NotEmpty();
        }
    }
}