// FileInformation: nyanya/nyanya.Meow/PaginatedTAProductInfosResponse.cs
// CreatedTime: 2014/08/29   2:26 PM
// LastUpdatedTime: 2014/09/01   5:28 PM

using System.Collections.Generic;
using System.Linq;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Domian.DTO;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     商承产品列表
    /// </summary>
    public class PaginatedTAProductInfosResponse
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PaginatedTAProductInfosResponse" /> class.
        ///     Only for document.
        /// </summary>
        public PaginatedTAProductInfosResponse()
        {
            this.Products = new List<TAProductInfoResponse>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PaginatedTAProductInfosResponse" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public PaginatedTAProductInfosResponse(IPaginatedDto<ProductWithSaleInfo<TAProductInfo>> model)
        {
            this.HasNextPage = model.HasNextPage;
            this.PageIndex = model.PageIndex;
            this.PageSize = model.PageSize;
            this.TotalCount = model.TotalCount;
            this.TotalPageCount = model.TotalPageCount;
            List<TAProductInfoResponse> list = model.Items.Select(i => i.ToTAProductInfoResponse()).ToList();
            this.Products = list;
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
        public List<TAProductInfoResponse> Products { get; set; }

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