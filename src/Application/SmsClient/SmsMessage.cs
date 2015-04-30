// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-17  10:12 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-19  12:54 PM
// ***********************************************************************
// <copyright file="SmsMessage.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Newtonsoft.Json;

namespace SmsClient
{
    public class SmsMessage
    {
        /// <summary>
        ///     手机号，多个号码以,分隔，这里不会验证手机号的格式是否正确
        /// </summary>
        [JsonProperty("cellphones")]
        public string Cellphones { get; set; }

        /// <summary>
        ///     短信通道
        /// </summary>
        [JsonProperty("channel")]
        public string Channel { get; set; }

        /// <summary>
        ///     短信内容
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///     短信签名
        /// </summary>
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
