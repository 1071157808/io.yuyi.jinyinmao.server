// FileInformation: nyanya/nyanya.Xingye/OrderListResponse.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   2:16 PM

using System.Collections.Generic;
using System.Linq;
using Domian.DTO;
using Xingye.Domain.Orders.ReadModels;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     订单列表
    /// </summary>
    public class OrderListResponse
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderListResponse" /> class.
        ///     Only for document.
        /// </summary>
        public OrderListResponse()
        {
            this.Orders = new List<OrderListItem>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderListResponse" /> class.
        /// </summary>
        /// <param name="model">The orders.</param>
        public OrderListResponse(IPaginatedDto<OrderInfo> model)
        {
            this.HasNextPage = model.HasNextPage;
            this.PageIndex = model.PageIndex;
            this.PageSize = model.PageSize;
            this.TotalCount = model.TotalCount;
            this.TotalPageCount = model.TotalPageCount;
            this.Orders = model.Items.Select(i => i.ToOrderListItem()).ToList();
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has next page.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has next page; otherwise, <c>false</c>.
        /// </value>
        public bool HasNextPage { get; set; }

        /// <summary>
        ///     Gets or sets the products.
        /// </summary>
        /// <value>
        ///     The products.
        /// </value>
        public List<OrderListItem> Orders { get; set; }

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