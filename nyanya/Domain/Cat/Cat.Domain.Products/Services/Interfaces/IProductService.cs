// FileInformation: nyanya/Cat.Domain.Products/IProductService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:26 PM

using Cat.Commands.Products;
using Cat.Domain.Products.Services.DTO;
using Domian.Models;
using System.Threading.Tasks;

namespace Cat.Domain.Products.Services.Interfaces
{
    public interface IProductService : IDomainService
    {
        Task<CanRepayResult> CanRepayAsync(string productNo);

        Task<CanUnShelvesResult> CanUnShelvesAsync(string productNo);

        bool FreezeShareCount(string productIdentifier, int count);

        Task SetSoldOut(params string[] productIdentifier);

        bool UnfreezeShareCount(string productIdentifier, int count);

        Task UnShelvesAsync(string productIdentifier);

        /// <summary>
        /// 是否可以更新可售份额
        /// </summary>
        /// <param name="productNo">产品编号</param>
        /// <returns></returns>
        Task<CanUpdateShareCountResult> CanUpdateShareCountAsync(string productNo);

        Task SetTotalSaleAmount(string productIdentifier, decimal totalSaleAmount);

        Task SetTotalInterest(string productIdentifier, decimal totalInterest);

        Task SetTotalRedeemAmount(string productIdentifier, decimal totalRedeemAmount);

        Task SetTotalRedeemInterest(string productIdentifier, decimal totalRedeemInterest);

        Task SetPerRemainRedeemAmount(string productIdentifier, decimal totalRedeemAmount, decimal todayRedeemAmount);

        Task<bool> CanLessPerRemainRedeemAmount(string productNo, decimal redeemAmount);

        Task<bool> CheckProductNoExists(string productNo);
    }
}