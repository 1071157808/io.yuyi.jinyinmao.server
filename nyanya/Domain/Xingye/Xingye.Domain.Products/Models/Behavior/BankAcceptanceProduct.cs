// FileInformation: nyanya/Xingye.Domain.Products/BankAcceptanceProduct.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:30 PM

using System;
using System.Threading.Tasks;
using Infrastructure.Cache.Couchbase;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using Xingye.Commands.Products;
using Xingye.Domain.Products.Database;
using Xingye.Events.Products;

namespace Xingye.Domain.Products.Models
{
    public partial class BankAcceptanceProduct
    {
        /// <summary>
        ///     Only for entity framework
        /// </summary>
        public BankAcceptanceProduct()
        {
        }

        public BankAcceptanceProduct(string projectIdentifier)
            : base(projectIdentifier)
        {
        }

        internal BankAcceptanceProduct BuildBankAcceptanceProduct(LaunchBAProduct productMaterial)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            this.BankName = productMaterial.BankName;
            this.BillNo = productMaterial.BillNo;
            this.BusinessLicense = productMaterial.BusinessLicense;
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
            return this;
        }

        internal async Task LaunchAsync()
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            this.ProductType = ProductType.BankAcceptance;
            this.LaunchTime = DateTime.Now;
            this.SoldOut = false;
            this.SoldOutTime = null;
            this.Repaid = false;

            try
            {
                using (ProductContext context = new ProductContext())
                {
                    if (!ProductShareCache.InitShareCache(this.ProductIdentifier, this.SaleInfo.FinancingSumCount))
                    {
                        throw new ApplicationBusinessException("Can not init product share count.{0}-{1}".FormatWith(this.ProductNo, this.ProductIdentifier));
                    }
                    await context.SaveAsync(this);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            this.EventBus.Publish(new BAProductLaunched(this.ProductIdentifier, this.GetType()));
        }
    }
}