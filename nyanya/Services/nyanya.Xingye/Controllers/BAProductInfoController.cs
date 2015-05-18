// FileInformation: nyanya/nyanya.Xingye/BAProductInfoController.cs
// CreatedTime: 2014/09/01   10:16 AM
// LastUpdatedTime: 2014/09/02   2:07 PM

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Domian.DTO;
using Infrastructure.Lib.Data;
using Infrastructure.Lib.Extensions;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Xingye.Models;
using Xingye.Domain.Products.ReadModels;
using Xingye.Domain.Products.Services.DTO;
using Xingye.Domain.Products.Services.Interfaces;

namespace nyanya.Xingye.Controllers
{
    /// <summary>
    ///     银承产品信息
    /// </summary>
    [RoutePrefix("ProductInfo/BA")]
    public class BAProductInfoController : ApiController
    {
        private readonly IBAProductInfoService baProductInfoService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BAProductInfoController" /> class.
        /// </summary>
        /// <param name="baProductInfoService">The ba product information service.</param>
        public BAProductInfoController(IBAProductInfoService baProductInfoService)
        {
            this.baProductInfoService = baProductInfoService;
        }

        /// <summary>
        ///     获取首页显示的产品信息（现在数量暂定为6个） => Notice => 字段ShowingStatus已经在服务端计算出了显示需要的状态
        /// </summary>
        /// <returns>
        ///     AvailableShareCount[int]: 可购买份额
        ///     BankName[string]: 银行名称
        ///     BillNo[string]: 银票编号
        ///     BusinessLicense[string]: 营业执照号码
        ///     CurrentValueDate[yyyy-MM-ddTHH:mm:ss]: 计算出的当时购买的起息日期
        ///     EndorseImageLink[string]: 票据大图链接
        ///     EndorseImageThumbnailLink[string]: 票据缩略图链接
        ///     EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     EnterpriseName[string]: 企业名称
        ///     ExtraYield[deciaml]: 额外收益率
        ///     FinancingSum[int]: 融资金额
        ///     FinancingSumCount[int]: 融资总份数,使用该字段显示产品信息
        ///     LaunchTime[yyyy-MM-ddTHH:mm:ss]: 产品上线时间
        ///     MaxShareCount[int]: 单笔订单最大购买份数
        ///     MinShareCount[int]: 单笔订单最小购买份数
        ///     OnPreSale[bool]: 是否在预售状态
        ///     OnSale[bool]: 是否在开售状态
        ///     PaidPercent[int]: 销售百分比
        ///     PaidShareCount[int]: 已付款份数
        ///     PayingShareCount[int]: 付款中份数
        ///     Period[int]: 项目周期
        ///     PreEndSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买结束时间
        ///     PreSale[bool]: 是否有提前购买设置
        ///     PreStartSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买开始时间
        ///     ProductIdentifier[string]: 产品唯一标识符
        ///     ProductName[string]: 产品名称
        ///     ProductNo[string]: 产品编号，银承产品以F开头
        ///     ProductNumber[int]: 产品期数
        ///     Repaid[bool]: 是否已经还款
        ///     RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日期
        ///     ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息时间
        ///     ShowingStatus[int]: 显示状态 10 => 待售, 20 => 预售, 30 => 在售, 40 => 售罄, 50 => 结束
        ///     SoldOut[bool]: 是否售罄
        ///     SoldOutTime[yyyy-MM-ddTHH:mm:ss|""]: 售罄时间
        ///     StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     SumShareCount[int]: 总销售份数,计算使用,前端计算暂时不会涉及到
        ///     UnitPrice[int]: 每一份的单价
        ///     Usage[string]: 融资用途
        ///     ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 下一日起息, 30 => 指定日期起息（此时ValueDate一定有值）
        ///     ValueDateString[string]: 备选的起息日显示文案
        ///     Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        /// </returns>
        [HttpGet, Route("Index")]
        [ResponseType(typeof(List<BAProductInfoResponse>))]
        public async Task<IHttpActionResult> Index()
        {
            IList<ProductWithSaleInfo<BAProductInfo>> infos = await this.baProductInfoService.GetTopProductWithSaleInfosAsync();
            List<BAProductInfoResponse> response = infos.Select(i => i.ToBAProductInfoResponse()).ToList();
            int indexCount = response.Count(p => p.ShowingStatus == ProductShowingStatus.BeforeSale || p.ShowingStatus == ProductShowingStatus.OnSale
                                                 || p.ShowingStatus == ProductShowingStatus.OnPreSale);
            if (indexCount <= 3)
            {
                response = response.Take(3).ToList();
            }
            return this.Ok(response);
        }

