using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using Cat.Domain.Meow.Services.Interfaces;
using nyanya.Internal.Models;
using Cat.Domain.Meow.Services.DTO;

namespace nyanya.Internal.Controllers
{
    /// <summary>
    ///     CommonController
    /// </summary>
    [RoutePrefix("Common")]
    public class CommonController : ApiControllerBase
    {
        private IStatisticsService statisticsService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommonController" /> class.
        /// </summary>
        /// <param name="statisticsService">The statistics service.</param>
        public CommonController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        /// <summary>
        /// 获取某段时间内的统计信息
        /// </summary>
        /// <param name="intervalTime">
        /// 间隔时间，以分钟计算[多少分钟内的数据 intervalTime=1440]
        /// 例如1440就是一天内的数据， 默认是1440 ， 取值范围【1 - 1440】之间
        /// </param>
        /// <returns>
        /// RegisterUserNum[int]：用户注册成功数
        /// SuccessLoginNum[int]：成功登录数
        /// FailedLoginNum[int]：失败登录数
        /// SuccessBankCardNum[int]：成功绑卡数
        /// FailedBankCardNum[int]：失败绑卡数
        /// SuccessOrderNum[int]：成功订单数
        /// FailedOrderNum[int]：失败订单数（不包含“余额不足”）
        /// OnSaleProductNum[int]：在售产品数
        /// </returns>
        [HttpGet, Route("Statistics"), Route("Statistics/{intervalTime:int=1}")]
        [RangeFilter("intervalTime", 1,1440)]
        [ResponseType(typeof(StatisticsReponse))]
        public async Task<IHttpActionResult> GetStatistics(int intervalTime = 1440)
        {
            intervalTime = intervalTime > 1440 || intervalTime <= 0 ? 1440 : intervalTime;
            DateTime start = DateTime.Now.AddMinutes(-intervalTime);
            StatisticsResult statisticsResult = await statisticsService.GetStatisticsAsync(start, DateTime.Now);
            return this.Ok(statisticsResult.ToStatisticsReponse());
        }
    }
}