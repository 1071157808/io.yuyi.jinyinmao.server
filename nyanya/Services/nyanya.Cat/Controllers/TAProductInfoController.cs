// FileInformation: nyanya/nyanya.Cat/TAProductInfoController.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/18   10:07 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Cat.Domain.Products.Services.Interfaces;
using Domian.DTO;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Cat.Models;
using Cat.Commands.Products;



namespace nyanya.Cat.Controllers
{
    /// <summary>
    ///     商承产品信息
    /// </summary>
    [RoutePrefix("ProductInfo/TA")]
    public class TAProductInfoController : ApiControllerBase
    {
        private readonly ITAProductInfoService taProductInfoService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BAProductInfoController" /> class.
        /// </summary>
        /// <param name="taProductInfoService">The ta product information service.</param>
        public TAProductInfoController(ITAProductInfoService taProductInfoService)
        {
            this.taProductInfoService = taProductInfoService;
        }

        /// <summary>
        ///     获取产品详情 => Notice => 字段ShowingStatus已经在服务端计算出了显示需要的状态
        /// </summary>
        /// <param name="productNo">产品编号</param>
        /// <returns>
        ///     AvailableShareCount[int]: 可购买份额
        ///     BillNo[string]: 商票编号
        ///     ConsignmentAgreementName[string]:委托协议名称
        ///     CurrentValueDate[yyyy-MM-ddTHH:mm:ss]: 计算出的当时购买的起息日期
        ///     Drawee[string]：付款方
        ///     DraweeInfo[string]:付款方信息
        ///     EndorseImageLink[string]: 票据大图链接
        ///     EndorseImageThumbnailLink[string]: 票据缩略图链接
        ///     EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     EnterpriseInfo[string]:融资方信息
        ///     EnterpriseLicense[string]: 融资方执照号码
        ///     EnterpriseName[string]: 企业名称
        ///     ExtraYield[decimal]: 额外收益率
        ///     FinancingSum[int]: 融资金额
        ///     FinancingSumCount[int]: 融资总份数,使用该字段显示产品信息
        ///     GuaranteeMode[int]: 担保方式，10 => 银行保兑, 20 => 央企担保, 30 => 国企担保, 40 => 国有担保公司担保, 50 => 担保公司担保, 60 => 上市集团担保, 70 => 集团担保
        ///     LaunchTime[yyyy-MM-ddTHH:mm:ss]: 产品上线时间
        ///     MaxShareCount[int]: 单笔订单最大购买份数
        ///     MinShareCount[int]: 单笔订单最小购买份数
        ///     OnPreSale[bool]: 是否在预售状态
        ///     OnSale[bool]: 是否在开售状态
        ///     PledgeAgreementName[string]: 质押担保协议名称
        ///     PaidPercent[int]: 已付款的百分比
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
        ///     Repaid[bool]: 是否已经操作还款
        ///     RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日期
        ///     Securedparty[string]: 担保方名称
        ///     SecuredpartyInfo[string]: 担保方信息 (施秉金鼎: 还款来源)
        ///     ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息时间
        ///     ShowingStatus[int]: 显示状态 10 => 待售, 20 => 预售, 30 => 在售, 40 => 售罄, 50 => 结束
        ///     SoldOut[bool]: 是否售罄
        ///     SoldOutTime[yyyy-MM-ddTHH:mm:ss|""]: 售罄时间
        ///     StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     SumShareCount[int]: 总销售份数,计算使用,前端计算暂时不会涉及到
        ///     UnitPrice[int]: 每一份的单价
        ///     Usage[string]: 融资用途 (施秉金鼎:资金用途)
        ///     ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 下一日起息, 30 => 指定日期起息（此时ValueDate一定有值）
        ///     ValueDateString[string]: 备选的起息日显示文案
        ///     Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        /// </returns>
        [HttpGet, Route(""), Route("{productNo}")]
        [ResponseType(typeof(TAProductInfoResponse))]
        public async Task<IHttpActionResult> Info(string productNo)
        {
            ProductWithSaleInfo<TAProductInfo> info = await this.taProductInfoService.GetProductWithSaleInfoByNoAsync(productNo);
            if (info == null)
            {
                return this.NotFound();
            }

            return this.Ok(info.ToTAProductInfoResponse());
        }

