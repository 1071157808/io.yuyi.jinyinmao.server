// FileInformation: nyanya/nyanya.Internal/RepayRequest.cs
// CreatedTime: 2014/08/26   5:01 PM
// LastUpdatedTime: 2014/09/01   11:35 AM

using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.RequestModels;

namespace nyanya.Internal.Models
{
    /// <summary>
    ///     RepayRequest
    /// </summary>
    public class RepayRequest : IRequestModel
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