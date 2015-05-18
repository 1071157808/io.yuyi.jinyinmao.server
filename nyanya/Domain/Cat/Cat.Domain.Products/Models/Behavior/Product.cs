// FileInformation: nyanya/Cat.Domain.Products/Product.cs
// CreatedTime: 2014/09/17   11:18 AM
// LastUpdatedTime: 2014/09/17   12:28 PM

using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Cat.Commands.Products;
using Cat.Domain.Products.Database;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Cat.Events.Products;
using Domian.Bus;
using Domian.Config;
using Infrastructure.Cache.Couchbase;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;

namespace Cat.Domain.Products.Models
{
    public partial class Product
    {
        /// <summary>
        ///     Only for entity framework
        /// </summary>
        // ReSharper disable MemberCanBeProtected.Global
        public Product()
        {
        }

        public Product(string productIdentifier)
        {
            this.ProductIdentifier = productIdentifier;
        }

        protected IEventBus EventBus
        {
            get { return CqrsDomainConfig.Configuration.EventBus; }
        }

        public async Task<Product> SetSoldOutAsync()
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            Product product;
            using (ProductContext context = new ProductContext())
            {
                product = await context.Query<Product>().FirstOrDefaultAsync(p => p.ProductIdentifier == this.ProductIdentifier);
                ProductShareCacheModel productShareCacheModel = this.GetShareCache();

                if (ShouldBeSetSoldOut(product.SoldOut, productShareCacheModel))
                {
                    product.SoldOut = true;
                    product.SoldOutTime = DateTime.Now;
                    await context.SaveChangesAsync();
                }
            }

            this.EventBus.Publish(new ProductSoldOut(this.ProductIdentifier, this.GetType()));
            return product;
        }