        /// <summary>
        ///     获取产品详情 => Notice => 字段ShowingStatus已经在服务端计算出了显示需要的状态
        /// </summary>
        /// <param name="productNo">产品编号</param>
        /// <returns>
        ///     AvailableShareCount[int]: 可购买份额
        ///     BankName[string]: 银行名称
        ///     BillNo[string]: 银票编号
        ///     BusinessLicense[string]: 营业执照号码
        ///     CurrentValueDate[yyyy-MM-ddTHH:mm:ss]: 计算出的当时购买的起息日期
        ///     EndorseImageLink[string]: 票据大图链接
        ///     EndorseImageThumbnailLink[string]: 票据缩略图链接
        ///     EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     EnterpriseName[string]: 企业名称
        ///     ExtraYield[deciaml]: 额外收益率
        ///     FinancingSum[int]: 融资金额
        ///     FinancingSumCount[int]: 融资总份数,使用该字段显示产品信息
        ///     LaunchTime[yyyy-MM-ddTHH:mm:ss]: 产品上线时间
        ///     MaxShareCount[int]: 单笔订单最大购买份数
        ///     MinShareCount[int]: 单笔订单最小购买份数
        ///     OnPreSale[bool]: 是否在预售状态
        ///     OnSale[bool]: 是否在开售状态
        ///     PaidPercent[int]: 销售百分比
        ///     PaidShareCount[int]: 已付款份数
        ///     PayingShareCount[int]: 付款中份数
        ///     Period[int]: 项目周期
        ///     PreEndSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买结束时间
        ///     PreSale[bool]: 是否有提前购买设置
        ///     PreStartSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买开始时间
        ///     ProductIdentifier[string]: 产品唯一标识符
        ///     ProductName[string]: 产品名称
        ///     ProductNo[string]: 产品编号，银承产品以F开头
        ///     ProductNumber[int]: 产品期数
        ///     Repaid[bool]: 是否已经还款
        ///     RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日期
        ///     ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息时间
        ///     ShowingStatus[int]: 显示状态 10 => 待售, 20 => 预售, 30 => 在售, 40 => 售罄, 50 => 结束
        ///     SoldOut[bool]: 是否售罄
        ///     SoldOutTime[yyyy-MM-ddTHH:mm:ss|""]: 售罄时间
        ///     StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     SumShareCount[int]: 总销售份数,计算使用,前端计算暂时不会涉及到
        ///     UnitPrice[int]: 每一份的单价
        ///     Usage[string]: 融资用途
        ///     ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 下一日起息, 30 => 指定日期起息（此时ValueDate一定有值）
        ///     ValueDateString[string]: 备选的起息日显示文案
        ///     Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        /// </returns>
        [HttpGet, Route(""), Route("{productNo}")]
        [ResponseType(typeof(BAProductInfoResponse))]
        public async Task<IHttpActionResult> Info(string productNo)
        {
            ProductWithSaleInfo<BAProductInfo> info = await this.baProductInfoService.GetProductWithSaleInfoByNoAsync(productNo);
            if (info == null)
            {
                return this.NotFound();
            }

            return this.Ok(info.ToBAProductInfoResponse());
        }

        /// <summary>
        ///     获取产品信息列表（现在每页数量暂定为10个，只能指定页数，从1开始，可按[收益率]和[项目周期]排序） => Notice => 字段ShowingStatus已经在服务端计算出了显示需要的状态
        /// </summary>
        /// <param name="number">页码，从1开始</param>
        /// <param name="yieldSortModel">收益率排序，1 => 升序，2 => 降序</param>
        /// <param name="periodSortModel">项目周期排序，1 => 升序，2 => 降序</param>
        /// <returns>
        ///     TotalPageCount[int]: 总页数
        ///     TotalCount[int]: 总数据数量
        ///     HasNextPage[bool]: 是否有下一页
        ///     PageIndex[int]: 当前页码,从1开始
        ///     PageSize[int]: 页面大小
        ///     Products[array]: 产品信息列表
        ///     - - AvailableShareCount[int]: 可购买份额
        ///     - - BankName[string]: 银行名称
        ///     - - BillNo[string]: 银票编号
        ///     - - BusinessLicense[string]: 营业执照号码
        ///     - - CurrentValueDate[yyyy-MM-ddTHH:mm:ss]: 计算出的当时购买的起息日期
        ///     - - EndorseImageLink[string]: 票据大图链接
        ///     - - EndorseImageThumbnailLink[string]: 票据缩略图链接
        ///     - - EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     - - EnterpriseName[string]: 企业名称
        ///     - - ExtraYield[deciaml]: 额外收益率
        ///     - - FinancingSum[int]: 融资金额
        ///     - - FinancingSumCount[int]: 融资总份数,使用该字段显示产品信息
        ///     - - LaunchTime[yyyy-MM-ddTHH:mm:ss]: 产品上线时间
        ///     - - MaxShareCount[int]: 单笔订单最大购买份数
        ///     - - MinShareCount[int]: 单笔订单最小购买份数
        ///     - - OnPreSale[bool]: 是否在预售状态
        ///     - - OnSale[bool]: 是否在开售状态
        ///     - - PaidPercent[int]: 销售百分比
        ///     - - PaidShareCount[int]: 已付款份数
        ///     - - PayingShareCount[int]: 付款中份数
        ///     - - Period[int]: 项目周期
        ///     - - PreEndSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买结束时间
        ///     - - PreSale[bool]: 是否有提前购买设置
        ///     - - PreStartSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买开始时间
        ///     - - ProductIdentifier[string]: 产品唯一标识符
        ///     - - ProductName[string]: 产品名称
        ///     - - ProductNo[string]: 产品编号，银承产品以F开头
        ///     - - ProductNumber[int]: 产品期数
        ///     - - Repaid[bool]: 是否已经还款
        ///     - - RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日期
        ///     - - ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     - - SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息时间
        ///     - - ShowingStatus[int]: 显示状态 10 => 待售, 20 => 预售, 30 => 在售, 40 => 售罄, 50 => 结束
        ///     - - SoldOut[bool]: 是否售罄
        ///     - - SoldOutTime[yyyy-MM-ddTHH:mm:ss|""]: 售罄时间
        ///     - - StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     - - SumShareCount[int]: 总销售份数,计算使用,前端计算暂时不会涉及到
        ///     - - UnitPrice[int]: 每一份的单价
        ///     - - Usage[string]: 融资用途
        ///     - - ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     - - ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 下一日起息, 30 => 指定日期起息（此时ValueDate一定有值）
        ///     - - ValueDateString[string]: 备选的起息日显示文案
        ///     - - Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        /// </returns>
        [HttpGet, Route("Page"), Route("Page/{number:int=1:min(1)}")]
        [RangeFilter("number", 1)]
        [ResponseType(typeof(PaginatedBAProductInfosResponse))]
        public async Task<IHttpActionResult> Page(int number = 1, int yieldSortModel = 0, int periodSortModel = 0)
        {
            var yieldOrderType = (ListSortDirection) yieldSortModel - 1;
            var periodOrderType = (ListSortDirection) periodSortModel - 1;

            var sortConditions = new List<SortCondition>();

            if (yieldOrderType.IsDefined())
            {
                sortConditions.Add(new SortCondition("Yield", yieldOrderType));
            }
            if (periodOrderType.IsDefined())
            {
                sortConditions.Add(new SortCondition("Period", periodOrderType));
            }

            var model = await this.baProductInfoService.GetProductWithSaleInfosAsync(number, sortConditions.ToArray());
            return this.Ok(new PaginatedBAProductInfosResponse(model));
        }

