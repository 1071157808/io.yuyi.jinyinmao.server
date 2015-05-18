// FileInformation: nyanya/nyanya.Meow/SendVeriCodeRequest.cs
// CreatedTime: 2014/08/29   2:26 PM
// LastUpdatedTime: 2014/09/01   5:26 PM

using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.RequestModels;


namespace nyanya.Meow.Models
{
    /// <summary>
    ///     获取升级信息请求参数
    /// </summary>
    public class ObtainRequest : IRequestModel
    {
        /// <summary>
        ///     app市场
        /// </summary>
        [Required]
        public string Channel { get; set; }

        /// <summary>
        ///     平台
        /// </summary>
        [Required]
        public string Source { get; set; }

        /// <summary>
        ///     当前版本号
        /// </summary>
        [Required]
        public string V { get; set; }
    }
}