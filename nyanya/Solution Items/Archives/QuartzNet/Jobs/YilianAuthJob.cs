// FileInformation: nyanya/QuartzNet/YilianAuthJob.cs
// CreatedTime: 2014/08/15   1:52 PM
// LastUpdatedTime: 2014/08/18   1:15 AM

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cqrs.Domain.Yilian.ReadModels;
using Cqrs.Domain.Yilian.Services;
using Cqrs.Domain.Yilian.Services.Interfaces;
using Quartz;

namespace QuartzNet.Jobs
{
    /// <summary>
    /// </summary>
    public class YilianAuthJob : DefaultJob, IYilianAuthJob
    {
        #region Private Fields

        /// <summary>
        ///     The yilian query service
        /// </summary>
        private readonly IYilianQueryService yilianQueryService;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="YilianAuthJob" /> class.
        /// </summary>
        public YilianAuthJob()
        {
            this.yilianQueryService = new YilianQueryService();
            this.waitTime = new TimeSpan(0, 0, 9, 0);
        }

        #endregion Public Constructors

        #region Public Methods

        public override async Task JobExecute(IJobExecutionContext context)
        {
            IList<YilianQueryView> yilianQueryList = await this.yilianQueryService.GetUnCallbackAuthRequestsAsync(DateTime.Now.AddMinutes(-3));
            if (context != null)
            {
                this.Logger.Info(DateTime.Now + " Find UnCallbackAuthRequests count : " + yilianQueryList.Count + " jobkey: " + context.JobDetail.Key);
            }
            await this.yilianQueryService.QueryYilian(yilianQueryList);
            foreach (YilianQueryView yilianQueryView in yilianQueryList)
            {
                this.Logger.Info(DateTime.Now + "try get no callback AuthRequests result , UserId : " + yilianQueryView.UserIdentifier);
            }
        }

        #endregion Public Methods
    }
}