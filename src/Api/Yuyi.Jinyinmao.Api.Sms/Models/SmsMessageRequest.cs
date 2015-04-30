// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-30  12:10 AM
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
        [Required, JsonProperty("cellphones")]
        public string Cellphones { get; set; }

        /// <summary>
        ///     短信通道
        /// </summary>
        [Required, EnumAvailableValues, JsonProperty("channel")]
        public SmsChannel Channel { get; set; }

        /// <summary>
        ///     短信内容
        /// </summary>
        [Required, JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///     短信签名
        /// </summary>
        [Required, JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
