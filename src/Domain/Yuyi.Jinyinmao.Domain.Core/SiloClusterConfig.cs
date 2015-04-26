// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  4:13 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  7:28 PM
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

            CloudTableClient client = CloudStorageAccount.CreateCloudTableClient();
            client.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromMilliseconds(500), 6);
            CommandStoreTable = CloudStorageAccount.CreateCloudTableClient().GetTableReference("CommandStore");
            SagasTable = CloudStorageAccount.CreateCloudTableClient().GetTableReference("Sagas");
            EventProcessingErrorsTable = CloudStorageAccount.CreateCloudTableClient().GetTableReference("EventProcessingErrors");
            EventStoreTable = CloudStorageAccount.CreateCloudTableClient().GetTableReference("EventStore");

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
        public static CloudTable CommandStoreTable { get; private set; }

        /// <summary>
        ///     Gets the event processing errors table.
        /// </summary>
        /// <value>The event processing errors table.</value>
        public static CloudTable EventProcessingErrorsTable { get; private set; }

        /// <summary>
        ///     Gets the event store table.
        /// </summary>
        /// <value>The event store table.</value>
        public static CloudTable EventStoreTable { get; private set; }

        /// <summary>
        /// Gets or sets the sagas table.
        /// </summary>
        /// <value>The sagas table.</value>
        public static CloudTable SagasTable { get; private set; }

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
            if (CloudStorageAccount.CreateCloudTableClient().GetTableReference("CommandStore") == null)
            {
                throw new ApplicationException("Can not connect to Command Store");
            }

            if (CloudStorageAccount.CreateCloudTableClient().GetTableReference("EventStore") == null)
            {
                throw new ApplicationException("Can not connect to Event Store");
            }

            if (CloudStorageAccount.CreateCloudTableClient().GetTableReference("EventProcessingErrors") == null)
            {
                throw new ApplicationException("Can not connect to Event Processing Errors");
            }

            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(ServiceBusConnectiongString);
            if (namespaceManager == null)
            {
                throw new ApplicationException("Can not connect to Events Service Bus");
            }

            if (!namespaceManager.TopicExists("login_password_reset"))
            {
                throw new ApplicationException("Can not connect to topic login_password_reset");
            }

            if (!namespaceManager.TopicExists("payment_password_reset"))
            {
                throw new ApplicationException("Can not connect to topic payment_password_reset");
            }

            if (!namespaceManager.TopicExists("payment_password_set"))
            {
                throw new ApplicationException("Can not connect to topic payment_password_set");
            }

            if (!namespaceManager.TopicExists("user_registered"))
            {
                throw new ApplicationException("Can not connect to topic user_registered");
            }
        }
    }
}
