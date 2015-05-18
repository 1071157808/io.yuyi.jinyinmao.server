using Cat.Domain.Orders.Models;
using Domian.DTO;
using System.Collections.Generic;
using System.Linq;

namespace nyanya.Cat.Models
{
    /// <summary>
    ///     资产包认购/提现记录列表
    /// </summary>
    public class ZCBBillListResponse
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderListResponse" /> class.
        ///     Only for document.
        /// </summary>
        public ZCBBillListResponse()
        {
            this.ZCBBills = new List<ZCBBillResponse>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderListResponse" /> class.
        /// </summary>
        /// <param name="model">The orders.</param>
        public ZCBBillListResponse(IPaginatedDto<ZCBBill> model)
        {
            this.HasNextPage = model.HasNextPage;
            this.PageIndex = model.PageIndex;
            this.PageSize = model.PageSize;
            this.TotalCount = model.TotalCount;
            this.TotalPageCount = model.TotalPageCount;
            this.ZCBBills = model.Items.Select(i => i.ToZCBBillResponse()).ToList();
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
        public List<ZCBBillResponse> ZCBBills { get; set; }

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