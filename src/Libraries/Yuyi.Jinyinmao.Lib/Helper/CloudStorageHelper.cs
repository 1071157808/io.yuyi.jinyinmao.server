// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  11:52 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-08  1:15 AM
// ***********************************************************************
// <copyright file="CloudStorageHelper.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Helper
{
    /// <summary>
    ///     CloudStorageHelper.
    /// </summary>
    public static class CloudStorageHelper
    {
        /// <summary>
        ///     Reads the data from table cache asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">The table.</param>
        /// <param name="cacheName">Name of the cache.</param>
        /// <param name="cacheId">The cache identifier.</param>
        /// <param name="cacheTime">The cache time.</param>
        /// <returns>T.</returns>
        public static T ReadDataFromTableCache<T>(this CloudTable table, string cacheName, string cacheId, TimeSpan cacheTime) where T : class
        {
            TableQuery<TableCacheEntity> query = new TableQuery<TableCacheEntity>()
                .Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, cacheName),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, cacheId)));
            TableCacheEntity cacheData = table.ExecuteQuery(query).FirstOrDefault();

            if (cacheData != null && DateTime.UtcNow - cacheData.TimeStamp < cacheTime)
            {
                if (cacheData.Data.IsNotNullOrEmpty())
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<T>(cacheData.Data);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        /// <summary>
        ///     set data to storage cache as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">The table.</param>
        /// <param name="cacheName">Name of the cache.</param>
        /// <param name="cacheId">The cache identifier.</param>
        /// <param name="data">The data.</param>
        /// <returns>Task.</returns>
        public static async Task SetDataToStorageCacheAsync<T>(this CloudTable table, string cacheName, string cacheId, T data) where T : class
        {
            TableCacheEntity cache = new TableCacheEntity
            {
                Data = data.ToJson(),
                PartitionKey = cacheName,
                RowKey = cacheId,
                TimeStamp = DateTime.UtcNow
            };

            await table.ExecuteAsync(TableOperation.InsertOrReplace(cache));
        }

        #region Nested type: TableCacheEntity

        /// <summary>
        ///     TableCacheEntity.
        /// </summary>
        public class TableCacheEntity : TableEntity
        {
            /// <summary>
            ///     Gets or sets the data.
            /// </summary>
            /// <value>The data.</value>
            public string Data { get; set; }

            /// <summary>
            ///     Gets or sets the time stamp.
            /// </summary>
            /// <value>The time stamp.</value>
            public DateTime TimeStamp { get; set; }
        }

        #endregion Nested type: TableCacheEntity
    }
}