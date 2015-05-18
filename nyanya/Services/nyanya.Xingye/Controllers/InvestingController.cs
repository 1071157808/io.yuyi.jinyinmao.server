// FileInformation: nyanya/nyanya.Xingye/InvestingController.cs
// CreatedTime: 2014/09/01   10:16 AM
// LastUpdatedTime: 2014/09/02   3:29 PM

using Domian.Bus;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Xingye.Filters;
using nyanya.Xingye.Helper;
using nyanya.Xingye.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Xingye.Commands.Orders;
using Xingye.Domain.Products.ReadModels;
using Xingye.Domain.Products.Services.DTO;
using Xingye.Domain.Products.Services.Interfaces;
using Xingye.Domain.Users.ReadModels;
using Xingye.Domain.Users.Services.DTO;
using Xingye.Domain.Users.Services.Interfaces;

namespace nyanya.Xingye.Controllers
{
    /// <summary>
    ///     InvestingController
    /// </summary>
    [RoutePrefix("Investing")]
    public class InvestingController : XingyeApiControllerBase
    {
        private readonly ICommandBus commandBus;
        private readonly IExactProductInfoService productInfoService;
        private readonly IExactUserInfoService userInfoService;
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvestingController" /> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="productInfoService">The product information service.</param>
        /// <param name="userInfoService">The user information service.</param>
        public InvestingController(IUserService userService, ICommandBus commandBus, IExactProductInfoService productInfoService, IExactUserInfoService userInfoService)
        {
            this.userService = userService;
            this.commandBus = commandBus;
            this.productInfoService = productInfoService;
            this.userInfoService = userInfoService;
        }

        /// <summary>
        ///     投资理财产品
        /// </summary>
        /// <param name="request">
        ///     ProductNo[string]: 产品编号
        ///     Count[int]: 购买的份数
        ///     BankCardNo[string]: 银行卡号
        ///     PaymentPassword[string]: 交易密码
        /// </param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     No Content
        ///     @{h2@} HttpStatusCode:400 @{/h2@}
        ///     "您的交易密码错误已超过3次，请重置交易密码后再进行支付。"
        ///     "交易密码错误，您还有{0}次机会"
        ///     "银行卡信息错误"
        ///     "该产品已停止售卖"
        ///     "对不起，剩余份额已不足！请重新选择！"
        ///     "该产品未开售或已停止售卖"
        ///     "购买份额错误"
        ///     "请稍后再试"
        /// </returns>
        [HttpPost, Route("")]
        [TokenAuthorize]
        [EmptyParameterFilter("request", Order = 1), ValidateModelState(Order = 2)]
        public async Task<IHttpActionResult> Investing(InvestingRequest request)
        {
            CheckPaymentPasswordResult result = await this.userService.CheckPaymentPasswordAsync(this.CurrentUser.Identifier, request.PaymentPassword);

            if (result.Lock)
            {
                return this.BadRequest("您的交易密码错误已超过3次，请重置交易密码后再进行支付。");
            }

            if (!result.Successful)
            {
                return this.BadRequest("交易密码错误，您还有{0}次机会。".FmtWith(result.RemainCount));
            }

            PaymentBankCardInfo paymentBankCardInfo = await this.userInfoService.GetBankCardInfoAsync(this.CurrentUser.Identifier, request.BankCardNo);

            if (paymentBankCardInfo == null)
            {
                return this.BadRequest("银行卡信息错误");
            }

            ProductWithSaleInfo<ProductInfo> product = await this.productInfoService.GetProductWithSaleInfoByNoAsync(request.ProductNo);

            if (product == null)
            {
                return this.BadRequest("该产品已停止售卖");
            }

            if (product.AvailableShareCount < request.Count)
            {
                return this.BadRequest("对不起，剩余份额已不足，请重新选择。");
            }

            if (!product.ProductInfo.OnSale)
            {
                return this.BadRequest("该产品未开售或已停止售卖");
            }

            if (product.ProductInfo.MinShareCount > request.Count || product.ProductInfo.MaxShareCount < request.Count)
            {
                return this.BadRequest("购买份额错误");
            }

            if (!BankCardHelper.CheckDailyLimit(paymentBankCardInfo.BankName, request.Count * product.ProductInfo.UnitPrice))
            {
                return this.BadRequest("支付金额超出银行卡单笔限额，请调整支付份额或者更换银行卡。");
            }

            this.commandBus.Excute(new BuildInvestingOrder(this.CurrentUser.Identifier, GuidUtils.NewGuidString())
            {
                EndorseImageLink = product.ProductInfo.EndorseImageLink,
                EndorseImageThumbnailLink = product.ProductInfo.EndorseImageThumbnailLink,
                InvestorBankCardNo = paymentBankCardInfo.BankCardNo,
                InvestorBankName = paymentBankCardInfo.BankName,
                InvestorCity = paymentBankCardInfo.City,
                InvestorCellphone = paymentBankCardInfo.Cellphone,
                InvestorCredential = paymentBankCardInfo.Credential,
                InvestorCredentialNo = paymentBankCardInfo.CredentialNo,
                InvestorRealName = paymentBankCardInfo.RealName,
                MaxShareCount = product.ProductInfo.MaxShareCount,
                MinShareCount = product.ProductInfo.MinShareCount,
                OrderNo = SequenceNoUtils.GenerateNo('X'),
                OrderTime = DateTime.Now,
                ProductIdentifier = product.ProductInfo.ProductIdentifier,
                ProductName = product.ProductInfo.ProductName,
                ProductNo = product.ProductInfo.ProductNo,
                ProductNumber = product.ProductInfo.ProductNumber,
                ProductType = product.ProductInfo.ProductType,
                RepaymentDeadline = product.ProductInfo.RepaymentDeadline,
                SettleDate = product.ProductInfo.SettleDate,
                ShareCount = request.Count,
                UnitPrice = product.ProductInfo.UnitPrice,
                ValueDate = product.ProductInfo.ValueDate,
                ValueDateMode = product.ProductInfo.ValueDateMode,
                Yield = product.ProductInfo.Yield
            });

            return this.OK();
        }
    }
}