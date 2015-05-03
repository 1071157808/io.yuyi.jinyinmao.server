// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  7:11 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-03  3:17 PM
// ***********************************************************************
// <copyright file="RegularProductController.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Filters;
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
        ///     需要使用使用产品编号加产品唯一标识调用接口，接口数据会只读取缓存中的数据
        ///     <br />
        ///     返回值为：{"Content": "协议内容"}
        /// </remarks>
        /// <param name="productNo">产品编号</param>
        /// <param name="productIdentifier">产品唯一标识</param>
        /// <param name="agreementIndex">协议序号</param>
        /// <returns>
        ///     Content[string]: 协议内容
        /// </returns>
        /// <response code="200"></response>
        /// <response code="404">无该协议</response>
        /// <response code="500"></response>
        [HttpGet, Route("Agreement/{productNo: minlength(5)}-{productIdentifier:length(32)}-{agreementIndex:int}"), CookieAuthorize]
        public async Task<IHttpActionResult> GetAgreement(string productNo, string productIdentifier, int agreementIndex)
        {
            string content = await this.productInfoService.GetAgreementAsync(productNo, productIdentifier, agreementIndex);

            if (content.IsNotNullOrEmpty())
            {
                return this.NotFound();
            }

            return this.Ok(new { Content = content });
        }

        /// <summary>
        ///     获取产品信息
        /// </summary>
        /// <remarks>需要使用使用产品编号加产品唯一标识调用接口，接口数据会有一分钟的缓存</remarks>
        /// <param name="productNo">产品编号</param>
        /// <param name="productIdentifier">产品唯一标识</param>
        /// <response code="200"></response>
        /// <response code="404">无该产品信息</response>
        /// <response code="500"></response>
        [HttpGet, Route("{productNo:minlength(5)}-{productIdentifier:length(32)}"), ResponseType(typeof(RegularProductInfoResponse))]
        public async Task<IHttpActionResult> GetInfo(string productNo, string productIdentifier)
        {
            RegularProductInfo info = await this.productInfoService.GetProductInfoAsync(productNo, productIdentifier);

            if (info == null)
            {
                return this.NotFound();
            }

            return this.Ok(info.ToResponse());
        }

        /// <summary>
        ///     获取产品信息列表
        /// </summary>
        /// <remarks>
        ///     每页数量为10个，页数从0开始。接口数据有有3分钟的缓存。
        /// </remarks>
        /// <param name="index">页码，从0开始，默认值为0</param>
        /// <param name="category">产品分类，默认值为100000010，详细的产品分类参考文档 </param>
        /// <response code="200"></response>
        /// <response code="404">无该产品信息</response>
        /// <response code="500"></response>
        [HttpGet, Route("Page/{index:int=1:min(1)}/{category:long=100000010}"), ResponseType(typeof(IPaginatedList<RegularProductInfoResponse>))]
        public async Task<IHttpActionResult> Page(int index = 1, long category = 100000010)
        {
            PaginatedList<RegularProductInfo> infos = await this.productInfoService.GetProductInfosAsync(index, 10, category);
            return this.Ok(infos.ToPaginated(i => i.ToResponse()));
        }

        /// <summary>
        ///     获取优先展示的产品信息列表
        /// </summary>
        /// <remarks>
        ///     接口数据有有3分钟的缓存。
        /// </remarks>
        /// <param name="number">数量，默认值为1</param>
        /// <param name="category">产品分类，默认值为100000010，详细的产品分类参考文档 </param>
        /// <response code="200"></response>
        /// <response code="404">无该产品信息</response>
        /// <response code="500"></response>
        [HttpGet, Route("Index/{number:int=1:min(1)}/{category:long=100000010}"), ResponseType(typeof(IList<RegularProductInfoResponse>))]
        public async Task<IHttpActionResult> Index(int number = 1, long category = 100000010)
        {
            IList<RegularProductInfo> infos = await this.productInfoService.GetTopProductInfosAsync(number, category);
            return this.Ok(infos.Select(i => i.ToResponse()));
        }

        /// <summary>
        ///     获取产品的已售金额
        /// </summary>
        /// <remarks>返回值为：{"Paid": "已售金额，以“分”为单位"}</remarks>
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
    }
}