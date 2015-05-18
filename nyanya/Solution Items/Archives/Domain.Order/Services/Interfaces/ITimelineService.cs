// FileInformation: nyanya/Domain.Meow/ITimelineService.cs
// CreatedTime: 2014/04/26   5:56 PM
// LastUpdatedTime: 2014/04/26   6:11 PM

using System.Threading.Tasks;
using Domain.Order.Models;

namespace Domain.Order.Services
{
    public interface ITimelineService
    {
        Task<Timeline> GetFutureItemsAsync(string userGuid, int skip, int take);

        Task<Timeline> GetPastItemsAsync(string userGuid, int skip, int take);

        Task<string> GetTimelineTimestamp(string userGuid);
    }
}