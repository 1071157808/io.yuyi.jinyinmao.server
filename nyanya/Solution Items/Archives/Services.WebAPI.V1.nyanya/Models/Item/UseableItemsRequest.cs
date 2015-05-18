// FileInformation: nyanya/Services.WebAPI.V1.nyanya/UseableItemsRequest.cs
// CreatedTime: 2014/08/08   9:18 AM
// LastUpdatedTime: 2014/08/08   9:27 AM

using System.ComponentModel.DataAnnotations;
using Services.WebAPI.Common.Validation;

namespace Services.WebAPI.V1.nyanya.Models
{
    /// <summary>
    ///     UseableItemsRequest
    /// </summary>
    public class UseableItemsRequest
    {
        /// <summary>
        ///     Gets or sets the order identifier.  订单标识
        /// </summary>
        /// <value>
        ///     The order identifier.
        /// </value>
        [Required]
        public string OrderIdentifier { get; set; }

        /// <summary>
        ///     查询的页数，从1开始计数
        /// </summary>
        [Minimum(1)]
        [Required]
        public int PageIndex { get; set; }

        /// <summary>
        ///     一页数据数量
        /// </summary>
        [Minimum(1)]
        [Maximum(20)]
        [Required]
        public int PageSize { get; set; }
    }
}