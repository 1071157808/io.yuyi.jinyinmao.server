// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-06  6:08 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ConsoleApplication
{
    internal class Program
    {
        private static readonly TableQuery<App> query = new TableQuery<App>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey",
            QueryComparisons.Equal, "jinyinmao"), TableOperators.And, "JymApiSms eq true"));

        private static Dictionary<Guid, App> allowedApps = new Dictionary<Guid, App>();
        private static DateTime configLoadTime = DateTime.MinValue;
        private static CloudStorageAccount storageAccount;

        private static void LoadAppKeysConfig()
        {
            if (DateTime.UtcNow - configLoadTime > TimeSpan.FromMinutes(5))
            {
                configLoadTime = DateTime.UtcNow;
                CloudTableClient client = storageAccount.CreateCloudTableClient();

                IEnumerable<App> apps = client.GetTableReference("AppKeys").ExecuteQuery(query);
                Dictionary<Guid, App> tempAllowedApps = apps.Where(a => a.Expiry > DateTime.UtcNow.AddHours(8)).ToDictionary(app => app.AppId);
                allowedApps = tempAllowedApps;
            }
        }

        private static void Main(string[] args)
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            LoadAppKeysConfig();
        }

        #region Nested type: App

        internal class App : TableEntity
        {
            /// <summary>
            ///     Gets or sets the application identifier.
            /// </summary>
            /// <value>The application identifier.</value>
            public Guid AppId { get; set; }

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
        }

        #endregion Nested type: App
    }
}