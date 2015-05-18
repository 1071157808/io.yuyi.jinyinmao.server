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
    ///
    /// </summary>
    public class OrderVerifyJob : DefaultJob, IOrderVerifyJob
    {
        #region Private Fields

        /// <summary>
        /// The yilian query service
        /// </summary>
        private readonly IYilianQueryService yilianQueryService;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="YilianAuthJob" /> class.
        /// </summary>
        public OrderVerifyJob()
        {
            yilianQueryService = new YilianQueryService();
            waitTime = new TimeSpan(0, 0, 9, 0);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Jobs the execute.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override async Task JobExecute(IJobExecutionContext context)
        {
            IList<YilianQueryView> yilianQueryList = await yilianQueryService.GetUnPaymentList(DateTime.Now.AddMinutes(-3));
            if (context != null)
            {
                Logger.Info(DateTime.Now + " Find no callback orders， count : " + yilianQueryList.Count + " jobkey: " + context.JobDetail.Key);
            }
            await yilianQueryService.QueryYilianPaymentInfo(yilianQueryList);
            foreach (YilianQueryView yilianQueryView in yilianQueryList)
            {
                Logger.Info(DateTime.Now + " try get no callback order result , OrderId : " + yilianQueryView.OrderIdentifier);
            }
        }

        #endregion Public Methods
    }
}