        /// <summary>
        ///     获取首个产品信息 => Notice => 字段ShowingStatus已经在服务端计算出了显示需要的状态
        /// </summary>
        /// <returns>
        ///     AvailableShareCount[int]: 可购买份额
        ///     BankName[string]: 银行名称
        ///     BillNo[string]: 银票编号
        ///     BusinessLicense[string]: 营业执照号码
        ///     CurrentValueDate[yyyy-MM-ddTHH:mm:ss]: 计算出的当时购买的起息日期
        ///     EndorseImageLink[string]: 票据大图链接
        ///     EndorseImageThumbnailLink[string]: 票据缩略图链接
        ///     EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     EnterpriseName[string]: 企业名称
        ///     ExtraYield[deciaml]: 额外收益率
        ///     FinancingSum[int]: 融资金额
        ///     FinancingSumCount[int]: 融资总份数,使用该字段显示产品信息
        ///     LaunchTime[yyyy-MM-ddTHH:mm:ss]: 产品上线时间
        ///     MaxShareCount[int]: 单笔订单最大购买份数
        ///     MinShareCount[int]: 单笔订单最小购买份数
        ///     OnPreSale[bool]: 是否在预售状态
        ///     OnSale[bool]: 是否在开售状态
        ///     PaidPercent[int]: 销售百分比
        ///     PaidShareCount[int]: 已付款份数
        ///     PayingShareCount[int]: 付款中份数
        ///     Period[int]: 项目周期
        ///     PreEndSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买结束时间
        ///     PreSale[bool]: 是否有提前购买设置
        ///     PreStartSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买开始时间
        ///     ProductIdentifier[string]: 产品唯一标识符
        ///     ProductName[string]: 产品名称
        ///     ProductNo[string]: 产品编号，银承产品以F开头
        ///     ProductNumber[int]: 产品期数
        ///     Repaid[bool]: 是否已经还款
        ///     RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日期
        ///     ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息时间
        ///     ShowingStatus[int]: 显示状态 10 => 待售, 20 => 预售, 30 => 在售, 40 => 售罄, 50 => 结束
        ///     SoldOut[bool]: 是否售罄
        ///     SoldOutTime[yyyy-MM-ddTHH:mm:ss|""]: 售罄时间
        ///     StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     SumShareCount[int]: 总销售份数,计算使用,前端计算暂时不会涉及到
        ///     UnitPrice[int]: 每一份的单价
        ///     Usage[string]: 融资用途
        ///     ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 下一日起息, 30 => 指定日期起息（此时ValueDate一定有值）
        ///     ValueDateString[string]: 备选的起息日显示文案
        ///     Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        /// </returns>
        [HttpGet, Route("Top")]
        [ResponseType(typeof(BAProductInfoResponse))]
        public async Task<IHttpActionResult> Top()
        {
            ProductWithSaleInfo<BAProductInfo> info = await this.baProductInfoService.GetTopProductWithSaleInfoAsync();
            return this.Ok(info.ToBAProductInfoResponse());
        }
    }
}