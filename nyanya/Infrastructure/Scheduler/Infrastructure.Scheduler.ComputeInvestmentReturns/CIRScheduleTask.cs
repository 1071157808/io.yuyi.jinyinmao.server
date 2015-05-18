// FileInformation: nyanya/Infrastructure.Scheduler.ComputeInvestmentReturns/CIRScheduleTask.cs
// CreatedTime: 2014/04/24   4:18 PM
// LastUpdatedTime: 2014/05/09   11:54 AM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using Domain.Meow.Models;
using Domain.Passport.Models;
using Domain.Scheduler.Models;
using Infrastructure.Cache.Couchbase;
using Infrastructure.Scheduler.Interface;
using MoreLinq;
using Newtonsoft.Json;

namespace Infrastructure.Scheduler.ComputeInvestmentReturns
{
    public class CIRScheduleTask : ScheduleTask
    {
        private int batchCount = 100;
        private MeowContext meowContext;
        private PassportContext passportContext;

        public override string TaskName
        {
            get { return "ComputeInvestmentReturns"; }
        }

        public override bool Run()
        {
            try
            {
                if (!base.Run())
                {
                    return false;
                }

                this.Initialize();
                Task t = this.ExecuteTask();
                t.Wait();
            }
            catch (Exception e)
            {
                this.Logger.Fatal("{0} | {1} | {2}", this.TaskName, e.Message, e.StackTrace);
                this.EncounterError();
            }
            finally
            {
                this.Complete();
                this.Dispose();
            }
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            this.meowContext.Dispose();
            this.passportContext.Dispose();
            base.Dispose(disposing);
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.meowContext = new MeowContext();
            this.passportContext = new PassportContext();

            this.ChangeStatus(SchedulerTaskStatus.Initialized);
        }

        // Only can be used here for specific scenario
        private async Task AddOrUpdateInvestmentStatistic(List<dynamic> users)
        {
            IEnumerable<int> userIds = users.Select(u => (int)u.Id).ToList();

            List<OrderStatistic> orderStatistics = await this.meowContext.OrderStatistics.AsNoTracking().Where(o => userIds.Contains(o.UserId) && o.OrderTime <= this.Start).ToListAsync();
            foreach (dynamic user in users)
            {
                List<OrderStatistic> orders = orderStatistics.Where(o => o.UserId == user.Id).ToList();
                InvestmentStatistic investmentStatistic = this.BuildInvestmentStatistic(orders, user);
                this.context.InvestmentStatistics.AddOrUpdate(i => i.UserId, investmentStatistic);
            }
            await this.context.SaveChangesAsync();
        }

        private InvestmentStatistic BuildInvestmentStatistic(List<OrderStatistic> orders, dynamic user)
        {
            InvestmentStatistic investmentStatistic = new InvestmentStatistic();
            investmentStatistic.UserId = user.Id;
            investmentStatistic.UserIdentifier = user.Guid;
            // hack: 这里的UpdateDate指计算累计收益时使用的时间
            investmentStatistic.UpdateDate = DateTime.Now;
            investmentStatistic.OrderCount = orders.Count;
            if (investmentStatistic.OrderCount == 0)
            {
                investmentStatistic.AccruedEarnings = 0;
                investmentStatistic.InterestPerSecond = 0;
            }
            else
            {
                investmentStatistic.AccruedEarnings = orders.Sum(o => o.GetAccruedEarningsWithExtraInterest());
                investmentStatistic.InterestPerSecond = orders.Sum(o => o.GetInterestPerSecondWithExtraInterest());
            }
            // hack: 这里的UpdateTime指该任务的执行批次时间，主要用于检索，区别于UpdateDate
            investmentStatistic.UpdateTime = this.StartTime;

            return investmentStatistic;
        }

        private async Task BuildTempTableAsync()
        {
            if (!this.FirstOfToday)
            {
                int[] triggers = { 10, 20 };
                var users = await this.passportContext.OrderChanges.AsNoTracking().Where(c => c.Time > this.lastProcessTime && c.Time <= this.Start && triggers.Contains(c.TriggerTypeCode)).Select(u => new { Id = u.UserId, Guid = u.UserGuid }).ToListAsync();
                users = users.DistinctBy(u => u.Id).ToList();
                // For test
                //List<dynamic> users = new List<dynamic>();
                //users.Add(new { Id = 6, Guid = "4e3f7e2e40874809927ef95cfebcb0bc" });
                await this.AddOrUpdateInvestmentStatistic(users.ToList<dynamic>());
            }
            else
            {
                int current = 0;
                int entitiesCount;

                do
                {
                    var users = await this.passportContext.UserInfos.AsNoTracking().OrderBy(u => u.Id).Skip(current).Take(this.batchCount).Select(u => new { u.Id, u.Guid }).ToListAsync();
                    entitiesCount = users.Count;
                    current += entitiesCount;

                    await this.AddOrUpdateInvestmentStatistic(users.ToList<dynamic>());
                } while (entitiesCount == this.batchCount && !this.hasCancel);
            }

            decimal averageSpeed = await this.context.InvestmentStatistics.Where(t => t.OrderCount > 0).AverageAsync(t => t.InterestPerSecond);
            decimal maxSpeed = (await this.context.InvestmentStatistics.OrderByDescending(t => t.InterestPerSecond).Take(10).Select(t => t.InterestPerSecond).ToListAsync()).Average();
            this.context.InvestmentStatistics.AddOrUpdate(i => i.UserIdentifier, new InvestmentStatistic
            {
                AccruedEarnings = maxSpeed,
                InterestPerSecond = averageSpeed,
                UpdateDate = this.Start,
                UserIdentifier = "Meow",
                UpdateTime = this.StartTime,
                UserId = 0
            });
            await this.context.SaveChangesAsync();
        }

        private async Task ExecuteTask()
        {
            this.ChangeStatus(SchedulerTaskStatus.Started);

            if (this.hasCancel)
            {
                return;
            }

            await this.BuildTempTableAsync();

            if (this.hasCancel)
            {
                return;
            }

            await this.RefreshCacheAsync();
        }

        private async Task RefreshCacheAsync()
        {
            int current = 0;
            int entitiesCount;
            do
            {
                List<InvestmentStatistic> entities = await this.context.InvestmentStatistics.Where(i => i.UpdateTime == this.StartTime).OrderBy(i => i.Id).Skip(current).Take(this.batchCount).ToListAsync();

                entitiesCount = entities.Count;
                current += entitiesCount;

                foreach (InvestmentStatistic e in entities)
                {
                    InvestmentStatisticCache.Set(e.UserIdentifier, JsonConvert.SerializeObject(e));
                }
            } while (entitiesCount == this.batchCount);
        }
    }
}