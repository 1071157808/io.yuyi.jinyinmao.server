// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  12:03 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-19  1:49 PM
// ***********************************************************************
// <copyright file="SmsMessageRequest.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Moe.AspNet.Validations;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Api.Sms.Services;

namespace Yuyi.Jinyinmao.Api.Sms.Models
{
    /// <summary>
    ///     SmsMessageRequest.
    /// </summary>
    public class SmsMessageRequest : IRequest
    {
        /// <summary>
        ///     手机号，多个号码以,分隔，这里不会验证手机号的格式是否正确
        /// </summary>
        [Required, JsonProperty(PropertyName = "cellphones")]
        public string Cellphones { get; set; }

        /// <summary>
        ///     短信通道
        /// </summary>
        [Required, EnumAvailableValues, JsonProperty(PropertyName = "channel")]
        public SmsChannel Channel { get; set; }

        /// <summary>
        ///     短信内容
        /// </summary>
        [Required, JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        ///     短信签名
        /// </summary>
        [Required, JsonProperty(PropertyName = "signature")]
        public string Signature { get; set; }
    }
}
