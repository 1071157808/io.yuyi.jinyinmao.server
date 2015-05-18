// FileInformation: nyanya/QuartzNet/DefaultSchdulerMgr.cs
// CreatedTime: 2014/08/13   7:01 PM
// LastUpdatedTime: 2014/08/13   7:21 PM

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using QuartzNet.Models;

namespace QuartzNet.Schdulers
{
    /// <summary>
    /// </summary>
    public class DefaultSchdulerMgr
    {
        #region Private Fields

        private readonly IScheduler scheduler;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultSchdulerMgr" /> class.
        /// </summary>
        public DefaultSchdulerMgr()
        {
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "MonitorProductSaleOut";

            // set thread pool info
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "1";

            //
            properties["quartz.jobStore.misfireThreshold"] = "540000";
            this.scheduler = this.getScheduler(properties);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultSchdulerMgr" /> class.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public DefaultSchdulerMgr(NameValueCollection properties)
        {
            this.scheduler = this.getScheduler(properties);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultSchdulerMgr" /> class.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="jobDetail">The job detail.</param>
        /// <param name="trigger">The trigger.</param>
        public DefaultSchdulerMgr(NameValueCollection properties, IJobDetail jobDetail, ITrigger trigger)
        {
            this.scheduler = this.getScheduler(properties);
            this.scheduler.ScheduleJob(jobDetail, trigger);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Adds the job. 不立即触发
        /// </summary>
        /// <param name="jobDetail">The job detail.</param>
        /// <param name="replace">if set to <c>true</c> [replace].</param>
        public void AddJob(IJobDetail jobDetail, bool replace)
        {
            this.scheduler.AddJob(jobDetail, replace);
        }

        /// <summary>
        ///     Deletes the job.
        /// </summary>
        /// <param name="jobKey">The job key.</param>
        /// <returns></returns>
        public bool DeleteJob(JobKey jobKey)
        {
            return this.scheduler.DeleteJob(jobKey);
        }

        /// <summary>
        ///     Deletes the jobs.
        /// </summary>
        /// <param name="jobKeys">The job keys.</param>
        /// <returns></returns>
        public bool DeleteJobs(IList<JobKey> jobKeys)
        {
            return this.scheduler.DeleteJobs(jobKeys);
        }

        /// <summary>
        ///     Executes the job once.
        /// </summary>
        public void ExecuteJobOnce()
        {
            if (this.scheduler.GetCurrentlyExecutingJobs().Count == 0)
            {
                IList<string> groupNames = this.scheduler.GetJobGroupNames();
                if (groupNames.Count == 1)
                {
                    List<JobKey> keys = this.scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupNames[0])).ToList();
                    IJob job = Activator.CreateInstance(this.scheduler.GetJobDetail(keys[0]).JobType) as IJob;
                    if (job != null) job.Execute(null);
                }
            }
        }

        /// <summary>
        ///     Executes the job once by trigger.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ExecuteJobOnceByTrigger<T>() where T : IJob
        {
            if (this.scheduler.GetCurrentlyExecutingJobs().Count == 0)
            {
                IList<string> groupNames = this.scheduler.GetJobGroupNames();
                foreach (string groupName in groupNames)
                {
                    DateTimeOffset runTime = DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow);
                    IJobDetail jobDetail = JobBuilder.Create<T>().WithIdentity("tempJob", groupName).Build();
                    ITrigger trigger = TriggerBuilder.Create().WithIdentity("tempTrigger", groupName).StartAt(runTime).Build();
                    this.scheduler.ScheduleJob(jobDetail, trigger);
                }
            }
        }

        /// <summary>
        ///     Gets the jobs information.
        /// </summary>
        /// <returns></returns>
        public SchdulerResponse GetJobsInfo()
        {
            SchdulerResponse response = new SchdulerResponse
            {
                Triggers = new List<TriggerInfo>(),
                Jobs = new List<JobInfo>()
            };
            IList<string> jobs = this.scheduler.GetJobGroupNames();
            IList<string> triggers = this.scheduler.GetTriggerGroupNames();
            foreach (string s in jobs)
            {
                response.JobGroupName = s;
                IList<JobKey> keys = this.scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(s)).ToList();
                foreach (JobKey key in keys)
                {
                    response.Jobs.Add(new JobInfo
                    {
                        JobKey = key,
                        Description = this.scheduler.GetJobDetail(key).Description,
                        JobType = this.scheduler.GetJobDetail(key).JobType
                    });
                }
                response.IsJobGroupPaused = this.scheduler.IsJobGroupPaused(s);
            }

