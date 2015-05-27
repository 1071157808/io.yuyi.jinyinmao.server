// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  8:27 PM
// ***********************************************************************
// <copyright file="SmsMessage.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SmsClient
{
    /// <summary>
    ///     SmsMessage.
    /// </summary>
    public class SmsMessage
    {
        /// <summary>
        ///     手机号，多个号码以,分隔，这里不会验证手机号的格式是否正确
        /// </summary>
        [JsonProperty("cellphones"), SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string Cellphones { get; set; }

        /// <summary>
        ///     短信通道
        /// </summary>
        [JsonProperty("channel"), SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string Channel { get; set; }

        /// <summary>
        ///     短信内容
        /// </summary>
        [JsonProperty("message"), SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string Message { get; set; }

        /// <summary>
        ///     短信签名
        /// </summary>
        [JsonProperty("signature"), SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string Signature { get; set; }
    }
}