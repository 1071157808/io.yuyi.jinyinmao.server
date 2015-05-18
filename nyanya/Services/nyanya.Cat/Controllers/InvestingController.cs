// ***********************************************************************
// Assembly         : nyanya
// Author           : Siqi Lu
// Created          : 2015-03-04  6:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-26  5:02 PM
// ***********************************************************************
// <copyright file="InvestingController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Cat.Commands.Orders;
using Cat.Commands.Products;
using Cat.Domain.Orders.Models;
using Cat.Domain.Orders.Services.Interfaces;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Cat.Domain.Products.Services.Interfaces;
using Cat.Domain.Users.ReadModels;
using Cat.Domain.Users.Services.DTO;
using Cat.Domain.Users.Services.Interfaces;
using Domian.Bus;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using nyanya.AspDotNet.Common.Controller;
using nyanya.AspDotNet.Common.Filters;
using nyanya.Cat.Filters;
using nyanya.Cat.Helper;
using nyanya.Cat.Models;
using ProductInfo = Cat.Domain.Products.ReadModels.ProductInfo;

namespace nyanya.Cat.Controllers
{
    /// <summary>
    ///     InvestingController
    /// </summary>
    [RoutePrefix("Investing")]
    public class InvestingController : ApiControllerBase
    {
        private readonly ICommandBus commandBus;
        private readonly IExactProductInfoService productInfoService;
        private readonly IExactUserInfoService userInfoService;
        private readonly IUserService userService;
        private readonly IZCBOrderService zcbOrderService;
        private readonly IZCBProductInfoService zcbProductInfoService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvestingController" /> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="productInfoService">The product information service.</param>
        /// <param name="userInfoService">The user information service.</param>
        /// <param name="zcbProductInfoService">The zcbproduct information service.</param>
        /// <param name="zcbOrderService">The ZCB order service.</param>
        public InvestingController(IUserService userService, ICommandBus commandBus, IExactProductInfoService productInfoService, IExactUserInfoService userInfoService, IZCBProductInfoService zcbProductInfoService, IZCBOrderService zcbOrderService)
        {
            this.userService = userService;
            this.commandBus = commandBus;
            this.productInfoService = productInfoService;
            this.userInfoService = userInfoService;
            this.zcbProductInfoService = zcbProductInfoService;
            this.zcbOrderService = zcbOrderService;
        }

        /// <summary>
        ///     投资理财产品
        /// </summary>
        /// <param name="request">
        ///     ProductNo[string]: 产品编号
        ///     Count[int]: 购买的份数
        ///     BankCardNo[string]: 银行卡号
        ///     PaymentPassword[string]: 支付密码
        ///     ActivityNo[decimal]：活动本金券类型(非必填参数)【0.1=>增益券0.1%，0.2=>增益券0.2%，0.3=>增益券0.3%，0.4=>增益券0.4%，0.5=>增益券0.5%，0.6=>增益券0.6%】[500 => 500 本金券,本金券 150119 活动所用]
        ///     ClientType[long]: 客户端标识(推广相关)
        ///     FlgMoreI1[long]: FlgMoreI1(推广相关)
        ///     FlgMoreI2[long]: FlgMoreI2(推广相关)
        ///     FlgMoreS1[string]: FlgMoreS1(推广相关)
        ///     FlgMoreS2[string]: FlgMoreS2(推广相关)
        /// </param>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     OrderNo[string]: 订单编号
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
        [HttpPost, Route(""), TokenAuthorize, EmptyParameterFilter("request", Order = 1), ValidateModelState(Order = 2)]
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
                this.Warning("Can not load card date.UserID：{0}-CardNo：{1}".FormatWith(this.CurrentUser.Identifier, request.BankCardNo));
                return this.BadRequest("银行卡信息错误");
            }

            ProductWithSaleInfo<ProductInfo> product = await this.productInfoService.GetProductWithSaleInfoByNoAsync(request.ProductNo);

            if (product == null)
            {
                return this.BadRequest("该产品已停止售卖");
            }

