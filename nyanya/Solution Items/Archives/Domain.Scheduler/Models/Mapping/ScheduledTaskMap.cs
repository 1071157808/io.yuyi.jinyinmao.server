// FileInformation: nyanya/Domain.Scheduler/ScheduledTaskMap.cs
// CreatedTime: 2014/04/29   2:38 PM
// LastUpdatedTime: 2014/05/04   11:27 AM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Scheduler.Models.Mapping
{
    public class ScheduledTaskMap : EntityTypeConfiguration<ScheduledTask>
    {
        public ScheduledTaskMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("ScheduledTask", "Scheduler");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FinishTime).HasColumnName("FinishTime");
            this.Property(t => t.HasFinished).HasColumnName("HasFinished");
            this.Property(t => t.Messages).HasColumnName("Messages");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.Successed).HasColumnName("Successed");
            this.Property(t => t.TaskName).HasColumnName("TaskName");
        }
    }
}