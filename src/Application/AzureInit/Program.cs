// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-19  2:22 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-17  4:19 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Events;

namespace AzureInit
{
    internal class Program
    {
        private static void InitDailyConfigs(string connectionString)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("Config");

            table.CreateIfNotExistsAsync();

            DateTime baseDay = new DateTime(2015, 5, 1);

            for (int i = 0; i < 300; i++)
            {
                DateTime day = baseDay.AddDays(i);

                bool isWorkDay = IsWorkDay(day);

                ConfigEntity config = new ConfigEntity
                {
                    DateIndex = i,
                    IsWorkday = isWorkDay,
                    JBYWithdrawalLimit = 1000,
                    JBYYield = 700,
                    PartitionKey = "jinyinmao-daily-config",
                    RowKey = day.ToString("yyyyMMdd")
                };
                table.Execute(TableOperation.Insert(config));

                Console.WriteLine("Inited DailyConfig {0}-{1}".FormatWith(day, config.ToJson()));

                if (day.DayOfYear >= 365)
                {
                    break;
                }
            }
        }

        private static void InitServiceBus(string connectionString)
        {
            Type[] types = Assembly.GetAssembly(typeof(BankCardAdded)).GetTypes();

            List<Type> eventTypes = types.Where(t => t.IsSubclassOf(typeof(Event))).ToList();

            foreach (Type type in eventTypes)
            {
                string eventName = type.Name.ToUnderScope();
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

                if (namespaceManager.TopicExists(eventName))
                {
                    namespaceManager.DeleteTopic(eventName);
                    Console.WriteLine("Deleted ServiceBus Topic {0}".FormatWith(eventName.ToLowerInvariant()));
                }

                TopicDescription description = new TopicDescription(eventName)
                {
                    DefaultMessageTimeToLive = TimeSpan.FromDays(1000),
                    DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(10),
                    EnableBatchedOperations = true,
                    IsAnonymousAccessible = false,
                    MaxSizeInMegabytes = 1024 * 5,
                    RequiresDuplicateDetection = true,
                    SupportOrdering = true
                };

                namespaceManager.CreateTopic(description);
                Console.WriteLine("Created ServiceBus Topic {0}".FormatWith(eventName.ToLowerInvariant()));

                if (namespaceManager.SubscriptionExists(eventName, "Default"))
                {
                    namespaceManager.DeleteSubscription(eventName, "Default");
                    Console.WriteLine("Deleted ServiceBus Subscription {0}-Default".FormatWith(eventName.ToLowerInvariant()));
                }

                SubscriptionDescription sd = new SubscriptionDescription(eventName, "Default")
                {
                    DefaultMessageTimeToLive = TimeSpan.FromDays(1000),
                    EnableBatchedOperations = true,
                    EnableDeadLetteringOnFilterEvaluationExceptions = true,
                    LockDuration = TimeSpan.FromMinutes(3),
                    MaxDeliveryCount = 10
                };

                namespaceManager.CreateSubscription(sd);
                Console.WriteLine("Created ServiceBus Subscription {0}-Default".FormatWith(eventName.ToLowerInvariant()));
            }
        }

        private static void InitStorage(string connectionString)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient blobClient = account.CreateCloudBlobClient();

            CloudBlobContainer commandsBlob = blobClient.GetContainerReference("commands");

            commandsBlob.CreateIfNotExists(BlobContainerPublicAccessType.Off);
            Console.WriteLine("Created Storage BlobContainer {0}".FormatWith(commandsBlob.Name));

            CloudBlobContainer eventsBlob = blobClient.GetContainerReference("events");

            eventsBlob.CreateIfNotExists(BlobContainerPublicAccessType.Off);
            Console.WriteLine("Created Storage BlobContainer {0}".FormatWith(eventsBlob.Name));

            CloudBlobContainer privateFilesBlob = blobClient.GetContainerReference("privatefiles");

            privateFilesBlob.CreateIfNotExists(BlobContainerPublicAccessType.Off);
            Console.WriteLine("Created Storage BlobContainer {0}".FormatWith(privateFilesBlob.Name));

            CloudBlobContainer publicFilesBlob = blobClient.GetContainerReference("publicfiles");

            publicFilesBlob.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            Console.WriteLine("Created Storage BlobContainer {0}".FormatWith(publicFilesBlob.Name));

            CloudTableClient tableClient = account.CreateCloudTableClient();

            string[] tables =
            {
                "Cache",
                "Config",
                "Errors",
                "Sagas",
                "ApiKeys",
                "Sms"
            };

            foreach (CloudTable cloudTable in tables.Select(table => tableClient.GetTableReference(table)))
            {
                cloudTable.CreateIfNotExists();
                Console.WriteLine("Created Storage Table {0}".FormatWith(cloudTable.Name));
            }

            InitDailyConfigs(connectionString);
        }

        private static bool IsWorkDay(DateTime day)
        {
            string[] holidays =
            {
                "5-1",
                "5-2",
                "5-3",
                "6-20",
                "6-21",
                "6-22",
                "9-3",
                "9-5",
                "9-26",
                "9-27",
                "10-1",
                "10-2",
                "10-3",
                "10-4",
                "10-5",
                "10-6",
                "10-7"
            };

            string[] fixWorkDays =
            {
                "9-6",
                "10-10"
            };

            if (holidays.Contains("{0}-{1}".FormatWith(day.Month, day.Day)))
            {
                return false;
            }

            if (fixWorkDays.Contains("{0}-{1}".FormatWith(day.Month, day.Day)))
            {
                return true;
            }

            return day.DayOfWeek != DayOfWeek.Sunday && day.DayOfWeek != DayOfWeek.Saturday;
        }

        private static void Main(string[] args)
        {
            string command;
            do
            {
                command = string.Empty;
                Console.WriteLine("ServiceBus or Storage");
                Console.WriteLine("====================================================");

                try
                {
                    string input;
                    do
                    {
                        input = Console.ReadLine();
                        command += input;
                    } while (!string.IsNullOrEmpty(input));

                    if (string.IsNullOrEmpty(command))
                    {
                        continue;
                    }

                    if (command.StartsWith("ServiceBus:", StringComparison.Ordinal))
                    {
                        InitServiceBus(command.Remove(0, "ServiceBus:".Length));
                    }

                    if (command.StartsWith("Storage:", StringComparison.Ordinal))
                    {
                        InitStorage(command.Remove(0, "Storage:".Length));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine("====================================================");
            } while (command.ToUpperInvariant() != "Q" && command.ToUpperInvariant() != "QUIT");
        }

        #region Nested type: ConfigEntity

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global"), SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public sealed class ConfigEntity : TableEntity
        {
            public int DateIndex { get; set; }

            public bool IsWorkday { get; set; }

            public int JBYWithdrawalLimit { get; set; }

            public int JBYYield { get; set; }
        }

        #endregion Nested type: ConfigEntity
    }
}