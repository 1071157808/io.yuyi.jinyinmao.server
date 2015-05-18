// FileInformation: nyanya/Infrastructure.Scheduler.Interface/ISchedulerTask.cs
// CreatedTime: 2014/04/24   4:08 PM
// LastUpdatedTime: 2014/05/04   5:52 PM

using System;

namespace Infrastructure.Scheduler.Interface
{
    public enum SchedulerTaskStatus
    {
        Completed = 1,
        Activated = 2,
        Initialized = 3,
        Started = 4,
        Canceled = 6,
        Faulted = 5,
    }

    public interface ISchedulerTask
    {
        bool CanCancel { get; }

        bool CanStart { get; }

        DateTime Finish { get; }

        string Messages { get; }

        DateTime Start { get; }

        string Status { get; }

        string TaskName { get; }

        bool Cancel();

        bool Run();
    }
}