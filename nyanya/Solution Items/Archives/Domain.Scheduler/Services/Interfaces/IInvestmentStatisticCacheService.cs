// FileInformation: nyanya/Domain.Scheduler/IInvestmentStatisticCacheService.cs
// CreatedTime: 2014/04/29   2:42 PM
// LastUpdatedTime: 2014/04/29   2:44 PM

using Domain.Scheduler.Models;
using Domain.Scheduler.ViewModels;

namespace Domain.Scheduler.Services
{
    public interface IInvestmentStatisticCacheService
    {
        InvestmentStatistic GetCache(string key);

        InvestmentStatisticHistoryViewModel GetHistoryCache(string userGuid);

        InvestmentStatistic GetOverallCache();

        bool SetHistoryCache(string userGuid, InvestmentStatisticHistoryViewModel newHistory);
    }
}