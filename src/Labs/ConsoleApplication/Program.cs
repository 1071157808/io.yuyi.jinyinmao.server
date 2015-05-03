// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-29  8:49 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Newtonsoft.Json;

namespace ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse("BlobEndpoint=https://jymdev.blob.core.chinacloudapi.cn/;QueueEndpoint=https://jymdev.queue.core.chinacloudapi.cn/;TableEndpoint=https://jymdev.table.core.chinacloudapi.cn/;AccountName=jymdev;AccountKey=vtO5YY0USufbaw4BP8gBMIuMe2aPi0an4DkpxakWl579cfTxeCT7mvv7M8oZZkdg8VTxM525WHjPZ6gkifvmiQ==");
            string appKey = "HbX+NpcfkW3oSYRkYKa35dw8CiNEx+bg+4lGRiYYsRUV5YP6sWJ031DYaMS1jgSTOYF8W4gQ+B14oZzJYU1lpxLQCpjBuct299omchoSENoXHEIn7CUxO1i0kbD8FF5f98fZhKCAq4xUHJVpakMkByfoc1MkHcq7GFw45EiwqketEuCZTWx4DLxLh6GyPWD0M5xqtVhVwM9bunnK1R2mcucW8vdONsTKHU5IC9uejom/xMOywS/WkdDDAfKMM6MHuT6nsDD3BMf9/kvjuErei175AQrlmxzLIsEP1qHmhm56bRLTZHAq9NlBvQ64T2pnKlocqF528G1xJnRCZcHAgQ==";

            Guid guid = Guid.NewGuid();

            account.CreateCloudTableClient().GetTableReference("ApiSms").Execute(TableOperation.Insert(new App()
            {
                AppId = guid,
                AppKey = appKey,
                AppName = "SmsClient",
                Expiry = DateTime.Now.AddDays(100),
                Notes = "SmsClient",
                PartitionKey = "api.sms.config.appkeys"
            }));
            Console.WriteLine("a");
        }

        /// <summary>
        ///     App.
        /// </summary>
        public class App : TableEntity
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="App" /> class.
            /// </summary>
            public App()
            {
                this.PartitionKey = "api.sms.config.appkeys";
                this.RowKey = Guid.NewGuid().ToString();
            }

            /// <summary>
            ///     Gets or sets the application identifier.
            /// </summary>
            /// <value>The application identifier.</value>
            public Guid AppId
            {
                get { return Guid.Parse(this.RowKey); }
                set { this.RowKey = value.ToString(); }
            }

            /// <summary>
            ///     Gets or sets the application key.
            /// </summary>
            /// <value>The application key.</value>
            public string AppKey { get; set; }

            /// <summary>
            ///     Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            public string AppName { get; set; }

            /// <summary>
            ///     Gets or sets the expiry.
            /// </summary>
            /// <value>The expiry.</value>
            public DateTime Expiry { get; set; }

            /// <summary>
            ///     Gets or sets the notes.
            /// </summary>
            /// <value>The notes.</value>
            public string Notes { get; set; }
        }
    }
}
