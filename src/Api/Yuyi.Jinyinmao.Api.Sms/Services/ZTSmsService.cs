// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-28  12:42 PM
// ***********************************************************************
// <copyright file="ZTSmsService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Yuyi.Jinyinmao.Api.Sms.Models;

namespace Yuyi.Jinyinmao.Api.Sms.Services
{
    /// <summary>
    ///     Class ZTSmsService.
    /// </summary>
    internal class ZTSmsService : ISmsService
    {
        private static readonly string getBalanceUrl;
        private static readonly string messageTemplate;
        private static readonly string password;
        private static readonly string sendMessageUrl;
        private static readonly CloudStorageAccount storageAccount;
        private static readonly string userName;
        private readonly string productId;

        static ZTSmsService()
        {
            sendMessageUrl = "http://www.ztsms.cn:8800/sendSms.do?";
            getBalanceUrl = "http://www.ztsms.cn:8800/balance.do?";
            string userNameConfig = ConfigurationManager.AppSettings.Get("SmsServiceUserName");
            string passwordConfig = ConfigurationManager.AppSettings.Get("SmsServicePassword");
            userName = userNameConfig.IsNotNullOrEmpty() ? userNameConfig : "jymao";
            password = passwordConfig.IsNotNullOrEmpty() ? passwordConfig : "DRTkGfh9";
            messageTemplate = "username={0}&password={1}&mobile={2}&content={3}【{4}】&dstime=&productid={5}&xh=";
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ZTSmsService" /> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        public ZTSmsService(int priority)
        {
            if (priority == 0)
            {
                this.productId = "676767";
            }
            else if (priority == 1)
            {
                this.productId = "48661";
            }
            else
            {
                this.productId = "251503";
            }
        }

        #region ISmsService Members

        public async Task<int?> GetBalanceAsync()
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
        /// <param name="appId"></param>
        /// <param name="cellphones">The cellphones.</param>
        /// <param name="message">The message.</param>
        /// <param name="signature">The signature.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task SendMessageAsync(string appId, string cellphones, string message, string signature)
        {
            string responseMessage = "";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(sendMessageUrl + messageTemplate.FormatWith(
                        userName, password, cellphones, message, signature, this.productId));
                    responseMessage = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                responseMessage = e.Message;
                throw;
            }
            finally
            {
                CloudTableClient client = storageAccount.CreateCloudTableClient();
                client.GetTableReference("ApiSms").Execute(TableOperation.Insert(new SmsMessage
                {
                    AppId = appId,
                    Cellphones = cellphones,
                    Message = message,
                    Notes = this.productId,
                    PartitionKey = "api.sms.data.messages",
                    RowKey = Guid.NewGuid().ToString("N"),
                    Response = responseMessage,
                    Time = DateTime.UtcNow.AddHours(8)
                }));
            }

            if (!responseMessage.StartsWith("1,"))
            {
                throw new ApplicationException("Sending message failed. " + responseMessage);
            }
        }

        #endregion ISmsService Members
    }
}
