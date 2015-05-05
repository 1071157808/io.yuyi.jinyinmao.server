// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-05  2:07 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-05  5:29 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DailyConfigInitApp
{
    public class ConfigEntity : TableEntity
    {
        public int DateIndex { get; set; }

        public bool IsWorkday { get; set; }

        public int JBYWithdrawalLimit { get; set; }

        public int JBYYield { get; set; }
    }

    internal class Program
    {
        private static bool IsWorkDay(DateTime day)
        {
            if (day.DayOfWeek == DayOfWeek.Sunday || day.DayOfWeek == DayOfWeek.Saturday)
            {
                return false;
            }

            if (day.Month == 5 && day.Day <= 3)
            {
                return false;
            }

            return true;
        }

        private static void Main(string[] args)
        {
            string connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("DailyConfig");

            DateTime baseDay = new DateTime(2015, 5, 1);

            for (int i = 0; i < 300; i++)
            {
                DateTime day = baseDay.AddDays(i);

                bool isWorkDay = IsWorkDay(day);

                table.Execute(TableOperation.Insert(new ConfigEntity
                {
                    DateIndex = i,
                    IsWorkday = isWorkDay,
                    JBYWithdrawalLimit = isWorkDay ? 1000 : 0,
                    JBYYield = 700,
                    PartitionKey = Guid.NewGuid().ToString("N"),
                    RowKey = day.ToString("yyyyMMdd")
                }));
            }
        }
    }
}