        public async Task SetTotalSaleAmount(decimal totalSaleAmount)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            using (ProductContext context = new ProductContext())
            {
                var ZcbProduct = await context.Query<ZCBProduct>().FirstOrDefaultAsync(p => p.ProductIdentifier == this.ProductIdentifier);
                if (ZcbProduct != null)
                {
                    ZcbProduct.TotalSaleAmount = totalSaleAmount;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task SetTotalInterest(decimal totalInterest)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            using (ProductContext context = new ProductContext())
            {
                var ZcbProduct = await context.Query<ZCBProduct>().FirstOrDefaultAsync(p => p.ProductIdentifier == this.ProductIdentifier);
                if (ZcbProduct != null)
                {
                    ZcbProduct.TotalInterest = totalInterest;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task SetTotalRedeemAmount(decimal totalRedeemAmount)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            using (ProductContext context = new ProductContext())
            {
                var ZcbProduct = await context.Query<ZCBProduct>().FirstOrDefaultAsync(p => p.ProductIdentifier == this.ProductIdentifier);
                if (ZcbProduct != null)
                {
                    ZcbProduct.TotalRedeemAmount = totalRedeemAmount;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task SetTotalRedeemInterest(decimal totalRedeemInterest)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            using (ProductContext context = new ProductContext())
            {
                var ZcbProduct = await context.Query<ZCBProduct>().FirstOrDefaultAsync(p => p.ProductIdentifier == this.ProductIdentifier);
                if (ZcbProduct != null)
                {
                    ZcbProduct.TotalRedeemInterest = totalRedeemInterest;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task SetPerRemainRedeemAmount(decimal totalRedeemAmount, decimal todayRedeemAmount)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            using (ProductContext context = new ProductContext())
            {
                var ZcbProduct = await context.Query<ZCBProduct>().FirstOrDefaultAsync(p => p.ProductIdentifier == this.ProductIdentifier);
                if (ZcbProduct != null)
                {
                    ZcbProduct.PerRemainRedeemAmount = GetPerRemainRedeemAmount(ZcbProduct, totalRedeemAmount,todayRedeemAmount);
                    await context.SaveChangesAsync();
                }
            }
        }

        private decimal GetPerRemainRedeemAmount(ZCBProduct ZcbProduct, decimal totalRedeemAmount, decimal todayRedeemAmount)
        {
            // 当日剩余取款金额 = （已募金额-总提现金额）* 10% - 今天赎回金额
            return (ZcbProduct.TotalSaleAmount - totalRedeemAmount)*0.1M - todayRedeemAmount;
        }

        internal static bool CanUnShelves(Product product)
        {
            return product != null && product.CanUnShelves();
        }

        internal static bool ShouldBeSetSoldOut<T>(ProductWithSaleInfo<T> product) where T : ProductInfo
        {
            return ShouldBeSetSoldOut(product.ProductInfo.SoldOut, product.AvailableShareCount, product.PayingShareCount, product.PaidShareCount, product.SumShareCount);
        }

        internal static bool ShouldBeSetSoldOut(bool soldOut, ProductShareCacheModel model)
        {
            return ShouldBeSetSoldOut(soldOut, model.Available, model.Paying, model.Paid, model.Sum);
        }

        internal static bool ShouldBeSetSoldOut(ProductShareCacheModel model)
        {
            return ShouldBeSetSoldOut(false, model);
        }

        internal bool FreezeShareCount(int count)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            return ProductShareCache.Paying(this.ProductIdentifier, count);
        }

        internal virtual AgreementsPackage GetAgreements()
        {
            return new AgreementsPackage
            {
                ConsignmentAgreementContent = this.ConsignmentAgreement.Content,
                ConsignmentAgreementName = this.ProductCategory == ProductCategory.FUDIAN ? "票据质押委托协议" : "委托协议",
                PledgeAgreementContent = this.PledgeAgreement.Content,
                PledgeAgreementName = "质押借款协议"
            };
        }
        internal virtual AgreementsPackage GetZCBPledgeAgreements(string pledgeAgreementName)
        {
            return new AgreementsPackage
            {
                PledgeAgreementContent = this.PledgeAgreement.Content,
                PledgeAgreementName = pledgeAgreementName
            };
        }

        internal ProductShareCacheModel GetShareCache()
        {
            return ProductShareCache.GetShareCache(this.ProductIdentifier);
        }

        internal async Task Paid(int count)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            int remain;
            if (ProductShareCache.Paid(this.ProductIdentifier, count, out remain))
            {
                if (remain == 0)
                {
                    await this.SetSoldOutAsync();
                }
            }
            else
            {
                throw new ApplicationBusinessException("PSC Paid Error.{0}|{1}".FormatWith(this.ProductIdentifier, count));
            }
        }

        internal async Task RepayAsync()
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            using (ProductContext context = new ProductContext())
            {
                Product product = await context.Query<Product>().FirstOrDefaultAsync(p => p.ProductIdentifier == this.ProductIdentifier);
                if (product == null || product.Repaid)
                {
                    return;
                }

                product.Repaid = true;
                await context.SaveChangesAsync();
            }

            this.EventBus.Publish(new ProductRepaid(this.ProductIdentifier, typeof(Product)));
        }

        internal bool UnfreezeShareCount(int count)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            if (ProductShareCache.Paying(this.ProductIdentifier, -count))
            {
                return true;
            }

            throw new ApplicationBusinessException("Product UnfreezeShareCount Error.{0}|{1}".FormatWith(this.ProductIdentifier, count));
        }

        internal async Task UnShelvesAsync()
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            using (ProductContext context = new ProductContext())
            {
                Product product = await context.Query<Product>().Include(p => p.SalePeriod).FirstOrDefaultAsync(p => p.ProductIdentifier == this.ProductIdentifier);
                if (!CanUnShelves(product))
                {
                    throw new ApplicationBusinessException("Can not unshelves product {0}-{1}".FormatWith(product.ProductNo, product.ProductIdentifier));
                }

                // 取消外键约束
                product.SalePeriod = null;
                await context.RemoveAsync(product);
            }

            this.EventBus.Publish(new BAProductUnShelved(this.ProductIdentifier, typeof(Product)));
        }

        /// <summary>
        /// 产品可售份额是否可以更新
        /// </summary>
        /// <param name="product">产品实体</param>
        /// <returns></returns>
        internal static bool CanUpdateShareCount(ZCBProduct product)
        {
            ProductShareCacheModel productShareCacheModel = ProductShareCache.GetShareCache(product.ProductIdentifier);
            return product != null && productShareCacheModel.Paying == 0 && ((product.SalePeriod.StartSellTime <= DateTime.Now && product.SalePeriod.EndSellTime >= DateTime.Now && productShareCacheModel.Available == 0) ||
                (product.SalePeriod.EndSellTime < DateTime.Now) || (product.SalePeriod.StartSellTime > DateTime.Now) || (product.EnableSale==0));
        }

        protected Agreement BuildAgreement(string content)
        {
            return new Agreement { Content = content };
        }

        protected EndorseLinks BuildImageLinks(string endorseImageLink, string endorseImageThumbnailLink)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            return new EndorseLinks
            {
                ProductIdentifier = this.ProductIdentifier,
                EndorseImageLink = endorseImageLink,
                EndorseImageThumbnailLink = endorseImageThumbnailLink
            };
        }

        protected SaleInfo BuildSaleInfo(int financingSumCount, int minShareCount, int maxShareCount, int unitPrice)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            return new SaleInfo
            {
                ProductIdentifier = this.ProductIdentifier,
                FinancingSumCount = financingSumCount,
                MinShareCount = minShareCount,
                MaxShareCount = maxShareCount,
                UnitPrice = unitPrice
            };
        }

        protected SalePeriod BuildSalePeriod(DateTime startSellTime, DateTime endSellTime, DateTime? preStartSellTime, DateTime? preEndSellTime)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            return new SalePeriod
            {
                ProductIdentifier = this.ProductIdentifier,
                EndSellTime = endSellTime,
                PreEndSellTime = preEndSellTime,
                PreStartSellTime = preStartSellTime,
                StartSellTime = startSellTime,
            };
        }

        protected ValueInfo BuildValueInfo(DateTime repaymentDeadline, DateTime? valueDate, ValueDateMode valueDateMode, DateTime settleDate)
        {
            Guard.IdentifierMustBeAssigned(this.ProductIdentifier, this.GetType().ToString());
            return new ValueInfo
            {
                ProductIdentifier = this.ProductIdentifier,
                RepaymentDeadline = repaymentDeadline,
                ValueDate = valueDate,
                ValueDateMode = valueDateMode,
                SettleDate = settleDate
            };
        }

        private static bool ShouldBeSetSoldOut(bool soldOut, int available, int paying, int paid, int sum)
        {
            return !soldOut && available == 0 && paying == 0 && paid == sum && sum > 0;
        }

        private bool CanUnShelves()
        {
            Guard.ArgumentNotNull(this.SalePeriod, "SalePeriod");
            DateTime time = DateTime.Now.AddMinutes(10);
            return this.SalePeriod.PreStartSellTime.GetValueOrDefault(DateTime.MaxValue) > time && this.SalePeriod.StartSellTime > time;
        }

        
    }
}