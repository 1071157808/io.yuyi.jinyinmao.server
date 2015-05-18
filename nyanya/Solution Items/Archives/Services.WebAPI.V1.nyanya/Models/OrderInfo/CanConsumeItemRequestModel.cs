// FileInformation: nyanya/Services.WebAPI.V1.nyanya/CanConsumeItemRequestModel.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/08/09   4:26 PM

using System.ComponentModel.DataAnnotations;
using Services.WebAPI.Common.Validation;

namespace Services.WebAPI.V1.nyanya.Models
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