// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  4:13 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-08  1:48 AM
// ***********************************************************************
// <copyright file="SiloClusterConfig.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure;
using Microsoft.ServiceBus;
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
            string storageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            CloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);

            CloudTableClient tableClient = CloudStorageAccount.CreateCloudTableClient();
            tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromMilliseconds(500), 6);
            CommandStoreTable = tableClient.GetTableReference("CommandStore");
            EventProcessingErrorsTable = tableClient.GetTableReference("EventProcessingErrors");
            EventStoreTable = tableClient.GetTableReference("EventStore");
            ProductCacheTable = tableClient.GetTableReference("ProductCache");
            SagasTable = tableClient.GetTableReference("Sagas");

            CloudBlobClient blobClient = CloudStorageAccount.CreateCloudBlobClient();
            blobClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromMilliseconds(500), 6);
            PublicFileContainer = blobClient.GetContainerReference("publicfiles");
            PrivateFileContainer = blobClient.GetContainerReference("privatefiles");

            ServiceBusConnectiongString = CloudConfigurationManager.GetSetting("ServiceBusConnectiongString");
        }

        /// <summary>
        ///     Gets the cloud storage account.
        /// </summary>
        /// <value>The cloud storage account.</value>
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
        public static CloudStorageAccount CloudStorageAccount { get; private set; }

        /// <summary>
        ///     Gets the command store table.
        /// </summary>
        /// <value>The command store table.</value>
        public static CloudTable CommandStoreTable { get; }

        /// <summary>
        ///     Gets the event processing errors table.
        /// </summary>
        /// <value>The event processing errors table.</value>
        public static CloudTable EventProcessingErrorsTable { get; }

        /// <summary>
        ///     Gets the event store table.
        /// </summary>
        /// <value>The event store table.</value>
        public static CloudTable EventStoreTable { get; }

        /// <summary>
        ///     Gets the private file container.
        /// </summary>
        /// <value>The private file container.</value>
        public static CloudBlobContainer PrivateFileContainer { get; }

        /// <summary>
        ///     Gets the product cache table.
        /// </summary>
        /// <value>The product cache table.</value>
        public static CloudTable ProductCacheTable { get; }

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

        /// <summary>
        ///     Checks the configuration.
        /// </summary>
        /// <exception cref="System.ApplicationException">
        ///     Can not connect to Command Store
        ///     or
        ///     Can not connect to Event Store
        ///     or
        ///     Can not connect to Event Processing Errors
        /// </exception>
        public static void CheckConfig()
        {
            if (!CommandStoreTable.Exists())
            {
                throw new ApplicationException("Can not connect to Command Store");
            }

            if (!EventProcessingErrorsTable.Exists())
            {
                throw new ApplicationException("Can not connect to Event Processing Errors");
            }

            if (!EventStoreTable.Exists())
            {
                throw new ApplicationException("Can not connect to Event Store");
            }

            if (!ProductCacheTable.Exists())
            {
                throw new ApplicationException("Can not connect to Product Cache");
            }

            if (!SagasTable.Exists())
            {
                throw new ApplicationException("Can not connect to Sagas");
            }

            if (!PublicFileContainer.Exists())
            {
                throw new ApplicationException("Can not connect to Event Processing Errors");
            }

            if (!PrivateFileContainer.Exists())
            {
                throw new ApplicationException("Can not connect to Event Processing Errors");
            }

            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(ServiceBusConnectiongString);
            if (namespaceManager == null)
            {
                throw new ApplicationException("Can not connect to Events Service Bus");
            }

            if (!namespaceManager.TopicExists("add_bank_card_resulted"))
            {
                throw new ApplicationException("Can not connect to topic add_bank_card_resulted");
            }

            if (!namespaceManager.TopicExists("authenticate_resulted"))
            {
                throw new ApplicationException("Can not connect to topic authenticate_resulted");
            }

            if (!namespaceManager.TopicExists("deposit_from_yilian_resulted"))
            {
                throw new ApplicationException("Can not connect to topic deposit_from_yilian_resulted");
            }

            if (!namespaceManager.TopicExists("login_password_reset"))
            {
                throw new ApplicationException("Can not connect to topic login_password_reset");
            }

            if (!namespaceManager.TopicExists("order_built"))
            {
                throw new ApplicationException("Can not connect to topic order_built");
            }

            if (!namespaceManager.TopicExists("order_repaid"))
            {
                throw new ApplicationException("Can not connect to topic order_repaid");
            }

            if (!namespaceManager.TopicExists("payment_password_reset"))
            {
                throw new ApplicationException("Can not connect to topic payment_password_reset");
            }

            if (!namespaceManager.TopicExists("payment_password_set"))
            {
                throw new ApplicationException("Can not connect to topic payment_password_set");
            }

            if (!namespaceManager.TopicExists("regular_product_issued"))
            {
                throw new ApplicationException("Can not connect to topic regular_product_issued");
            }

            if (!namespaceManager.TopicExists("user_registered"))
            {
                throw new ApplicationException("Can not connect to topic user_registered");
            }

            if (!namespaceManager.TopicExists("withdrawal_resulted"))
            {
                throw new ApplicationException("Can not connect to topic withdrawal_resulted");
            }
        }
    }
}