            foreach (string s in triggers)
            {
                response.TriggerGroupName = s;
                IList<TriggerKey> triggerList = this.scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(s)).ToList();
                foreach (TriggerKey triggerKey in triggerList)
                {
                    this.scheduler.GetTriggerState(triggerKey);
                    response.Triggers.Add(new TriggerInfo
                    {
                        TriggerKey = triggerKey,
                        TriggerNextFireTimeUtc = this.scheduler.GetTrigger(triggerKey).GetNextFireTimeUtc().GetValueOrDefault(),
                        TriggerPreviousFireTimeUtc = this.scheduler.GetTrigger(triggerKey).GetPreviousFireTimeUtc().GetValueOrDefault(),
                        TriggerState = this.scheduler.GetTriggerState(triggerKey)
                    });
                }
            }
            response.CurrentlyExecutingJobs = this.scheduler.GetCurrentlyExecutingJobs();
            response.CurrentlyExecutingJobsCount = this.scheduler.GetCurrentlyExecutingJobs().Count;
            response.IsStart = this.scheduler.IsStarted;
            response.InStandbyMode = this.scheduler.InStandbyMode;
            return response;
        }

        /// <summary>
        ///     Gets the state of the trigger.
        /// </summary>
        /// <param name="triggerKey">The trigger key.</param>
        /// <returns></returns>
        public TriggerState GetTriggerState(TriggerKey triggerKey)
        {
            return this.scheduler.GetTriggerState(triggerKey);
        }

        /// <summary>
        ///     Determines whether this instance is started.
        /// </summary>
        /// <returns></returns>
        public bool IsStarted()
        {
            return this.scheduler.IsStarted;
        }

        /// <summary>
        ///     Pauses the job.
        /// </summary>
        /// <param name="jobKey">The job key.</param>
        public void PauseJob(JobKey jobKey)
        {
            this.scheduler.PauseJob(jobKey);
        }

        /// <summary>
        ///     Pauses the jobs.
        /// </summary>
        /// <param name="groupMatcher">The group matcher.</param>
        public void PauseJobs(GroupMatcher<JobKey> groupMatcher)
        {
            this.scheduler.PauseJobs(groupMatcher);
        }

        /// <summary>
        ///     Pauses the trigger.
        /// </summary>
        /// <param name="triggerKey">The trigger key.</param>
        public void PauseTrigger(TriggerKey triggerKey)
        {
            this.scheduler.PauseTrigger(triggerKey);
        }

        /// <summary>
        ///     Pauses the triggers.
        /// </summary>
        public void PauseTriggers()
        {
            foreach (string groupName in this.scheduler.GetTriggerGroupNames())
            {
                this.scheduler.PauseTriggers(GroupMatcher<TriggerKey>.GroupEquals(groupName));
            }
        }

        /// <summary>
        ///     Reschedules the job.
        /// </summary>
        /// <param name="triggerkey">The triggerkey.</param>
        /// <param name="trigger">The trigger.</param>
        /// <returns></returns>
        public DateTimeOffset? RescheduleJob(TriggerKey triggerkey, ITrigger trigger)
        {
            return this.scheduler.RescheduleJob(triggerkey, trigger);
        }

        /// <summary>
        ///     Resumes the jobs.
        /// </summary>
        /// <param name="jobKey">The job key.</param>
        public void ResumeJobs(JobKey jobKey)
        {
            this.scheduler.ResumeJob(jobKey);
        }

        /// <summary>
        ///     Resumes the trigger.
        /// </summary>
        /// <param name="triggerKey">The trigger key.</param>
        public void ResumeTrigger(TriggerKey triggerKey)
        {
            this.scheduler.ResumeTrigger(triggerKey);
        }

        /// <summary>
        ///     Resumes the triggers.
        /// </summary>
        public void ResumeTriggers()
        {
            foreach (string groupName in this.scheduler.GetTriggerGroupNames())
            {
                this.scheduler.ResumeTriggers(GroupMatcher<TriggerKey>.GroupEquals(groupName));
            }
        }

        /// <summary>
        ///     Schedules the job. 建议使用该方法
        /// </summary>
        /// <param name="jobDetail">The job detail.</param>
        /// <param name="trigger">The trigger.</param>
        /// <returns></returns>
        public DateTimeOffset ScheduleJob(IJobDetail jobDetail, ITrigger trigger)
        {
            return this.scheduler.ScheduleJob(jobDetail, trigger);
        }

        /// <summary>
        ///     Schedules the job.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        /// <returns></returns>
        public DateTimeOffset ScheduleJob(ITrigger trigger)
        {
            return this.scheduler.ScheduleJob(trigger);
        }

        /// <summary>
        ///     Schedules the jobs.
        /// </summary>
        /// <param name="triggersAndJobs">The triggers and jobs.</param>
        /// <param name="replace">if set to <c>true</c> [replace].</param>
        public void ScheduleJobs(IDictionary<IJobDetail, Quartz.Collection.ISet<ITrigger>> triggersAndJobs, bool replace)
        {
            this.scheduler.ScheduleJobs(triggersAndJobs, replace);
        }

        /// <summary>
        ///     Shutdowns the specified wait for jobs to complete.
        /// </summary>
        /// <param name="waitForJobsToComplete">if set to <c>true</c> [the scheduler will not allow this method to return until all currently executing jobs have completed].</param>
        public void Shutdown(bool waitForJobsToComplete)
        {
            this.scheduler.Shutdown(waitForJobsToComplete);
        }

        /// <summary>
        ///     Standbies this instance.
        /// </summary>
        public void Standby()
        {
            this.scheduler.Standby();
        }

        /// <summary>
        /// </summary>
        public void Start()
        {
            this.scheduler.Start();
        }

        /// <summary>
        ///     Triggers the job.
        /// </summary>
        /// <param name="jobKey">The job key.</param>
        public void TriggerJob(JobKey jobKey)
        {
            this.scheduler.TriggerJob(jobKey);
        }

        /// <summary>
        ///     Triggers the job.
        /// </summary>
        /// <param name="jobKey">The job key.</param>
        /// <param name="jobDataMap">The job data map.</param>
        public void TriggerJob(JobKey jobKey, JobDataMap jobDataMap)
        {
            this.scheduler.TriggerJob(jobKey, jobDataMap);
        }

        /// <summary>
        ///     Uns the schedule job.停止调度
        /// </summary>
        /// <param name="triggerKey">The trigger key.</param>
        /// <returns></returns>
        public bool UnScheduleJob(TriggerKey triggerKey)
        {
            return this.scheduler.UnscheduleJob(triggerKey);
        }

        #endregion Public Methods

        #region Private Methods

        private IScheduler getScheduler(NameValueCollection properties)
        {
            StdSchedulerFactory sf = new StdSchedulerFactory();
            sf.Initialize(properties);
            IScheduler sched = sf.GetScheduler();

            return sched;
        }

        #endregion Private Methods
    }
}