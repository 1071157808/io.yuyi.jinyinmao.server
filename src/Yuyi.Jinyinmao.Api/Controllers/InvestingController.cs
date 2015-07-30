// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : InvestingController.cs
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-29  9:52 AM
// ***********************************************************************
// <copyright file="InvestingController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Moe.AspNet.Filters;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Api.Models.Investing;
using Yuyi.Jinyinmao.Api.Models.Order;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Packages.Helper;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     InvestingController.
    /// </summary>
    [RoutePrefix("Investing")]
    public class InvestingController : ApiControllerBase
    {
        private readonly IProductService productService;
        private readonly IUserInfoService userInfoService;
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvestingController" /> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="userInfoService">The user information service.</param>
        /// <param name="productService">The product information service.</param>
        public InvestingController(IUserService userService, IUserInfoService userInfoService, IProductService productService)
        {
            this.userService = userService;
            this.userInfoService = userInfoService;
            this.productService = productService;
        }

        /// <summary>
        ///     投资金包银理财产品
        /// </summary>
        /// <param name="request">
        ///     金包银理财产品投资请求
        /// </param>
        /// <response code="200"></response>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     IJI1:请重置支付密码后再试
        ///     <br />
        ///     IJI2:支付密码错误&lt;br&gt;错误5次会锁定支付功能
        ///     <br />
        ///     IJI3:账户余额不足
        ///     <br />
        ///     IJI4:产品剩余份额不足"
        ///     <br />
        ///     IJI5:该产品未开售
        ///     <br />
        ///     IJI6:购买金额错误
        ///     <br />
        ///     IJI7:购买失败
        ///     <br />
        ///     IJI8:该产品已售罄
        /// </response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [Route("JBY"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(JBYTransactionInfoResponse))]
        public async Task<IHttpActionResult> JBYInvesting(InvestingRequest request)
        {
            CheckPaymentPasswordResult result = await this.userService.CheckPaymentPasswordAsync(this.CurrentUser.Id, request.PaymentPassword);

            if (result.Lock)
            {
                return this.BadRequest("IJI1:请重置支付密码后再试");
            }

            if (!result.Success)
            {
                return this.BadRequest("IJI2:支付密码错误<br>错误5次会锁定支付功能");
            }

            SettleAccountInfo settleAccountInfo = await this.userInfoService.GetSettleAccountInfoAsync(this.CurrentUser.Id);
            if (settleAccountInfo.Balance < request.Amount)
            {
                return this.BadRequest("IJI3:账户余额不足");
            }

            JBYProductInfo productInfo = await this.productService.GetJBYProductInfoAsync();
            if (productInfo == null || productInfo.FinancingSumAmount - productInfo.PaidAmount < request.Amount || productInfo.SoldOut)
            {
                return this.BadRequest("IJI4:产品剩余份额不足");
            }

            if (!string.Equals(productInfo.ProductId.ToGuidString(), request.ProductIdentifier, StringComparison.InvariantCultureIgnoreCase))
            {
                return this.BadRequest("IJI4:产品剩余份额不足");
            }

            if (productInfo.StartSellTime > DateTime.UtcNow.AddHours(8))
            {
                return this.BadRequest("IJI5:该产品未开售");
            }

            if (request.Amount % productInfo.UnitPrice != 0)
            {
                return this.BadRequest("IJI6:购买金额错误");
            }

            if (productInfo.SoldOut || productInfo.EndSellTime < DateTime.UtcNow.AddHours(8))
            {
                return this.BadRequest("IJI8:该产品已售罄");
            }

            JBYAccountTransactionInfo info = await this.userService.InvestingAsync(new JBYInvesting
            {
                Amount = request.Amount,
                Args = this.BuildArgs(),
                ProductCategory = ProductCategoryCodeHelper.PC100000030,
                UserId = this.CurrentUser.Id
            });

            if (info == null)
            {
                return this.BadRequest("IJI7:购买失败");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     投资定期理财产品
        /// </summary>
        /// <param name="request">
        ///     定期理财产品投资请求
        /// </param>
        /// <response code="200"></response>
        /// <response code="400">
        ///     请求格式不合法
        ///     <br />
        ///     IRI1:请重置支付密码后再试
        ///     <br />
        ///     IRI2:支付密码错误&lt;br&gt;错误5次会锁定支付功能
        ///     <br />
        ///     IRI3:账户余额不足
        ///     <br />
        ///     IRI4:产品剩余份额不足"
        ///     <br />
        ///     IRI5:该产品未开售
        ///     <br />
        ///     IRI6:购买金额错误
        ///     <br />
        ///     IRI7:购买失败
        /// </response>
        /// <response code="401">AUTH:请先登录</response>
        /// <response code="500"></response>
        [Route("Regular"), CookieAuthorize, ActionParameterRequired, ActionParameterValidate(Order = 1), ResponseType(typeof(OrderInfoResponse))]
        public async Task<IHttpActionResult> RegularInvesting(InvestingRequest request)
        {
            Guid productId;
            if (!Guid.TryParseExact(request.ProductIdentifier, "N", out productId))
            {
                return this.BadRequest("请求格式不合法");
            }

            CheckPaymentPasswordResult result = await this.userService.CheckPaymentPasswordAsync(this.CurrentUser.Id, request.PaymentPassword);

            if (result.Lock)
            {
                return this.BadRequest("IRI1:请重置支付密码后再试");
            }

            if (!result.Success)
            {
                return this.BadRequest("IRI2:支付密码错误<br>错误5次会锁定支付功能");
            }

            SettleAccountInfo settleAccountInfo = await this.userInfoService.GetSettleAccountInfoAsync(this.CurrentUser.Id);
            if (settleAccountInfo.Balance < request.Amount)
            {
                return this.BadRequest("IRI3:账户余额不足");
            }

            RegularProductInfo productInfo = await this.productService.GetProductInfoAsync(productId);
            if (productInfo == null || productInfo.FinancingSumAmount - productInfo.PaidAmount < request.Amount || productInfo.SoldOut)
            {
                return this.BadRequest("IRI4:产品剩余份额不足");
            }

            if (productInfo.StartSellTime > DateTime.UtcNow.AddHours(8))
            {
                return this.BadRequest("IRI5:该产品未开售");
            }

            if (request.Amount % productInfo.UnitPrice != 0)
            {
                return this.BadRequest("IRI6:购买金额错误");
            }

            OrderInfo order = await this.userService.InvestingAsync(new RegularInvesting
            {
                Amount = request.Amount,
                Args = this.BuildArgs(),
                CouponId = request.CouponId,
                ProductCategory = productInfo.ProductCategory,
                ProductId = Guid.ParseExact(request.ProductIdentifier, "N"),
                UserId = this.CurrentUser.Id
            });

            if (order == null)
            {
                return this.BadRequest("IRI7:购买失败");
            }

            return this.Ok(order.ToResponse());
        }
    }
}