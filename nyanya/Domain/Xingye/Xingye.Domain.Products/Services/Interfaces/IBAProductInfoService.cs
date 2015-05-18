// FileInformation: nyanya/Xingye.Domain.Products/IBAProductInfoService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:26 PM

using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Lib.Data;
using Xingye.Domain.Products.ReadModels;
using Xingye.Domain.Products.Services.DTO;
using Domian.DTO;
using Domian.Models;

namespace Xingye.Domain.Products.Services.Interfaces
{
    public interface IBAProductInfoService : IDomainService
    {
        Task<IList<ProductWithSaleInfo<BAProductInfo>>> GetBeingSoldOutProductWithSaleInfosAsync();

        Task<BAProductInfo> GetProductInfoByIdentifierAsync(string productIdentifier);

        Task<BAProductInfo> GetProductInfoByNoAsync(string productNo);

        Task<IPaginatedDto<BAProductInfo>> GetProductInfosAsync(int pageIndex, int pageSize = 10);

        Task<ProductWithSaleInfo<BAProductInfo>> GetProductWithSaleInfoByIdentifierAsync(string productIdentifier);

        Task<ProductWithSaleInfo<BAProductInfo>> GetProductWithSaleInfoByNoAsync(string productNo);

        Task<IPaginatedDto<ProductWithSaleInfo<BAProductInfo>>> GetProductWithSaleInfosAsync(int pageIndex, int pageSize = 10);

        Task<IPaginatedDto<ProductWithSaleInfo<BAProductInfo>>> GetProductWithSaleInfosAsync(int pageIndex, SortCondition[] sortConditions, int pageSize = 10);

        Task<BAProductInfo> GetTopProductInfoAsync();

        Task<IList<BAProductInfo>> GetTopProductInfosAsync(int topPageCount = 6);

        Task<ProductWithSaleInfo<BAProductInfo>> GetTopProductWithSaleInfoAsync();

        Task<IList<ProductWithSaleInfo<BAProductInfo>>> GetTopProductWithSaleInfosAsync(int topPageCount = 6);
    }

    public interface IExactBAProductInfoService : IBAProductInfoService
    {
    }
}