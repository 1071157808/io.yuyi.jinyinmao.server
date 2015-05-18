// FileInformation: nyanya/Domain.Scheduler/IInvestmentStatisticService.cs
// CreatedTime: 2014/04/28   5:46 PM
// LastUpdatedTime: 2014/04/29   2:36 PM

using System.Threading.Tasks;
using Domain.Scheduler.ViewModels;

namespace Domain.Scheduler.Services
{
    public interface IInvestmentStatisticService
    {
        Task<InvestmentStatisticViewModel> GetInvestmentStatistic(string userGuid);

        Task<InvestmentStatisticSummaryViewModel> GetInvestmentStatisticSummary(string userGuid);
    }
}