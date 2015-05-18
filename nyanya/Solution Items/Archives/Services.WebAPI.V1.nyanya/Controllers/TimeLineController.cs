// FileInformation: nyanya/Services.WebAPI.V1.nyanya/TimeLineController.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/07/16   11:19 PM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Domain.Amp.Models;
using Domain.Meow.Models;
using Domain.Order.Models;
using Domain.Order.Services;
using Domian.Passport.Services;
using Services.WebAPI.Common.Controller;
using Services.WebAPI.Common.Filters;
using Services.WebAPI.V1.nyanya.App_Start;
using Services.WebAPI.V1.nyanya.Filters;
using Services.WebAPI.V1.nyanya.Models.Order;
using Services.WebAPI.V1.nyanya.Models.Passport.User;

namespace Services.WebAPI.V1.nyanya.Controllers.Order
{
    /// <summary>
    ///     时间线
    /// </summary>
    [RoutePrefix("TimeLine")]
    public class TimeLineController : ApiControllerBase
    {
        private readonly AmpContext ampContext = new AmpContext();
        private readonly MeowContext meowContext = new MeowContext();
        private readonly OrderContext orderContext = new OrderContext();
        private readonly ITimelineService timelineService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TimeLineController" /> class.
        /// </summary>
        /// <param name="timelineService">The timeline service.</param>
        public TimeLineController(ITimelineService timelineService)
        {
            this.timelineService = timelineService;
        }

        /// <summary>
        ///     时间线节点
        /// </summary>
        /// <param name="num">时间节点数量（1-15）</param>
        /// <param name="timestamp"> 时间线时间戳 </param>
        /// <returns>
        ///     Updated[bool]: true => 时间线已经更新 false => 时间线没有更新
        ///     Timestamp: 时间线时间戳，string类型，格式为yyyyMMddHHmm_s  (如果Updated => false,该值无意义)
        ///     Items: 节点列表 (如果Updated => false,该值无意义)
        ///     -- Identifier: 节点唯一标识，主要用于传递参数
        ///     -- Interest: 利息，收益，预期收益
        ///     -- Principal: 本金，投资金额
        ///     -- Time: 节点时间
        ///     -- Type: 节点类型
        ///     -- -- 0, （起始节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Beginning.jpg"@}时间线起始节点@{/a@}
        ///     -- -- 10,（订单节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Order.jpg"@}时间线订单节点@{/a@}
        ///     -- -- 20,（今天节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Today.jpg"@}时间线今天节点@{/a@}
        ///     -- -- 30,（产品节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Product.jpg"@}时间线产品节点@{/a@}
        ///     -- -- 40 （终止节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-End.jpg"@}时间线终止节点@{/a@}
        ///     -- IsPast: 当前节点是否是过去节点，主要用于产品节点，即Type = 30的时间线节点，判断是否是已还款产品
        ///     -- -- true: 过去节点，主要体现在产品已经还款 @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Paid.jpg"@}时间线已还款节点@{/a@}
        ///     -- -- false: 未来节点，位置在今天节点之后，产品是否还款取决于Status参数
        ///     -- Status: 结点的状态，现在只用于判断产品是否结息，即只有Type = 30并且IsPast = false的时间线节点需要用到该字段
        ///     -- -- 50: 产品已结息 @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Prepayment.jpg"@}时间线提前还款节点@{/a@}
        ///     -- -- others: 产品未结息 @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Unpaid.jpg"@}时间线未还款节点@{/a@}
        /// </returns>
        [HttpGet]
        [Route("Check")]
        [TokenAuthorize]
        [RangeFilter("num", 1, 15, 1)]
        [ResponseType(typeof(TimelineDto))]
        public async Task<IHttpActionResult> CheckTimelineTimestamp(int num, string timestamp)
        {
            //var currentUser = this.CurrentUser;

            //bool updated = (!String.IsNullOrEmpty(timestamp)) /* 兼容原接口，没有此参数，就认为忽略更新机制 */&& timestamp != await this.timelineService.GetTimelineTimestamp(currentUser.Identifier);

            //// 如果时间线已经更新，就返回更新后的第一页时间线
            //if (updated)
            //{
            //    return this.Ok(await this.GetCurrentTimelineDto(num, currentUser));
            //}

            //return this.Ok(new TimelineDto { Updated = false });
            throw new NotImplementedException();
        }

