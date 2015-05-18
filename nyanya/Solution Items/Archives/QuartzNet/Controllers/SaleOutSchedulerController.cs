// FileInformation: nyanya/QuartzNet/SaleOutSchedulerController.cs
// CreatedTime: 2014/08/13   7:01 PM
// LastUpdatedTime: 2014/08/13   7:12 PM

using System.Web.Http;
using Quartz;
using QuartzNet.Jobs;
using QuartzNet.Schdulers;
using Services.WebAPI.Common.Controller;

namespace QuartzNet.Controllers
{
    /// <summary>
    /// </summary>
    [RoutePrefix("SaleOutScheduler")]
    public class SaleOutSchedulerController : ApiControllerBase
    {
        #region Public Fields

        /// <summary>
        ///     The schduler MGR
        /// </summary>
        public readonly DefaultSchdulerMgr schdulerMgr = SchedulerConfig.SetSaleOutSchdulerMgr;

        #endregion Public Fields

        #region Public Methods

        /// <summary>
        ///     Executes the job once.
        /// </summary>
        [HttpGet, Route("ExecuteJobOnce")]
        public void ExecuteJobOnce()
        {
            this.schdulerMgr.ExecuteJobOnceByTrigger<ProductSaleOutJob>();
        }

        /// <summary>
        ///     Pauses the triggers.
        /// </summary>
        [HttpGet, Route("PauseTriggers")]
        public void PauseTriggers()
        {
            this.schdulerMgr.PauseTriggers();
        }

        /// <summary>
        ///     Schdulers the Shutdown.the scheduler will not allow this method to return until all currently executing jobs have completed.
        /// </summary>
        [HttpGet, Route("ResumeTriggers")]
        public void ResumeTriggers()
        {
            this.schdulerMgr.ResumeTriggers();
        }

        /// <summary>
        ///     Schdulers the information.
        /// </summary>
        /// <returns>
        ///     TriggerState
        ///     <summary>
        ///         Indicates that the <see cref="ITrigger" /> is in the "normal" state.
        ///     </summary>
        ///     Normal,
        ///     <summary>
        ///         Indicates that the <see cref="ITrigger" /> is in the "paused" state.
        ///     </summary>
        ///     Paused,
        ///     <summary>
        ///         Indicates that the <see cref="ITrigger" /> is in the "complete" state.
        ///     </summary>
        ///     <remarks>
        ///         "Complete" indicates that the trigger has not remaining fire-times in
        ///         its schedule.
        ///     </remarks>
        ///     Complete,
        ///     <summary>
        ///         Indicates that the <see cref="ITrigger" /> is in the "error" state.
        ///     </summary>
        ///     <remarks>
        ///         <para>
        ///             A <see cref="ITrigger" /> arrives at the error state when the scheduler
        ///             attempts to fire it, but cannot due to an error creating and executing
        ///             its related job. Often this is due to the <see cref="IJob" />'s
        ///             class not existing in the classpath.
        ///         </para>
        ///         <para>
        ///             When the trigger is in the error state, the scheduler will make no
        ///             attempts to fire it.
        ///         </para>
        ///     </remarks>
        ///     Error,
        ///     <summary>
        ///         Indicates that the <see cref="ITrigger" /> is in the "blocked" state.
        ///     </summary>
        ///     <remarks>
        ///         A <see cref="ITrigger" /> arrives at the blocked state when the job that
        ///         it is associated with has a <see cref="DisallowConcurrentExecutionAttribute" /> and it is
        ///         currently executing.
        ///     </remarks>
        ///     <seealso cref="DisallowConcurrentExecutionAttribute" />
        ///     Blocked,
        ///     <summary>
        ///         Indicates that the <see cref="ITrigger" /> does not exist.
        ///     </summary>
        ///     None
        /// </returns>
        [HttpGet, Route("")]
        public IHttpActionResult SchdulerInfo()
        {
            return this.Ok(this.schdulerMgr.GetJobsInfo());
        }

        /// <summary>
        ///     Schdulers the stand by.
        /// </summary>
        [HttpGet, Route("StandBy")]
        public void SchdulerStandBy()
        {
            this.schdulerMgr.Standby();
        }

        /// <summary>
        ///     Schdulers the start.
        /// </summary>
        [HttpGet, Route("Start")]
        public void SchdulerStart()
        {
            this.schdulerMgr.Start();
        }

        #endregion Public Methods
    }
}