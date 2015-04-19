// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:14 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-19  5:22 PM
// ***********************************************************************
// <copyright file="SmsMessage.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Yuyi.Jinyinmao.Api.Sms.Models
{
    /// <summary>
    ///     SmsMessage.
    /// </summary>
    public class SmsMessage : TableEntity
    {
        /// <summary>
        ///     手机号，多个号码以,分隔，这里不会验证手机号的格式是否正确
        /// </summary>
        public string Cellphones { get; set; }

        /// <summary>
        ///     短信内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        ///     短信结果
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        ///     短信签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        ///     短信发送时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
