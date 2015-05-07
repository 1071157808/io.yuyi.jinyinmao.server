// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  1:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  3:19 PM
// ***********************************************************************
// <copyright file="AdminController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using System.Web.Http;
using Moe.AspNet.Filters;
using Yuyi.Jinyinmao.Api.Filters;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     AdminProductController.
    /// </summary>
    [Route("Admin"), IpAuthorize]
    public class AdminController : ApiControllerBase
    {
        private readonly IProductInfoService productInfoService;
        private readonly IProductService productService;
        private readonly IUserService userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AdminController" /> class.
        /// </summary>
        /// <param name="productInfoService">The product information service.</param>
        /// <param name="productService">The product service.</param>
        /// <param name="userService">The user service.</param>
        public AdminController(IProductInfoService productInfoService, IProductService productService, IUserService userService)
        {
            this.productInfoService = productInfoService;
            this.productService = productService;
            this.userService = userService;
        }

        /// <summary>
        ///     发行定期理财产品
        /// </summary>
        /// <param name="request">
        ///     The request.
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

            await this.productService.HitShelves(new IssueRegularProduct
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
                FinancingSumCount = request.FinancingSumCount,
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
        /// <param name="productNo">产品编号</param>
        /// <param name="productCategory">产品类别</param>
        /// <response code="200"></response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="401">未授权</response>
        /// <response code="500"></response>
        [Route("RegularProduct/Repay/{productIdentifier:length(32)}-{productNo:minlength(5)}-{productCategory:length(9)}")]
        public IHttpActionResult RegularProductRepay(string productIdentifier, string productNo, string productCategory)
        {
            Guid productId = Guid.ParseExact(productIdentifier, "N");
            this.productService.RepayAsync(productId, productNo, productCategory);

            return this.Ok();
        }

        /// <summary>
        ///     重新刷新用户数据
        /// </summary>
        /// <param name="userIdentifier">用户唯一标识</param>
        /// <response code="200"></response>
        /// <response code="400">请求格式不合法</response>
        /// <response code="401">未授权</response>
        /// <response code="500"></response>
        [Route("User/Reload/{userIdentifier:length(32)}")]
        public async Task<IHttpActionResult> ReloadUserData(string userIdentifier)
        {
            Guid userId = Guid.ParseExact(userIdentifier, "N");
            await this.userService.ReloadDataAsync(userId);

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