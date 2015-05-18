// FileInformation: nyanya/Services.WebAPI.V1.nyanya/PaginatedBAProductInfosResponse.cs
// CreatedTime: 2014/07/29   10:36 AM
// LastUpdatedTime: 2014/08/11   12:33 PM

using System.Collections.Generic;
using System.Linq;
using Cqrs.Domain.DTO;
using Cqrs.Domain.Products.ReadModels;
using Cqrs.Domain.Products.Services.DTO;

namespace Services.WebAPI.V1.nyanya.Models
{
    /// <summary>
    ///     银承产品列表
    /// </summary>
    public class PaginatedBAProductInfosResponse
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PaginatedBAProductInfosResponse" /> class.
        ///     Only for document.
        /// </summary>
        public PaginatedBAProductInfosResponse()
        {
            this.Products = new List<BAProductInfoResponse>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PaginatedBAProductInfosResponse" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public PaginatedBAProductInfosResponse(IPaginatedDto<ProductWithSaleInfo<BAProductInfo>> model)
        {
            this.HasNextPage = model.HasNextPage;
            this.PageIndex = model.PageIndex;
            this.PageSize = model.PageSize;
            this.TotalCount = model.TotalCount;
            this.TotalPageCount = model.TotalPageCount;
            this.Products = model.Items.Select(i => i.ToBAProductInfoResponse()).ToList();
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has next page.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has next page; otherwise, <c>false</c>.
        /// </value>
        public bool HasNextPage { get; set; }

        /// <summary>
        ///     Gets or sets the index of the page.
        /// </summary>
        /// <value>
        ///     The index of the page.
        /// </value>
        public int PageIndex { get; set; }

        /// <summary>
        ///     页面大小
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        ///     Gets or sets the products.
        /// </summary>
        /// <value>
        ///     The products.
        /// </value>
        public List<BAProductInfoResponse> Products { get; set; }

        /// <summary>
        ///     总数据数量
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        ///     Gets or sets the total page count.
        /// </summary>
        /// <value>
        ///     The total page count.
        /// </value>
        public int TotalPageCount { get; set; }
    }
}