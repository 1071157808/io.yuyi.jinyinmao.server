// FileInformation: nyanya/nyanya.Xingye.Internal/NoticeRequest.cs
// CreatedTime: 2014/09/11   3:52 PM
// LastUpdatedTime: 2014/09/11   3:52 PM

using System;
using System.ComponentModel.DataAnnotations;
using Xingye.Commands.Products;
using nyanya.AspDotNet.Common.RequestModels;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Xingye.Internal.Models
{
    /// <summary>
    ///     公告请求
    /// </summary>
    public class NoticeRequest : IRequestModel
    {
        /// <summary>
        ///     Gets or sets the content of the notice.
        /// </summary>
        [Required]
        [MaxLength(20000)]
        public string Content { get; set; }

        /// <summary>
        ///     Gets or sets the expire time of the notice.
        /// </summary>
        [Required]
        public string ExpireTime { get; set; }

        /// <summary>
        ///     Gets or sets if notice is available.
        /// </summary>
        [Required]
        public bool Flag { get; set; }

    }
}