        /// <summary>
        ///     时间线今日的情况，即是时间线第一页应该显示的节点
        /// </summary>
        /// <param name="count">时间线节点数量（1-15）</param>
        /// <returns>
        ///     Updated[bool]: true => 时间线已经更新 false => 时间线没有更新
        ///     Timestamp: 时间线时间戳，string类型，格式为yyyyMMddHHmm_s
        ///     Items: 节点列表
        ///     -- Identifier: 节点唯一标识，主要用于传递参数
        ///     -- Interest: 利息，收益，预期收益
        ///     -- Principal: 本金，投资金额
        ///     -- Time: 节点时间
        ///     -- Type: 节点类型
        ///     -- -- 0, （起始节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Beginning.jpg"@}时间线起始节点@{/a@}
        ///     -- -- 10,（订单节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Order.jpg"@}时间线订单节点@{/a@}
        ///     -- -- 20,（今天节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Today.jpg"@}时间线今天节点@{/a@}
        ///     -- -- 30,（产品节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Product.jpg"@}时间线产品节点@{/a@}
        ///     -- -- 40 （终止节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-End.jpg"@}时间线终止节点@{/a@}
        ///     -- IsPast: 当前节点是否是过去节点，主要用于产品节点，即Type = 30的时间线节点，判断是否是已还款产品
        ///     -- -- true: 过去节点，主要体现在产品已经还款 @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Paid.jpg"@}时间线已还款节点@{/a@}
        ///     -- -- false: 未来节点，位置在今天节点之后，产品是否还款取决于Status参数
        ///     -- Status: 结点的状态，现在只用于判断产品是否结息，即只有Type = 30并且IsPast = false的时间线节点需要用到该字段
        ///     -- -- 50: 产品已结息 @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Prepayment.jpg"@}时间线提前还款节点@{/a@}
        ///     -- -- others: 产品未结息 @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Unpaid.jpg"@}时间线未还款节点@{/a@}
        /// </returns>
        [HttpGet]
        [Route("Current")]
        [Route("Current/{count:int:min(1):max(15)}")]
        [TokenAuthorize]
        [RangeFilter("count", 1, 15, 1)]
        [ResponseType(typeof(TimelineDto))]
        public async Task<IHttpActionResult> Current(int count)
        {
            //CurrentUser currentUser = this.GetCurrentUser();

            //TimelineDto timelineDto = await this.GetCurrentTimelineDto(count, currentUser);

            //return this.Ok(timelineDto);
            throw new NotImplementedException();
        }

