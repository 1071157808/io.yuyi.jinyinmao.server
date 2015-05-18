// FileInformation: nyanya/Services.WebAPI.V1.nyanya/InvestingController.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/16   10:16 AM

using System;
using System.Threading.Tasks;
using System.Web.Http;
using Cqrs.Commands.Order;
using Cqrs.Domain.Bus;
using Cqrs.Domain.Products.ReadModels;
using Cqrs.Domain.Products.Services.DTO;
using Cqrs.Domain.Products.Services.Interfaces;
using Cqrs.Domain.Users.ReadModels;
using Cqrs.Domain.Users.Services.DTO;
using Cqrs.Domain.Users.Services.Interfaces;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using Services.WebAPI.Common.Controller;
using Services.WebAPI.Common.Filters;
using Services.WebAPI.V1.nyanya.Filters;
using Services.WebAPI.V1.nyanya.Helper;
using Services.WebAPI.V1.nyanya.Models;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>
    ///     InvestingController
    /// </summary>
    [RoutePrefix("Investing")]
    public class InvestingController : ApiControllerBase
    {
        #region Private Fields

        private readonly ICommandBus commandBus;
        private readonly IExactProductInfoService productInfoService;
        private readonly IExactUserInfoService userInfoService;
        private readonly IUserService userService;

        #endregion Private Fields

        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     投资理财产品
        /// </summary>
        /// <param name="request">
        ///     ProductNo[string]: 产品编号
        ///     Count[int]: 购买的份数
        ///     BankCardNo[string]: 银行卡号
        ///     PaymentPassword[string]: 支付密码
        /// </param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     No Content
        ///     @{h2@} HttpStatusCode:400 @{/h2@}
        ///     "为保障您的资金账户安全，请重置支付密码后再试"
        ///     "支付密码错误，您还有{0}次机会"
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
                return this.BadRequest("为保障您的资金账户安全<br>请重置支付密码后再试");
            }

            if (!result.Successful)
            {
                return this.BadRequest("支付密码错误，您还有{0}次机会".FmtWith(result.RemainCount));
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
                return this.BadRequest("对不起，剩余份额已不足！请重新选择！");
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
                return this.BadRequest("支付金额超出银行卡单笔限额<br>请调整支付份额或者更换银行卡");
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
                OrderNo = SequenceNoUtils.GenerateNo('O'),
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

        #endregion Public Methods
    }
}