        /// <summary>
        ///     获取产品信息列表（现在每页数量暂定为10个，只能指定页数，从1开始） => Notice => 字段ShowingStatus已经在服务端计算出了显示需要的状态
        /// </summary>
        /// <param name="number">页码，从1开始</param>
        /// <param name="category">产品分类（10金银猫产品 30施秉金鼎） </param>
        /// <returns>
        ///     TotalPageCount[int]: 总页数
        ///     TotalCount[int]: 总数据数量
        ///     HasNextPage[bool]: 是否有下一页
        ///     PageIndex[int]: 当前页码,从1开始
        ///     PageSize[int]: 页面大小
        ///     Products[array]: 产品信息列表
        ///     - - AvailableShareCount[int]: 可购买份额
        ///     - - BillNo[string]: 商票编号
        ///     - - ConsignmentAgreementName[string]:委托协议名称
        ///     - - CurrentValueDate[yyyy-MM-ddTHH:mm:ss]: 计算出的当时购买的起息日期
        ///     - - Drawee[string]：付款方
        ///     - - DraweeInfo[string]:付款方信息
        ///     - - EndorseImageLink[string]: 票据大图链接
        ///     - - EndorseImageThumbnailLink[string]: 票据缩略图链接
        ///     - - EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     - - EnterpriseInfo[string]:融资方信息
        ///     - - EnterpriseLicense[string]: 融资方执照号码
        ///     - - EnterpriseName[string]: 企业名称
        ///     - - ExtraYield[decimal]: 额外收益率
        ///     - - FinancingSum[int]: 融资金额
        ///     - - FinancingSumCount[int]: 融资总份数,使用该字段显示产品信息
        ///     - - GuaranteeMode[int]: 担保方式，10 => 银行保兑, 20 => 央企担保, 30 => 国企担保, 40 => 国有担保公司担保, 50 => 担保公司担保, 60 => 上市集团担保, 70 => 集团担保
        ///     - - LaunchTime[yyyy-MM-ddTHH:mm:ss]: 产品上线时间
        ///     - - MaxShareCount[int]: 单笔订单最大购买份数
        ///     - - MinShareCount[int]: 单笔订单最小购买份数
        ///     - - OnPreSale[bool]: 是否在预售状态
        ///     - - OnSale[bool]: 是否在开售状态
        ///     - - PledgeAgreementName[string]: 质押担保协议名称
        ///     - - PaidPercent[int]: 已付款的百分比
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
        ///     - - Repaid[bool]: 是否已经操作还款
        ///     - - RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日期
        ///     - - Securedparty[string]: 担保方名称
        ///     - - SecuredpartyInfo[string]: 担保方信息 (施秉金鼎: 还款来源)
        ///     - - ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     - - SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息时间
        ///     - - ShowingStatus[int]: 显示状态 10 => 待售, 20 => 预售, 30 => 在售, 40 => 售罄, 50 => 结束
        ///     - - SoldOut[bool]: 是否售罄
        ///     - - SoldOutTime[yyyy-MM-ddTHH:mm:ss|""]: 售罄时间
        ///     - - StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     - - SumShareCount[int]: 总销售份数,计算使用,前端计算暂时不会涉及到
        ///     - - UnitPrice[int]: 每一份的单价
        ///     - - Usage[string]: 融资用途 (施秉金鼎:资金用途)
        ///     - - ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     - - ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 下一日起息, 30 => 指定日期起息（此时ValueDate一定有值）
        ///     - - ValueDateString[string]: 备选的起息日显示文案
        ///     - - Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        ///     - - ProductCategory[int]：产品分类 （10金银猫产品 30施秉金鼎）
        /// </returns>
        [HttpGet, Route("Page"), Route("Page/{number:int=1:min(1)}"), Route("Page/{number:int=1:min(1)}/{category:int=10}")]
        [RangeFilter("number", 1), RangeFilter("category", 10)]
        [ResponseType(typeof(PaginatedTAProductInfosResponse))]
        public async Task<IHttpActionResult> Page(int number = 1, int category = 10)
        {
            ProductCategory productCategory = category == 30 ? ProductCategory.SHIBING : ProductCategory.JINYINMAO;
            IPaginatedDto<ProductWithSaleInfo<TAProductInfo>> model = await this.taProductInfoService.GetProductWithSaleInfosAsync(number, productCategory);
            return this.Ok(new PaginatedTAProductInfosResponse(model));
        }