        /// <summary>
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/M-GET-TimeLine-OrderInfo-orderNo.jpg"@}时间线订单详情 - M @{/a@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-OrderInfo-orderNo.jpg"@}时间线订单详情 - APP@{/a@}
        /// </summary>
        /// <param name="orderNo">订单编号(string)</param>
        /// <returns>
        ///     Order: 订单信息
        ///     -- CreatedAt: 下单时间
        ///     -- ExpectedPrice: 预期收益
        ///     -- OrderId: 订单Id
        ///     -- OrderNo: 订单号
        ///     -- Price: 购买额度
        ///     -- UseableItemCount: 可用道具数量  -1 => 已使用, 0 => 未使用道具，也无合适道具使用, (n > 1) => 有n个道具可用
        ///     -- ItemTitle: 如果已经使用了道具 => 道具名称，否则为空
        ///     -- ExtraInterest: 如果已经使用了道具 => 道具增收，否则为零
        ///     Product: 产品信息
        ///     -- BankName: 银行名称
        ///     -- DueDate: 最迟还款日
        ///     -- Duration: 理财周期
        ///     -- ExtraYield: 额外收益率
        ///     -- FundedPercentage[int]: 募集进度
        ///     -- IsRecommand[bool]: 推荐：1 => 推荐；0 => 不推荐
        ///     -- MaxNumber: 最大投资份数
        ///     -- MinNumber: 最小投资份数
        ///     -- Name: 产品名称
        ///     -- ProductIdentifier: 产品标识符
        ///     -- PubBegin: 发售开始时间
        ///     -- PubEnd: 发售结束时间
        ///     -- SellingStatus: 销售状态
        ///     -- SettleDay: 结息日
        ///     -- TotalNumber: 总销售额
        ///     -- Unit: 投资单位
        ///     -- ValueDay: 起息日
        ///     -- Yield: 收益率
        ///     -- Id: 产品Id
        /// </returns>
        [HttpGet]
        [Route("OrderInfo")]
        [Route("OrderInfo/{orderNo}")]
        [TokenAuthorize]
        [ParameterRequire("orderNo", 1)]
        [ResponseType(typeof(TimeLineOrderInfoDto))]
        public async Task<IHttpActionResult> GetOrderInfo(string orderNo)
        {
            //CurrentUser currentUser = this.GetCurrentUser();

            //OrderSummary orderSummary = await this.orderContext.OrderSummaries.AsNoTracking().FirstOrDefaultAsync(o => o.UserGuid == currentUser.Identifier && o.OrderNo == orderNo);
            //if (orderSummary == null)
            //{
            //    return this.NotFound();
            //}

            ////HACK:订单临时代码
            //OHPItem item = await this.meowContext.OHPItems.AsNoTracking().Include(i => i.Category).FirstOrDefaultAsync(i => i.OrderId == orderSummary.OrderId);

            //OrderWithItemDto orderWithItemDto = orderSummary.ToOrderWithItemDto(item);

            //Product product = await this.ampContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductIdentifier == orderSummary.ProductIdentifer);
            //SummaryProductDto summaryProductDto = product.ToSummaryProductDto();

            //return this.Ok(new TimeLineOrderInfoDto { Order = orderWithItemDto, Product = summaryProductDto });
            throw new NotImplementedException();
        }

