// FileInformation: nyanya/Xingye.Domain.Products/IProductInfoService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:26 PM

using System.Collections.Generic;
using System.Threading.Tasks;
using Xingye.Domain.Products.ReadModels;
using Xingye.Domain.Products.Services.DTO;
using Domian.DTO;
using Infrastructure.Cache.Couchbase;

namespace Xingye.Domain.Products.Services.Interfaces
{
    public interface IExactProductInfoService : IProductInfoService
    {
    }

    public interface IProductInfoService
    {
        Task<AgreementsPackage> GetAgreementsInfoAsync(string productIdentifier);

        Task<IList<ProductWithSaleInfo<ProductInfo>>> GetBeingSoldOutProductWithSaleInfosAsync();

        Task<string> GetConsignmentAgreementAsync(string productIdentifier);

        Task<IList<ProductInfo>> GetOnSaleProductInfosAsync();

        Task<IList<ProductWithSaleInfo<ProductInfo>>> GetOnSaleProductWithSaleInfosAsync();

        Task<string> GetPledgeAgreementAsync(string productIdentifier);

        Task<ProductInfo> GetProductInfoByIdentifierAsync(string productIdentifier);

        Task<ProductInfo> GetProductInfoByNoAsync(string productNo);

        Task<IPaginatedDto<ProductInfo>> GetProductInfosAsync(int pageIndex, int pageSize = 10);

        ProductShareCacheModel GetProductSaleProcess(string productIdentifier);

        IDictionary<string, ProductShareCacheModel> GetProductSaleProcess(IEnumerable<string> productIdentifiers);

        Task<ProductWithSaleInfo<ProductInfo>> GetProductWithSaleInfoByIdentifierAsync(string productIdentifier);

        Task<ProductWithSaleInfo<ProductInfo>> GetProductWithSaleInfoByNoAsync(string productNo);

        Task<IPaginatedDto<ProductWithSaleInfo<ProductInfo>>> GetProductWithSaleInfosAsync(int pageIndex, int pageSize = 10);

        Task<int> GetRepaidCountAsync();

        string GetSnapshot(string productIdentifier);

        Task<ProductInfo> GetTopProductInfoAsync();

        Task<ProductWithSaleInfo<ProductInfo>> GetTopProductWithSaleInfoAsync();
    }
}