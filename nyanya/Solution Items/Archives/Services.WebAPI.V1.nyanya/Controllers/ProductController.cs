// FileInformation: nyanya/Services.WebAPI.V1.nyanya/ProductController.cs
// CreatedTime: 2014/08/14   7:21 AM
// LastUpdatedTime: 2014/08/14   7:38 AM

using System.Threading.Tasks;
using System.Web.Http;
using Cqrs.Commands.Products;
using Cqrs.Domain.Bus;
using Cqrs.Domain.Products.Services.DTO;
using Cqrs.Domain.Products.Services.Interfaces;
using Services.WebAPI.Common.Controller;
using Services.WebAPI.Common.Filters;
using Services.WebAPI.V1.nyanya.Filters;
using Services.WebAPI.V1.nyanya.Models;

namespace Services.WebAPI.V1.nyanya.Controllers
{
    /// <summary>
    ///     ProductController
    /// </summary>
    [RoutePrefix("Product")]
    public class ProductController : ApiControllerBase
    {
        #region Private Fields

        private readonly ICommandBus commandBus;
        private readonly IProductService productService;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductController" /> class.
        /// </summary>
        /// <param name="productService">The product service.</param>
        /// <param name="commandBus">The command bus.</param>
        public ProductController(IProductService productService, ICommandBus commandBus)
        {
            this.productService = productService;
            this.commandBus = commandBus;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     产品还款通知接口
        /// </summary>
        /// <param name="request">
        ///     ProductNo[string]:产品编号
        /// </param>
        /// <returns>200 | 400</returns>
        [Route("Repay")]
        [IpAuthorize]
        [EmptyParameterFilter("request", Order = 1)]
        [ValidateModelState(Order = 2)]
        public async Task<IHttpActionResult> UnShelves(RepayRequest request)
        {
            CanRepayResult result = await this.productService.CanRepayAsync(request.ProductNo);
            if (!result.Result)
            {
                return this.BadRequest("该产品不能还款");
            }

            this.commandBus.Excute(new ProductRepay(result.ProductIdentifier));

            return this.OK();
        }

        #endregion Public Methods
    }
}