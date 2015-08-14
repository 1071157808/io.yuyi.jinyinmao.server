// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  12:41 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-08  4:00 PM
// ***********************************************************************
// <copyright file="DailyConfigHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Yuyi.Jinyinmao.Packages.Helper
{
    /// <summary>
    ///     DailyConfigHelper.
    /// </summary>
    public static class DailyConfigHelper
    {
        private static readonly CloudStorageAccount Account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
        private static readonly CloudTable Table = Account.CreateCloudTableClient().GetTableReference("Config");

        /// <summary>
        ///     Gets the daily configuration.
        /// </summary>
        /// <param name="date">The date.UTC+8</param>
        /// <returns>DailyConfig.</returns>
        public static DailyConfig GetDailyConfig(DateTime date)
        {
            date = date.Date;
            string dateString = date.ToString("yyyyMMdd");

            string query = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "jinyinmao-daily-config"), TableOperators.And,
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, dateString));

            ConfigEntity config = Table.ExecuteQuery(new TableQuery<ConfigEntity>().Where(query)).FirstOrDefault();

            if (config == null)
            {
                return null;
            }

            return new DailyConfig
            {
                BonusAmount = config.BonusAmount,
                Date = date,
                DateString = dateString,
                IsWorkDay = config.IsWorkday,
                JBYWithdrawalLimit = config.JBYWithdrawalLimit,
                JBYYield = config.JBYYield
            };
        }

        /// <summary>
        ///     Gets the last work day configuration.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>DateTime.</returns>
        public static DailyConfig GetLastWorkDayConfig(int offset = 0)
        {
            return GetLastWorkDayConfig(DateTime.UtcNow.AddHours(8), offset);
        }

        /// <summary>
        ///     Gets the last work day configuration.
        /// </summary>
        /// <param name="fromDate">From date.UTC+8</param>
        /// <param name="offset">The offset.</param>
        /// <returns>DateTime.</returns>
        public static DailyConfig GetLastWorkDayConfig(DateTime fromDate, int offset = 0)
        {
            offset = offset < 0 ? 0 : offset;
            offset += 1;

            DailyConfig config = GetTodayDailyConfig();

            for (int i = 1; i < 100; i++)
            {
                DateTime date = fromDate.AddDays(-i);
                config = GetDailyConfig(date);
                if (config != null && config.IsWorkDay)
                {
                    offset -= 1;
                }

                if (offset == 0)
                {
                    break;
                }
            }

            return config;
        }

        /// <summary>
        ///     Gets the next work day configuration.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>DailyConfig.</returns>
        public static DailyConfig GetNextWorkDayConfig(int offset = 0)
        {
            return GetNextWorkDayConfig(DateTime.UtcNow.AddHours(8), offset);
        }

        /// <summary>
        ///     Gets the next work day configuration.
        /// </summary>
        /// <param name="fromDate">From date.UTC+8</param>
        /// <param name="offset">The offset.</param>
        /// <returns>DailyConfig.</returns>
        public static DailyConfig GetNextWorkDayConfig(DateTime fromDate, int offset = 0)
        {
            offset = offset < 0 ? 0 : offset;
            offset += 1;

            DailyConfig config = GetTodayDailyConfig();

            for (int i = 1; i < 100; i++)
            {
                DateTime date = fromDate.AddDays(i);
                config = GetDailyConfig(date);
                if (config != null && config.IsWorkDay)
                {
                    offset -= 1;
                }

                if (offset == 0)
                {
                    break;
                }
            }

            return config;
        }

        /// <summary>
        ///     Gets the today daily configuration.
        /// </summary>
        /// <returns>DailyConfig.</returns>
        public static DailyConfig GetTodayDailyConfig()
        {
            return GetDailyConfig(DateTime.UtcNow.AddHours(8));
        }
    }

    /// <summary>
    ///     ConfigEntity.
    /// </summary>
    public class ConfigEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the bonus amount.
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
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool IsWorkday { get; set; }

        /// <summary>
        ///     Gets or sets the jby withdrawal limit.
        /// </summary>
        /// <value>The jby withdrawal limit.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public long JBYWithdrawalLimit { get; set; }

        /// <summary>
        ///     Gets or sets the jby yield.
        /// </summary>
        /// <value>The jby yield.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public int JBYYield { get; set; }
    }

    /// <summary>
    ///     DailyConfig.
    /// </summary>
    public class DailyConfig
    {
        /// <summary>
        /// Gets or sets the bonus amount.
        /// </summary>
        /// <value>The bonus amount.</value>
        public long BonusAmount { get; set; }

        /// <summary>
        ///     Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Gets or sets the date string.
        /// </summary>
        /// <value>The date string.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string DateString { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is work day.
        /// </summary>
        /// <value><c>true</c> if this instance is work day; otherwise, <c>false</c>.</value>
        public bool IsWorkDay { get; set; }

        /// <summary>
        ///     Gets or sets the jby withdrawal limit.
        /// </summary>
        /// <value>The jby withdrawal limit.</value>
        public long JBYWithdrawalLimit { get; set; }

        /// <summary>
        ///     Gets or sets the yield.
        /// </summary>
        /// <value>The yield.</value>
        public int JBYYield { get; set; }
    }
}