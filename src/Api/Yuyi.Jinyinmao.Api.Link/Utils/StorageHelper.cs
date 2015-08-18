using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;
using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Api.Link.Utils
{
    public static class StorageHelper
    {
        private static readonly CloudStorageAccount Account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("DataConnectionString"));
        private static readonly CloudTableClient Client = Account.CreateCloudTableClient();

        public async static Task InsertTableAsync(string name, TableEntity entity)
        {
            CloudTable table = Client.GetTableReference(name);
            await table.CreateIfNotExistsAsync();
            await table.ExecuteAsync(TableOperation.InsertOrReplace(entity));
        }

        public async static Task<T> FindByCondition<T>(string name, string partitionKey, string rowKey) where T : ITableEntity
        {
            CloudTable table = Client.GetTableReference(name);
            await table.CreateIfNotExistsAsync();            
            TableResult result = await table.ExecuteAsync(TableOperation.Retrieve<T>(partitionKey, rowKey));
            return (T)result.Result;
        }

        public async static Task LogLinkHitsAsync(string name, TableEntity entity)
        {
            CloudTable table = Client.GetTableReference(name);
            await table.CreateIfNotExistsAsync();

            await table.ExecuteAsync(TableOperation.InsertOrReplace(entity));
        }

    }
}