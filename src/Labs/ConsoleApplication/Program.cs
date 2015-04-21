// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-21  3:58 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Security.Cryptography;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Guid id = Guid.NewGuid();
            string APIKey;

            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] secretKeyByteArray = new byte[32]; //256 bit
                cryptoProvider.GetBytes(secretKeyByteArray);
                APIKey = Convert.ToBase64String(secretKeyByteArray);
            }
            CloudStorageAccount account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("ApiSms");
            table.Execute(TableOperation.Insert(new App
            {
                AppId = id,
                AppKey = APIKey,
                AppName = "SmsClient",
                Expiry = DateTime.Now.AddYears(10),
                Notes = "SmsClient",
                PartitionKey = "api.sms.config.appkeys",
                RowKey = id.ToString("N")
            }));
        }
    }
}
