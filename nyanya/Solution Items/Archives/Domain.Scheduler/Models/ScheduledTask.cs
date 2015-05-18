// FileInformation: nyanya/Domain.SchedulerTask/SchedulerTask.cs
// CreatedTime: 2014/04/28   1:29 PM
// LastUpdatedTime: 2014/04/28   1:29 PM

using System;

namespace Domain.Scheduler.Models
{
    public class ScheduledTask
    {
        public DateTime FinishTime { get; set; }

        public bool HasFinished { get; set; }

        public int Id { get; set; }

        public string Messages { get; set; }

        public DateTime StartTime { get; set; }

        public bool Successed { get; set; }

        public string TaskName { get; set; }
    }
}