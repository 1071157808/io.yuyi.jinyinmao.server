using Domian.DTO;
using Infrastructure.Lib.Extensions;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Cat.Filters;
using nyanya.Cat.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Xingye.Domain.Orders.ReadModels;
using Xingye.Domain.Orders.Services.Interfaces;

namespace nyanya.Cat.Controllers
{
    [RoutePrefix("XYOrders")]
    public class XYOrderInfoController : ApiControllerBase
    {
        private readonly IBAOrderInfoService baOrderInfoService;
        private readonly IOrderInfoService orderInfoService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderInfoController" /> class.
        /// </summary>
        /// <param name="baOrderInfoService">The ba order information service.</param>
        /// <param name="orderInfoService">The order information service.</param>
        public XYOrderInfoController(IBAOrderInfoService baOrderInfoService, IOrderInfoService orderInfoService)
        {
            this.baOrderInfoService = baOrderInfoService;
            this.orderInfoService = orderInfoService;
        }

        /// <summary>
        ///     银票失败订单列表
        /// </summary>
        /// <returns>
        ///     HasNextPage: 是否有下一页
        ///     PageIndex: 页码
        ///     PageSize: 一页节点数量
        ///     TotalCount: 所有的节点数量
        ///     TotalPageCount: 总页数
        ///     Orders:节点列表
        ///     -- ExtraInterest[decimal]: 额外收益
        ///     -- Interest[decimal]: 预期收益
        ///     -- Message[string]: 提示信息
        ///     -- OrderIdentifier[string]: 订单唯一标识
        ///     -- OrderNo[string]: 订单编号
        ///     -- OrderTime[yyyy-MM-ddTHH:mm:ss]: 下单时间
        ///     -- Principal[decimal]: 订单金额
        ///     -- ProductIdentifier[string]: 项目唯一标识
        ///     -- ProductName[string]: 项目名称
        ///     -- ProductNo[string]: 项目编号
        ///     -- ProductNumber[int]: 项目期数
        ///     -- SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息日期
        ///     -- ShareCount[int]: 订单的购买份数
        ///     -- ShowingStatus[int]: 项目状态  10 => 付款中, 20 => 待起息, 30 => 已起息, 40 => 已结息, 50 => 支付失败
        ///     -- TotalAmount[decimal]: 订单的本息总额
        ///     -- UseableItemCount[int]: 可用道具数 => -1:已用道具  0:无可用道具  1~999:有可用道具
        ///     -- ValueDate[yyyy-MM-ddTHH:mm:ss]: 起息日期
        ///     -- Yield[decimal]: 订单的预期收益率
        /// </returns>
        [HttpGet, Route("BA/Failed")]
        [TokenAuthorize]
        [ResponseType(typeof(OrderListResponse))]
        public async Task<IHttpActionResult> GetBAFailedOrders()
        {
            IPaginatedDto<BAOrderInfo> orders = await this.baOrderInfoService.GetFailedOrderInfosAsync(this.CurrentUser.Identifier);

            return this.Ok(new OrderListResponse(orders));
        }

        /// <summary>
        ///     银票成功订单列表
        /// </summary>
        /// <param name="pageIndex">pageIndex(int >= 1): 页码</param>
        /// <param name="sortMode">排序规则 1 => 按下单时间排序，2 => 按结息日期排序</param>
        /// <returns>
        ///     HasNextPage: 是否有下一页
        ///     PageIndex: 页码
        ///     PageSize: 一页节点数量
        ///     TotalCount: 所有的节点数量
        ///     TotalPageCount: 总页数
        ///     Orders:节点列表
        ///     -- ExtraInterest[decimal]: 额外收益
        ///     -- Interest[decimal]: 预期收益
        ///     -- Message[string]: 提示信息
        ///     -- OrderIdentifier[string]: 订单唯一标识
        ///     -- OrderNo[string]: 订单编号
        ///     -- OrderTime[yyyy-MM-ddTHH:mm:ss]: 下单时间
        ///     -- Principal[decimal]: 订单金额
        ///     -- ProductIdentifier[string]: 项目唯一标识
        ///     -- ProductName[string]: 项目名称
        ///     -- ProductNo[string]: 项目编号
        ///     -- ProductNumber[int]: 项目期数
        ///     -- SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息日期
        ///     -- ShareCount[int]: 订单的购买份数
        ///     -- ShowingStatus[int]: 项目状态  10 => 付款中, 20 => 待起息, 30 => 已起息, 40 => 已结息, 50 => 支付失败
        ///     -- TotalAmount[decimal]: 订单的本息总额
        ///     -- UseableItemCount[int]: 可用道具数 => -1:已用道具  0:无可用道具  1~999:有可用道具
        ///     -- ValueDate[yyyy-MM-ddTHH:mm:ss]: 起息日期
        ///     -- Yield[decimal]: 订单的预期收益率
        /// </returns>
        [HttpGet, Route("BA"), Route("BA/{pageIndex:min(1):int=1}/{sortMode:min(1):int=1}")]
        [RangeFilter("pageIndex", 1)]
        [TokenAuthorize]
        [ResponseType(typeof(OrderListResponse))]
        public async Task<IHttpActionResult> GetBAOrders(int pageIndex = 1, int sortMode = 1)
        {
            OrderSortingStrategy sortingStrategy = sortMode == 1 ? OrderSortingStrategy.ByOrderTime : OrderSortingStrategy.BySettleDate;

            IPaginatedDto<BAOrderInfo> orders = await this.baOrderInfoService.GetSuccessfulOrderInfosAsync(this.CurrentUser.Identifier, pageIndex, 10, sortingStrategy);

            return this.Ok(new OrderListResponse(orders));
        }