        /// <summary>
        ///     获取首个产品信息 => Notice => 字段ShowingStatus已经在服务端计算出了显示需要的状态
        /// </summary>
        /// <param name="category">产品分类(10金银猫产品 30施秉金鼎产品)</param>
        /// <returns>
        ///     AvailableShareCount[int]: 可购买份额
        ///     BillNo[string]: 商票编号
        ///     ConsignmentAgreementName[string]:委托协议名称
        ///     CurrentValueDate[yyyy-MM-ddTHH:mm:ss]: 计算出的当时购买的起息日期
        ///     Drawee[string]：付款方
        ///     DraweeInfo[string]:付款方信息
        ///     EndorseImageLink[string]: 票据大图链接
        ///     EndorseImageThumbnailLink[string]: 票据缩略图链接
        ///     EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     EnterpriseInfo[string]:融资方信息
        ///     EnterpriseLicense[string]: 融资方执照号码
        ///     EnterpriseName[string]: 企业名称
        ///     ExtraYield[decimal]: 额外收益率
        ///     FinancingSum[int]: 融资金额
        ///     FinancingSumCount[int]: 融资总份数,使用该字段显示产品信息
        ///     GuaranteeMode[int]: 担保方式，10 => 银行保兑, 20 => 央企担保, 30 => 国企担保, 40 => 国有担保公司担保, 50 => 担保公司担保, 60 => 上市集团担保, 70 => 集团担保
        ///     LaunchTime[yyyy-MM-ddTHH:mm:ss]: 产品上线时间
        ///     MaxShareCount[int]: 单笔订单最大购买份数
        ///     MinShareCount[int]: 单笔订单最小购买份数
        ///     OnPreSale[bool]: 是否在预售状态
        ///     OnSale[bool]: 是否在开售状态
        ///     PledgeAgreementName[string]: 质押担保协议名称
        ///     PaidPercent[int]: 已付款的百分比
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
        ///     Repaid[bool]: 是否已经操作还款
        ///     RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日期
        ///     Securedparty[string]: 担保方名称
        ///     SecuredpartyInfo[string]: 担保方信息 (施秉金鼎: 还款来源)
        ///     ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息时间
        ///     ShowingStatus[int]: 显示状态 10 => 待售, 20 => 预售, 30 => 在售, 40 => 售罄, 50 => 结束
        ///     SoldOut[bool]: 是否售罄
        ///     SoldOutTime[yyyy-MM-ddTHH:mm:ss|""]: 售罄时间
        ///     StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     SumShareCount[int]: 总销售份数,计算使用,前端计算暂时不会涉及到
        ///     UnitPrice[int]: 每一份的单价
        ///     Usage[string]: 融资用途 (施秉金鼎:资金用途)
        ///     ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 下一日起息, 30 => 指定日期起息（此时ValueDate一定有值）
        ///     ValueDateString[string]: 备选的起息日显示文案
        ///     Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        ///     ProductCategory[int]：产品分类 （10金银猫产品 30施秉金鼎）
        /// </returns>
        [HttpGet, Route("Top"), Route("Top/{category:int=10}")]
        [RangeFilter("category", 10)]
        [ResponseType(typeof(TAProductInfoResponse))]
        public async Task<IHttpActionResult> Top(int category = 10)
        {
            ProductCategory productCategory = category == 30 ? ProductCategory.SHIBING : ProductCategory.JINYINMAO;
            ProductWithSaleInfo<TAProductInfo> info = await this.taProductInfoService.GetTopProductWithSaleInfoAsync(productCategory);
            if (info == null)
            {
                return this.OK();
            }
            return this.Ok(info.ToTAProductInfoResponse());
        }

