// FileInformation: nyanya/QuartzNet/SchedulerConfig.cs
// CreatedTime: 2014/08/12   11:47 AM
// LastUpdatedTime: 2014/08/18   1:14 AM

using System;
using System.Collections.Specialized;
using Infrastructure.Lib.Logs;
using Infrastructure.Lib.Logs.Implementation;
using Infrastructure.SMS;
using Quartz;
using QuartzNet.Jobs;
using QuartzNet.Schdulers;

namespace QuartzNet
{
    /// <summary>
    /// </summary>
    public static class SchedulerConfig
    {
        #region Public Fields

        /// <summary>
        ///     The job logger
        /// </summary>
        public static readonly ILogger JobLogger = new NLogger("SchedulerLogger");

        /// <summary>
        ///     The set sale out schduler MGR
        /// </summary>
        public static readonly DefaultSchdulerMgr SetSaleOutSchdulerMgr = CreateSaleOutSchedulerMgr();

        /// <summary>
        ///     The set yilian authentication schduler MGR
        /// </summary>
        public static readonly DefaultSchdulerMgr SetYilianAuthSchdulerMgr = CreateYilianAuthSchdulerMgr();

        /// <summary>
        ///     The set yilian order schduler MGR
        /// </summary>
        public static readonly DefaultSchdulerMgr SetYilianOrderSchdulerMgr = CreateYilianOrderSchdulerMgr();

        /// <summary>
        ///     The SMS service
        /// </summary>
        public static readonly ISmsAlertsService SmsService = new SmsService();

        #endregion Public Fields

        #region Public Methods

        /// <summary>
        ///     Creates the sale out scheduler MGR.
        /// </summary>
        /// <returns></returns>
        public static DefaultSchdulerMgr CreateSaleOutSchedulerMgr()
        {
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "MonitorProductSaleOut";


//            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
//            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz";
//            properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
//            properties["quartz.jobStore.dataSource"] = "myDS";
//            properties["quartz.dataSource.myDS.connectionString"] = "Server=fe_auth.db.dev.ad.jinyinmao.com.cn;Database=EF_QUARTZ;Uid=User_nyanya;Pwd=Password01!";
//            properties["quartz.dataSource.myDS.provider"] = "SqlServer-20";
//            properties["quartz.jobStore.useProperties"] = "true";
            // set thread pool info
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "1";

            //
            properties["quartz.jobStore.misfireThreshold"] = "300000";

            DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);

            IJobDetail job = JobBuilder.Create<ProductSaleOutJob>().WithIdentity("saleOutJob", "saleOutGroup").Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("saleTrigger", "saleOutGroup").StartAt(runTime).WithSimpleSchedule(x => x.WithInterval(new TimeSpan(0, 0, 1, 0, 0))
                .RepeatForever().WithMisfireHandlingInstructionNextWithRemainingCount()).Build();
            return new DefaultSchdulerMgr(properties, job, trigger);
        }

        /// <summary>
        ///     Creates the yilian authentication schduler MGR.
        /// </summary>
        /// <returns></returns>
        public static DefaultSchdulerMgr CreateYilianAuthSchdulerMgr()
        {
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "MonitorYilianAuthCallback";

//            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
//            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz";
//            properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
//            properties["quartz.jobStore.dataSource"] = "myDS";
//            properties["quartz.dataSource.myDS.connectionString"] = "Server=fe_auth.db.dev.ad.jinyinmao.com.cn;Database=EF_QUARTZ;Uid=User_nyanya;Pwd=Password01!";
//            properties["quartz.dataSource.myDS.provider"] = "SqlServer-20";
////            properties["quartz.jobStore.useProperties"] = "true";
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "1";

            //
            properties["quartz.jobStore.misfireThreshold"] = "150000";

            DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);
            IJobDetail job = JobBuilder.Create<YilianAuthJob>().WithIdentity("yilianAuthJob", "yilianAuthGroup").Build();

            ITrigger trigger = TriggerBuilder.Create().WithIdentity("yilianAuthTrigger", "yilianAuthGroup").StartAt(runTime).WithSimpleSchedule(x => x.WithInterval(new TimeSpan(0, 0, 0, 30, 0))
                .RepeatForever().WithMisfireHandlingInstructionNextWithRemainingCount()).Build();
            return new DefaultSchdulerMgr(properties, job, trigger);
        }

        /// <summary>
        ///     Creates the yilian order schduler MGR.
        /// </summary>
        /// <returns></returns>
        public static DefaultSchdulerMgr CreateYilianOrderSchdulerMgr()
        {
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "MonitorYilianOrderCallback";
//            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
//            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz";
//            properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
//            properties["quartz.jobStore.dataSource"] = "myDS";
//            // set thread pool info
//            properties["quartz.dataSource.myDS.connectionString"] = "Server=fe_auth.db.dev.ad.jinyinmao.com.cn;Database=EF_QUARTZ;Uid=User_nyanya;Pwd=Password01!";
//            properties["quartz.dataSource.myDS.provider"] = "SqlServer-20";
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "1";

            //
            properties["quartz.jobStore.misfireThreshold"] = "150000";

            DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);
            IJobDetail job = JobBuilder.Create<OrderVerifyJob>().WithIdentity("yilianOrderJob", "yilianOrderGroup").Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("yilianOrderTrigger", "yilianOrderGroup").StartAt(runTime).WithSimpleSchedule(x => x.WithInterval(new TimeSpan(0, 0, 0, 30, 0))
                .RepeatForever().WithMisfireHandlingInstructionNextWithRemainingCount()).Build();
            return new DefaultSchdulerMgr(properties, job, trigger);
        }

        /// <summary>
        ///     Registers the scheduler.
        /// </summary>
        public static void RegisterScheduler()
        {
            SetSaleOutSchdulerMgr.Start();
            SetYilianAuthSchdulerMgr.Start();
            SetYilianOrderSchdulerMgr.Start();
        }

        #endregion Public Methods
    }
}