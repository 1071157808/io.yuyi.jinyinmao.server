// FileInformation: nyanya/Xingye.Domain.Products/IProductService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:26 PM

using System.Threading.Tasks;
using Xingye.Domain.Products.Services.DTO;
using Domian.Models;

namespace Xingye.Domain.Products.Services.Interfaces
{
    public interface IProductService : IDomainService
    {
        Task<CanRepayResult> CanRepayAsync(string productNo);

        Task<CanUnShelvesResult> CanUnShelvesAsync(string productNo);

        bool FreezeShareCount(string productIdentifier, int count);

        Task SetSoldOut(params string[] productIdentifier);

        bool UnfreezeShareCount(string productIdentifier, int count);

        Task UnShelvesAsync(string productIdentifier);
    }
}