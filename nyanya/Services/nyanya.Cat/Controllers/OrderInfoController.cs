// ***********************************************************************
// Project          : nyanya
// Author           : Siqi Lu
// Created          : 2015-05-18  2:54 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  4:57 PM
// ***********************************************************************
// <copyright file="OrderInfoController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Cat.Commands.Orders;
using Cat.Commands.Products;
using Cat.Domain.Orders.Models;
using Cat.Domain.Orders.ReadModels;
using Cat.Domain.Orders.Services.DTO;
using Cat.Domain.Orders.Services.Interfaces;
using Cat.Domain.Products.Models;
using Cat.Domain.Products.Services.Interfaces;
using Cat.Domain.Users.ReadModels;
using Cat.Domain.Users.Services.DTO;
using Cat.Domain.Users.Services.Interfaces;
using Domian.Bus;
using Domian.DTO;
using Infrastructure.Lib.Extensions;
using Newtonsoft.Json;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Cat.Filters;
using nyanya.Cat.Models;
using nyanya.Cat.Order;

namespace nyanya.Cat.Controllers
{
    /// <summary>
    ///     OrderInfoController
    /// </summary>
    [RoutePrefix("Orders")]
    public class OrderInfoController : ApiControllerBase
    {
        /// <summary>
        ///     The ba order information service
        /// </summary>
        private readonly IBAOrderInfoService baOrderInfoService;

        /// <summary>
        ///     The command bus
        /// </summary>
        private readonly ICommandBus commandBus;

        /// <summary>
        ///     The order information service
        /// </summary>
        private readonly IOrderInfoService orderInfoService;

        /// <summary>
        ///     The product service
        /// </summary>
        private readonly IProductInfoService productService;

        /// <summary>
        ///     The ta order information service
        /// </summary>
        private readonly ITAOrderInfoService taOrderInfoService;

        /// <summary>
        ///     The user information service
        /// </summary>
        private readonly IExactUserInfoService userInfoService;

        /// <summary>
        ///     The user service
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        ///     The ZCB order service
        /// </summary>
        private readonly IZCBOrderService zcbOrderService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderInfoController" /> class.
        /// </summary>
        /// <param name="baOrderInfoService">The ba order information service.</param>
        /// <param name="taOrderInfoService">The ta order information service.</param>
        /// <param name="orderInfoService">The order information service.</param>
        /// <param name="userInfoService">The user information service.</param>
        /// <param name="zcbOrderService">The ZCB order service.</param>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="productService">The product service.</param>
        public OrderInfoController(IBAOrderInfoService baOrderInfoService, ITAOrderInfoService taOrderInfoService, IOrderInfoService orderInfoService, IExactUserInfoService userInfoService, IZCBOrderService zcbOrderService, ICommandBus commandBus, IUserService userService, IProductInfoService productService)
        {
            this.baOrderInfoService = baOrderInfoService;
            this.taOrderInfoService = taOrderInfoService;
            this.orderInfoService = orderInfoService;
            this.userInfoService = userInfoService;
            this.zcbOrderService = zcbOrderService;
            this.commandBus = commandBus;
            this.userService = userService;
            this.productService = productService;
        }

        /// <summary>
        ///     获取用户【500元本金活动】状态
        /// </summary>
        /// <returns>
        ///     Status：状态(10=&gt;不符合，20=&gt;符合但没有下单，30=&gt;符合且已经下单，40=&gt;已过期)
        ///     ExtraInterest：额外收益
        ///     MiniCash: 起投金额
        /// </returns>
        [HttpGet, Route("GetActivityStatus1000"), TokenAuthorize, ResponseType(typeof(UserActivityResponse))]
        public async Task<IHttpActionResult> GetActivityStatus1000()
        {
            UserInfo userInfo = await this.userInfoService.GetUserInfoAsync(this.CurrentUser.Identifier);

            if (userInfo == null)
            {
                this.Warning("Can not load user date.{0}".FormatWith(this.CurrentUser.Identifier));
                return this.BadRequest("无法获取用户信息");
            }

            return this.Ok(await this.GetUserActivityStatu_1000(userInfo));
        }

