// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  4:13 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-22  6:53 PM
// ***********************************************************************
// <copyright file="SiloClusterConfig.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     SiloClusterConfig.
    /// </summary>
    public static class SiloClusterConfig
    {
        static SiloClusterConfig()
        {
            string storageConnectionString = CloudConfigurationManager.GetSetting("DataConnectionString");
            CloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);

            CloudTableClient tableClient = CloudStorageAccount.CreateCloudTableClient();
            tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromMilliseconds(500), 6);
            tableClient.DefaultRequestOptions.MaximumExecutionTime = TimeSpan.FromMinutes(5);
            tableClient.DefaultRequestOptions.ServerTimeout = TimeSpan.FromMinutes(5);
            ErrorLogsTable = tableClient.GetTableReference("Errors");
            CacheTable = tableClient.GetTableReference("Cache");
            SagasTable = tableClient.GetTableReference("Sagas");

            CloudBlobClient blobClient = CloudStorageAccount.CreateCloudBlobClient();
            blobClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromMilliseconds(500), 6);
            blobClient.DefaultRequestOptions.MaximumExecutionTime = TimeSpan.FromMinutes(5);
            blobClient.DefaultRequestOptions.ServerTimeout = TimeSpan.FromMinutes(5);
            PublicFileContainer = blobClient.GetContainerReference("publicfiles");
            PrivateFileContainer = blobClient.GetContainerReference("privatefiles");
            CommandStoreContainer = blobClient.GetContainerReference("commands");
            EventStoreContainer = blobClient.GetContainerReference("events");

            ServiceBusConnectiongString = CloudConfigurationManager.GetSetting("ServiceBusConnectiongString");
        }

        /// <summary>
        ///     Gets the cache table.
        /// </summary>
        /// <value>The product cache table.</value>
        public static CloudTable CacheTable { get; }

        /// <summary>
        ///     Gets the cloud storage account.
        /// </summary>
        /// <value>The cloud storage account.</value>
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
        public static CloudStorageAccount CloudStorageAccount { get; private set; }

        /// <summary>
        ///     Gets the command store container.
        /// </summary>
        /// <value>The command store container.</value>
        public static CloudBlobContainer CommandStoreContainer { get; }

        /// <summary>
        ///     Gets the error logs table.
        /// </summary>
        /// <value>The error logs table.</value>
        public static CloudTable ErrorLogsTable { get; }

        /// <summary>
        ///     Gets the event store container.
        /// </summary>
        /// <value>The event store container.</value>
        public static CloudBlobContainer EventStoreContainer { get; }

        /// <summary>
        ///     Gets the private file container.
        /// </summary>
        /// <value>The private file container.</value>
        public static CloudBlobContainer PrivateFileContainer { get; }

        /// <summary>
        ///     Gets the public file container.
        /// </summary>
        /// <value>The public file container.</value>
        public static CloudBlobContainer PublicFileContainer { get; }

        /// <summary>
        ///     Gets or sets the sagas table.
        /// </summary>
        /// <value>The sagas table.</value>
        public static CloudTable SagasTable { get; }

        /// <summary>
        ///     Gets or sets the service bus connectiong string.
        /// </summary>
        /// <value>The service bus connectiong string.</value>
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
        public static string ServiceBusConnectiongString { get; private set; }
    }
}