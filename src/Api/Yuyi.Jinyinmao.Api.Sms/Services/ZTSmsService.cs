// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-25  4:57 PM
// ***********************************************************************
// <copyright file="ZTSmsService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
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
    internal class ZtSmsService : ISmsService
    {
        private static readonly string GetBalanceUrl;
        private static readonly string MessageTemplate;
        private static readonly string Password;
        private static readonly string SendMessageUrl;
        private static readonly CloudStorageAccount StorageAccount;
        private static readonly string UserName;
        private readonly string productId;

        static ZtSmsService()
        {
            SendMessageUrl = "http://www.ztsms.cn:8800/sendSms.do?";
            GetBalanceUrl = "http://www.ztsms.cn:8800/balance.do?";
            string userNameConfig = CloudConfigurationManager.GetSetting("SmsServiceUserName");
            string passwordConfig = CloudConfigurationManager.GetSetting("SmsServicePassword");
            UserName = userNameConfig.IsNotNullOrEmpty() ? userNameConfig : "jymao";
            Password = passwordConfig.IsNotNullOrEmpty() ? passwordConfig : "DRTkGfh9";
            MessageTemplate = "username={0}&password={1}&mobile={2}&content={3}【{4}】&dstime=&productid={5}&xh=";
            StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ZtSmsService" /> class.
        /// </summary>
        /// <param name="priority">The priority.</param>
        public ZtSmsService(int priority)
        {
            switch (priority)
            {
                case 0:
                    this.productId = "676767";
                    break;

                case 1:
                    this.productId = "48661";
                    break;

                default:
                    this.productId = "251503";
                    break;
            }
        }

        #region ISmsService Members

        public async Task<int?> GetBalanceAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string getBalanceRequstUrl = GetBalanceUrl + "username={0}&password={1}".FormatWith(UserName, Password);
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
                    HttpResponseMessage response = await client.GetAsync(SendMessageUrl + MessageTemplate.FormatWith(
                        UserName, Password, cellphones, message, signature, this.productId));
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
                CloudTableClient client = StorageAccount.CreateCloudTableClient();
                client.GetTableReference("Sms").Execute(TableOperation.Insert(new SmsMessage
                {
                    AppId = appId,
                    Cellphones = cellphones,
                    Message = message,
                    Notes = this.productId,
                    PartitionKey = "jinyinmao-api-sms",
                    RowKey = Guid.NewGuid().ToString("N"),
                    Response = responseMessage,
                    Time = DateTime.UtcNow.AddHours(8)
                }));
            }

            if (!responseMessage.StartsWith("1,", StringComparison.Ordinal))
            {
                throw new ApplicationException("Sending message failed. " + responseMessage);
            }
        }

        #endregion ISmsService Members
    }
}