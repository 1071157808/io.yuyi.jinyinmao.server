﻿// FileInformation: nyanya/Services.WebAPI.V1.nyanya/BAProductController.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/11   12:25 PM

using System.Threading.Tasks;
using System.Web.Http;
using Cqrs.Commands.Products;
using Cqrs.Domain.Bus;
using Cqrs.Domain.Products.Services.DTO;
using Cqrs.Domain.Products.Services.Interfaces;
using Services.WebAPI.Common.Controller;
using Services.WebAPI.Common.Filters;
using Services.WebAPI.V1.nyanya.Filters;
using Services.WebAPI.V1.nyanya.Models;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>
    ///     银承产品接口
    /// </summary>
    [RoutePrefix("Product")]
    public class BAProductController : ApiControllerBase
    {

        private readonly ICommandBus commandBus;
        private readonly IProductService productService;



        /// <summary>
        ///     Initializes a new instance of the <see cref="BAProductController" /> class.
        /// </summary>
        /// <param name="productService">The product service.</param>
        /// <param name="commandBus">The command bus.</param>
        public BAProductController(IProductService productService, ICommandBus commandBus)
        {
            this.productService = productService;
            this.commandBus = commandBus;
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
        /// </param>
        /// <returns>200 | 400</returns>
        [Route("BA/HitShelves")]
        [IpAuthorize]
        [EmptyParameterFilter("request", Order = 1)]
        [ValidateModelState(Order = 2)]
        public IHttpActionResult HitShelves(HitShelvesRequest request)
        {
            LaunchBAProduct material = request.ToLaunchBAProduct();
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

    }
}