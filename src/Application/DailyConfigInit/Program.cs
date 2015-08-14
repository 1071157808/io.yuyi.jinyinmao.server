// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Program.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-14  10:16
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;

namespace DailyConfigInit
{
    /// <summary>
    ///     Program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        ///     Initializes the daily configuration.
        /// </summary>
        /// <param name="table">The table.</param>
        private static void InitDailyConfig(CloudTable table)
        {
            table.CreateIfNotExistsAsync();

            DateTime baseDay = new DateTime(2015, 5, 1);

            for (int i = 0; i < 300; i++)
            {
                DateTime day = baseDay.AddDays(i);

                bool isWorkDay = IsWorkDay(day);

                ConfigEntity config = new ConfigEntity
                {
                    BonusAmount = 1500 * 100,
                    DateIndex = i,
                    IsWorkday = isWorkDay,
                    JBYWithdrawalLimit = 1000 * 10000 * 100,
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

        /// <summary>
        ///     Initializes the daily configs.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="version">The version.</param>
        private static void InitDailyConfigs(string connectionString, string tableName, string version = "")
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(tableName);

            if (!table.Exists() || version.IsNullOrWhiteSpace())
            {
                InitDailyConfig(table);
            }

            if (table.Exists() && version == "3.1")
            {
                DateTime baseDay = new DateTime(2015, 5, 1);

                for (int i = 0; i < 1000; i++)
                {
                    DateTime day = baseDay.AddDays(i);

                    string dateString = day.ToString("yyyyMMdd");

                    string query = TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "jinyinmao-daily-config"), TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, dateString));

                    ConfigEntity config = table.ExecuteQuery(new TableQuery<ConfigEntity>().Where(query)).FirstOrDefault();

                    if (config == null)
                    {
                        continue;
                    }

                    config.BonusAmount = 1500 * 100;
                    config.JBYWithdrawalLimit = config.JBYWithdrawalLimit * 10000 * 100;

                    table.Execute(TableOperation.Replace(config));

                    Console.WriteLine("Updateded DailyConfig {0}-{1}".FormatWith(day, config.ToJson()));
                }
            }
        }

        /// <summary>
        ///     Determines whether [is work day] [the specified day].
        /// </summary>
        /// <param name="day">The day.</param>
        /// <returns><c>true</c> if [is work day] [the specified day]; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private static void Main(string[] args)
        {
            PrintHelp();
            string command;
            do
            {
                Console.WriteLine("Command:");
                command = Console.ReadLine();
                if (command == null)
                {
                    command = "";
                    continue;
                }

                List<string> parameters = command.Split(' ').ToList();

                int index = parameters.IndexOf("-C");
                if (index == -1 || parameters.Count < index + 2)
                {
                    Console.WriteLine("Missing connectiong string.");
                    continue;
                }

                string connectiongString = parameters[index + 1];
                if (string.IsNullOrEmpty(connectiongString))
                {
                    Console.WriteLine("Missing connectiong string.");
                    continue;
                }

                int tableNameIndex = parameters.IndexOf("-T");
                if (tableNameIndex == -1 || parameters.Count < tableNameIndex + 2)
                {
                    Console.WriteLine("Missing table name.");
                    continue;
                }

                string tableName = parameters[tableNameIndex + 1];
                if (string.IsNullOrEmpty(connectiongString))
                {
                    Console.WriteLine("Missing table name.");
                    continue;
                }

                int versionIndex = parameters.IndexOf("-V");
                string version = string.Empty;
                if (tableNameIndex != -1 && parameters.Count >= tableNameIndex + 2)
                {
                    version = parameters[versionIndex + 1];
                }

                InitDailyConfigs(connectiongString, tableName, version);
            } while (command.ToUpperInvariant() != "QUIT");
        }

        /// <summary>
        ///     Prints the help.
        /// </summary>
        private static void PrintHelp()
        {
            string helpText = "-C[REQUIRED]     ConnectiongString: ConnectiongString for the storage\n" +
                              "-T[REQUIRED]     TableName: Daily Config table name\n" +
                              "-V[OPTIONS]      Version: 13.1";

            Console.WriteLine(helpText);
        }

        #region Nested type: ConfigEntity

        /// <summary>
        ///     ConfigEntity. This class cannot be inherited.
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public sealed class ConfigEntity : TableEntity
        {
            /// <summary>
            ///     Gets or sets the bonus amount.
            /// </summary>
            /// <value>The bonus amount.</value>
            public long BonusAmount { get; set; }

            /// <summary>
            ///     Gets or sets the index of the date.
            /// </summary>
            /// <value>The index of the date.</value>
            public int DateIndex { get; set; }

            /// <summary>
            ///     Gets or sets a value indicating whether this instance is workday.
            /// </summary>
            /// <value><c>true</c> if this instance is workday; otherwise, <c>false</c>.</value>
            public bool IsWorkday { get; set; }

            /// <summary>
            ///     Gets or sets the jby withdrawal limit.
            /// </summary>
            /// <value>The jby withdrawal limit.</value>
            public long JBYWithdrawalLimit { get; set; }

            /// <summary>
            ///     Gets or sets the jby yield.
            /// </summary>
            /// <value>The jby yield.</value>
            public int JBYYield { get; set; }
        }

        #endregion Nested type: ConfigEntity
    }
}