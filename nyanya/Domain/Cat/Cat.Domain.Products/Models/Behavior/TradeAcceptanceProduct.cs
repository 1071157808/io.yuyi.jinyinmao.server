// FileInformation: nyanya/Cat.Domain.Products/TradeAcceptanceProduct.cs
// CreatedTime: 2014/09/17   11:18 AM
// LastUpdatedTime: 2014/09/17   12:28 PM

using System;
using System.Threading.Tasks;
using Cat.Commands.Products;
using Cat.Domain.Products.Database;
using Cat.Domain.Products.ReadModels;
using Cat.Events.Products;
using Infrastructure.Cache.Couchbase;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;

namespace Cat.Domain.Products.Models
{
    public partial class TradeAcceptanceProduct
    {
        public TradeAcceptanceProduct()
        {
        }

        public TradeAcceptanceProduct(string projectIdentifier)
            : base(projectIdentifier)
        {
        }

        internal TradeAcceptanceProduct BuildTradeAcceptanceProduct(LaunchTAProduct productMaterial)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            this.SecuredpartyInfo = productMaterial.SecuredpartyInfo;
            this.Securedparty = productMaterial.Securedparty;
            this.BillNo = productMaterial.BillNo;
            this.EnterpriseLicense = productMaterial.EnterpriseLicense;
            this.Links = this.BuildImageLinks(productMaterial.EndorseImageLink, productMaterial.EndorseImageThumbnailLink);
            this.SalePeriod = this.BuildSalePeriod(productMaterial.StartSellTime, productMaterial.EndSellTime, productMaterial.PreStartSellTime, productMaterial.PreEndSellTime);
            this.EnterpriseName = productMaterial.EnterpriseName;
            this.SaleInfo = this.BuildSaleInfo(productMaterial.FinancingSumCount, productMaterial.MinShareCount, productMaterial.MaxShareCount, productMaterial.UnitPrice);
            this.Period = productMaterial.Period;
            this.ProductNo = productMaterial.ProductNo;
            this.ProductName = productMaterial.ProductName;
            this.ProductNumber = productMaterial.ProductNumber;
            this.ValueInfo = this.BuildValueInfo(productMaterial.RepaymentDeadline, productMaterial.ValueDate, productMaterial.ValueDateMode, productMaterial.SettleDate);
            this.Usage = productMaterial.Usage;
            this.Yield = productMaterial.Yield;
            this.LaunchTime = null;
            this.ConsignmentAgreement = this.BuildAgreement(productMaterial.ConsignmentAgreement);
            this.PledgeAgreement = this.BuildAgreement(productMaterial.PledgeAgreement);
            this.ConsignmentAgreementName = productMaterial.ConsignmentAgreementName;
            this.PledgeAgreementName = productMaterial.PledgeAgreementName;
            this.Drawee = productMaterial.Drawee;
            this.DraweeInfo = productMaterial.DraweeInfo;
            this.EnterpriseInfo = productMaterial.EnterpriseInfo;
            this.ProductType = ProductType.TradeAcceptance;
            this.GuaranteeMode = productMaterial.GuaranteeMode;
            this.ProductCategory = productMaterial.ProductCategory;
            return this;
        }

        internal override AgreementsPackage GetAgreements()
        {
            return new AgreementsPackage
            {
                ConsignmentAgreementContent = this.ConsignmentAgreement.Content,
                ConsignmentAgreementName = this.ConsignmentAgreementName,
                PledgeAgreementContent = this.PledgeAgreement.Content,
                PledgeAgreementName = this.PledgeAgreementName
            };
        }

        internal async Task LaunchAsync()
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            this.ProductType = ProductType.TradeAcceptance;
            this.LaunchTime = DateTime.Now;
            this.SoldOut = false;
            this.SoldOutTime = null;
            this.Repaid = false;

            using (ProductContext context = new ProductContext())
            {
                if (!ProductShareCache.InitShareCache(this.ProductIdentifier, this.SaleInfo.FinancingSumCount))
                {
                    throw new ApplicationBusinessException("Can not init product share count.{0}-{1}".FormatWith(this.ProductNo, this.ProductIdentifier));
                }
                await context.SaveAsync(this);
            }
            this.EventBus.Publish(new TAProductLaunched(this.ProductIdentifier, this.GetType()));
        }
    }
}