using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using Serilog;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Sagas;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SagasTransfer
{
    internal class Program
    {
        private static string connectionString = "BlobEndpoint=https://jymstoredev.blob.core.chinacloudapi.cn/;QueueEndpoint=https://jymstoredev.queue.core.chinacloudapi.cn/;TableEndpoint=https://jymstoredev.table.core.chinacloudapi.cn/;AccountName=jymstoredev;AccountKey=1dCLRLeIeUlLAIBsS9rYdCyFg3UNU239MkwzNOj3BYbREOlnBmM4kfTPrgvKDhSmh6sRp2MdkEYJTv4Ht3fCcg==";
        private static string transferConnectionString = "BlobEndpoint=https://jymstoredevlocal.blob.core.chinacloudapi.cn/;QueueEndpoint=https://jymstoredevlocal.queue.core.chinacloudapi.cn/;TableEndpoint=https://jymstoredevlocal.table.core.chinacloudapi.cn/;AccountName=jymstoredevlocal;AccountKey=sw0XYWye73+JhBp1vNLpH9lUOUWit7nphWW2AFC322ucEBAXFZaRvcsRyhosGsD1VK3bUnCnW0nRSoW0yh2uDA==";

        private static void DeleteTable(CloudTable table, TableEntity entity)
        {
            table.Execute(TableOperation.Delete(entity));
        }

        private static void InsertTable(CloudTable table, TableEntity entity)
        {
            table.Execute(TableOperation.InsertOrReplace(entity));
        }

        private static void Main(string[] args)
        {
            //string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string baseDir = "e:/log";
            Console.WriteLine();
            try
            {
                IList<string> list = args.ToList();
                int index = list.IndexOf("-S");
                if (-1 != index && list.Count >= index + 2)
                {
                    connectionString = list[index + 1];
                }
                index = list.IndexOf("-D");
                if (-1 != index && list.Count >= index + 2)
                {
                    transferConnectionString = list[index + 1];
                }

                index = list.IndexOf("-P");
                if (-1 != index && list.Count >= index + 2)
                {
                    baseDir = list[index + 1];
                }
                DirectoryInfo dir = new DirectoryInfo(Path.Combine(baseDir, "Saga"));
                if (!dir.Exists)
                {
                    dir.Create();
                }
                string path = Path.Combine(dir.FullName, $"{DateTime.Now.ToString("yyyyMMdd")}.csv");
                Console.WriteLine("transfer start");


                Transfer(path);

                Console.WriteLine("transfer finish");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error");
                new LoggerConfiguration().WriteTo.RollingFile(Path.Combine(baseDir, "/Error/Log-{Date}.txt")).CreateLogger().Information("{@ex}", ex);
            }
            Console.ReadKey();
        }

        private static void Operation(CloudTable table, TableBatchOperation batch)
        {
            table.ExecuteBatch(batch);
        }

        private async static void  Transfer(string path)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            TableRequestOptions request = new TableRequestOptions
            {
                RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(5), 10)
            };

            DateTime now = DateTime.Now;
            string sagaName = "Sagas" + now.ToString("yyyyMMdd");
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudTableClient client = account.CreateCloudTableClient();
            client.DefaultRequestOptions = request;
            var table = client.GetTableReference("Sagas");

            CloudStorageAccount transferAccount = CloudStorageAccount.Parse(transferConnectionString);
            CloudTableClient transferClient = transferAccount.CreateCloudTableClient();
            var sagaTable = transferClient.GetTableReference(sagaName);
            transferClient.DefaultRequestOptions = request;
            table.CreateIfNotExists();
            sagaTable.CreateIfNotExists();

            var query = new TableQuery<SagaStateRecord>().Take(2);
            List<SagaStateRecord> list = new List<SagaStateRecord>();
            TableContinuationToken token = null;
            do
            {
                var segement = await table.ExecuteQuerySegmentedAsync(query, token);
                list.AddRange(segement.Results);
                token = segement.ContinuationToken;
            } while (token != null);
            Console.WriteLine(list.Count());
            var group = list.GroupBy(x => x.PartitionKey);

            foreach (var sagas in group)
            {
                TableBatchOperation batchTransfer = new TableBatchOperation();
                TableBatchOperation batchDel = new TableBatchOperation();
                var saga = list.Find(x => x.PartitionKey == sagas.Key
                            && (x.CurrentProcessingStatus == (int)DepositSagaStatus.Finished
                            || x.CurrentProcessingStatus == (int)DepositSagaStatus.Fault));
                if (saga == null) return;

                foreach (var item in sagas)
                {
                    batchTransfer.InsertOrReplace(item);
                    batchDel.Delete(item);
                }

                Operation(sagaTable, batchTransfer);
                Operation(table, batchDel);
            }
            Console.WriteLine(list.Count());
            //string line = string.Empty;
            //if (File.Exists(path))
            //{
            //    using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)))
            //    {
            //        string temp;
            //        while ((temp = reader.ReadLine()) != null)
            //        {
            //            line = temp;
            //        }
            //    }
            //}

            //string rowKey = string.IsNullOrWhiteSpace(line) ? "" : line.Split(',')[1];
            //query = sagaTable.CreateQuery<SagaStateRecord>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, rowKey));
            //token = null;
            list.Clear();
            do
            {
                var segement = await table.ExecuteQuerySegmentedAsync(query, token);
                list.AddRange(segement.Results);
                token = segement.ContinuationToken;
            } while (token != null);

            using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Append, FileAccess.ReadWrite, FileShare.ReadWrite)))
            {
                Log.Logger = new LoggerConfiguration().WriteTo.TextWriter(writer, outputTemplate: "{Message}").CreateLogger();
            }
            watch.Stop();
            Console.WriteLine(watch.ToString());
            Console.WriteLine("end");
        }
    }
}