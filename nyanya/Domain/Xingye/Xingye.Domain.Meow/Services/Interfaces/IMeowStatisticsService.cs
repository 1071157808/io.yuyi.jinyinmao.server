// FileInformation: nyanya/Xingye.Domain.Meow/IMeowStatisticsService.cs
// CreatedTime: 2014/08/10   12:10 AM
// LastUpdatedTime: 2014/08/10   2:29 AM

using System.Threading.Tasks;
using Xingye.Domain.Meow.Services.DTO;

namespace Xingye.Domain.Meow.Services.Interfaces
{
    public interface IMeowStatisticsService
    {
        Task<IndexStatistics> GetIndexStatisticsAsync();
    }
}