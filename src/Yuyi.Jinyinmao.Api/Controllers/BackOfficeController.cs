// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  1:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  8:51 PM
// ***********************************************************************
// <copyright file="BackOfficeController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using System.Web.Http;
using Moe.AspNet.Filters;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Api.Models.BackOffice;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Packages.Helper;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     BackOfficeController.
    /// </summary>
    [Route("BackOffice"), HMACAuthentication, IpAuthorize]
    public class BackOfficeController : ApiControllerBase
    {
        private readonly IProductInfoService productInfoService;
        private readonly IProductService productService;
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BackOfficeController" /> class.
        /// </summary>
        /// <param name="productInfoService">The product information service.</param>
        /// <param name="productService">The product service.</param>
        /// <param name="userService">The user service.</param>
        public BackOfficeController(IProductInfoService productInfoService, IProductService productService, IUserService userService)
        {
            this.productInfoService = productInfoService;
            this.productService = productService;
            this.userService = userService;
        }

        /// <summary>
        ///     发行金包银理财产品
        /// </summary>
        /// <param name="request">
        ///     JBY产品上架请求
        /// </param>
        /// <response code="200">上架成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">上架失败：产品编号已存在</response>
        /// <response code="401">未授权</response>
        /// <response code="500"></response>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [Route("RegularProduct/Issue"), ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> JBYProductIssue(IssueJBYProductRequest request)
        {
            bool result = await this.productInfoService.CheckProductNoExistsAsync(request.ProductNo);
            if (result)
            {
                return this.BadRequest("上架失败：产品编号已存在");
            }

            DailyConfig config = DailyConfigHelper.GetDailyConfig(DateTime.UtcNow.AddHours(8));

            await this.productService.HitShelvesAsync(new IssueJBYProduct
            {
                Agreement1 = request.Agreement1,
                Agreement2 = request.Agreement2,
                Args = this.BuildArgs(),
                EndSellTime = request.EndSellTime,
                FinancingSumAmount = request.FinancingSumAmount,
                IssueNo = request.IssueNo,
                IssueTime = DateTime.UtcNow.AddHours(8),
                ProductCategory = request.ProductCategory,
                ProductId = Guid.NewGuid(),
                ProductName = request.ProductName,
                ProductNo = request.ProductNo,
                StartSellTime = request.StartSellTime,
                UnitPrice = request.UnitPrice,
                ValueDateMode = request.ValueDateMode,
                WithdrawalLimit = config.JBYWithdrawalLimit,
                Yield = config.JBYYield
            });

            return this.Ok();
        }

        /// <summary>
        ///     发行定期理财产品
        /// </summary>
        /// <param name="request">
        ///     产品上架请求
        /// </param>
        /// <response code="200">上架成功</response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="400">上架失败：产品编号已存在</response>
        /// <response code="401">未授权</response>
        /// <response code="500"></response>
        /// <returns>Task&lt;IHttpActionResult&gt;.</returns>
        [Route("RegularProduct/Issue"), ActionParameterRequired, ActionParameterValidate(Order = 1)]
        public async Task<IHttpActionResult> RegularProductIssue(IssueProductRequest request)
        {
            bool result = await this.productInfoService.CheckProductNoExistsAsync(request.ProductNo);
            if (result)
            {
                return this.BadRequest("上架失败：产品编号已存在");
            }

            await this.productService.HitShelvesAsync(new IssueRegularProduct
            {
                Agreement1 = request.Agreement1,
                Agreement2 = request.Agreement2,
                Args = this.BuildArgs(),
                BankName = request.BankName,
                Drawee = request.Drawee,
                DraweeInfo = request.DraweeInfo,
                EndorseImageLink = request.EndorseImageLink,
                EndSellTime = request.EndSellTime,
                EnterpriseInfo = request.EnterpriseInfo,
                EnterpriseLicense = request.EnterpriseLicense,
                EnterpriseName = request.EnterpriseName,
                FinancingSumCount = request.FinancingSumAmount,
                IssueNo = request.IssueNo,
                Period = request.Period,
                PledgeNo = request.PledgeNo,
                ProductCategory = request.ProductCategory,
                ProductId = Guid.NewGuid(),
                ProductName = request.ProductName,
                ProductNo = request.ProductNo,
                RepaymentDeadline = request.RepaymentDeadline.Date,
                RiskManagement = request.RiskManagement,
                RiskManagementInfo = request.RiskManagementInfo,
                RiskManagementMode = request.RiskManagementMode,
                SettleDate = request.SettleDate.Date,
                StartSellTime = request.StartSellTime,
                UnitPrice = request.UnitPrice,
                Usage = request.Usage,
                ValueDate = request.ValueDate,
                ValueDateMode = request.ValueDateMode,
                Yield = request.Yield
            });

            return this.Ok();
        }

        /// <summary>
        ///     定期理财产品还款通知
        /// </summary>
        /// <param name="productIdentifier">产品唯一标识</param>
        /// <response code="200"></response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="401">未授权</response>
        /// <response code="500"></response>
        [Route("RegularProduct/Repay/{productIdentifier:length(32)}}")]
        public IHttpActionResult RegularProductRepay(string productIdentifier)
        {
            Guid productId = Guid.ParseExact(productIdentifier, "N");
            this.productService.RepayAsync(productId);

            return this.Ok();
        }

        /// <summary>
        ///     用户取现到账
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        /// <param name="transcationIdentifier">交易流水唯一标识</param>
        /// <response code="200"></response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="401">未授权</response>
        /// <response code="500"></response>
        [Route("Withdrawal/{userIdentifier:length(32)}-{transcationIdentifier:length(32)}")]
        public async Task<IHttpActionResult> WithdrawalTranscationFinished(string userIdentifier, string transcationIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            Guid transcationId = Guid.ParseExact(transcationIdentifier, "N");
            await this.userService.WithdrawalResultedAsync(userId, transcationId);

            return this.Ok();
        }
    }
}