// FileInformation: nyanya/Infrastructure.Scheduler.Interface/ScheduleTask.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/07/09   9:34 AM

using System;
using System.Linq;
using System.Text;
using System.Threading;
using Domain.Scheduler.Models;
using Infrastructure.SMS;
using NLog;

namespace Infrastructure.Scheduler.Interface
{
    public abstract class ScheduleTask : ISchedulerTask, IDisposable
    {
        protected SchedulerContext context;

        protected bool hasCancel;

        protected DateTime lastProcessTime;

        private Logger logger;
        protected SchedulerTaskStatus status;

        protected ScheduleTask()
        {
            this.Note = new StringBuilder();
            this.status = SchedulerTaskStatus.Completed;
        }

        protected bool FirstOfToday
        {
            get { return this.lastProcessTime.Date < this.Start.Date; }
        }

        protected Logger Logger
        {
            get { return this.logger ?? (this.logger = LogManager.GetLogger("SchedulerTaskLogger")); }
        }

        protected StringBuilder Note { get; set; }

        protected long StartTime
        {
            get { return this.Start.ToBinary(); }
        }

        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion IDisposable Members

        #region ISchedulerTask Members

        public virtual bool CanCancel
        {
            get { return true; }
        }

        public virtual bool CanStart
        {
            get { return this.status == SchedulerTaskStatus.Completed || this.status == SchedulerTaskStatus.Faulted || this.status == SchedulerTaskStatus.Canceled; }
        }

        public DateTime Finish { get; protected set; }

        public string Messages
        {
            get { return string.Format(" | {0} | {1} | {2}", this.TaskName, this.StartTime, this.Note); }
        }

        public DateTime Start { get; protected set; }

        public string Status
        {
            get { return this.status.ToString(); }
        }

        public abstract string TaskName { get; }

        public virtual bool Cancel()
        {
            if (this.CanCancel)
            {
                this.Cancel(true);
                return true;
            }

            this.Logger.Warn(" | {0} failed to cancel at {1} from status {2}\n", this.TaskName, DateTime.Now, this.status);
            return false;
        }

        public virtual bool Run()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(this, 1000, ref lockTaken);
                if (!lockTaken)
                {
                    this.Logger.Warn(" | {0} failed to take the lock at {1} from status {2}\n", this.TaskName, DateTime.Now, this.status);
                    return false;
                }

                if (this.CanStart)
                {
                    this.ChangeStatus(SchedulerTaskStatus.Activated);
                    return true;
                }
            }
            catch (TimeoutException)
            {
                return false;
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(this);
                }
            }

            this.Logger.Warn(" | {0} failed to activate at {1} from status {2}\n", this.TaskName, DateTime.Now, this.status);
            return false;
        }

        #endregion ISchedulerTask Members

        protected void ChangeStatus(SchedulerTaskStatus schedulerTaskStatus)
        {
            this.Note.AppendFormat("From {0} to {1} at {2}\n", this.status, schedulerTaskStatus, DateTime.Now);
            this.status = schedulerTaskStatus;
        }

        protected void Complete()
        {
            if (!this.hasCancel || this.status == SchedulerTaskStatus.Faulted || this.status == SchedulerTaskStatus.Canceled)
            {
                this.ChangeStatus(SchedulerTaskStatus.Completed);
            }
            this.Finish = DateTime.Now;
            ScheduledTask task = new ScheduledTask
            {
                FinishTime = this.Finish,
                HasFinished = true,
                Messages = this.Messages,
                StartTime = this.Start,
                Successed = this.status == SchedulerTaskStatus.Completed,
                TaskName = this.TaskName
            };
            this.context.ScheduledTasks.Add(task);
            this.context.SaveChanges();
            this.Logger.Info(this.Messages);
        }

        /// <summary>
        ///     Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                }
                this.logger = null;
            }
        }

        protected void EncounterError()
        {
            this.ChangeStatus(SchedulerTaskStatus.Faulted);
            this.Cancel(false);
            this.SendErrorMessage();
        }

        protected void GetLastProcessTime()
        {
            ScheduledTask task = this.context.ScheduledTasks.Where(t => t.TaskName == this.TaskName && t.HasFinished && t.Successed).OrderByDescending(t => t.FinishTime).FirstOrDefault();
            this.lastProcessTime = task == null ? DateTime.MinValue : task.StartTime.AddMinutes(-10);
        }

        protected virtual void Initialize()
        {
            this.hasCancel = false;
            this.Start = DateTime.Now;
            this.logger = LogManager.GetLogger("SchedulerTaskLogger");
            if (this.logger == null)
            {
                throw new NLogConfigurationException("Can not find SchedulerTaskLogger");
            }
            this.Note.Clear();
            this.context = new SchedulerContext();
            this.GetLastProcessTime();
        }

        private void Cancel(bool cancel)
        {
            this.hasCancel = true;
            if (cancel)
            {
                this.ChangeStatus(SchedulerTaskStatus.Canceled);
            }
        }

        private void SendErrorMessage()
        {
            ISmsService smsService = new SmsService();
            // ReSharper disable once CSharpWarnings::CS4014
            smsService.SendAsync("15800780728", this.TaskName + DateTime.Now);
        }
    }
}