using System.Threading.Tasks;
using Cat.Domain.Orders.ReadModels;

namespace Cat.Domain.Orders.Services.Interfaces
{
    public interface IExactTimelineInfoService : ITimelineInfoService
    {
    }

    public interface ITimelineInfoService
    {
        Task<Timeline> GetTimelineAsync(string userIdentifier);
    }
}