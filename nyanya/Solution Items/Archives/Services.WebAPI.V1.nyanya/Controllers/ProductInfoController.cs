// FileInformation: nyanya/Services.WebAPI.V1.nyanya/ProductInfoController.cs
// CreatedTime: 2014/08/11   12:28 PM
// LastUpdatedTime: 2014/08/15   8:25 PM

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Cqrs.Domain.Products.Services.Interfaces;
using Infrastructure.Cache.Couchbase;
using Microsoft.Ajax.Utilities;
using Services.WebAPI.Common.Filters;
using Services.WebAPI.V1.nyanya.Filters;
using Services.WebAPI.V1.nyanya.Models;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>
    ///     ProductInfo
    /// </summary>
    [RoutePrefix("ProductInfo")]
    public class ProductInfoController : ApiController
    {
        #region Private Fields

        private readonly IProductInfoService productInfoService;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductInfoController" /> class.
        /// </summary>
        /// <param name="productInfoService">The product information service.</param>
        public ProductInfoController(IProductInfoService productInfoService)
        {
            this.productInfoService = productInfoService;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     产品协议模板
        /// </summary>
        /// <param name="productIdentifier">The product identifier.</param>
        /// <returns>
        ///     Content[string]: 协议内容
        /// </returns>
        [HttpGet, Route("ConsignmentAgreement"), Route("ConsignmentAgreement/{productIdentifier:length(32)}")]
        [TokenAuthorize]
        public async Task<IHttpActionResult> GetConsignmentAgreement(string productIdentifier)
        {
            string content = await this.productInfoService.GetConsignmentAgreementAsync(productIdentifier);

            if (content.IsNullOrWhiteSpace())
            {
                return this.BadRequest("协议不存在");
            }

            return this.Ok(new { Content = content });
        }

        /// <summary>
        ///     产品协议模板
        /// </summary>
        /// <param name="productIdentifier">The product identifier.</param>
        /// <returns>
        ///     Content[string]: 协议内容
        /// </returns>
        [HttpGet, Route("PledgeAgreement"), Route("PledgeAgreement/{productIdentifier:length(32)}")]
        [TokenAuthorize]
        public async Task<IHttpActionResult> GetPledgeAgreement(string productIdentifier)
        {
            string content = await this.productInfoService.GetPledgeAgreementAsync(productIdentifier);

            if (content.IsNullOrWhiteSpace())
            {
                return this.BadRequest("协议不存在");
            }

            return this.Ok(new { Content = content });
        }

        /// <summary>
        ///     获取产品的销售进度信息
        /// </summary>
        /// <param name="productIdentifier">项目唯一标识，32位字符串，不是项目编号</param>
        /// <returns>
        ///     Available[int]: 可购买份额
        ///     Sum[int]: 融资金额
        ///     Paid[int]: 已付款份数
        ///     Paying[int]: 付款中份数
        /// </returns>
        [HttpGet, Route("SaleProcess"), Route("SaleProcess/{productIdentifier:length(32)}")]
        [ParameterRequire("productIdentifier")]
        [ResponseType(typeof(ProductSaleProcessResponse))]
        public IHttpActionResult GetSaleProcess(string productIdentifier)
        {
            ProductShareCacheModel info = this.productInfoService.GetProductSaleProcess(productIdentifier);
            return this.Ok(info.ToProductSaleProcessResponse());
        }

        #endregion Public Methods
    }
}