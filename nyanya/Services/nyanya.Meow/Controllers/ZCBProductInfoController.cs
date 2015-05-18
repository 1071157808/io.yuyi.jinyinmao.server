using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Cat.Domain.Products.Services.Interfaces;
using Domian.DTO;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Meow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace nyanya.Meow.Controllers
{
    /// <summary>
    ///     资产包产品信息
    /// </summary>
    [RoutePrefix("ProductInfo/ZCB")]
    public class ZCBProductInfoController : ApiController
    {
        private readonly IZCBProductInfoService zcbProductInfoService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ZCBProductInfoController" /> class.
        /// </summary>
        /// <param name="zcbProductInfoService">The ba product information service.</param>
        public ZCBProductInfoController(IZCBProductInfoService zcbProductInfoService)
        {
            this.zcbProductInfoService = zcbProductInfoService;
        }

        /// <summary>
        ///     获取首页显示的产品信息（{count}产品数量{category}产品分类） => Notice => 字段ShowingStatus已经在服务端计算出了显示需要的状态
        /// </summary>
        /// <param name="count">产品数量(最大值为10)</param>
        /// <returns>
        ///     AvailableShareCount[int]: 可购买份额
        ///     EnableSale[int]: 是否可售 0 => 否 1 => 是
        ///     TotalSaleCount[int]：累计购买份额
        ///     TotalInterest[decimal]：累计收益
        ///     ProductCategory[int]: 产品分类 10 => 金银猫产品，20 => 富滇产品
        ///     CurrentValueDate[yyyy-MM-ddTHH:mm:ss]: 计算出的当时购买的起息日期
        ///     EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     FinancingSum[int]: 融资金额
        ///     FinancingSumCount[int]: 融资总份数,使用该字段显示产品信息
        ///     LaunchTime[yyyy-MM-ddTHH:mm:ss]: 产品上线时间
        ///     MaxShareCount[int]: 单笔订单最大购买份数
        ///     MinShareCount[int]: 单笔订单最小购买份数
        ///     OnPreSale[bool]: 是否在预售状态
        ///     OnSale[bool]: 是否在开售状态
        ///     PreEndSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买结束时间
        ///     PreSale[bool]: 是否有提前购买设置
        ///     PreStartSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买开始时间
        ///     ProductIdentifier[string]: 产品唯一标识符
        ///     ProductName[string]: 产品名称
        ///     ProductNo[string]: 产品编号，银承产品以F开头
        ///     ProductNumber[int]: 产品期数
        ///     ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     ShowingStatus[int]: 显示状态 10 => 待售, 30 => 在售, 50 => 结束
        ///     StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     SumShareCount[int]: 总销售份数,计算使用,前端计算暂时不会涉及到
        ///     UnitPrice[int]: 每一份的单价
        ///     ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 下一日起息, 30 => 指定日期起息（此时ValueDate一定有值）
        ///     ValueDateString[string]: 备选的起息日显示文案
        ///     Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        ///     ConsignmentAgreementName[string]：授权委托书名称
        ///     PledgeAgreementName[string]：投资协议名称
        ///     PaidPercent[int]: 销售百分比
        ///     PaidShareCount[int]: 已付款份数
        /// </returns>
        [HttpGet, Route("Index"), Route("Index/{count:int=1:min(1)}")]
        [RangeFilter("count", 1)]
        [ResponseType(typeof(List<ZCBProductInfoResponse>))]
        public async Task<IHttpActionResult> Index(int count = 1)
        {
            count = count > 10 ? 10 : count;
            IList<ProductWithSaleInfo<ZCBProductInfo>> infos = await this.zcbProductInfoService.GetTopProductWithSaleInfosAsync(count);
            List<ZCBProductInfoResponse> response = infos.Select(i => i.ToZCBProductInfoResponse()).ToList();
            int indexCount = response.Count(p => p.ShowingStatus == ProductShowingStatus.BeforeSale || p.ShowingStatus == ProductShowingStatus.OnSale || p.ShowingStatus == ProductShowingStatus.OnPreSale);
            if (indexCount <= count)
            {
                response = response.Take(count).ToList();
            }

            return this.Ok(response);
        }

        /// <summary>
        ///     获取产品详情 => Notice => 字段ShowingStatus已经在服务端计算出了显示需要的状态
        /// </summary>
        /// <param name="productNo">产品编号</param>
        /// <returns>
        ///     AvailableShareCount[int]: 可购买份额
        ///     EnableSale[int]: 是否可售 0 => 否 1 => 是
        ///     TotalSaleCount[int]：累计购买份额
        ///     TotalInterest[decimal]：累计收益
        ///     ProductCategory[int]: 产品分类 10 => 金银猫产品，20 => 富滇产品
        ///     CurrentValueDate[yyyy-MM-ddTHH:mm:ss]: 计算出的当时购买的起息日期
        ///     EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     FinancingSum[int]: 融资金额
        ///     FinancingSumCount[int]: 融资总份数,使用该字段显示产品信息
        ///     LaunchTime[yyyy-MM-ddTHH:mm:ss]: 产品上线时间
        ///     MaxShareCount[int]: 单笔订单最大购买份数
        ///     MinShareCount[int]: 单笔订单最小购买份数
        ///     OnPreSale[bool]: 是否在预售状态
        ///     OnSale[bool]: 是否在开售状态
        ///     PreEndSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买结束时间
        ///     PreSale[bool]: 是否有提前购买设置
        ///     PreStartSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买开始时间
        ///     ProductIdentifier[string]: 产品唯一标识符
        ///     ProductName[string]: 产品名称
        ///     ProductNo[string]: 产品编号，银承产品以F开头
        ///     ProductNumber[int]: 产品期数
        ///     ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     ShowingStatus[int]: 显示状态 10 => 待售, 30 => 在售,50 => 结束
        ///     StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     SumShareCount[int]: 总销售份数,计算使用,前端计算暂时不会涉及到
        ///     UnitPrice[int]: 每一份的单价
        ///     ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 下一日起息, 30 => 指定日期起息（此时ValueDate一定有值）
        ///     ValueDateString[string]: 备选的起息日显示文案
        ///     Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        ///     ConsignmentAgreementName[string]：授权委托书名称
        ///     PledgeAgreementName[string]：投资协议名称
        ///     PaidPercent[int]: 销售百分比
        ///     PaidShareCount[int]: 已付款份数
        /// </returns>
        [HttpGet, Route(""), Route("{productNo}")]
        [ResponseType(typeof(ZCBProductInfoResponse))]
        public async Task<IHttpActionResult> Info(string productNo)
        {
            ProductWithSaleInfo<ZCBProductInfo> info = await this.zcbProductInfoService.GetProductWithSaleInfoByNoAsync(productNo);
            if (info == null)
            {
                return this.NotFound();
            }

            return this.Ok(info.ToZCBProductInfoResponse());
        }

        /// <summary>
        ///     获取产品信息列表（现在每页数量暂定为10个，只能指定页数，从1开始） => Notice => 字段ShowingStatus已经在服务端计算出了显示需要的状态
        /// </summary>
        /// <param name="number">页码，从1开始</param>
        /// <returns>
        ///     TotalPageCount[int]: 总页数
        ///     TotalCount[int]: 总数据数量
        ///     HasNextPage[bool]: 是否有下一页
        ///     PageIndex[int]: 当前页码,从1开始
        ///     PageSize[int]: 页面大小
        ///     Products[array]: 产品信息列表
        ///     - - AvailableShareCount[int]: 可购买份额
        ///     - - EnableSale[int]: 是否可售 0 => 否 1 => 是
        ///     - - TotalSaleCount[int]：累计购买份额
        ///     - - TotalInterest[decimal]：累计收益
        ///     - - ProductCategory[int]: 产品分类 10 => 金银猫产品，20 => 富滇产品
        ///     - - CurrentValueDate[yyyy-MM-ddTHH:mm:ss]: 计算出的当时购买的起息日期
        ///     - - EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     - - FinancingSum[int]: 融资金额
        ///     - - FinancingSumCount[int]: 融资总份数,使用该字段显示产品信息
        ///     - - LaunchTime[yyyy-MM-ddTHH:mm:ss]: 产品上线时间
        ///     - - MaxShareCount[int]: 单笔订单最大购买份数
        ///     - - MinShareCount[int]: 单笔订单最小购买份数
        ///     - - OnPreSale[bool]: 是否在预售状态
        ///     - - OnSale[bool]: 是否在开售状态
        ///     - - PreEndSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买结束时间
        ///     - - PreSale[bool]: 是否有提前购买设置
        ///     - - PreStartSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买开始时间
        ///     - - ProductIdentifier[string]: 产品唯一标识符
        ///     - - ProductName[string]: 产品名称
        ///     - - ProductNo[string]: 产品编号，银承产品以F开头
        ///     - - ProductNumber[int]: 产品期数
        ///     - - ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     - - ShowingStatus[int]: 显示状态 10 => 待售, 30 => 在售, 50 => 结束
        ///     - - StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     - - SumShareCount[int]: 总销售份数,计算使用,前端计算暂时不会涉及到
        ///     - - UnitPrice[int]: 每一份的单价
        ///     - - ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     - - ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 下一日起息, 30 => 指定日期起息（此时ValueDate一定有值）
        ///     - - ValueDateString[string]: 备选的起息日显示文案
        ///     - - Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        ///     - - ConsignmentAgreementName[string]：授权委托书名称
        ///     - - PledgeAgreementName[string]：投资协议名称
        ///     - - PaidPercent[int]: 销售百分比
        ///     - - PaidShareCount[int]: 已付款份数
        /// </returns>
        [HttpGet, Route("Page"), Route("Page/{number:int=1:min(1)}")]
        [RangeFilter("number", 1)]
        [ResponseType(typeof(PaginatedZCBProductInfosResponse))]
        public async Task<IHttpActionResult> Page(int number = 1)
        {
            IPaginatedDto<ProductWithSaleInfo<ZCBProductInfo>> model = await this.zcbProductInfoService.GetProductWithSaleInfosAsync(number);
            return this.Ok(new PaginatedZCBProductInfosResponse(model));
        }
    }
}