        /// <summary>
        ///     获取首页产品信息（{count}产品数量） => Notice => 字段ShowingStatus已经在服务端计算出了显示需要的状态
        /// </summary>
        /// <param name="count">产品数量(最大值为10)</param>
        /// <param name="category">产品分类(10金银猫产品 30施秉金鼎产品)</param>
        /// <returns>
        ///     AvailableShareCount[int]: 可购买份额
        ///     BillNo[string]: 商票编号
        ///     ConsignmentAgreementName[string]:委托协议名称
        ///     CurrentValueDate[yyyy-MM-ddTHH:mm:ss]: 计算出的当时购买的起息日期
        ///     Drawee[string]：付款方
        ///     DraweeInfo[string]:付款方信息
        ///     EndorseImageLink[string]: 票据大图链接
        ///     EndorseImageThumbnailLink[string]: 票据缩略图链接
        ///     EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     EnterpriseInfo[string]:融资方信息
        ///     EnterpriseLicense[string]: 融资方执照号码
        ///     EnterpriseName[string]: 企业名称
        ///     ExtraYield[decimal]: 额外收益率
        ///     FinancingSum[int]: 融资金额
        ///     FinancingSumCount[int]: 融资总份数,使用该字段显示产品信息
        ///     GuaranteeMode[int]: 担保方式，10 => 银行保兑, 20 => 央企担保, 30 => 国企担保, 40 => 国有担保公司担保, 50 => 担保公司担保, 60 => 上市集团担保, 70 => 集团担保
        ///     LaunchTime[yyyy-MM-ddTHH:mm:ss]: 产品上线时间
        ///     MaxShareCount[int]: 单笔订单最大购买份数
        ///     MinShareCount[int]: 单笔订单最小购买份数
        ///     OnPreSale[bool]: 是否在预售状态
        ///     OnSale[bool]: 是否在开售状态
        ///     PledgeAgreementName[string]: 质押担保协议名称
        ///     PaidPercent[int]: 已付款的百分比
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
        ///     Repaid[bool]: 是否已经操作还款
        ///     RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日期
        ///     Securedparty[string]: 担保方名称
        ///     SecuredpartyInfo[string]: 担保方信息 (施秉金鼎: 还款来源)
        ///     ServerTime[yyyy-MM-ddTHH:mm:ss]: 服务器当前时间
        ///     SettleDate[yyyy-MM-ddTHH:mm:ss]: 结息时间
        ///     ShowingStatus[int]: 显示状态 10 => 待售, 20 => 预售, 30 => 在售, 40 => 售罄, 50 => 结束
        ///     SoldOut[bool]: 是否售罄
        ///     SoldOutTime[yyyy-MM-ddTHH:mm:ss|""]: 售罄时间
        ///     StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     SumShareCount[int]: 总销售份数,计算使用,前端计算暂时不会涉及到
        ///     UnitPrice[int]: 每一份的单价
        ///     Usage[string]: 融资用途 (施秉金鼎:资金用途)
        ///     ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 下一日起息, 30 => 指定日期起息（此时ValueDate一定有值）
        ///     ValueDateString[string]: 备选的起息日显示文案
        ///     Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        ///     ProductCategory[int]：10金银猫 30施秉金鼎
        /// </returns>
        [HttpGet, Route("Index"), Route("Index/{count:int=1:min(1)}"), Route("Index/{count:int=1:min(1)}/{category:int=10}")]
        [RangeFilter("count", 1), RangeFilter("category", 10)]
        [ResponseType(typeof(List<TAProductInfoResponse>))]
        public async Task<IHttpActionResult> Index(int count = 1, int category = 10)
        {
            count = count >= 10 ? 10 : count;
            ProductCategory productCategory = category == 30 ? ProductCategory.SHIBING : ProductCategory.JINYINMAO;
            IList<ProductWithSaleInfo<TAProductInfo>> infos = await this.taProductInfoService.GetTopProductWithSaleInfosAsync(count,productCategory);
            List<TAProductInfoResponse> response = infos.Select(i => i.ToTAProductInfoResponse()).ToList();
            int indexCount = response.Count(p => p.ShowingStatus == ProductShowingStatus.BeforeSale || p.ShowingStatus == ProductShowingStatus.OnSale
                                                 || p.ShowingStatus == ProductShowingStatus.OnPreSale);
            if (indexCount <= count)
            {
                response = response.Take(count).ToList();
            }

            return this.Ok(response);
        }
    }
}
