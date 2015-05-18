using Cat.Commands.Products;
using Cat.Domain.Products.Models;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Domian.DTO;
using Domian.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cat.Domain.Products.Services.Interfaces
{
    public interface IZCBProductInfoService : IDomainService
    {
        Task<IList<ProductWithSaleInfo<ZCBProductInfo>>> GetTopProductWithSaleInfosAsync(int topPageCount = 6, ProductCategory ProductCategory = ProductCategory.JINYINMAO);

        Task<ProductWithSaleInfo<ZCBProductInfo>> GetProductWithSaleInfoByNoAsync(string productNo);

        Task<IPaginatedDto<ProductWithSaleInfo<ZCBProductInfo>>> GetProductWithSaleInfosAsync(int pageIndex, ProductCategory productCategory = ProductCategory.JINYINMAO, int pageSize = 10);

        Task<IList<string>> GetSaleProductIdentifierListAsync();

        Task<IList<ZCBProduct>> GetZcbProductList();

        Task<IList<ZCBHistory>> GetZcbHistorys(string productIdentifier);

        Task<decimal> GetZcbProductYesterDayYield(string productIdentifier);

        Task<RedeemAmountModel> GetRedeemPrincipal(string productNo);

        Task<ZCBProductInfo> GetProductInfoByIdentifierAsync(string productIdentifier);

        Task<int> CheckEnableSaleZcbProduct(string productIdentifier);

        Task UnShelvesZcbProduct(string productNo);
    }

    public interface IExactZCBProductInfoService : IZCBProductInfoService
    {
    }
}