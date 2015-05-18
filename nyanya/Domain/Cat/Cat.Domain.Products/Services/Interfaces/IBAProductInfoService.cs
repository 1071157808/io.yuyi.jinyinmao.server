// FileInformation: nyanya/Cat.Domain.Products/IBAProductInfoService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:26 PM

using System.Collections.Generic;
using System.Threading.Tasks;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Domian.DTO;
using Domian.Models;
using Cat.Commands.Products;

namespace Cat.Domain.Products.Services.Interfaces
{
    public interface IBAProductInfoService : IDomainService
    {
        Task<IList<ProductWithSaleInfo<BAProductInfo>>> GetBeingSoldOutProductWithSaleInfosAsync();

        Task<BAProductInfo> GetProductInfoByIdentifierAsync(string productIdentifier);

        Task<BAProductInfo> GetProductInfoByNoAsync(string productNo);

        Task<IPaginatedDto<BAProductInfo>> GetProductInfosAsync(int pageIndex, int pageSize = 10, ProductCategory productCategory = ProductCategory.JINYINMAO);

        Task<ProductWithSaleInfo<BAProductInfo>> GetProductWithSaleInfoByIdentifierAsync(string productIdentifier);

        Task<ProductWithSaleInfo<BAProductInfo>> GetProductWithSaleInfoByNoAsync(string productNo);

        Task<IPaginatedDto<ProductWithSaleInfo<BAProductInfo>>> GetProductWithSaleInfosAsync(int pageIndex, ProductCategory productCategory = ProductCategory.JINYINMAO, int pageSize = 10);

        Task<BAProductInfo> GetTopProductInfoAsync(ProductCategory productCategory = ProductCategory.JINYINMAO);

        Task<IList<BAProductInfo>> GetTopProductInfosAsync(int topPageCount = 6, ProductCategory productCategory = ProductCategory.JINYINMAO);

        Task<ProductWithSaleInfo<BAProductInfo>> GetTopProductWithSaleInfoAsync(ProductCategory productCategory = ProductCategory.JINYINMAO);

        Task<IList<ProductWithSaleInfo<BAProductInfo>>> GetTopProductWithSaleInfosAsync(int topPageCount = 6, ProductCategory productCategory = ProductCategory.JINYINMAO);
    }

    public interface IExactBAProductInfoService : IBAProductInfoService
    {
    }
}