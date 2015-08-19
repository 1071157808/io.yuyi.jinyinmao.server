using System.Configuration;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Yuyi.Jinyinmao.Api.Link.Utils
{
    /// <summary>
    /// StorageHelper.
    /// </summary>
    public static class StorageHelper
    {
        /// <summary>
        /// The account
        /// </summary>
        private static readonly CloudStorageAccount Account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("DataConnectionString"));

        /// <summary>
        /// The client
        /// </summary>
        private static readonly CloudTableClient Client = Account.CreateCloudTableClient();

        /// <summary>
        /// Finds the by condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public async static Task<T> FindByCondition<T>(string name, string partitionKey, string rowKey) where T : ITableEntity
        {
            CloudTable table = Client.GetTableReference(name);
            TableResult result = await table.ExecuteAsync(TableOperation.Retrieve<T>(partitionKey, rowKey));
            return (T)result.Result;
        }

        /// <summary>
        /// log link hits as an asynchronous operation.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        public async static Task LogLinkHitsAsync(string name, TableEntity entity)
        {
            CloudTable table = Client.GetTableReference(name);
            await table.ExecuteAsync(TableOperation.InsertOrReplace(entity));
        }
    }
}