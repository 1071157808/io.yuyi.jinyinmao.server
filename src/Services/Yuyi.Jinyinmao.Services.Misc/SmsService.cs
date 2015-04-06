// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  4:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-06  5:16 PM
// ***********************************************************************
// <copyright file="SmsService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Services
{
    /// <summary>
    ///     Class SmsService.
    /// </summary>
    public class SmsService : ISmsService
    {
        private static readonly string getBalanceUrl;
        private static readonly string messageTemplate;
        private static readonly string password;
        private static readonly string sendMessageUrl;
        private static readonly bool smsServiceEnable;
        private static readonly string userName;

        static SmsService()
        {
            sendMessageUrl = "http://www.ztsms.cn:8800/sendSms.do?";
            getBalanceUrl = "http://www.ztsms.cn:8800/balance.do?";
            string userNameConfig = ConfigurationManager.AppSettings.Get("SmsServiceUserName");
            string passwordConfig = ConfigurationManager.AppSettings.Get("SmsServicePassword");
            string smsServiceEnableConfig = ConfigurationManager.AppSettings.Get("SmsServiceEnable");
            userName = userNameConfig.IsNotNullOrEmpty() ? userNameConfig : "jymao";
            password = passwordConfig.IsNotNullOrEmpty() ? passwordConfig : "DRTkGfh9";
            smsServiceEnable = smsServiceEnableConfig.IsNotNullOrEmpty();
            messageTemplate = "username={0}&password={1}&mobile={2}&content={3}【{4}】&dstime=&productid={5}&xh=";
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
                string getBalanceRequstUrl = getBalanceUrl + "username={0}&password={1}".FormatWith(userName, password);
                HttpResponseMessage response = await client.GetAsync(getBalanceRequstUrl);
                return (await response.Content.ReadAsStringAsync()).ToInt();
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

            string productId = message.Contains("验证码") ? "676767" : "48661";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(sendMessageUrl + messageTemplate.FormatWith(
                    userName, password, cellphones, message, signature, productId));
                return (await response.Content.ReadAsStringAsync()).StartsWith("1,");
            }
        }

        #endregion ISmsService Members
    }
}
