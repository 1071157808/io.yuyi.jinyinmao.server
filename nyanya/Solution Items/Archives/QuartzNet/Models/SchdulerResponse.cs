using System;
using System.Collections.Generic;
using Quartz;

namespace QuartzNet.Models
{
    /// <summary>
    ///
    /// </summary>
    public class JobInfo
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the job key.
        /// </summary>
        /// <value>
        /// The job key.
        /// </value>
        public JobKey JobKey { get; set; }

        /// <summary>
        /// Gets or sets the type of the job.
        /// </summary>
        /// <value>
        /// The type of the job.
        /// </value>
        public Type JobType { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    ///
    /// </summary>
    public class SchdulerResponse
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the currently executing jobs.
        /// </summary>
        /// <value>
        /// The currently executing jobs.
        /// </value>
        public IList<IJobExecutionContext> CurrentlyExecutingJobs { get; set; }

        /// <summary>
        /// Gets or sets the currently executing jobs count.
        /// </summary>
        /// <value>
        /// The currently executing jobs count.
        /// </value>
        public int CurrentlyExecutingJobsCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [in standby mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [in standby mode]; otherwise, <c>false</c>.
        /// </value>
        public bool InStandbyMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is job group paused.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is job group paused; otherwise, <c>false</c>.
        /// </value>
        public bool IsJobGroupPaused { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is start.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is start; otherwise, <c>false</c>.
        /// </value>
        public bool IsStart { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is trigger group paused.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is trigger group paused; otherwise, <c>false</c>.
        /// </value>
        public bool IsTriggerGroupPaused { get; set; }

        /// <summary>
        /// Gets or sets the name of the job group.
        /// </summary>
        /// <value>
        /// The name of the job group.
        /// </value>
        public string JobGroupName { get; set; }

        /// <summary>
        /// Gets or sets the jobs keys.
        /// </summary>
        /// <value>
        /// The jobs keys.
        /// </value>
        public IList<JobInfo> Jobs { get; set; }

        /// <summary>
        /// Gets or sets the name of the trigger group.
        /// </summary>
        /// <value>
        /// The name of the trigger group.
        /// </value>
        public string TriggerGroupName { get; set; }

        /// <summary>
        /// Gets or sets the triggers.
        /// </summary>
        /// <value>
        /// The triggers.
        /// </value>
        public IList<TriggerInfo> Triggers { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    ///
    /// </summary>
    public class TriggerInfo
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the trigger key.
        /// </summary>
        /// <value>
        /// The trigger key.
        /// </value>
        public TriggerKey TriggerKey { get; set; }

        /// <summary>
        /// Gets or sets the trigger next fire time UTC.
        /// </summary>
        /// <value>
        /// The trigger next fire time UTC.
        /// </value>
        public DateTimeOffset? TriggerNextFireTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the trigger previous fire time UTC.
        /// </summary>
        /// <value>
        /// The trigger previous fire time UTC.
        /// </value>
        public DateTimeOffset? TriggerPreviousFireTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the state of the trigger.
        /// </summary>
        /// <value>
        /// The state of the trigger.
        /// </value>
        public TriggerState TriggerState { get; set; }

        #endregion Public Properties
    }
}