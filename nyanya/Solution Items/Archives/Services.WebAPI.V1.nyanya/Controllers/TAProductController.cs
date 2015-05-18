// FileInformation: nyanya/Services.WebAPI.V1.nyanya/TAProductController.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/27   4:43 PM

using System.Web.Http;
using Cqrs.Commands.Products;
using Cqrs.Domain.Bus;
using Services.WebAPI.Common.Controller;
using Services.WebAPI.Common.Filters;
using Services.WebAPI.V1.nyanya.Filters;
using Services.WebAPI.V1.nyanya.Models;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>
    ///     商承产品
    /// </summary>
    [RoutePrefix("Product")]
    public class TAProductController : ApiControllerBase
    {
        private readonly ICommandBus commandBus;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BAProductController" /> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        public TAProductController(ICommandBus commandBus)
        {
            this.commandBus = commandBus;
        }

        /// <summary>
        ///     商承产品上架
        /// </summary>
        /// <param name="request">
        ///     SecuredpartyInfo[string]: 担保方信息
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
        /// </param>
        /// <returns>200</returns>
        [Route("TA/HitShelves")]
        [EmptyParameterFilter("request")]
        [ValidateModelState(Order = 1)]
        [IpAuthorize]
        public IHttpActionResult HitShelves(TAHitShelvesRequest request)
        {
            LaunchTAProduct material = request.ToLaunchTAProduct();
            this.commandBus.Excute(material);
            return this.OK();
        }
    }
}