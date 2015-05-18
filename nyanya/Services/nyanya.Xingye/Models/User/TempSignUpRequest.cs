// FileInformation: nyanya/nyanya.Xingye/TempSignUpRequest.cs
// CreatedTime: 2014/09/03   10:37 AM
// LastUpdatedTime: 2014/09/03   10:37 AM

using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib;
using nyanya.AspDotNet.Common.RequestModels;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     注册请求
    /// </summary>
    public class TempSignUpRequest : IRequestModel
    {
        /// <summary>
        ///     校验码验证成功口令
        /// </summary>
        [Required]
        public string Token { get; set; }
    }
}
