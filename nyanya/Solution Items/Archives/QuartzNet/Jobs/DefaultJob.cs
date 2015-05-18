// FileInformation: nyanya/QuartzNet/DefaultJob.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/25   3:31 PM

using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Lib;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Logs;
using Infrastructure.SMS;
using Quartz;
using ServiceStack;

namespace QuartzNet.Jobs
{
    /// <summary>
    /// </summary>
    public abstract class DefaultJob : IJob
    {
        #region Protected Fields

        /// <summary>
        ///     The wait time
        /// </summary>
        protected TimeSpan waitTime = new TimeSpan(0, 0, 30, 0);

        #endregion Protected Fields

        #region Protected Properties

        /// <summary>
        ///     Gets the alert service.
        /// </summary>
        /// <value>
        ///     The alert service.
        /// </value>
        protected ISmsAlertsService AlertService
        {
            get { return SchedulerConfig.SmsService; }
        }

        /// <summary>
        ///     Gets the logger.
        /// </summary>
        /// <value>
        ///     The logger.
        /// </value>
        protected ILogger Logger
        {
            get { return SchedulerConfig.JobLogger; }
        }

        #endregion Protected Properties

        #region Public Methods

        /// <summary>
        ///     Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
        ///     fires that is associated with the <see cref="T:Quartz.IJob" />.
        /// </summary>
        /// <param name="context">The execution context.</param>
        /// <remarks>
        ///     The implementation may wish to set a  result object on the
        ///     JobExecutionContext before this method exits.  The result itself
        ///     is meaningless to Quartz, but may be informative to
        ///     <see cref="T:Quartz.IJobListener" />s or
        ///     <see cref="T:Quartz.ITriggerListener" />s that are watching the job's
        ///     execution.
        /// </remarks>
        public void Execute(IJobExecutionContext context)
        {
            Task task = Task.Run(async () =>
                await this.JobExecuteHandle(context));
            task.Wait(this.waitTime);
        }

        /// <summary>
        ///     Jobs the execute.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public abstract Task JobExecute(IJobExecutionContext context);

        /// <summary>
        ///     Jobs the execute handle.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task JobExecuteHandle(IJobExecutionContext context)
        {
            try
            {
                if (context != null)
                {
                    this.Logger.Info(context.JobDetail.Key + "start time:" +
                                     DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }

                await this.JobExecute(context);
                if (context != null)
                {
                    this.Logger.Info(context.JobDetail.Key + " end time:" +
                                     DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                this.Logger.Error(e, NyanyaResources.JobScheduler_DbUpdateConcurrencyException.FmtWith(GetExceptionInfo(context, e)));
            }
            catch (DbUpdateException e)
            {
                this.Logger.Error(e, NyanyaResources.JobScheduler_DbUpdateException.FmtWith(GetExceptionInfo(context, e)));
            }
            catch (BusinessConcurrenceException e)
            {
                string message = GetExceptionInfo(context, e);
                this.Logger.Error(e, NyanyaResources.JobScheduler_BusinessConcurrenceException.FmtWith(message));
            }
            catch (BusinessValidationFailedException e)
            {
                string message = GetExceptionInfo(context, e);
                this.Logger.Error(e, NyanyaResources.JobScheduler_BusinessValidationFailedException.FmtWith(message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_JobScheduler_BusinessValidationFailedException.FmtWith(message));
            }
            catch (ApplicationBusinessException e)
            {
                string message = GetExceptionInfo(context, e);
                this.Logger.Error(e, NyanyaResources.JobScheduler_BusinessConcurrenceException.FmtWith(message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_JobScheduler_ApplicationBusinessException.FmtWith(message));
            }
            catch (Exception e)
            {
                string message = GetExceptionInfo(context, e);
                this.Logger.Error(e, NyanyaResources.JobScheduler_UnexpectedException.FmtWith(e.GetType(), message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_JobScheduler_UnexpectedException.FmtWith(e.GetType(), message));
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Gets the exception information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        private static string GetExceptionInfo(IJobExecutionContext context, DbUpdateException e)
        {
            string extraMessage;
            try
            {
                extraMessage = "{0} {1}".FmtWith(e.Source, e.Entries.Select(entity => entity.Entity).ToJson());
            }
            catch (Exception exception)
            {
                extraMessage = exception.Message;
            }
            return "{0} {1} {2} {3} {4} {5}".FmtWith(context.JobDetail.Key, context.JobDetail.JobType, context.FireTimeUtc, context.ToJson(), e.Message, extraMessage);
        }

        /// <summary>
        ///     Gets the exception information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        private static string GetExceptionInfo(IJobExecutionContext context, Exception e)
        {
            return "{0} {1} {2} {3} {4}".FmtWith(context.JobDetail.Key, context.JobDetail.JobType, context.FireTimeUtc, context.ToJson(), e.Message);
        }

        #endregion Private Methods
    }
}