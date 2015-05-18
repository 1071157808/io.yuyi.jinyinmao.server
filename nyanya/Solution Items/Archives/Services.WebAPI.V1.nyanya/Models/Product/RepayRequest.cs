using System.ComponentModel.DataAnnotations;
using Services.WebAPI.Common.RequestModels;

namespace Services.WebAPI.V1.nyanya.Models
{
    /// <summary>
    ///     RepayRequest
    /// </summary>
    public class RepayRequest : IRequestModel
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