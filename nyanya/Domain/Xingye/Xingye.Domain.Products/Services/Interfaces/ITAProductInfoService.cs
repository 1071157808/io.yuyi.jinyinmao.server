// FileInformation: nyanya/Cqrs.Domain.Product/ITAProductInfoService.cs
// CreatedTime: 2014/07/28   11:35 AM
// LastUpdatedTime: 2014/08/10   11:43 AM

using System.Collections.Generic;
using System.Threading.Tasks;
using Xingye.Domain.Products.ReadModels;
using Xingye.Domain.Products.Services.DTO;
using Domian.DTO;
using Domian.Models;

namespace Xingye.Domain.Products.Services.Interfaces
{
    public interface IExactTAProductInfoService : ITAProductInfoService
    {
    }

    public interface ITAProductInfoService : IDomainService
    {
        Task<IList<ProductWithSaleInfo<TAProductInfo>>> GetBeingSoldOutProductWithSaleInfosAsync();

        Task<TAProductInfo> GetProductInfoByIdentifierAsync(string productIdentifier);

        Task<TAProductInfo> GetProductInfoByNoAsync(string productNo);

        Task<IPaginatedDto<TAProductInfo>> GetProductInfosAsync(int pageIndex, int pageSize = 10);

        Task<ProductWithSaleInfo<TAProductInfo>> GetProductWithSaleInfoByIdentifierAsync(string productIdentifier);

        Task<ProductWithSaleInfo<TAProductInfo>> GetProductWithSaleInfoByNoAsync(string productNo);

        Task<IPaginatedDto<ProductWithSaleInfo<TAProductInfo>>> GetProductWithSaleInfosAsync(int pageIndex, int pageSize = 10);

        Task<TAProductInfo> GetTopProductInfoAsync();

        Task<ProductWithSaleInfo<TAProductInfo>> GetTopProductWithSaleInfoAsync();
    }
}