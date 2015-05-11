// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  7:11 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  12:19 AM
// ***********************************************************************
// <copyright file="RegularProductController.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Models;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     RegularProductController.
    /// </summary>
    [RoutePrefix("Product/Regular")]
    public class RegularProductController : ApiController
    {
        private readonly IProductInfoService productInfoService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegularProductController" /> class.
        /// </summary>
        /// <param name="productInfoService">The product information service.</param>
        public RegularProductController(IProductInfoService productInfoService)
        {
            this.productInfoService = productInfoService;
        }

        /// <summary>
        ///     获取产品协议模板
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
        /// <response code="401">RPGA:无此协议</response>
        /// <response code="500"></response>
        [HttpGet, Route("Agreement/{productIdentifier:length(32)}-{agreementIndex:int}")]
        public async Task<IHttpActionResult> GetAgreement(string productIdentifier, int agreementIndex)
        {
            agreementIndex = agreementIndex < 1 ? 1 : agreementIndex;

            Guid productId;
            if (!Guid.TryParseExact(productIdentifier, "N", out productId))
            {
                return this.BadRequest("RPGA:无此协议");
            }

            string content = await this.productInfoService.GetAgreementAsync(productId, agreementIndex);

            if (content.IsNotNullOrEmpty())
            {
                return this.BadRequest("RPGA:无此协议");
            }

            return this.Ok(new { Content = content });
        }

        /// <summary>
        ///     获取产品信息
        /// </summary>
        /// <remarks>需要使用产品唯一标识调用接口，接口数据会有一分钟的缓存，包括销售份额数据也会缓存</remarks>
        /// <param name="productIdentifier">产品唯一标识</param>
        /// <response code="200"></response>
        /// <response code="400">RPRI:无此产品信息</response>
        /// <response code="500"></response>
        [HttpGet, Route("{productIdentifier:length(32)}"), ResponseType(typeof(RegularProductInfoResponse))]
        public async Task<IHttpActionResult> GetInfo(string productIdentifier)
        {
            Guid productId;
            if (!Guid.TryParseExact(productIdentifier, "N", out productId))
            {
                return this.BadRequest("RPRI:无此产品信息");
            }

            RegularProductInfo info = await this.productInfoService.GetProductInfoAsync(productId);

            if (info == null)
            {
                return this.BadRequest("RPRI:无此产品信息");
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     获取产品的已售金额
        /// </summary>
        /// <remarks>该接口是实时接口，返回值为：{"Paid": "已售金额，以“分”为单位"}</remarks>
        /// <param name="productIdentifier">项目唯一标识，32位字符串，不是项目编号</param>
        /// <response code="200"></response>
        /// <response code="404">无该产品</response>
        /// <response code="500"></response>
        [HttpGet, Route("Sold/{productIdentifier:length(32)}")]
        public async Task<IHttpActionResult> GetSaleProcess(string productIdentifier)
        {
            Guid productId;
            if (Guid.TryParseExact(productIdentifier, "N", out productId))
            {
                return this.Ok(new { Paid = await this.productInfoService.GetProductPaidAmountAsync(productId) });
            }

            return this.NotFound();
        }

        /// <summary>
        ///     获取优先展示的产品信息列表
        /// </summary>
        /// <remarks>
        ///     接口数据有有3分钟的缓存。
        /// </remarks>
        /// <param name="number">数量，最小为1</param>
        /// <param name="categories">产品分类，默认值为100000010，详细的产品分类参考文档，可以传递数组 </param>
        /// <response code="200"></response>
        /// <response code="404">无该产品信息</response>
        /// <response code="500"></response>
        [HttpGet, Route("Index/{number:int=1:min(1)}"), ResponseType(typeof(IList<RegularProductInfoResponse>))]
        public async Task<IHttpActionResult> Index(int number = 1, [FromUri] long[] categories = null)
        {
            number = number < 1 ? 1 : number;

            if (categories == null)
            {
                categories = new long[] { 100000010 };
            }

            IList<RegularProductInfo> infos = await this.productInfoService.GetTopProductInfosAsync(number, categories);
            return this.Ok(infos.Select(i => i.ToResponse()));
        }

        /// <summary>
        ///     获取产品信息列表
        /// </summary>
        /// <remarks>
        ///     每页数量为10个，页数从0开始。接口数据有有3分钟的缓存。
        /// </remarks>
        /// <param name="index">页码，从0开始，最小为0</param>
        /// <param name="categories">产品分类，默认值为100000010，详细的产品分类参考文档，可以传递数组 </param>
        /// <response code="200"></response>
        /// <response code="404">无该产品信息</response>
        /// <response code="500"></response>
        [HttpGet, Route("Page/{index:int=0:min(0)}"), ResponseType(typeof(PaginatedResponse<RegularProductInfoResponse>))]
        public async Task<IHttpActionResult> Page(int index = 0, [FromUri] long[] categories = null)
        {
            index = index < 0 ? 0 : index;

            if (categories == null)
            {
                categories = new long[] { 100000010 };
            }

            PaginatedList<RegularProductInfo> infos = await this.productInfoService.GetProductInfosAsync(index, 10, categories);
            return this.Ok(infos.ToPaginated(i => i.ToResponse()).ToResponse());
        }
    }
}