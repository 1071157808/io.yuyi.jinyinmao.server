// FileInformation: nyanya/Services.WebAPI.V1.nyanya/FeedbackRequestModel.cs
// CreatedTime: 2014/07/21   12:52 AM
// LastUpdatedTime: 2014/07/21   12:56 AM

using System.ComponentModel.DataAnnotations;
using Services.WebAPI.Common.RequestModels;

namespace Services.WebAPI.V1.nyanya.Models
{
    /// <summary>
    /// 反馈请求
    /// </summary>
    public class FeedbackRequest : IRequestModel
    {
        /// <summary>
        /// 反馈内容
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(200, ErrorMessage = "反馈内容不能超过200字")]
        public string Content { get; set; }
    }
}