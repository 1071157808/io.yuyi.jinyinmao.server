// FileInformation: nyanya/nyanya.Meow/UseableItemsRequest.cs
// CreatedTime: 2014/09/01   10:27 AM
// LastUpdatedTime: 2014/09/01   11:34 AM

using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Meow.Models
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