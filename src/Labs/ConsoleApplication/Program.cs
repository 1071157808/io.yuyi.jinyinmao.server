// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-20  12:55 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Yuyi.Jinyinmao.Packages.Helper;

namespace ConsoleApplication
{
    internal class Program
    {
        private static DateTime GetLastInvestingConfirmTime(DateTime date)
        {
            DailyConfig confirmConfig = DailyConfigHelper.GetLastWorkDayConfig(date, 1);
            return confirmConfig.Date.Date.AddDays(1).AddMilliseconds(-1);
        }

        private static void InitDailyConfigs(string connectionString)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("DailyConfig");

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
                    JBYWithdrawalLimit = isWorkDay ? 1000 : 0,
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
            string password = MD5Hash.ComputeMD5Hash("81cc6acb");
            Console.WriteLine(password == "4094122b55ff30796dde034c4d145bd4");
        }
    }
}