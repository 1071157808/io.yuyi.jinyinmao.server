// FileInformation: nyanya/nyanya.Xingye.Internal/UnShelvesRequest.cs
// CreatedTime: 2014/08/26   5:01 PM
// LastUpdatedTime: 2014/09/01   11:35 AM

using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.RequestModels;

namespace nyanya.Xingye.Internal.Models
{
    /// <summary>
    ///     UnShelvesRequest
    /// </summary>
    public class UnShelvesRequest : IRequestModel
    {
        /// <summary>
        ///     Gets or sets the product no.
        /// </summary>
        /// <value>
        ///     The product no.
        /// </value>
        [Required]
        [MaxLength(50)]
        public string ProductNo { get; set; }
    }
}