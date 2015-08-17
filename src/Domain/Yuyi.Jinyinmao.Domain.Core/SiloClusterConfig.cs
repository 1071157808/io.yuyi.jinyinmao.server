// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SiloClusterConfig.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  14:24
// ***********************************************************************
// <copyright file="SiloClusterConfig.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
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
    [SuppressMessage("ReSharper", "MemberCanBeInternal")]
    public static class SiloClusterConfig
    {
        static SiloClusterConfig()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;

            string storageConnectionString = CloudConfigurationManager.GetSetting("DataConnectionString");
            ServiceBusConnectionString = CloudConfigurationManager.GetSetting("ServiceBusConnectionString");

            CloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);

            CloudTableClient tableClient = CloudStorageAccount.CreateCloudTableClient();
            tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), 6);
            tableClient.DefaultRequestOptions.MaximumExecutionTime = TimeSpan.FromMinutes(5);
            tableClient.DefaultRequestOptions.ServerTimeout = TimeSpan.FromMinutes(5);
            CacheTable = tableClient.GetTableReference("JYMCache");
            SagasTable = tableClient.GetTableReference("JYMSagaLogs");

            CloudBlobClient blobClient = CloudStorageAccount.CreateCloudBlobClient();
            blobClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), 6);
            blobClient.DefaultRequestOptions.MaximumExecutionTime = TimeSpan.FromMinutes(5);
            blobClient.DefaultRequestOptions.ServerTimeout = TimeSpan.FromMinutes(5);
            PublicFileContainer = blobClient.GetContainerReference("publicfiles");
            PrivateFileContainer = blobClient.GetContainerReference("privatefiles");
            CommandStoreContainer = blobClient.GetContainerReference("commands");
            EventStoreContainer = blobClient.GetContainerReference("events");
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
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static CloudStorageAccount CloudStorageAccount { get; }

        /// <summary>
        ///     Gets the command store container.
        /// </summary>
        /// <value>The command store container.</value>
        public static CloudBlobContainer CommandStoreContainer { get; }

        /// <summary>
        ///     Gets the event store container.
        /// </summary>
        /// <value>The event store container.</value>
        public static CloudBlobContainer EventStoreContainer { get; }

        /// <summary>
        ///     Gets the private file container.
        /// </summary>
        /// <value>The private file container.</value>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
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
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public static string ServiceBusConnectionString { get; private set; }

        /// <summary>
        /// Checks the storage.
        /// </summary>
        public static void CheckStorage()
        {
            CacheTable.CreateIfNotExists();
            SagasTable.CreateIfNotExists();

            PublicFileContainer.CreateIfNotExists();
            PrivateFileContainer.CreateIfNotExists();
            CommandStoreContainer.CreateIfNotExists();
            EventStoreContainer.CreateIfNotExists();
        }
    }
}