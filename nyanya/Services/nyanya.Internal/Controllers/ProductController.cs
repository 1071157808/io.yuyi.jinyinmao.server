// FileInformation: nyanya/nyanya.Internal/ProductController.cs
// CreatedTime: 2014/08/27   4:36 PM
// LastUpdatedTime: 2014/09/01   5:25 PM

using Cat.Commands.Orders;
using Cat.Commands.Products;
using Cat.Domain.Products.Services.DTO;
using Cat.Domain.Products.Services.Interfaces;
using Domian.Bus;
using Infrastructure.Lib.Utility;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Internal.Filters;
using nyanya.Internal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace nyanya.Internal.Controllers
{
    /// <summary>
    ///     ProductController
    /// </summary>
    [RoutePrefix("Product")]
    public class ProductController : ApiControllerBase
    {
        private readonly ICommandBus commandBus;
        private readonly IProductService productService;
        private readonly IZCBProductInfoService zcbProductService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductController" /> class.
        /// </summary>
        /// <param name="productService">The product service.</param>
        /// <param name="commandBus">The command bus.</param>
        public ProductController(IProductService productService, ICommandBus commandBus,IZCBProductInfoService zcbProductService)
        {
            this.productService = productService;
            this.commandBus = commandBus;
            this.zcbProductService = zcbProductService;
        }

        /// <summary>
        ///     银承产品上架
        /// </summary>
        /// <param name="request">
        ///     BankName[string]: 银行名称
        ///     BillNo[string]: 银票编号
        ///     BusinessLicense[string]: 营业执照号码
        ///     EndorseImageLink[string]: 票据大图链接
        ///     EndorseImageThumbnailLink[string]: 票据缩略图链接
        ///     EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     EnterpriseName[string]: 企业名称
        ///     ConsignmentAgreement[string]: 委托协议
        ///     FinancingSumCount[int]: 融资总份数
        ///     MaxShareCount[int]: 单笔订单最大购买份数
        ///     MinShareCount[int]: 单笔订单最小购买份数
        ///     Period[int]: 项目周期
        ///     PreEndSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买结束时间
        ///     PreStartSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买开始时间
        ///     PledgeAgreement[string]:担保协议
        ///     ProductName[string]: 产品名称
        ///     ProductNo[string]: 产品编号，银承产品以F开头
        ///     ProductNumber[int]: 产品期数
        ///     RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日期
        ///     StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     UnitPrice[int]: 每一份的单价
        ///     Usage[string]: 融资用途
        ///     ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 指定日期起息
        ///     Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        ///     SettleDate[yyyy-MM-ddTHH:mm:ss]:结息日
        ///     ProductCategory[int]：产品分类 10 => 金银猫产品 20 => 富滇产品 40 => 阜新产品（默认金银猫产品）
        /// </param>
        /// <returns>200 | 400</returns>
        [Route("BA/HitShelves")]
        [IpAuthorize]
        [EmptyParameterFilter("request", Order = 1)]
        [ValidateModelState(Order = 2)]
        public async Task<IHttpActionResult> HitShelves(BAHitShelvesRequest request)
        {
            var result = await this.productService.CheckProductNoExists(request.ProductNo);
            if (result)
            {
                return this.BadRequest("该产品编号已经存在，请核实");
            }
            LaunchBAProduct material = request.ToLaunchBAProduct();
            this.commandBus.Excute(material);
            return this.OK();
        }

        /// <summary>
        ///     商承产品上架
        /// </summary>
        /// <param name="request">
        ///     SecuredpartyInfo[string]: 担保方信息（还款来源）
        ///     Securedparty[string]: 担保方
        ///     BillNo[string]: 编号
        ///     EnterpriseLicense[string]: 营业执照号码
        ///     EndorseImageLink[string]: 票据大图链接
        ///     EndorseImageThumbnailLink[string]: 票据缩略图链接
        ///     EndSellTime[yyyy-MM-ddTHH:mm:ss]: 售卖结束时间
        ///     EnterpriseName[string]: 企业名称
        ///     ConsignmentAgreement[string]: 委托协议
        ///     FinancingSumCount[int]: 融资总份数
        ///     MaxShareCount[int]: 单笔订单最大购买份数
        ///     MinShareCount[int]: 单笔订单最小购买份数
        ///     Period[int]: 项目周期
        ///     PreEndSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买结束时间
        ///     PreStartSellTime[yyyy-MM-ddTHH:mm:ss]: 提前购买开始时间
        ///     PledgeAgreement[string]:担保协议
        ///     ProductName[string]: 产品名称
        ///     ProductNo[string]: 产品编号，银承产品以F开头
        ///     ProductNumber[int]: 产品期数
        ///     RepaymentDeadline[yyyy-MM-ddTHH:mm:ss]: 最迟还款日期
        ///     StartSellTime[yyyy-MM-ddTHH:mm:ss]: 开售时间
        ///     UnitPrice[int]: 每一份的单价
        ///     Usage[string]: 融资用途
        ///     ValueDate[yyyy-MM-ddTHH:mm:ss|""]: 固定起息日期
        ///     ValueDateMode[int]: 起息方式 10 => 购买当天起息, 20 => 指定日期起息
        ///     Yield[decimal]: 收益率（保留位数为最小有效位数，即可能是7.3或者7.34）
        ///     Drawee[string]：付款方
        ///     DraweeInfo[string]:付款方信息
        ///     EnterpriseInfo[string]:融资方信息
        ///     PledgeAgreementName[string]:担保协议题目
        ///     ConsignmentAgreementName[string]:委托协议题名
        ///     SettleDate[yyyy-MM-ddTHH:mm:ss]:结息日
        ///     GuaranteeMode[int]：担保方式
        ///     ProductCategory[int]：产品分类 10 => 金银猫产品 30 => 施秉金鼎产品 （默认金银猫产品）
        /// </param>
        /// <returns>200</returns>
        [Route("TA/HitShelves")]
        [EmptyParameterFilter("request")]
        [ValidateModelState(Order = 1)]
        [IpAuthorize]
        public async Task<IHttpActionResult> HitShelves(TAHitShelvesRequest request)
        {
            var result = await this.productService.CheckProductNoExists(request.ProductNo);
            if (result)
            {
                return this.BadRequest("该产品编号已经存在，请核实");
            }
            LaunchTAProduct material = request.ToLaunchTAProduct();
            this.commandBus.Excute(material);
            return this.OK();
        }

        private async Task<IHttpActionResult> HitShelves(ZCBHitShelvesRequest request)
        {
            var result = await this.productService.CheckProductNoExists(request.ProductNo);
            if (result)
            {
                return this.BadRequest("该产品编号已经存在，请核实");
            }
            LaunchZCBProduct material = request.ToLaunchZCBProduct();
            this.commandBus.Excute(material);
            return this.OK();
        }

        /// <summary>
        ///     银承产品下架
        /// </summary>
        /// <param name="request">
        ///     ProductNo[string]:产品编号，银承产品以F开头
        /// </param>
        /// <returns>200 | 400</returns>
        [Route("BA/UnShelves")]
        [IpAuthorize]
        [EmptyParameterFilter("request", Order = 1)]
        [ValidateModelState(Order = 2)]
        public async Task<IHttpActionResult> UnShelves(UnShelvesRequest request)
        {
            CanUnShelvesResult result = await this.productService.CanUnShelvesAsync(request.ProductNo);
            if (!result.Result)
            {
                return this.BadRequest("该产品不能下架");
            }

            this.commandBus.Excute(new BAProductUnShelves(result.ProductIdentifier));

            return this.OK();
        }

        /// <summary>
        ///     产品还款通知接口
        /// </summary>
        /// <param name="request">
        ///     ProductNo[string]:产品编号
        /// </param>
        /// <returns>200 | 400</returns>
        [Route("Repay")]
        [IpAuthorize]
        [EmptyParameterFilter("request", Order = 1)]
        [ValidateModelState(Order = 2)]
        public async Task<IHttpActionResult> UnShelves(RepayRequest request)
        {
            CanRepayResult result = await this.productService.CanRepayAsync(request.ProductNo);
            if (!result.Result)
            {
                return this.BadRequest("该产品不能还款");
            }

            this.commandBus.Excute(new ProductRepay(result.ProductIdentifier));

            return this.OK();
        }

        /// <summary>
        ///     更新资产包产品可售份额
        ///     可以更新的条件（在售卖期，份额已买完；不在售买期；没有开启售卖）
        /// </summary>
        /// <param name="request">
        ///     ProductNo[string]：产品编号(目前只有一个产品，约定产品编号为: ZCB0001)
        ///     ProductName[string]：产品名称
        ///     FinancingSumCount[int]：融资总份数
        ///     UnitPrice[int]：每一份的单价
        ///     NextStartSellTime[yyyy-MM-dd HH:mm:ss]: 下次开售时间
        ///     NextEndSellTime[yyyy-MM-dd HH:mm:ss]: 下次售卖结束时间
        ///     NextYield[decimal]: 下次售卖年化收益
        ///     EnableSale[int]：是否开启售卖（0否 1是）
        ///     PledgeAgreementName[string]:投资协议名称
        ///     PledgeAgreement[string]:投资协议
        ///     ConsignmentAgreementName[string]:授权委托书名称
        ///     ConsignmentAgreement[string]: 授权委托书
        ///     PerRemainRedeemAmount[decimal]：产品下次可取款额度
        /// </param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     No Content
        ///     @{h2@} HttpStatusCode:400 @{/h2@}
        ///     "有未卖完的份额或有正在购买的份额，稍后再试"
        /// </returns>
        [Route("ZCB/UpdateShareCount")]
        [IpAuthorize]
        [EmptyParameterFilter("request", Order = 1)]
        [ValidateModelState(Order = 2)]
        public async Task<IHttpActionResult> UpdateShareCountShelves(ZCBUpdateShelvesRequest request)
        {
            var result = await this.productService.CheckProductNoExists(request.ProductNo);
            string productIdentifier = "";
            //如果产品不存在，则初始化产品信息
            if (!result)
            {
                ZCBHitShelvesRequest zcbHitShelvesRequest = request.ToZCBHitShelves();
                await this.HitShelves(zcbHitShelvesRequest);
            }
            else
            {
                CanUpdateShareCountResult canResult = await this.productService.CanUpdateShareCountAsync(request.ProductNo);
                if (string.IsNullOrEmpty(canResult.ProductIdentifier))
                {
                    return this.BadRequest("没有改产品编号，请核实");
                }
                //产品下架
                if (request.EnableSale == 0)
                {
                    await this.zcbProductService.UnShelvesZcbProduct(canResult.ProductIdentifier);
                    return this.Ok();
                }
                
                if (!canResult.Result)
                {
                    return this.BadRequest("有未卖完的份额或有正在购买的份额，请稍后再试！");
                }
                productIdentifier = canResult.ProductIdentifier;
                this.commandBus.Excute(request.ToZCBUpdateShareCount(productIdentifier));
            }
            return this.OK();
        }

        /// <summary>
        ///    后台回款后返回结果接口
        /// </summary>
        /// <param name="request">
        ///  SN[list] : 打款成功的列表
        /// </param>
        [Route("ZCB/SetRedeemBillResult")]
        [IpAuthorize]
        [EmptyParameterFilter("request", Order = 1)]
        public async Task<IHttpActionResult> SetZCBRedeemBillResult(ZCBRedeemBillRequest request)
        {
            if (request.SN != null && request.SN.Count > 0)
            {
                IList<string> snList = request.SN;
                this.commandBus.Excute(new SetZCBRedeemBillsResult(snList));
            }

            return this.OK();
        }
    }
}