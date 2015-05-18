﻿// FileInformation: nyanya/Services.WebAPI.V1.nyanya/ItemConsumeRequest.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/29   2:41 PM

using System.ComponentModel.DataAnnotations;

namespace Services.WebAPI.V1.nyanya.Models
{
    /// <summary>
    ///     ItemConsumeRequest
    /// </summary>
    public class ItemConsumeRequest
    {
        /// <summary>
        ///     Gets or sets the item identifier.
        /// </summary>
        /// <value>
        ///     The item identifier.
        /// </value>
        [Required]
        [Range(1, 999999999)]
        public int ItemId { get; set; }

        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>
        ///     The order identifier.
        /// </value>
        [Required]
        public string OrderIdentifier { get; set; }
    }
}