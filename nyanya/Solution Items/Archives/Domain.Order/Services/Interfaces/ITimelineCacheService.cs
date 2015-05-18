using System.Threading.Tasks;
using Domain.Order.Models;

namespace Domain.Order.Services.Interfaces
{
    public interface ITimelineCacheService
    {
        Task<Timeline> GetCache(string key, bool refreshCache);
    }
}