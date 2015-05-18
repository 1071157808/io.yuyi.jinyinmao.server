// FileInformation: nyanya/Services.WebAPI.V1.nyanya/InvestmentStatisticController.cs
// CreatedTime: 2014/08/11   9:28 AM
// LastUpdatedTime: 2014/08/11   9:28 AM

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Domain.Meow.Models;
using Domain.Scheduler.Services;
using Domain.Scheduler.ViewModels;
using Services.WebAPI.Common.Controller;
using Services.WebAPI.V1.nyanya.Filters;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>收益统计分析</summary>
    [RoutePrefix("Statistic")]
    public class InvestmentStatisticController : ApiControllerBase
    {
        private readonly IInvestmentStatisticService investmentStatisticService;
        private readonly MeowContext meowContext = new MeowContext();

        /// <summary>Initializes a new instance of the <see cref="InvestmentStatisticController" /> class.</summary>
        /// <param name="investmentStatisticService">InvestmentStatisticService</param>
        public InvestmentStatisticController(IInvestmentStatisticService investmentStatisticService)
        {
            this.investmentStatisticService = investmentStatisticService;
        }

        /// <summary>
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/M-GET-Statistic-Investment.jpg"@}个人收益情况分享 - WebView @{/a@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/M-GET-Statistic-Investment-Summary.jpg"@}个人收益概况 - M @{/a@}
        /// </summary>
        /// <returns>
        ///     @{h3@} 当客户有订单时(HasOrders:true)，返回的是他个人的收益情况: @{/h3@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/M-GET-Statistic-Investment.jpg"@}个人收益情况分享 - WebView @{/a@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/M-GET-Statistic-Investment-Summary.jpg"@}个人收益概况- M @{/a@}
        ///     DefeatedPercent: 打败的百分比
        ///     HasOrders: 用户是否已下过订单
        ///     InterestPerSecond: 每秒赚取的利息，单位：元
        ///     TimeForOneCNY: 赚取一元钱所需的时间，单位：秒
        ///     @{br@}
        ///     @{h3@} 当客户没有订单时(HasOrders:false)，只有下列字段有意义，都表示整体的收益情况: @{/h3@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/M-GET-Statistic-Investment2.jpg"@}个人收益情况分享 - WebView @{/a@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/M-GET-Statistic-Investment-Summary2.jpg"@}个人收益概况 - M @{/a@}
        ///     HasOrders: 用户是否已下过订单
        ///     InterestPerSecond: 每秒赚取的利息，单位：元
        ///     TimeForOneCNY: 赚取一元钱所需的时间，单位：秒
        /// </returns>
        [HttpGet]
        [Route("Investment")]
        [TokenAuthorize]
        [ResponseType(typeof(InvestmentStatisticDto))]
        public async Task<IHttpActionResult> GetInvestmentStatistic()
        {
            InvestmentStatisticViewModel investmentStatistic = await this.investmentStatisticService.GetInvestmentStatistic(this.CurrentUser.Identifier);

            InvestmentStatisticDto investmentStatisticDto = investmentStatistic.ToInvestmentStatisticDto();

            return this.Ok(investmentStatisticDto);
        }

        /// <summary>
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-Statistic-Investment-Summary.jpg"@}个人收益概况 - APP @{/a@}
        /// </summary>
        /// <returns>
        ///     @{h3@} 当客户有订单时(HasOrders:true)，返回的是他个人的收益概况: @{/h3@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-Statistic-Investment-Summary.jpg"@}APP @{/a@}
        ///     AccruedEarnings: 当前累计收益
        ///     AppEearningSpeed: 赚钱速度，单位：元/分钟
        ///     DefeatedPercent: 打败的百分比
        ///     EarningsAgain： 距上次又赚了
        ///     EarningsAgainDuration: 距上次查看的时间。int型，粒度到秒
        ///     HasOrders[bool]: 用户是否已下过订单
        ///     HasShown[bool]: 是否是首次展示
        ///     InterestPerSecond: 每秒赚取的利息，单位：元
        ///     @{br@}
        ///     @{h3@} 当客户没有订单时(HasOrders:false)，只有下列字段有意义，都表示整体的收益概况: @{/h3@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-Statistic-Investment-Summary2.jpg"@}APP @{/a@}
        ///     AppEearningSpeed: App使用的赚钱速度，单位：元/分钟 HasOrders[bool]: 用户是否已下过订单
        /// </returns>
        [Route("Investment/Summary")]
        [TokenAuthorize]
        [ResponseType(typeof(InvestmentStatisticSummaryDto))]
        public async Task<IHttpActionResult> GetInvestmentStatisticSummary()
        {
            InvestmentStatisticSummaryViewModel investmentStatisticSummary = await this.investmentStatisticService.GetInvestmentStatisticSummary(this.CurrentUser.Identifier);

            InvestmentStatisticSummaryDto investmentStatisticSummaryDto = investmentStatisticSummary.ToInvestmentStatisticSummaryDto();

            return this.Ok(investmentStatisticSummaryDto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.meowContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}