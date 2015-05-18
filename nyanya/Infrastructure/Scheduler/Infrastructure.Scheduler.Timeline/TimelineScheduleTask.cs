// FileInformation: nyanya/Infrastructure.Scheduler.Timeline/TimelineScheduleTask.cs
// CreatedTime: 2014/04/26   3:28 PM
// LastUpdatedTime: 2014/05/04   3:55 PM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Order.Models;
using Domain.Passport.Models;
using Infrastructure.Cache.Couchbase;
using Infrastructure.Scheduler.Interface;
using Newtonsoft.Json;

namespace Infrastructure.Scheduler.Timeline
{
    public sealed class TimelineScheduleTask : ScheduleTask
    {
        private const int BatchCount = 100;
        private OrderContext orderContext;
        private PassportContext passportContext;

        public override string TaskName
        {
            get { return "Timeline"; }
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
            if (this.orderContext != null)
            {
                this.orderContext.Dispose();
            }

            if (this.passportContext != null)
            {
                this.passportContext.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.orderContext = new OrderContext();
            this.passportContext = new PassportContext();
            this.ChangeStatus(SchedulerTaskStatus.Initialized);
        }

        private async Task ExecuteTask()
        {
            this.ChangeStatus(SchedulerTaskStatus.Started);

            if (this.hasCancel)
            {
                return;
            }

            await this.RefreshCacheAsync();
        }

        private async Task RefreshCacheAsync()
        {
            if (!this.FirstOfToday)
            {
                int[] triggers = { 10, 30 };
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmm_ss").Remove(14);
                List<string> users = await this.passportContext.OrderChanges.AsNoTracking().Where(c => c.Time > this.lastProcessTime && c.Time <= this.Start && triggers.Contains(c.TriggerTypeCode)).Select(u => u.UserGuid).ToListAsync();
                users = users.Distinct().ToList();
                List<TimelineItem> timelineItems = await this.orderContext.TimelineItems.AsNoTracking().Where(t => users.Contains(t.UserGuid)).ToListAsync();

                foreach (string user in users)
                {
                    List<TimelineItem> items = timelineItems.Where(t => t.UserGuid == user).OrderBy(i => i.Time).ThenBy(i => i.Type).ThenBy(i => i.Identifier).ToList();
                    string json = JsonConvert.SerializeObject(new Domain.Order.Models.Timeline { Timestamp = timestamp, Items = items });
                    TimelineCache.Set(user, json);
                }
            }
            else
            {
                int current = 0;
                int entitiesCount;

                do
                {
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmm_ss").Remove(14);
                    List<string> users = await this.passportContext.UserInfos.AsNoTracking().OrderBy(u => u.Id).Skip(current).Take(BatchCount).Select(u => u.Guid).ToListAsync();
                    entitiesCount = users.Count;
                    current += entitiesCount;

                    List<TimelineItem> timelineItems = await this.orderContext.TimelineItems.AsNoTracking().Where(t => users.Contains(t.UserGuid)).ToListAsync();

                    foreach (string user in users)
                    {
                        List<TimelineItem> items = timelineItems.Where(t => t.UserGuid == user).OrderBy(i => i.Time).ThenBy(i => i.Type).ThenBy(i => i.Identifier).ToList();
                        string json = JsonConvert.SerializeObject(new Domain.Order.Models.Timeline { Timestamp = timestamp, Items = items });
                        TimelineCache.Set(user, json);
                    }
                } while (entitiesCount == BatchCount && !this.hasCancel);
            }
        }
    }
}