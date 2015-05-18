// FileInformation: nyanya/Infrastructure.Data.EntityFramework.Extensions/IQueryableExtensions.cs
// CreatedTime: 2014/03/30   9:56 PM
// LastUpdatedTime: 2014/03/30   10:06 PM

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.EntityFramework.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IQueryableExtensions
    {
        public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            int totalCount = query.Count();
            IEnumerable<T> collection = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(pageIndex, pageSize, totalCount, collection);
        }

        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            int totalCount = query.Count();
            List<T> collection = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(pageIndex, pageSize, totalCount, collection);
        }
    }
}