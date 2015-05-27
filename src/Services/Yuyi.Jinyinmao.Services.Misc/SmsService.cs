// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-26  10:33 PM
// ***********************************************************************
// <copyright file="SmsService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure;
using Moe.Lib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     SmsMessage.
    /// </summary>
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

    /// <summary>
    ///     Class SmsService.
    /// </summary>
    public class SmsService : ISmsService
    {
        private static readonly string apiBaseAddress;
        private static readonly bool smsServiceEnable;

        static SmsService()
        {
            bool temp;
            string smsServiceEnableConfig = CloudConfigurationManager.GetSetting("SmsServiceEnable");
            string smsServiceAddressConfig = CloudConfigurationManager.GetSetting("SmsServiceAddress");
            smsServiceEnable = smsServiceEnableConfig.IsNotNullOrEmpty() && bool.TryParse(smsServiceEnableConfig, out temp) && temp;
            apiBaseAddress = smsServiceAddressConfig.IsNotNullOrEmpty() && smsServiceAddressConfig.IsUrl() ? smsServiceAddressConfig : "https://jym-dev-apisms.yuyidev.com/";
        }

        #region ISmsService Members

        /// <summary>
        ///     Gets the balance asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<int> GetBalanceAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                int balance = 9999999;
                string getBalanceRequstUrl = apiBaseAddress + "Balance?channel=100001";
                HttpResponseMessage response = await client.GetAsync(getBalanceRequstUrl);
                string responseContent = await response.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(responseContent);
                if (Convert.ToBoolean(result.GetValue("supportBalanceQuery").ToString()))
                {
                    balance = Convert.ToInt32(result.GetValue("balance").ToString());
                }
                return balance;
            }
        }

        /// <summary>
        ///     Sends the message asynchronous.
        /// </summary>
        /// <param name="cellphones">The cellphones.</param>
        /// <param name="message">The message.</param>
        /// <param name="signature">The signature.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> SendMessageAsync(string cellphones, string message, string signature = "金银猫")
        {
            if (!smsServiceEnable)
            {
                return true;
            }

            string channel = message.Contains("验证码") ? "100001" : "100002";
            SmsMessage smsMessage = new SmsMessage { Cellphones = cellphones, Channel = channel, Message = message, Signature = signature };
            using (HttpClient client = HttpClientFactory.Create(new ApiKeyAuthDelegatingHandler()))
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(apiBaseAddress + "Send", smsMessage);
                return response.IsSuccessStatusCode;
            }
        }

        #endregion ISmsService Members
    }
}