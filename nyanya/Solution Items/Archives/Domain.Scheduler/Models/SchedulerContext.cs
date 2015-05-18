// FileInformation: nyanya/Domain.Scheduler/SchedulerContext.cs
// CreatedTime: 2014/04/28   1:31 PM
// LastUpdatedTime: 2014/04/29   2:41 PM

using System.Data.Entity;
using Domain.Scheduler.Models.Mapping;

namespace Domain.Scheduler.Models
{
    public class SchedulerContext : DbContext
    {
        static SchedulerContext()
        {
            Database.SetInitializer<SchedulerContext>(null);
        }

        public SchedulerContext()
            : base("Name=SchedulerContext")
        {
        }

        public DbSet<InvestmentStatistic> InvestmentStatistics { get; set; }

        public DbSet<ScheduledTask> ScheduledTasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ScheduledTaskMap());
            modelBuilder.Configurations.Add(new InvestmentStatisticMap());
        }
    }
}