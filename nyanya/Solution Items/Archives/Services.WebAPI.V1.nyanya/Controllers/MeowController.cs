// FileInformation: nyanya/Services.WebAPI.V1.nyanya/MeowController.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/28   2:50 PM

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Cqrs.Domain.Meow.Services;
using Cqrs.Domain.Meow.Services.DTO;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>
    ///     Meow 的配置接口
    /// </summary>
    [RoutePrefix("Meow")]
    public class MeowController : ApiController
    {
        /// <summary>
        ///     获取首页的统计信息
        /// </summary>
        /// <returns>
        ///     OrderCount[int]: 累计购买订单数量
        ///     ProductCount[int]: 累计还款期数
        ///     OutletCount[int]: 线下网店数量
        /// </returns>
        [Route("IndexStatistics")]
        [ResponseType(typeof(IndexStatistics))]
        public async Task<IHttpActionResult> GetIndexStatistics()
        {
            return this.Ok(await new MeowStatisticsService().GetIndexStatisticsAsync());
        }
    }
}