        /// <summary>
        ///     银票失败订单列表
        /// </summary>
        /// <param name="category">产品分类(10金银猫产品 20富滇产品)</param>
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
        ///     -- ShowingStatus[int]: 项目状态  10 =&gt; 付款中, 20 =&gt; 待起息, 30 =&gt; 已起息, 40 =&gt; 已结息, 50 =&gt; 支付失败
        ///     -- TotalAmount[decimal]: 订单的本息总额
        ///     -- UseableItemCount[int]: 可用道具数 =&gt; -1:已用道具  0:无可用道具  1~999:有可用道具
        ///     -- ValueDate[yyyy-MM-ddTHH:mm:ss]: 起息日期
        ///     -- Yield[decimal]: 订单的预期收益率
        ///     -- ProductCategory[int]: 产品分类 10 =&gt; 金银猫产品，20 =&gt; 富滇产品，40 =&gt; 阜新产品
        /// </returns>
        [HttpGet, Route("BA/Failed"), Route("BA/Failed/{category:int=10:min(10)}"), RangeFilter("category", 10), TokenAuthorize, ResponseType(typeof(OrderListResponse))]
        public async Task<IHttpActionResult> GetBAFailedOrders(int category = 10)
        {
            ProductCategory productCategory = category == 20 ? ProductCategory.FUDIAN : category == 40 ? ProductCategory.FUXIN : ProductCategory.JINYINMAO;
            IPaginatedDto<BAOrderInfo> orders = await this.baOrderInfoService.GetFailedOrderInfosAsync(this.CurrentUser.Identifier, productCategory);

            return this.Ok(new OrderListResponse(orders));
        }

