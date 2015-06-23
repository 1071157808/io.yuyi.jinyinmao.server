// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-06-19  9:41 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-19  9:49 AM
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
    internal class Program
    {
        private static void InitDailyConfigs(string connectionString, string tableName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(tableName);

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

                InitDailyConfigs(connectiongString, tableName);
            } while (command.ToUpperInvariant() != "QUIT");
        }

        private static void PrintHelp()
        {
            string helpText = "-C[REQUIRED]     ConnectiongString: ConnectiongString for the storage\n" +
                              "-T[REQUIRED]     TableName: Daily Config table name";

            Console.WriteLine(helpText);
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