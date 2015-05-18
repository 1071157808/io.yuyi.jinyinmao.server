// FileInformation: nyanya/nyanya.Cat/FeedbackRequestModel.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   11:25 AM

using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.RequestModels;

namespace nyanya.Cat.Models
{
    /// <summary>
    ///     反馈请求
    /// </summary>
    public class FeedbackRequest : IRequestModel
    {
        /// <summary>
        ///     反馈内容
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(2000, ErrorMessage = "反馈内容不能超过2000字")]
        public string Content { get; set; }
    }
}