        /// <summary>
        ///     订单详情
        /// </summary>
        /// <param name="orderIdentifier">订单唯一标识</param>
        /// <returns>
        ///     BankCardCity[string]: 开户行城市全称，如 上海|上海
        ///     BankCardCityName[string]: 开户行城市名
        ///     BankCardNo[string]: 银行卡号
        ///     BankName[string]: 银行名称
        ///     ConsignmentAgreementName[string]: 委托协议名称
        ///     EndorseImageLink[string]: 质押物链接
        ///     EndorseImageThumbnailLink[string]: 质押物缩略图链接
        ///     ExtraInterest[decimal]: 额外收益
        ///     HasResult[bool]: 是否有支付结果
        ///     Interest[decimal]: 预期收益
        ///     InvestorCellphone[string]: 投资人手机号
        ///     InvestorCredential[string]: 证件类型名称，直接显示
        ///     InvestorCredentialNo[string]: 证件编号，已加码，可以直接显示
        ///     InvestorRealName[string]: 投资人真实姓名
        ///     IsPaid[bool]: 是否支付成功,只有当HasResult为true时，该值有意义，推荐使用ShowingStatus字段
        ///     Message[string]: 提示信息
        ///     OrderIdentifier[string]: 订单唯一标识
        ///     OrderNo[string]: 订单编号
        ///     OrderTime[yyyy-MM-ddTHH:mm:ss]: 下单时间
        ///     OrderType[int]: 订单类型 10 => 银票订单, 20 => 商票订单
        ///     Period[int]: 投资周期
        ///     PledgeAgreementName[string]: 质押借款协议名称
        ///     Principal[decimal]: 订单金额
        ///     ProductIdentifier[string]: 项目唯一标识
        ///     ProductName[string]: 项目名称
        ///     ProductNo[string]: 项目编号
        ///     ProductNumber[int]: 项目期数
        ///     ProductUnitPrice[decimal]: 项目的单价，即每份多少钱
        ///     ResultTime[yyyy-MM-ddTHH:mm:ss]: 支付清算时间
        ///     RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日
        ///     SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息日期
        ///     ShareCount[int]: 订单的购买份数
        ///     ShowingStatus[int]: 项目状态  10 => 付款中, 20 => 待起息, 30 => 已起息, 40 => 已结息, 50 => 支付失败
        ///     TotalAmount[decimal]: 订单的本息总额
        ///     UseableItemCount[int]: 可用道具数 => -1:已用道具  0:无可用道具  1~999:有可用道具
        ///     ValueDate[yyyy-MM-ddTHH:mm:ss]: 起息日期
        ///     Yield[decimal]: 订单的预期收益率
        /// </returns>
        [HttpGet, Route("")]
        [ParameterRequire("orderIdentifier")]
        [TokenAuthorize]
        [ResponseType(typeof(OrderInfoResponse))]
        public async Task<IHttpActionResult> GetOrder(string orderIdentifier)
        {
            OrderInfo order = await this.orderInfoService.GetOrderInfoAsync(this.CurrentUser.Identifier, orderIdentifier);

            if (order == null)
            {
                return this.BadRequest("订单不存在");
            }

            return this.Ok(order.ToOrderInfoResponse());
        }

        /// <summary>
        ///     订单委托协议
        /// </summary>
        /// <param name="orderIdentifier">订单唯一标识</param>
        /// <returns>
        ///     Content[string]: 委托协议内容
        /// </returns>
        [HttpGet, Route("ConsignmentAgreement/")]
        [ParameterRequire("orderIdentifier")]
        [TokenAuthorize]
        public async Task<IHttpActionResult> GetOrderConsignmentAgreement(string orderIdentifier)
        {
            string content = await this.orderInfoService.GetConsignmentAgreementAsync(orderIdentifier);

            if (content.IsNullOrWhiteSpace())
            {
                return this.BadRequest("订单不存在");
            }

            return this.Ok(new { Content = content });
        }

        /// <summary>
        ///     订单质押借款协议
        /// </summary>
        /// <param name="orderIdentifier">订单唯一标识</param>
        /// <returns>
        ///     Content[string]: 委托协议内容
        /// </returns>
        [HttpGet, Route("PledgeAgreement/")]
        [ParameterRequire("orderIdentifier")]
        [TokenAuthorize]
        public async Task<IHttpActionResult> GetOrderPledgeAgreement(string orderIdentifier)
        {
            string content = await this.orderInfoService.GetPledgeAgreementAsync(orderIdentifier);

            if (content.IsNullOrWhiteSpace())
            {
                return this.BadRequest("订单不存在");
            }

            return this.Ok(new { Content = content });
        }
    }
}