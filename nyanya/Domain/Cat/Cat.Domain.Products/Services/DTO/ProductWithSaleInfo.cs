// FileInformation: nyanya/Cqrs.Domain.Product/ProductWithSaleInfo.cs
// CreatedTime: 2014/07/28   1:53 AM
// LastUpdatedTime: 2014/07/28   1:57 AM

using Cat.Domain.Products.ReadModels;
using Infrastructure.Cache.Couchbase;

namespace Cat.Domain.Products.Services.DTO
{
    public class ProductWithSaleInfo<T> where T : ProductInfo
    {
        public ProductWithSaleInfo(T productInfo)
        {
            this.ProductInfo = productInfo;
            this.AvailableShareCount = 0;
            this.PaidShareCount = this.ProductInfo.FinancingSumCount;
            this.PayingShareCount = 0;
            this.SumShareCount = this.ProductInfo.FinancingSumCount;
        }

        public ProductWithSaleInfo(T productInfo, ProductShareCacheModel shareModel)
        {
            this.ProductInfo = productInfo;
            this.AvailableShareCount = shareModel.Available;
            this.PaidShareCount = shareModel.Paid;
            this.PayingShareCount = shareModel.Paying;
            this.SumShareCount = shareModel.Sum;
        }

        public int AvailableShareCount { get; set; }

        public int PaidShareCount { get; set; }

        public int PayingShareCount { get; set; }

        public T ProductInfo { get; private set; }

        public int SumShareCount { get; set; }
    }
}