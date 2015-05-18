using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cqrs.Domain.Models;
using Cqrs.Domain.Products.ReadModels;
using Cqrs.Domain.Products.Services.DTO;

namespace Cqrs.Domain.Products.Services.Interfaces
{
    public interface IJymProductInfoService<TInfo, TListModel, TSaleInfo,TSaleListModel> : IDomainService
        where TInfo : ProductInfo
        where TListModel : ProductListModel<TInfo>
        where TSaleInfo: ProductWithSaleInfo
        where TSaleListModel : ProductWithSaleListModel<TSaleInfo>
        
    {
        Task<TInfo> GetProductInfoByIdentifier(string productIdentifier);

        Task<TInfo> GetProductInfoByNo(string productNo);

        Task<TListModel> GetProductList(int pageNumber);

        Task<TSaleInfo> GetProductWithSaleInfoByIdentifier(string productIdentifier);

        Task<TSaleInfo> GetProductWithSaleInfoByNo(string productNo);

        Task<TSaleListModel> GetProductWithSaleList(int pageNumber);

        Task<TInfo> GetTopProductInfo();

        Task<IList<TInfo>> GetTopProductList();

        Task<TSaleInfo> GetTopProductWithSale();

        Task<IList<TSaleInfo>> GetTopProductWithSaleList();
    }
}
