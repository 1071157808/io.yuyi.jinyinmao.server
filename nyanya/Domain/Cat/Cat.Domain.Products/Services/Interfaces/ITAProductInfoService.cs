// FileInformation: nyanya/Cqrs.Domain.Product/ITAProductInfoService.cs
// CreatedTime: 2014/07/28   11:35 AM
// LastUpdatedTime: 2014/08/10   11:43 AM

using System.Collections.Generic;
using System.Threading.Tasks;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Domian.DTO;
using Domian.Models;
using Cat.Commands.Products;

namespace Cat.Domain.Products.Services.Interfaces
{
    public interface IExactTAProductInfoService : ITAProductInfoService
    {
    }

    public interface ITAProductInfoService : IDomainService
    {
        Task<IList<ProductWithSaleInfo<TAProductInfo>>> GetBeingSoldOutProductWithSaleInfosAsync();

        Task<TAProductInfo> GetProductInfoByIdentifierAsync(string productIdentifier);

        Task<TAProductInfo> GetProductInfoByNoAsync(string productNo);

        Task<IPaginatedDto<TAProductInfo>> GetProductInfosAsync(int pageIndex, int pageSize = 10, ProductCategory productCategory = ProductCategory.JINYINMAO);

        Task<ProductWithSaleInfo<TAProductInfo>> GetProductWithSaleInfoByIdentifierAsync(string productIdentifier);

        Task<ProductWithSaleInfo<TAProductInfo>> GetProductWithSaleInfoByNoAsync(string productNo);

        Task<IPaginatedDto<ProductWithSaleInfo<TAProductInfo>>> GetProductWithSaleInfosAsync(int pageIndex, ProductCategory productCategory = ProductCategory.JINYINMAO, int pageSize = 10);

        Task<TAProductInfo> GetTopProductInfoAsync(ProductCategory productCategory = ProductCategory.JINYINMAO);

        Task<ProductWithSaleInfo<TAProductInfo>> GetTopProductWithSaleInfoAsync(ProductCategory productCategory = ProductCategory.JINYINMAO);

        Task<IList<ProductWithSaleInfo<TAProductInfo>>> GetTopProductWithSaleInfosAsync(int topPageCount = 6, ProductCategory productCategory = ProductCategory.JINYINMAO);
    }
}