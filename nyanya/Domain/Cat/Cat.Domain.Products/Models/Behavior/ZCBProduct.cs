using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Cat.Commands.Products;
using Cat.Domain.Products.Database;
using Cat.Events.Products;
using Infrastructure.Cache.Couchbase;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using System.Collections.Generic;

namespace Cat.Domain.Products.Models
{
    public partial class ZCBProduct
    {
        /// <summary>
        ///     Only for entity framework
        /// </summary>
        public ZCBProduct()
        {
        }
        public ZCBProduct(string projectIdentifier)
            : base(projectIdentifier)
        {
        }

        internal ZCBHistory BuildZCBHistory(ZCBUpdateShareCount productMaterial)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            return new ZCBHistory
            {
                ProductIdentifier = this.ProductIdentifier,
                ProductNo = productMaterial.ProductNo,
                ProductName = productMaterial.ProductName,
                FinancingSumCount = productMaterial.FinancingSumCount,
                SubProductNo = productMaterial.SubProductNo,
                UnitPrice = productMaterial.UnitPrice,
                NextStartSellTime = productMaterial.NextStartSellTime,
                NextEndSellTime = productMaterial.NextEndSellTime,
                NextYield = productMaterial.NextYield,
                EnableSale = productMaterial.EnableSale,
                PledgeAgreementName = productMaterial.PledgeAgreementName,
                PledgeAgreement = productMaterial.PledgeAgreement,
                ConsignmentAgreementName = productMaterial.ConsignmentAgreementName,
                ConsignmentAgreement = productMaterial.ConsignmentAgreement,
                PerRemainRedeemAmount = productMaterial.PerRemainRedeemAmount,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
        }

        internal IList<ZCBHistory> BuildZCBHistory(LaunchZCBProduct productMaterial)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            return new List<ZCBHistory> {
                new ZCBHistory
                {
                    ProductIdentifier = this.ProductIdentifier,
                    ProductNo = productMaterial.ProductNo,
                    ProductName = productMaterial.ProductName,
                    FinancingSumCount = productMaterial.FinancingSumCount,
                    SubProductNo = productMaterial.SubProductNo,
                    UnitPrice = productMaterial.UnitPrice,
                    NextStartSellTime = productMaterial.StartSellTime,
                    NextEndSellTime = productMaterial.EndSellTime,
                    NextYield = productMaterial.Yield,
                    EnableSale = productMaterial.EnableSale,
                    PledgeAgreementName = productMaterial.PledgeAgreementName,
                    PledgeAgreement = productMaterial.PledgeAgreement,
                    ConsignmentAgreementName = productMaterial.ConsignmentAgreementName,
                    ConsignmentAgreement = productMaterial.ConsignmentAgreement,
                    PerRemainRedeemAmount = productMaterial.PerRemainRedeemAmount,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                }
            };
        }

        internal ZCBProduct BuildZCBProduct(LaunchZCBProduct productMaterial)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            this.EnableSale = productMaterial.EnableSale;
            this.Links = this.BuildImageLinks(productMaterial.EndorseImageLink, productMaterial.EndorseImageThumbnailLink);
            this.SalePeriod = this.BuildSalePeriod(productMaterial.StartSellTime, productMaterial.EndSellTime, productMaterial.PreStartSellTime, productMaterial.PreEndSellTime);
            this.SaleInfo = this.BuildSaleInfo(productMaterial.FinancingSumCount, productMaterial.MinShareCount, productMaterial.MaxShareCount, productMaterial.UnitPrice);
            this.Period = productMaterial.Period;
            this.ProductNo = productMaterial.ProductNo;
            this.SubProductNo = productMaterial.SubProductNo;
            this.ProductName = productMaterial.ProductName;
            this.ProductNumber = productMaterial.ProductNumber;
            this.ValueInfo = this.BuildValueInfo(productMaterial.RepaymentDeadline, productMaterial.ValueDate, productMaterial.ValueDateMode, productMaterial.SettleDate);
            this.Yield = productMaterial.Yield;
            this.LaunchTime = null;
            this.ConsignmentAgreement = this.BuildAgreement(productMaterial.ConsignmentAgreement);
            this.PledgeAgreement = this.BuildAgreement(productMaterial.PledgeAgreement);
            this.ProductCategory = productMaterial.ProductCategory;
            this.ConsignmentAgreementName = productMaterial.ConsignmentAgreementName;
            this.PledgeAgreementName = productMaterial.PledgeAgreementName;
            this.PerRemainRedeemAmount = productMaterial.PerRemainRedeemAmount;
            this.ZCBHistorys = this.BuildZCBHistory(productMaterial);
            return this;
        }
        internal async Task LaunchAsync()
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            this.ProductType = ProductType.ZCBAcceptance;
            this.LaunchTime = DateTime.Now;
            this.SoldOut = false;
            this.SoldOutTime = null;
            this.Repaid = false;
            this.TotalInterest = 0;
            this.TotalSaleAmount = 0;
            this.TotalRedeemAmount = 0;
            this.TotalRedeemInterest = 0;

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

            this.EventBus.Publish(new ZCBProductLaunched(this.ProductIdentifier, this.GetType()));
        }

        internal async Task UpdateZCBProductAsync(ZCBUpdateShareCount productMaterial)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            try
            {
                using (ProductContext context = new ProductContext())
                {
                    ZCBProduct product = await context.Query<ZCBProduct>().Include(o => o.SaleInfo).Include(o => o.SalePeriod).Include(o => o.PledgeAgreement).Include(o => o.ConsignmentAgreement).FirstOrDefaultAsync(p => p.ProductIdentifier == this.ProductIdentifier);
                    if (!CanUpdateShareCount(product))
                    {
                        throw new ApplicationBusinessException("Can not updatesharecount product {0}-{1}".FormatWith(product.ProductNo, product.ProductIdentifier));
                    }
                    //更新CouchBase
                    if (!ProductShareCache.UpdateShareCache(this.ProductIdentifier, productMaterial.FinancingSumCount))
                    {
                        throw new ApplicationBusinessException("Can not update product share count.{0}-{1}".FormatWith(this.ProductNo, this.ProductIdentifier));
                    }

                    product.SaleInfo = this.BuildSaleInfo(productMaterial.FinancingSumCount, 1, productMaterial.FinancingSumCount, (int)productMaterial.UnitPrice);
                    product.SalePeriod.StartSellTime = productMaterial.NextStartSellTime;
                    product.SalePeriod.EndSellTime = productMaterial.NextEndSellTime;
                    product.SubProductNo = productMaterial.SubProductNo;
                    product.EnableSale = productMaterial.EnableSale;
                    product.ConsignmentAgreementName = productMaterial.ConsignmentAgreementName;
                    product.ConsignmentAgreement.Id = product.ConsignmentAgreementId;
                    product.ConsignmentAgreement.Content = productMaterial.ConsignmentAgreement;
                    product.PledgeAgreementName = productMaterial.PledgeAgreementName;
                    product.PledgeAgreement.Id = product.PledgeAgreementId;
                    product.PledgeAgreement.Content = productMaterial.PledgeAgreement;
                    product.PerRemainRedeemAmount = productMaterial.PerRemainRedeemAmount;
                    product.ProductName = productMaterial.ProductName;
                    product.Yield = productMaterial.NextYield;
                    context.Add(this.BuildZCBHistory(productMaterial));
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            this.EventBus.Publish(new ZCBUpdateShareCounted(this.ProductIdentifier, this.GetType()));
        }

        
    }

    
}
