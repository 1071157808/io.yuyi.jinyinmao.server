// FileInformation: nyanya/nyanya.Meow/TimelineController.cs
// CreatedTime: 2014/09/15   9:34 AM
// LastUpdatedTime: 2014/09/17   10:20 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Cat.Domain.Orders.ReadModels;
using Cat.Domain.Orders.Services.Interfaces;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Cat.Domain.Products.Services.Interfaces;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Meow.Filters;
using nyanya.Meow.Models;

namespace nyanya.Meow.Controllers
{
    /// <summary>
    ///     TimelineController
    /// </summary>
    public class TimelineController : ApiControllerBase
    {
        private readonly IOrderInfoService orderInfoService;
        private readonly IProductInfoService productInfoService;
        private readonly ITimelineInfoService timelineInfoService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TimelineController" /> class.
        /// </summary>
        /// <param name="timelineInfoService">The timeline information service.</param>
        /// <param name="productInfoService">The product information service.</param>
        /// <param name="orderInfoService">The order information service.</param>
        public TimelineController(ITimelineInfoService timelineInfoService, IProductInfoService productInfoService, IOrderInfoService orderInfoService)
        {
            this.timelineInfoService = timelineInfoService;
            this.productInfoService = productInfoService;
            this.orderInfoService = orderInfoService;
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
        ///     -- -- 0, （起始节点）
        ///     -- -- 10,（订单节点）
        ///     -- -- 20,（今天节点）
        ///     -- -- 30,（产品节点）
        ///     -- -- 40 （终止节点）
        ///     -- IsPast: 当前节点是否是过去节点，主要用于产品节点，即Type = 30的时间线节点，判断是否是已还款产品
        ///     -- -- true: 过去节点，主要体现在产品已经还款，时间线已还款节点
        ///     -- -- false: 未来节点，位置在今天节点之后，产品是否还款取决于Status参数
        /// </returns>
        [HttpGet]
        [Route("Check")]
        [TokenAuthorize]
        [RangeFilter("num", 1, 15, 1)]
        [ResponseType(typeof(TimelineResponse))]
        public async Task<IHttpActionResult> CheckTimelineTimestamp(int num, string timestamp)
        {
            string userIdentifier = this.CurrentUser.Identifier;
            Timeline timeline = await this.timelineInfoService.GetTimelineAsync(userIdentifier);
            bool updated = timestamp != timeline.Timestamp;

            // 如果时间线已经更新，就返回更新后的第一页时间线
            if (updated)
            {
                return this.Ok(this.GetCurrentTimelineResponse(timeline, num, userIdentifier));
            }

            return this.Ok(new TimelineResponse { Updated = false });
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
        ///     -- -- 0, （起始节点）
        ///     -- -- 10,（订单节点）
        ///     -- -- 20,（今天节点）
        ///     -- -- 30,（产品节点）
        ///     -- -- 40 （终止节点）
        ///     -- IsPast: 当前节点是否是过去节点，主要用于产品节点，即Type = 30的时间线节点，判断是否是已还款产品
        ///     -- -- true: 过去节点，主要体现在产品已经还款
        ///     -- -- false: 未来节点，位置在今天节点之后，产品是否还款取决于Status参数
        /// </returns>
        [HttpGet]
        [Route("Current")]
        [Route("Current/{count:int:min(1):max(15)}")]
        [TokenAuthorize]
        [RangeFilter("count", 1, 15, 1)]
        [ResponseType(typeof(TimelineResponse))]
        public async Task<IHttpActionResult> Current(int count)
        {
            Timeline timeline = await this.timelineInfoService.GetTimelineAsync(this.CurrentUser.Identifier);
            TimelineResponse timelineResponse = this.GetCurrentTimelineResponse(timeline, count, this.CurrentUser.Identifier);
            return this.Ok(timelineResponse);
        }

        /// <summary>
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/M-GET-TimeLine-OrderInfo-orderNo.jpg"@}时间线订单详情 - M @{/a@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-OrderInfo-orderNo.jpg"@}时间线订单详情 - APP@{/a@}
        /// </summary>
        /// <param name="orderIdentifier">订单编号(string)</param>
        /// <returns>
        ///     ExtraInterest[decimal]: 额外收益
        ///     Interest[decimal]: 预期收益
        ///     OrderIdentifier[string]: 订单唯一标识
        ///     OrderNo[string]: 订单编号
        ///     OrderTime[yyyy-MM-ddTHH:mm:ss]: 下单时间
        ///     OrderType[int]: 订单类型 10 => 银票订单, 20 => 商票订单
        ///     Period[int]: 投资周期
        ///     Principal[decimal]: 订单金额
        ///     ProductIdentifier[string]: 项目唯一标识
        ///     ProductName[string]: 项目名称
        ///     ProductNo[string]: 项目编号
        ///     ProductNumber[int]: 项目期数
        ///     ProductUnitPrice[decimal]: 项目的单价，即每份多少钱
        ///     RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日
        ///     SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息日期
        ///     ShareCount[int]: 订单的购买份数
        ///     ShowingStatus[int]: 项目状态  10 => 付款中, 20 => 待起息, 30 => 已起息, 40 => 已结息, 50 => 支付失败
        ///     TotalAmount[decimal]: 订单的本息总额
        ///     UseableItemCount[int]: 可用道具数 => -1:已用道具  0:无可用道具  1~999:有可用道具
        ///     ValueDate[yyyy-MM-ddTHH:mm:ss]: 起息日期
        ///     Yield[decimal]: 订单的预期收益率
        /// </returns>
        [HttpGet]
        [Route("Order")]
        [TokenAuthorize]
        [ParameterRequire("orderIdentifier", 1)]
        [ResponseType(typeof(TimelineOrderNodeResponse))]
        public async Task<IHttpActionResult> GetOrderInfo(string orderIdentifier)
        {
            OrderInfo order = await this.orderInfoService.GetOrderInfoAsync(CurrentUser.Identifier, orderIdentifier);

            if (order == null)
            {
                return this.CanNotFound();
            }
            return this.Ok(order.ToTimelineOrderNodeResponse());
        }

        /// <summary>
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/M-GET-TimeLine-RefundInfo-productIdentifier.jpg"@} 时间线还款日详情 - M @{/a@}
        ///     @{a href="http://mdev.jinyinmao.com.cn/public/dev/APP-GET-TimeLine-RefundInfo-productIdentifier.jpg"@} 时间线还款日详情 - APP@{/a@}
        /// </summary>
        /// <param name="productIdentifier">产品编号(string)</param>
        /// <returns>
        ///     Product: 产品信息
        ///     -- AvailableShareCount[int]: 可购买份额
        ///     -- CurrentValueDate[yyyy-MM-ddTHH:mm:ss]: 计算出的当时购买的起息日期
        ///     -- EndorseImageLink[string]: 抵押物大图链接
        ///     -- EndorseImageThumbnailLink[string]: 抵押物缩略图链接
        ///     -- EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     -- EnterpriseName[string]: 企业名称
        ///     -- ExtraYield[deciaml]: 额外收益率
        ///     -- FinancingSum[int]: 融资金额
        ///     -- FinancingSumCount[int]: 融资总份数,使用该字段显示产品信息
        ///     -- LaunchTime[yyyy-MM-ddTHH:mm:ss]: 产品上线时间
        ///     -- MaxShareCount[int]: 单笔订单最大购买份数
        ///     -- MinShareCount[int]: 单笔订单最小购买份数
        ///     -- OnPreSale[bool]: 是否在预售状态
        ///     -- OnSale[bool]: 是否在开售状态
        ///     -- PaidPercent[int]: 销售百分比
        ///     -- PaidShareCount[int]: 已付款份数
        ///     -- PayingShareCount[int]: 付款中份数
        ///     -- Period[int]: 项目周期
        ///     -- PreEndSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买结束时间
        ///     -- PreSale[bool]: 是否有提前购买设置
        ///     -- PreStartSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买开始时间
        ///     -- ProductIdentifier[string]: 产品唯一标识符
        ///     -- ProductName[string]: 产品名称
        ///     -- ProductNo[string]: 产品编号
        ///     -- ProductNumber[int]: 产品期数
        ///     -- Repaid[bool]: 是否已经还款
        ///     -- RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日期
        ///     -- ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     -- SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息时间
        ///     -- ShowingStatus[int]: 显示状态 10 => 待售, 20 => 预售, 30 => 在售, 40 => 售罄, 50 => 结束
        ///     -- SoldOut[bool]: 是否售罄
        ///     -- SoldOutTime[yyyy-MM-ddTHH:mm:ss|""]: 售罄时间
        ///     -- StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     -- SumShareCount[int]: 总销售份数,计算使用,前端计算暂时不会涉及到
        ///     -- UnitPrice[int]: 每一份的单价
        ///     -- ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     -- ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 下一日起息, 30 => 指定日期起息（此时ValueDate一定有值）
        ///     -- ValueDateString[string]: 备选的起息日显示文案
        ///     -- Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        ///     Orders: 订单列表
        ///     -- Interest: 收益
        ///     -- OrderIdentifier: 订单唯一标识
        ///     -- OrderNo: 订单号
        ///     -- OrderTime: 下单时间
        ///     -- Principal: 订单本金
        /// </returns>
        [HttpGet]
        [Route("Refund")]
        [TokenAuthorize]
        [ParameterRequire("productIdentifier", 1)]
        [ResponseType(typeof(TimelineRefundNodeResponse))]
        public async Task<IHttpActionResult> GetRefundInfo(string productIdentifier)
        {
            ProductWithSaleInfo<ProductInfo> product = await this.productInfoService.GetProductWithSaleInfoByIdentifierAsync(productIdentifier);
            if (product == null)
            {
                return this.CanNotFound();
            }

            IList<OrderInfo> orders = await this.orderInfoService.GetTheUserOrders(this.CurrentUser.Identifier, productIdentifier);

            return this.Ok(new TimelineRefundNodeResponse { Product = product.ToProductInfoResponse(), Orders = orders.Select(o => o.ToRefundInfoOrderDto()) });
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
        ///     -- -- 0, （起始节点）
        ///     -- -- 10,（订单节点）
        ///     -- -- 20,（今天节点）
        ///     -- -- 30,（产品节点）
        ///     -- -- 40 （终止节点）
        ///     -- IsPast: 当时节点是否是过去节点，主要用于产品节点，即Type = 30的时间线节点，判断是否是已还款产品
        ///     -- -- true: 过去节点，主要体现在产品已经还款
        ///     -- -- false: 未来节点，位置在今天节点之后，产品是否还款取决于Status参数
        /// </returns>
        [HttpGet]
        [Route("Items")]
        [TokenAuthorize]
        [RangeFilter("num", 1, 15, 1)]
        public async Task<IHttpActionResult> Items(int start, int num, string timestamp)
        {
            string userIdentifier = this.CurrentUser.Identifier;
            Timeline timeline = await this.timelineInfoService.GetTimelineAsync(userIdentifier);
            bool updated = timestamp != timeline.Timestamp;

            // 如果时间线已经更新，就返回更新后的第一页时间线
            if (updated)
            {
                return this.Ok(this.GetCurrentTimelineResponse(timeline, num, userIdentifier));
            }

            bool needFuture = start >= 0;

            List<TimelineNode> timelineNodes;

            if (needFuture)
            {
                timelineNodes = timeline.Nodes.Where(n => n.Time > DateTime.Today && n.Type == TimelineNodeType.Product)
                    .OrderBy(n => n.Time).Skip(start - 1).Take(num).ToList();
            }
            else
            {
                timelineNodes = timeline.Nodes.Where(n => n.Time > DateTime.Today && n.Type == TimelineNodeType.Product)
                    .OrderByDescending(n => n.Time).Skip(-start - 1).Take(num).ToList();
            }

            TimelineResponse timelineResponse = new TimelineResponse
            {
                Items = timelineNodes.Select(n => n.ToTimelineNodeDto()),
                Updated = true,
                Timestamp = timeline.Timestamp
            };
            return this.Ok(timelineResponse);
        }

        private TimelineResponse GetCurrentTimelineResponse(Timeline timeline, int count, string userIdentifier)
        {
            List<TimelineNode> timelineNodes = new List<TimelineNode>();

            // 历史节点
            List<TimelineNode> pastNodes = new List<TimelineNode>();

            // 今日节点
            TimelineNode todayNode = timeline.GetTodayNode();

            // 未来节点
            IList<TimelineNode> futureNodes = timeline.Nodes.Where(n => n.Time > DateTime.Today && n.Type == TimelineNodeType.Product)
                .OrderBy(n => n.Time).Take(count - 2).ToList();

            // 未来节点足够 (未来节点 + 今日节点 + 一个过去节点(有可能就是起始节点))
            if (futureNodes.Count == count - 2)
            {
                // 过去节点,如果没有过去节点，就使用起始节点
                TimelineNode pastNode = timeline.Nodes.Where(n => n.Time < DateTime.Today).OrderByDescending(n => n.Time).First();
                pastNodes.Add(pastNode);
            }

            // 未来节点缺一个，需要结束节点补充 (未来节点 + 结束节点 + 今日节点 + 一个过去节点(有可能就是起始节点))
            if (futureNodes.Count == count - 3)
            {
                // 结束节点
                futureNodes.Add(timeline.GetEndNode());

                // 过去节点,如果没有过去节点，就使用起始节点
                TimelineNode pastNode = timeline.Nodes.Where(n => n.Time < DateTime.Today).OrderByDescending(n => n.Time).First();
                pastNodes.Add(pastNode);
            }

            // 未来节点 + 结束节点依旧不够，需要结束节点 + 过去节点(可能包括起始节点)补充 (未来节点 + 结束节点 + 今日节点 + 过去节点(可能包括起始节点))
            if (futureNodes.Count < count - 3)
            {
                // 结束节点
                futureNodes.Add(timeline.GetEndNode());

                // 历史节点
                int pastCount = count - futureNodes.Count - 1 - 1;
                pastNodes.AddRange(timeline.Nodes.Where(n => n.Time < DateTime.Today).OrderByDescending(n => n.Time).Take(pastCount));
            }

            timelineNodes.AddRange(pastNodes);
            timelineNodes.Add(todayNode);
            timelineNodes.AddRange(futureNodes);

            TimelineResponse timelineResponse = new TimelineResponse
            {
                Items = timelineNodes.OrderBy(n => n.Time).Select(n => n.ToTimelineNodeDto()),
                Updated = true,
                Timestamp = timeline.Timestamp
            };
            return timelineResponse;
        }
    }
}