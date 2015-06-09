// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-10  11:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-23  12:59 PM
// ***********************************************************************
// <copyright file="CurrentProductController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Models.Product;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     CurrentProductController.
    /// </summary>
    [RoutePrefix("Product/Current")]
    public class CurrentProductController : ApiControllerBase
    {
        private readonly IProductInfoService productInfoService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CurrentProductController" /> class.
        /// </summary>
        /// <param name="productInfoService">The product information service.</param>
        public CurrentProductController(IProductInfoService productInfoService)
        {
            this.productInfoService = productInfoService;
        }

        /// <summary>
        ///     获取金包银产品协议模板
        /// </summary>
        /// <remarks>
        ///     需要使用使用产品唯一标识调用接口，接口数据会只读取缓存中的数据
        ///     <br />
        ///     返回值为：{"Content": "协议内容"}
        /// </remarks>
        /// <param name="productIdentifier">产品唯一标识</param>
        /// <param name="agreementIndex">协议序号</param>
        /// <returns>
        ///     Content[string]: 协议内容
        /// </returns>
        /// <response code="200"></response>
        /// <response code="401">CPGA:无此协议</response>
        /// <response code="500"></response>
        [HttpGet, Route("Agreement/{productIdentifier:length(32)}-{agreementIndex:int}")]
        public async Task<IHttpActionResult> GetAgreement(string productIdentifier, int agreementIndex)
        {
            agreementIndex = agreementIndex < 1 ? 1 : agreementIndex;

            Guid productId;
            if (!Guid.TryParseExact(productIdentifier, "N", out productId))
            {
                return this.BadRequest("CPGA:无此协议");
            }

            string content = await this.productInfoService.GetJBYAgreementAsync(productId, agreementIndex);

            if (content.IsNullOrEmpty())
            {
                return this.BadRequest("CPGA:无此协议");
            }

            return this.Ok(new { Content = content });
        }

        /// <summary>
        ///     获取金包银产品信息
        /// </summary>
        /// <remarks>
        ///     接口数据会有1分钟的缓存，包括销售份额数据也会缓存
        ///     <br />
        ///     当该期产品已经售罄时，如果有下一期待售产品，则显示下一期待售产品信息；否则显示已售罄的产品的信息
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="500"></response>
        [HttpGet, Route("JBY"), ResponseType(typeof(JBYInfoResponse))]
        public async Task<IHttpActionResult> GetJBYInfo()
        {
            JBYProductInfo info = await this.productInfoService.GetJBYProductInfoAsync();

            if (info == null)
            {
                return this.BadRequest("敬请期待");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     获取金包银产品的已售金额
        /// </summary>
        /// <remarks>该接口是实时接口，返回值为：{"Paid": "已售金额，以“分”为单位"}</remarks>
        /// <response code="200"></response>
        /// <response code="404">无该产品</response>
        /// <response code="500"></response>
        [HttpGet, Route("Sold/{productIdentifier:length(32)}")]
        public async Task<IHttpActionResult> GetJBYSaleProcess()
        {
            return this.Ok(new { Paid = await this.productInfoService.GetJBYProductPaidAmountAsync() });
        }
    }
}