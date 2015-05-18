// FileInformation: nyanya/Domain.Amp/IProductService.cs
// CreatedTime: 2014/03/31   11:42 AM
// LastUpdatedTime: 2014/03/31   11:44 AM

using Domain.Amp.Models;
using Infrastructure.Data.EntityFramework.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Amp.Services
{
    public interface IProductService
    {
        Task<PaginatedList<Product>> GetSummaryProductsAsync(int pageIndex, int pageSize);

        Task<IList<TopProduct>> SelectTopProductsAsync(int number = 3);
    }
}