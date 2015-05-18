// FileInformation: nyanya/Cat.Domain.Products/IProductInfoService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:26 PM

using Cat.Commands.Products;
using Cat.Domain.Products.Models;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Domian.DTO;
using Infrastructure.Cache.Couchbase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cat.Domain.Products.Services.Interfaces
{
    public interface IExactProductInfoService : IProductInfoService
    {
    }

    public interface IProductInfoService
    {
        Task<AgreementsPackage> GetAgreementsInfoAsync(string productIdentifier);

        Task<AgreementsPackage> GetZCBPledgeAgreementInfoAsync(string productIdentifier);

        Task<IList<ProductWithSaleInfo<ProductInfo>>> GetBeingSoldOutProductWithSaleInfosAsync();

        Task<string> GetConsignmentAgreementAsync(string productIdentifier);

        Task<IList<ProductInfo>> GetOnSaleProductInfosAsync();

        Task<IList<ProductWithSaleInfo<ProductInfo>>> GetOnSaleProductWithSaleInfosAsync();

        Task<string> GetPledgeAgreementAsync(string productIdentifier);

        Task<ProductInfo> GetProductInfoByIdentifierAsync(string productIdentifier);

        Task<ProductInfo> GetProductInfoByNoAsync(string productNo);

        Task<IPaginatedDto<ProductInfo>> GetProductInfosAsync(int pageIndex, int pageSize = 10, ProductCategory productCategory = ProductCategory.JINYINMAO);

        ProductShareCacheModel GetProductSaleProcess(string productIdentifier);

        IDictionary<string, ProductShareCacheModel> GetProductSaleProcess(IEnumerable<string> productIdentifiers);

        Task<ProductWithSaleInfo<ProductInfo>> GetProductWithSaleInfoByIdentifierAsync(string productIdentifier);

        Task<ProductWithSaleInfo<ProductInfo>> GetProductWithSaleInfoByNoAsync(string productNo);

        Task<IPaginatedDto<ProductWithSaleInfo<ProductInfo>>> GetProductWithSaleInfosAsync(int pageIndex, ProductCategory productCategory = ProductCategory.JINYINMAO, int pageSize = 10);

        Task<int> GetRepaidCountAsync();

        string GetSnapshot(string productIdentifier);

        Task<Product> GetProductByNo(string productNo);

        Task<ProductInfo> GetTopProductInfoAsync(ProductCategory productCategory = ProductCategory.JINYINMAO);

        Task<ProductWithSaleInfo<ProductInfo>> GetTopProductWithSaleInfoAsync(ProductCategory productCategory = ProductCategory.JINYINMAO);

        Task<decimal> GetProductYield(string productNo);

        Task<Product> GetFirstProduct(ProductType productType);
    }
}