            if (product.ProductInfo.ProductType == ProductType.ZCBAcceptance)
            {
                int enableSale = await this.zcbProductInfoService.CheckEnableSaleZcbProduct(product.ProductInfo.ProductIdentifier);
                if (ProductResponseHelper.GetZcbProductShowingStatus(product.ProductInfo, enableSale, product.AvailableShareCount, product.PayingShareCount) == ProductShowingStatus.Finished)
                {
                    return this.BadRequest("该产品已停止售卖");
                }
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
            string tempOrderNo = SequenceNoUtils.GenerateNo('O');
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
                OrderNo = tempOrderNo,
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
                Yield = product.ProductInfo.Yield,
                ProductCategory = product.ProductInfo.ProductCategory,
                SubProductNo = product.ProductInfo.ProductNo + "_" + DateTime.Now.ToString("yyyyMMdd"),
                ActivityNo = (int)(10 * request.ActivityNo),
                ClientType = request.ClientType,
                FlgMoreI1 = request.FlgMoreI1,
                FlgMoreI2 = request.FlgMoreI2,
                FlgMoreS1 = request.FlgMoreS1.ToStringIncludeNull(),
                FlgMoreS2 = request.FlgMoreS2.ToStringIncludeNull(),
                IpClient = HttpContext.Current.Request.UserHostAddress
            });

            return this.Ok(new OrderNoResponse
            {
                OrderNo = tempOrderNo
            });
        }

        /// <summary>
        ///     金包银复投
        /// </summary>
        /// <returns>
        ///     @{h2@} HttpStatusCode:200 @{/h2@}
        ///     OrderNo[string]: 订单编号
        ///     @{h2@} HttpStatusCode:400 @{/h2@}
        ///     "对不起，剩余份额已不足！请重新选择！"
        ///     "现在无正在售卖的金包银产品"
        ///     "购买份额错误"
        ///     "请稍后再试"
        /// </returns>
        [HttpGet, Route(""), TokenAuthorize]
        public async Task<IHttpActionResult> Reinvesting()
        {
            return this.OK();

            BankCardSummaryInfo bankCard = (await this.userInfoService.GetBankCardsInfoAsync(this.CurrentUser.Identifier)).First(c => c.IsDefault);
            PaymentBankCardInfo paymentBankCardInfo = await this.userInfoService.GetBankCardInfoAsync(this.CurrentUser.Identifier, bankCard.BankCardNo);
            ZCBUser zcbUser = await this.zcbOrderService.GetZCBUserAsync(this.CurrentUser.Identifier);
            int count = (int)(zcbUser.TotalPrincipal - zcbUser.TotalRedeemInterest);
            IList<ProductWithSaleInfo<ZCBProductInfo>> infos = await this.zcbProductInfoService.GetTopProductWithSaleInfosAsync(1);
            if (infos == null || infos.Count == 0)
            {
                return this.BadRequest("现在无正在售卖的金包银产品");
            }

            var product = infos.First();

            if (product.ProductInfo.ProductType == ProductType.ZCBAcceptance)
            {
                int enableSale = await this.zcbProductInfoService.CheckEnableSaleZcbProduct(product.ProductInfo.ProductIdentifier);
                if (ProductResponseHelper.GetZcbProductShowingStatus(product.ProductInfo, enableSale, product.AvailableShareCount, product.PayingShareCount) == ProductShowingStatus.Finished)
                {
                    return this.BadRequest("现在无正在售卖的金包银产品");
                }
            }

            if (!product.ProductInfo.OnSale)
            {
                return this.BadRequest("该产品未开售或已停止售卖");
            }

            string tempOrderNo = SequenceNoUtils.GenerateNo('Z');
            this.commandBus.Excute(new BuildInvestingOrder(this.CurrentUser.Identifier, GuidUtils.NewGuidString())
            {
                EndorseImageLink = product.ProductInfo.EndorseImageLink,
                EndorseImageThumbnailLink = product.ProductInfo.EndorseImageThumbnailLink,
                InvestorBankCardNo = bankCard.BankCardNo,
                InvestorBankName = paymentBankCardInfo.BankName,
                InvestorCity = paymentBankCardInfo.City,
                InvestorCellphone = paymentBankCardInfo.Cellphone,
                InvestorCredential = paymentBankCardInfo.Credential,
                InvestorCredentialNo = paymentBankCardInfo.CredentialNo,
                InvestorRealName = paymentBankCardInfo.RealName,
                MaxShareCount = 1000000,
                MinShareCount = 1,
                OrderNo = tempOrderNo,
                OrderTime = DateTime.Now,
                ProductIdentifier = product.ProductInfo.ProductIdentifier,
                ProductName = product.ProductInfo.ProductName,
                ProductNo = product.ProductInfo.ProductNo,
                ProductNumber = product.ProductInfo.ProductNumber,
                ProductType = product.ProductInfo.ProductType,
                RepaymentDeadline = product.ProductInfo.RepaymentDeadline,
                SettleDate = product.ProductInfo.SettleDate,
                ShareCount = count,
                UnitPrice = 1,
                ValueDate = product.ProductInfo.ValueDate,
                ValueDateMode = product.ProductInfo.ValueDateMode,
                Yield = product.ProductInfo.Yield,
                ProductCategory = product.ProductInfo.ProductCategory,
                SubProductNo = product.ProductInfo.ProductNo + "_" + DateTime.Now.ToString("yyyyMMdd"),
                ActivityNo = 0,
                ClientType = 0,
                FlgMoreI1 = 0,
                FlgMoreI2 = 0,
                FlgMoreS1 = "",
                FlgMoreS2 = "",
                IpClient = HttpContext.Current.Request.UserHostAddress
            });

            return this.Ok(new OrderNoResponse
            {
                OrderNo = tempOrderNo
            });
        }
    }
}