        /// <summary>
        ///     银票成功订单列表
        /// </summary>
        /// <param name="pageIndex">pageIndex(int &gt;= 1): 页码</param>
        /// <param name="sortMode">排序规则 1 =&gt; 按下单时间排序，2 =&gt; 按结息日期排序</param>
        /// <param name="category">产品分类(10金银猫产品 20富滇产品)</param>
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
        ///     -- ProductCategory[int]: 产品分类 10 => 金银猫产品，20 => 富滇产品，40 => 阜新产品
        ///     -- IsRepaid[bool]: 是否已经操作还款
        ///     -- ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     -- RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日
        /// </returns>
        [HttpGet, Route("BA"), Route("BA/{pageIndex:min(1):int=1}/{sortMode:min(1):int=1}"), Route("BA/{pageIndex:min(1):int=1}/{sortMode:min(1):int=1}/{category:int=10}"), RangeFilter("pageIndex", 1), RangeFilter("category", 10), TokenAuthorize, ResponseType(typeof(OrderListResponse))]
        public async Task<IHttpActionResult> GetBAOrders(int pageIndex = 1, int sortMode = 1, int category = 10)
        {
            OrderSortingStrategy sortingStrategy = sortMode == 1 ? OrderSortingStrategy.ByOrderTime : OrderSortingStrategy.BySettleDate;

            ProductCategory productCategory = category == 20 ? ProductCategory.FUDIAN : category == 40 ? ProductCategory.FUXIN : ProductCategory.JINYINMAO;

            IPaginatedDto<BAOrderInfo> orders = await this.baOrderInfoService.GetSuccessfulOrderInfosAsync(this.CurrentUser.Identifier, pageIndex, 10, sortingStrategy, productCategory);

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
        ///     IsRepaid[bool]: 是否已经操作还款
        ///     ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        /// </returns>
        [HttpGet, Route(""), ParameterRequire("orderIdentifier"), TokenAuthorize, ResponseType(typeof(OrderInfoResponse))]
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
        [HttpGet, Route("ConsignmentAgreement/"), ParameterRequire("orderIdentifier"), TokenAuthorize]
        public async Task<IHttpActionResult> GetOrderConsignmentAgreement(string orderIdentifier)
        {
            string content = await this.orderInfoService.GetConsignmentAgreementAsync(orderIdentifier);

            if (StringEx.IsNullOrWhiteSpace(content))
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
        [HttpGet, Route("PledgeAgreement/"), ParameterRequire("orderIdentifier"), TokenAuthorize]
        public async Task<IHttpActionResult> GetOrderPledgeAgreement(string orderIdentifier)
        {
            string content = await this.orderInfoService.GetPledgeAgreementAsync(orderIdentifier);

            if (StringEx.IsNullOrWhiteSpace(content))
            {
                return this.BadRequest("订单不存在");
            }

            return this.Ok(new { Content = content });
        }

        /// <summary>
        ///     用户资产包提现所需参数
        /// </summary>
        /// <returns>
        ///     RedeemCount：用户当日已提现次数
        ///     TodayIsInvesting：今天是否有投资金额 10 => 是 20 => 否
        ///     InvestingAndUnRedeemPrincipal：今天投资金额和还未处理的提现金额的总和
        ///     AvailableRedeemPrincipal（当天实际可以提现的本金）  = 当前正在投资总额 - 今天投资金额和还未处理的提现金额的总和）
        /// </returns>
        [HttpGet, Route("ZCB/RedeemParameter"), TokenAuthorize, ResponseType(typeof(RedeemParametersResponse))]
        public async Task<IHttpActionResult> GetRedeemParameters()
        {
            Task<int> RedeemCount = this.zcbOrderService.CheckRedeemPrincipalCount(this.CurrentUser.Identifier);
            Task<decimal> UserTodayInvesting = this.zcbOrderService.GetUserTodayInvesting(this.CurrentUser.Identifier);
            Task<decimal> UnRedeemPrincipal = this.zcbOrderService.GetUnRedeemPrincipal(this.CurrentUser.Identifier);
            await Task.WhenAll(RedeemCount, UserTodayInvesting, UnRedeemPrincipal);

            return this.Ok(new RedeemParametersResponse(RedeemCount.Result, UserTodayInvesting.Result > 0 ? 10 : 20, UserTodayInvesting.Result + UnRedeemPrincipal.Result));
        }

        /// <summary>
        ///     商票失败订单列表
        /// </summary>
        /// <param name="category">产品分类(10金银猫产品 30施秉金鼎产品)</param>
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
        ///     -- ShowingStatus[int]: 项目状态  10 =&gt; 付款中, 20 =&gt; 待起息, 30 =&gt; 已起息, 40 =&gt; 已结息, 50 =&gt; 支付失败
        ///     -- TotalAmount[decimal]: 订单的本息总额
        ///     -- UseableItemCount[int]: 可用道具数 =&gt; -1:已用道具  0:无可用道具  1~999:有可用道具
        ///     -- ValueDate[yyyy-MM-ddTHH:mm:ss]: 起息日期
        ///     -- Yield[decimal]: 订单的预期收益率
        ///     -- ProductCategory[int]：产品分类 10金银猫产品 30施秉金鼎产品
        /// </returns>
        [HttpGet, Route("TA/Failed"), Route("TA/Failed/{category:int=10:min(10)}"), RangeFilter("category", 10), TokenAuthorize, ResponseType(typeof(OrderListResponse))]
        public async Task<IHttpActionResult> GetTAFailedOrders(int category = 10)
        {
            ProductCategory productCategory = category == 30 ? ProductCategory.SHIBING : ProductCategory.JINYINMAO;

            IPaginatedDto<TAOrderInfo> orders = await this.taOrderInfoService.GetFailedOrderInfosAsync(this.CurrentUser.Identifier, productCategory);

            return this.Ok(new OrderListResponse(orders));
        }

        /// <summary>
        ///     商票成功订单列表
        /// </summary>
        /// <param name="pageIndex">pageIndex(int &gt;= 1): 页码</param>
        /// <param name="sortMode">排序规则 1 =&gt; 按下单时间排序，2 =&gt; 按结息日期排序</param>
        /// <param name="category">产品分类(10金银猫产品 30施秉金鼎产品)</param>
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
        ///     -- ProductCategory[int]：产品分类 10金银猫产品 30施秉金鼎产品
        ///     -- IsRepaid[bool]: 是否已经操作还款
        ///     -- ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     -- RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日
        /// </returns>
        [HttpGet, Route("TA"), Route("TA/{pageIndex:min(1):int=1}/{sortMode:min(1):int=1}"), Route("TA/{pageIndex:min(1):int=1}/{sortMode:min(1):int=1}/{category:int=10}"), RangeFilter("pageIndex", 1), RangeFilter("category", 10), TokenAuthorize, ResponseType(typeof(OrderListResponse))]
        public async Task<IHttpActionResult> GetTAOrders(int pageIndex = 1, int sortMode = 1, int category = 10)
        {
            OrderSortingStrategy sortingStrategy = sortMode == 1 ? OrderSortingStrategy.ByOrderTime : OrderSortingStrategy.BySettleDate;

            ProductCategory productCategory = category == 30 ? ProductCategory.SHIBING : ProductCategory.JINYINMAO;

            IPaginatedDto<TAOrderInfo> orders = await this.taOrderInfoService.GetSuccessfulOrderInfosAsync(this.CurrentUser.Identifier, pageIndex, 10, sortingStrategy, productCategory);

            return this.Ok(new OrderListResponse(orders));
        }

        /// <summary>
        ///     资产包认购/提现流程列表
        /// </summary>
        /// <param name="pageIndex">pageIndex(int &gt;= 1): 页码</param>
        /// <returns>
        ///     HasNextPage: 是否有下一页
        ///     PageIndex: 页码
        ///     PageSize: 一页节点数量
        ///     TotalCount: 所有的节点数量
        ///     TotalPageCount: 总页数
        ///     ZCBBills：节点列表
        ///     -- BillIdentifier[string]：流水标示号
        ///     -- ProductIdentifier[string]：项目唯一标识
        ///     -- CreateTime[yyyy-MM-ddTHH:mm:ss]：创建时间
        ///     -- Type[int]：交易类型 10 =&gt; 认购 20 =&gt; 提现
        ///     -- Principal[decimal]：交易金额
        ///     -- BankCardNo[string]：银行卡号
        ///     -- BankName[string]：银行名称
        ///     -- BankCardCity[string]：开户行城市全称，如 上海|上海
        ///     -- Status[int]：流水状态 10 =&gt; 付款中 20 =&gt; 认购成功 30 =&gt; 认购失败 40 =&gt; 取现已申请 50 =&gt; 取现成功 60 =&gt; 提现失败
        ///     -- Remark[string]：流水信息描述
        ///     -- DalayDate[yyyy-MM-ddTHH:mm:ss]：预计提现到账时间
        ///     -- AgreementName[string]：协议名称（Status=20或Status=50的时候，该变量才会有值）
        /// </returns>
        [HttpGet, Route("ZCB/ZCBBill"), Route("ZCB/ZCBBill/{pageIndex:min(1):int=1}"), RangeFilter("pageIndex", 1), TokenAuthorize, ResponseType(typeof(List<ZCBBillListResponse>))]
        public async Task<IHttpActionResult> GetZCBBills(int pageIndex = 1)
        {
            IPaginatedDto<ZCBBill> zcbBills = await this.zcbOrderService.GetZCBBillListAsync(this.CurrentUser.Identifier, pageIndex, 10);
            return this.Ok(new ZCBBillListResponse(zcbBills));
        }

        /// <summary>
        ///     资产包订单用户总览
        /// </summary>
        /// ProductIdentifier[string]: 产品唯一标示符
        /// ProductNo[string]: 产品编号
        /// TotalPrincipal[decimal]: 投资总金额
        /// CurrentPrincipal[decimal]: 当前正在投资总额
        /// TotalInterest[decimal]: 累计总收益
        /// TotalRedeemInterest[decimal]: 累计提现收益
        /// YesterdayInterest[decimal]: 昨日收益
        /// Yield[decimal]: 今日利率
        /// <returns></returns>
        [HttpGet, Route("ZCBUser"), TokenAuthorize, ResponseType(typeof(ZCBUserResponse))]
        public async Task<IHttpActionResult> GetZCBUser()
        {
            ZCBUser zcbUser = await this.zcbOrderService.GetZCBUserAsync(this.CurrentUser.Identifier);
            if (zcbUser == null)
            {
                var product = await this.productService.GetFirstProduct(ProductType.ZCBAcceptance);
                if (product == null)
                {
                    return this.Ok(new ZCBUserResponse());
                }
                zcbUser = new ZCBUser
                {
                    ProductIdentifier = product.ProductIdentifier,
                    ProductNo = product.ProductNo,
                    CurrentPrincipal = 0,
                    TotalInterest = 0,
                    TotalRedeemInterest = 0,
                    YesterdayInterest = 0,
                    TotalPrincipal = 0
                };
                return this.Ok(zcbUser.ToZCBUserResponse(product.Yield));
            }
            decimal yield = await this.productService.GetProductYield(zcbUser.ProductNo);
            return this.Ok(zcbUser.ToZCBUserResponse(yield));
        }

        /// <summary>
        ///     资产包用户每日收益列表
        /// </summary>
        /// <param name="pageIndex">pageIndex(int =1)：页码</param>
        /// <param name="startTime">startTime(yyyy-MM-dd)：开始时间</param>
        /// <param name="endTime">endTime(yyyy-MM-dd)：结束时间</param>
        /// <returns>
        ///     HasNextPage：是否有下一页
        ///     PageIndex：页码
        ///     PageSize：一页节点数量
        ///     TotalCount：所有的节点数量
        ///     TotalPageCount：总页数
        ///     ZCBUserBills：节点列表
        ///     -- BillDate[yyyy-MM-dd]：日期
        ///     -- Principal[decimal]：投资总额
        ///     -- Yield[decimal]：收益率
        ///     -- Interest[decimal]：收益
        ///     -- Remark[string]：备注
        /// </returns>
        [HttpGet, Route("ZCB/ZCBUserBill"), Route("ZCB/ZCBUserBill/{pageIndex:min(1):int=1}"), RangeFilter("pageIndex", 1), TokenAuthorize, ResponseType(typeof(List<ZCBUserBillListResponse>))]
        public async Task<IHttpActionResult> GetZCBUserBills(DateTime? startTime, DateTime? endTime, int pageIndex = 1)
        {
            if (startTime > endTime)
            {
                return this.NotFound();
            }
            IPaginatedDto<ZCBUserBill> zcbUserBills = await this.zcbOrderService.GetZCBUserBillListAsync(this.CurrentUser.Identifier, startTime.GetValueOrDefault().Date, endTime.GetValueOrDefault().Date, pageIndex, 10);
            return this.Ok(new ZCBUserBillListResponse(zcbUserBills));
        }

        /// <summary>
        ///     赎回本金
        /// </summary>
        /// <param name="request">
        ///     ProductNo[string]：产品编号
        ///     BankCardNo[string]：银行卡号
        ///     RedeemPrincipal[decimal]：赎回本金
        ///     PaymentPassword[string]：支付密码
        /// </param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     No Content
        ///     @{h2@} HttpStatusCode:400 @{/h2@}
        ///     "提现金额不能有小数"
        ///     "为保障您的资金账户安全，请重置支付密码后再试"
        ///     "支付密码错误，您还有{0}次机会"
        ///     "订单不存在"
        ///     "购买当日不能取款"
        ///     "当日取款次数已满2次"
        ///     "输入金额大于可取回金额"
        ///     "输入金额大于单次取款限额"
        ///     "输入金额大于项目可取限额"
        /// </returns>
        [HttpPost, Route("RedeemPrincipal"), TokenAuthorize, EmptyParameterFilter("request", Order = 1), ValidateModelState(Order = 2), ResponseType(typeof(RedeemBillResponse))]
        public async Task<IHttpActionResult> RedeemPrincipal(RedeemPrincipalRequest request)
        {
            if (!this.IsInt(request.RedeemPrincipal.ToString()))
            {
                return this.BadRequest("提现金额不能有小数");
            }

            CheckPaymentPasswordResult result = await this.userService.CheckPaymentPasswordAsync(this.CurrentUser.Identifier, request.PaymentPassword);

            if (result.Lock)
            {
                return this.BadRequest("为保障您的资金账户安全<br>请重置支付密码后再试");
            }

            if (!result.Successful)
            {
                return this.BadRequest("支付密码错误，您还有{0}次机会".FmtWith(result.RemainCount));
            }

            if (request.RedeemPrincipal > 50000)
            {
                return this.BadRequest("输入金额大于单次取款限额");
            }
            if (await this.zcbOrderService.CheckRedeemPrincipalCount(this.CurrentUser.Identifier) >= 2)
            {
                return this.BadRequest("当日取款次数已满2次");
            }
            Product productResult = await this.productService.GetProductByNo(request.ProductNo);
            if (productResult == null || productResult.ProductIdentifier == "" ||
                productResult.ProductType != ProductType.ZCBAcceptance)
            {
                return this.BadRequest("产品错误");
            }

            var remain =
                await
                    this.zcbOrderService.CheckRedeemPrincipalAsync(this.CurrentUser.Identifier,
                        productResult.ProductIdentifier);
            if (remain == null)
            {
                return this.BadRequest("产品错误");
            }

            if (request.RedeemPrincipal > remain.RemainPrincipal + remain.RemainRedeemInterest && remain.TodayPrincipal > 0)
            {
                if (remain.RemainPrincipal > 0)
                {
                    return this.BadRequest("购买当天不能提现，您当前可提现金额为{0}".FmtWith(remain.RemainPrincipal + remain.RemainRedeemInterest));
                }
                return this.BadRequest("购买当天不能提现");
            }
            if (request.RedeemPrincipal > remain.RemainPrincipal + remain.RemainRedeemInterest)
            {
                return this.BadRequest("所剩余额不足");
            }

            BuildRedeemPrincipal command = this.BuildRedeemPrincipalCommand(request, remain, productResult);
            var commandResult = this.commandBus.ResultExcute(command);
            var response =
                JsonConvert.DeserializeObject(commandResult.Data.ToString(), typeof(int?))
                    as int?;
            int redeemDays = response ?? 0;
            if (redeemDays == 0)
            {
                return this.BadRequest("请稍后再试");
            }
            return this.Ok(new RedeemBillResponse
            {
                RedeemDays = redeemDays + 1
            });
        }

        /// <summary>
        ///     Builds the redeem principal command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="info">The information.</param>
        /// <param name="productResult">The product result.</param>
        /// <returns></returns>
        private BuildRedeemPrincipal BuildRedeemPrincipalCommand(RedeemPrincipalRequest request, ZCBUserRemainRedeemInfo info, Product productResult)
        {
            return new BuildRedeemPrincipal(this.CurrentUser.Identifier)
            {
                BankCardNo = request.BankCardNo,
                RedeemPrincipal = request.RedeemPrincipal,
                ProductIdentifier = productResult.ProductIdentifier,
                FinallyRedeem = info.TodayPrincipal <= 0 && (request.RedeemPrincipal >= info.RemainPrincipal && request.RedeemPrincipal <= (info.RemainPrincipal + info.RemainRedeemInterest)) //最后一笔的标志
            };
        }

        /// <summary>
        ///     获取用户【500元本金活动】状态
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>
        ///     Status：状态(20=&gt;符合但没有下单，30=&gt;符合且已经下单，40=&gt;已过期)
        ///     ExtraInterest：额外收益
        ///     MiniCash: 起投金额
        /// </returns>
        private async Task<UserActivityResponse> GetUserActivityStatu_1000(UserInfo userInfo)
        {
            //用户活动响应
            var result = new UserActivityResponse();

            //前置条件：[2015-1-17 -- 2015-1-23]
            var beginTime = new DateTime(2015, 1, 17);
            var endTime = new DateTime(2015, 1, 24);

            //是否过期
            if (DateTime.Now < beginTime || DateTime.Now > endTime)
            {
                result.Status = 40;
                return result;
            }

            result.MiniCash = 50;
            result.Status = 20;

            //是否已使用过理财券
            var userActivityOrders = await this.orderInfoService.GetTheUserBetweenDateActivityOrders(userInfo.UserIdentifier, beginTime, endTime);
            if (userActivityOrders != null && userActivityOrders.Count > 0)
            {
                result.Status = 30;
                var firstOrDefault = userActivityOrders.OrderByDescending(x => x.ExtraInterest).FirstOrDefault();
                if (firstOrDefault == null) return result;
                result.ExtraInterest = firstOrDefault.ExtraInterest;
            }
            return result;
        }

        private bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
    }
}