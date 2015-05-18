// FileInformation: nyanya/nyanya.Cat/CanConsumeItemRequestModel.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   11:25 AM

using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Cat.Models
{
    /// <summary>
    ///     CanConsumeItemRequestModel
    /// </summary>
    public class CanConsumeItemRequestModel
    {
        /// <summary>
        ///     Gets or sets the item id.
        /// </summary>
        /// <value>
        ///     The item id.
        /// </value>
        [Required]
        [Minimum(1)]
        public int ItemId { get; set; }

        /// <summary>
        ///     查询的页数，从1开始计数
        /// </summary>
        /// <value>
        ///     The index of the page.
        /// </value>
        [Required]
        [Minimum(1)]
        public int PageIndex { get; set; }

        /// <summary>
        ///     一页数据数量
        /// </summary>
        /// <value>
        ///     The size of the page.
        /// </value>
        [Minimum(1)]
        [Maximum(20)]
        [Required]
        public int PageSize { get; set; }
    }
}