        /// <summary>
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/M-GET-TimeLine-RefundInfo-productIdentifier.jpg"@} 时间线还款日详情 - M @{/a@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-RefundInfo-productIdentifier.jpg"@} 时间线还款日详情 - APP@{/a@}
        /// </summary>
        /// <param name="productIdentifier">产品编号(string)</param>
        /// <returns>
        ///     Product: 产品信息
        ///     -- BankName: 银行名称
        ///     -- DueDate: 最迟还款日
        ///     -- Duration: 理财周期
        ///     -- ExtraYield: 额外收益率
        ///     -- FundedPercentage[int]: 募集进度
        ///     -- IsRecommand[bool]: 推荐：1 => 推荐；0 => 不推荐
        ///     -- MaxNumber: 最大投资份数
        ///     -- MinNumber: 最小投资份数
        ///     -- Name: 产品名称
        ///     -- ProductIdentifier: 产品标识符
        ///     -- PubBegin: 发售开始时间
        ///     -- PubEnd: 发售结束时间
        ///     -- SellingStatus: 销售状态
        ///     -- SettleDay: 结息日
        ///     -- TotalNumber: 总销售额
        ///     -- Unit: 投资单位
        ///     -- ValueDay: 起息日
        ///     -- Yield: 收益率
        ///     -- Id: 产品Id
        ///     Orders: 订单列表
        ///     -- CreatedAt: 下单时间
        ///     -- ExpectedPrice: 预期收益
        ///     -- OrderNo: 订单号
        ///     -- Price: 购买额度
        ///     -- ExtraInterest: 如果已经使用了道具 => 道具增收，否则为零
        ///     Now: 系统当前时间，格式yyyyMMddTHHmmss
        /// </returns>
        [HttpGet]
        [Route("RefundInfo")]
        [Route("RefundInfo/{productIdentifier}")]
        [TokenAuthorize]
        [ParameterRequire("productIdentifier", 1)]
        [ResponseType(typeof(RefundInfoDto))]
        public async Task<IHttpActionResult> GetRefundInfo(string productIdentifier)
        {
            //CurrentUser currentUser = this.GetCurrentUser();

            //Product product = await this.ampContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductIdentifier == productIdentifier);
            //if (product == null)
            //{
            //    return this.NotFound();
            //}

            //SummaryProductDto summaryProductDto = product.ToSummaryProductDto();

            //List<int> orderIds = await this.orderContext.OrderSummaries.AsNoTracking().Where(o => o.UserGuid == currentUser.Identifier && o.ProductIdentifer == productIdentifier).Select(o => o.OrderId).ToListAsync();
            //List<OrderListItem> orders = await this.orderContext.OrderListItems.AsNoTracking().Where(o => orderIds.Contains(o.Id)).ToListAsync();

            //List<OrderSummaryDto> orderDtos = orders.Select(o => o.ToOrderSummaryDto()).ToList();

            //return this.Ok(new RefundInfoDto { Product = summaryProductDto, Orders = orderDtos });
            throw new NotImplementedException();
        }

        /// <summary>
        ///     时间线节点
        /// </summary>
        /// <param name="start">时间节点开始位置(负数即寻找过去节点，正数即寻找未来节点)</param>
        /// <param name="num">时间节点数量（1-15）</param>
        /// <param name="timestamp"> 时间线时间戳 </param>
        /// <returns>
        ///     Updated[bool]: true => 时间线已经更新 false => 时间线没有更新
        ///     Timestamp: 时间线时间戳，string类型，格式为yyyyMMddHHmm_s
        ///     Items: 节点列表
        ///     -- Identifier: 节点唯一标识，主要用于传递参数
        ///     -- Interest: 利息，收益，预期收益
        ///     -- Principal: 本金，投资金额
        ///     -- Time: 节点时间
        ///     -- Type: 节点类型
        ///     -- -- 0, （起始节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Beginning.jpg"@}时间线起始节点@{/a@}
        ///     -- -- 10,（订单节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Order.jpg"@}时间线订单节点@{/a@}
        ///     -- -- 20,（今天节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Today.jpg"@}时间线今天节点@{/a@}
        ///     -- -- 30,（产品节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Product.jpg"@}时间线产品节点@{/a@}
        ///     -- -- 40 （终止节点）@{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-End.jpg"@}时间线终止节点@{/a@}
        ///     -- IsPast: 当时节点是否是过去节点，主要用于产品节点，即Type = 30的时间线节点，判断是否是已还款产品
        ///     -- -- true: 过去节点，主要体现在产品已经还款 @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Paid.jpg"@}时间线已还款节点@{/a@}
        ///     -- -- false: 未来节点，位置在今天节点之后，产品是否还款取决于Status参数
        ///     -- Status: 结点的状态，现在只用于判断产品是否结息，即只有Type = 30并且IsPast = false的时间线节点需要用到该字段
        ///     -- -- 50: 产品已结息 @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Prepayment.jpg"@}时间线提前还款节点@{/a@}
        ///     -- -- others: 产品未结息 @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-Unpaid.jpg"@}时间线未还款节点@{/a@}
        /// </returns>
        [HttpGet]
        [Route("Items")]
        [TokenAuthorize]
        [RangeFilter("num", 1, 15, 1)]
        public async Task<IHttpActionResult> Items(int start, int num, string timestamp)
        {
            //CurrentUser currentUser = this.GetCurrentUser();

            //bool updated = (!String.IsNullOrEmpty(timestamp)) /* 兼容原接口，没有此参数，就认为忽略更新机制 */&& timestamp != await this.timelineService.GetTimelineTimestamp(currentUser.Identifier);

            //// 如果时间线已经更新，就返回更新后的第一页时间线
            //if (updated)
            //{
            //    return this.Ok(await this.GetCurrentTimelineDto(num, currentUser));
            //}

            //bool needFuture = start >= 0;

            //Timeline timeline;
            //List<TimelineItem> timelineItems;

            //if (needFuture)
            //{
            //    timeline = await this.timelineService.GetFutureItemsAsync(currentUser.Identifier, start, num);
            //    timelineItems = timeline.Items;

            //    if (timelineItems.Count < num) timelineItems.Add(TimelineItem.GetEndItem(ApplicationConfig.AppSettings.GetTimelineEndItemMessage()));
            //}
            //else
            //{
            //    timeline = await this.timelineService.GetPastItemsAsync(currentUser.Identifier, -start, num);
            //    timelineItems = timeline.Items;
            //    timelineItems.Reverse();

            //    if (timelineItems.Count < num) timelineItems.Insert(0, TimelineItem.GetBeginningItem(currentUser.SignUpTime));
            //}

            //return this.Ok(new TimelineDto { Items = timelineItems.Select(timelineItem => timelineItem.ToTimelineItemDto()), Timestamp = timeline.Timestamp, Updated = false });
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ampContext.Dispose();
                this.meowContext.Dispose();
                this.orderContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task<TimelineDto> GetCurrentTimelineDto(int count, CurrentUser currentUser)
        {
            List<TimelineItem> timelineItems = new List<TimelineItem>();
            List<TimelineItem> pastItems = new List<TimelineItem>();
            TimelineItem todayItem = await TimelineItem.GetTodayItem(currentUser.Guid);

            // 未来节点
            Timeline futureTimeline = await this.timelineService.GetFutureItemsAsync(currentUser.Guid, 0, count - 2);
            List<TimelineItem> futureItems = futureTimeline.Items;

            // 未来节点足够 (未来节点 + 今日节点 + 一个过去节点(有可能就是起始节点))
            if (futureItems.Count == count - 2)
            {
                // 过去节点
                TimelineItem pastItem = (await this.timelineService.GetPastItemsAsync(currentUser.Guid, 0, 1)).Items.FirstOrDefault();

                // 起始节点: 如果没有过去节点，就使用起始节点
                pastItems.Add(pastItem ?? TimelineItem.GetBeginningItem(currentUser.SignUpTime));
            }

            // 未来节点缺一个，需要结束节点补充 (未来节点 + 结束节点 + 今日节点 + 一个过去节点(有可能就是起始节点))
            if (futureItems.Count == count - 3)
            {
                // 结束节点
                futureItems.Add(TimelineItem.GetEndItem(ApplicationConfig.AppSettings.GetTimelineEndItemMessage()));

                // 过去节点
                TimelineItem pastItem = (await this.timelineService.GetPastItemsAsync(currentUser.Guid, 0, 1)).Items.FirstOrDefault();

                // 起始节点: 如果没有过去节点，就使用起始节点
                pastItems.Add(pastItem ?? TimelineItem.GetBeginningItem(currentUser.SignUpTime));
            }

            // 未来节点 + 结束节点依旧不够，需要结束节点 + 过去节点(可能包括起始节点)补充 (未来节点 + 结束节点 + 今日节点 + 过去节点(可能包括起始节点))
            if (futureItems.Count < count - 3)
            {
                // 结束节点
                futureItems.Add(TimelineItem.GetEndItem(ApplicationConfig.AppSettings.GetTimelineEndItemMessage()));

                // 过去节点
                int pastCount = count - futureItems.Count - 1 - 1;
                pastItems = (await this.timelineService.GetPastItemsAsync(currentUser.Guid, 0, pastCount)).Items;

                pastItems.Reverse();

                // 起始节点: 如果过去节点数量不足，就添加起始节点
                if (pastItems.Count < pastCount)
                {
                    pastItems.Insert(0, TimelineItem.GetBeginningItem(currentUser.SignUpTime));
                }
            }

            timelineItems.AddRange(pastItems);
            timelineItems.Add(todayItem);
            timelineItems.AddRange(futureItems);

            TimelineDto timelineDto = new TimelineDto { Items = timelineItems.Select(timelineItem => timelineItem.ToTimelineItemDto()), Updated = true, Timestamp = futureTimeline.Timestamp };
            return timelineDto;
        }
    }
}