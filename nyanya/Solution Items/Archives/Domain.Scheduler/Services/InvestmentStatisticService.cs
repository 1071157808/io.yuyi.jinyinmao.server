// FileInformation: nyanya/Domain.Scheduler/InvestmentStatisticService.cs
// CreatedTime: 2014/05/08 9:18 AM
// LastUpdatedTime: 2014/05/08 1:21 PM

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Scheduler.Models;
using Domain.Scheduler.ViewModels;

namespace Domain.Scheduler.Services
{
    /// <summary>
    /// InvestmentStatisticService
    /// </summary>
    public class InvestmentStatisticService : IInvestmentStatisticService
    {
        #region IInvestmentStatisticService Members

        /// <summary>
        /// Gets the investment statistic.
        /// </summary>
        /// <param name="userGuid">The user unique identifier.</param>
        /// <returns></returns>
        public async Task<InvestmentStatisticViewModel> GetInvestmentStatistic(string userGuid)
        {
            IInvestmentStatisticCacheService cacheService = new InvestmentStatisticCacheService();
            InvestmentStatistic investmentStatistic = cacheService.GetCache(userGuid);

            // 如果缓存中没有数据，则该用户是新注册用户 如果该用户没有下单，则返回null值，最终会返回给用户公司整体的收益速度，则返回结果是正确的（延迟率取决于整体速度的缓存延迟率） 如果该用户下单了，那么只可能是在一个轮询周期内下单，并且查看了收益速度，则由于轮询间隔的原因，缓存和数据库中都不会存在该数据，只能返回公司整体的收益速度，这个结果是不准确的

            OverallInvestmentStatisticSummaryViewModel overall = await this.GetOverallInvestmentStatisticSummary();

            InvestmentStatisticViewModel viewModel = new InvestmentStatisticViewModel();

            // 新用户 investmentStatistic == null 如果缓存中没有数据，则该用户是新注册用户
            // 如果该用户没有下单，则返回null值，最终会返回给用户公司整体的收益速度，则返回结果是正确的（延迟率取决于整体速度的缓存延迟率） 如果该用户下单了，那么只可能是在一个轮询周期内下单，并且查看了收益速度，则由于轮询间隔的原因，缓存和数据库中都不会存在该数据，只能返回公司整体的收益速度，这个结果是不准确的
            //
            // 没有订单的用户 investmentStatistic.OrderCount == 0 ，返回给用户公司整体的收益速度
            if (investmentStatistic == null || investmentStatistic.OrderCount == 0)
            {
                // 只需要这2个字段
                viewModel.IsOverall = true;
                viewModel.InterestPerSecond = overall.InterestPerSecond;
            }
            else
            {
                viewModel.IsOverall = false;

                viewModel.PercentRank = (investmentStatistic.InterestPerSecond >= overall.MaxSpeed) ? 99
                    : Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(investmentStatistic.InterestPerSecond / overall.MaxSpeed))) * 100;

                viewModel.InterestPerSecond = investmentStatistic.InterestPerSecond;
            }

            return viewModel;
        }

        /// <summary>
        /// Gets the investment statistic summary.
        /// </summary>
        /// <param name="userGuid">The user unique identifier.</param>
        public async Task<InvestmentStatisticSummaryViewModel> GetInvestmentStatisticSummary(string userGuid)
        {
            IInvestmentStatisticCacheService cacheService = new InvestmentStatisticCacheService();
            InvestmentStatistic investmentStatistic = cacheService.GetCache(userGuid);

            // 如果缓存中没有数据，则该用户是新注册用户 如果该用户没有下单，则返回null值，最终会返回给用户公司整体的收益速度，则返回结果是正确的（延迟率取决于整体速度的缓存延迟率） 如果该用户下单了，那么只可能是在一个轮询周期内下单，并且查看了收益速度，则由于轮询间隔的原因，缓存和数据库中都不会存在该数据，只能返回公司整体的收益速度，这个结果是不准确的

            OverallInvestmentStatisticSummaryViewModel overall = await this.GetOverallInvestmentStatisticSummary();

            InvestmentStatisticSummaryViewModel viewModel = new InvestmentStatisticSummaryViewModel();

            // 新用户 investmentStatistic == null 如果缓存中没有数据，则该用户是新注册用户
            // 如果该用户没有下单，则返回null值，最终会返回给用户公司整体的收益速度，则返回结果是正确的（延迟率取决于整体速度的缓存延迟率） 如果该用户下单了，那么只可能是在一个轮询周期内下单，并且查看了收益速度，则由于轮询间隔的原因，缓存和数据库中都不会存在该数据，只能返回公司整体的收益速度，这个结果是不准确的
            //
            // 没有订单的用户 investmentStatistic.OrderCount == 0 ，返回给用户公司整体的收益速度
            if (investmentStatistic == null || investmentStatistic.OrderCount == 0)
            {
                // 只需要这2个字段
                viewModel.IsOverall = true;
                viewModel.InterestPerSecond = overall.InterestPerSecond;
            }
            else
            {
                InvestmentStatisticHistoryViewModel history = cacheService.GetHistoryCache(userGuid);

                viewModel.IsOverall = false;
                viewModel.AccruedEarnings = investmentStatistic.GetCurrentAccruedEarnings();

                viewModel.HasShown = history != null;
                viewModel.EarningsAgain = history == null ? 0 : viewModel.AccruedEarnings - history.LastEarnings;
                viewModel.EarningsAgainDuration = history == null ? 0 : Convert.ToInt32((DateTime.Now - history.LastShowingTime).TotalSeconds);

                viewModel.PercentRank = (investmentStatistic.InterestPerSecond >= overall.MaxSpeed) ? 99
                    : Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(investmentStatistic.InterestPerSecond / overall.MaxSpeed))) * 100;

                viewModel.InterestPerSecond = investmentStatistic.InterestPerSecond;

                // 将这次查看的当前累计收益存入缓存
                decimal accruedEarnings = viewModel.AccruedEarnings;

                // ReSharper disable once UnusedVariable
                Task<bool> task = Task.Run(() =>
                {
                    InvestmentStatisticHistoryViewModel newHistory = new InvestmentStatisticHistoryViewModel
                    {
                        LastEarnings = accruedEarnings,
                        LastShowingTime = DateTime.Now
                    };

                    return cacheService.SetHistoryCache(userGuid, newHistory);
                });
            }

            return viewModel;
        }

        #endregion IInvestmentStatisticService Members

        /// <summary>
        /// Gets the overall investment statistic summary.
        /// </summary>
        private async Task<OverallInvestmentStatisticSummaryViewModel> GetOverallInvestmentStatisticSummary()
        {
            // 整体收益速度一定是不为空的，实际中应该是缓存中一定有该数据值
            IInvestmentStatisticCacheService cacheService = new InvestmentStatisticCacheService();
            InvestmentStatistic investmentStatistic = cacheService.GetOverallCache();

            // 假设缓存中不存在该值（实际情况中很少发生，几乎只能在有错误的情况下才会发生），数据库中就一定要有该值，否则一定有错误
            if (investmentStatistic == null)
            {
                using (SchedulerContext context = new SchedulerContext())
                {
                    investmentStatistic = await context.InvestmentStatistics.AsNoTracking().Where(i => i.UserIdentifier == "Meow").FirstAsync();
                }
            }

            // hack: MaxSpeed = investmentStatistic.AccruedEarnings
            return new OverallInvestmentStatisticSummaryViewModel { InterestPerSecond = investmentStatistic.InterestPerSecond, MaxSpeed = investmentStatistic.AccruedEarnings };
        }
    }
}