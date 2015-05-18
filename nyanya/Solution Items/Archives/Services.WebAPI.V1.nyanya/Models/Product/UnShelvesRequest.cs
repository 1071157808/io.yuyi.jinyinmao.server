// FileInformation: nyanya/Services.WebAPI.V1.nyanya/UnShelvesRequest.cs
// CreatedTime: 2014/07/30   5:41 PM
// LastUpdatedTime: 2014/08/11   12:11 PM

using System.ComponentModel.DataAnnotations;
using Services.WebAPI.Common.RequestModels;

namespace Services.WebAPI.V1.nyanya.Models
{
    /// <summary>
    ///     UnShelvesRequest
    /// </summary>
    public class UnShelvesRequest : IRequestModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the product no.
        /// </summary>
        /// <value>
        ///     The product no.
        /// </value>
        [Required]
        [MaxLength(50)]
        public string ProductNo { get; set; }

        #endregion Public Properties
    }
}