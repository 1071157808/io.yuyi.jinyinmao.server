
using System.Collections.Generic;
using System.Linq;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Domian.DTO;

namespace nyanya.Cat.Models
{
    /// <summary>
    ///     资产包产品列表
    /// </summary>
    public class PaginatedZCBProductInfosResponse
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PaginatedZCBProductInfosResponse" /> class.
        ///     Only for document.
        /// </summary>
        public PaginatedZCBProductInfosResponse()
        {
            this.Products = new List<ZCBProductInfoResponse>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PaginatedZCBProductInfosResponse" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public PaginatedZCBProductInfosResponse(IPaginatedDto<ProductWithSaleInfo<ZCBProductInfo>> model)
        {
            this.HasNextPage = model.HasNextPage;
            this.PageIndex = model.PageIndex;
            this.PageSize = model.PageSize;
            this.TotalCount = model.TotalCount;
            this.TotalPageCount = model.TotalPageCount;
            this.Products = model.Items.Select(i => i.ToZCBProductInfoResponse()).ToList();
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
        public List<ZCBProductInfoResponse> Products { get; set; }

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