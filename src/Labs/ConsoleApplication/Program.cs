// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-19  3:31 PM
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
            string APIKey;

            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] secretKeyByteArray = new byte[32]; //256 bit
                cryptoProvider.GetBytes(secretKeyByteArray);
                APIKey = Convert.ToBase64String(secretKeyByteArray);
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient client = storageAccount.CreateCloudTableClient();

            App app = new App
            {
                AppId = Guid.NewGuid(),
                AppKey = APIKey,
                Expiry = DateTime.Now.AddYears(100),
                AppName = "SmsClient",
                Notes = "SmsClient"
            };

            TableResult r = client.GetTableReference("ApiSms").Execute(TableOperation.Insert(app));
            Console.WriteLine(r.Result);
        }
    }
}
