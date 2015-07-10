
﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Sagas;

namespace SagasTransfer
{
    internal class Program
    {
        private static string connectionString = "BlobEndpoint=https://jymstoredev.blob.core.chinacloudapi.cn/;QueueEndpoint=https://jymstoredev.queue.core.chinacloudapi.cn/;TableEndpoint=https://jymstoredev.table.core.chinacloudapi.cn/;AccountName=jymstoredev;AccountKey=1dCLRLeIeUlLAIBsS9rYdCyFg3UNU239MkwzNOj3BYbREOlnBmM4kfTPrgvKDhSmh6sRp2MdkEYJTv4Ht3fCcg==";
        private static string transferConnectionString = "BlobEndpoint=https://jymstoredevlocal.blob.core.chinacloudapi.cn/;QueueEndpoint=https://jymstoredevlocal.queue.core.chinacloudapi.cn/;TableEndpoint=https://jymstoredevlocal.table.core.chinacloudapi.cn/;AccountName=jymstoredevlocal;AccountKey=sw0XYWye73+JhBp1vNLpH9lUOUWit7nphWW2AFC322ucEBAXFZaRvcsRyhosGsD1VK3bUnCnW0nRSoW0yh2uDA==";

        private static async void DeleteTable(CloudTable table, SagaStateRecord saga)
        {
            await table.ExecuteAsync(TableOperation.Delete(saga));
        }

        private static async void InsertTable(CloudTable table, SagaStateRecord saga)
        {
            await table.ExecuteAsync(TableOperation.InsertOrReplace(saga));
        }

        private static void Main(string[] args)
        {

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(baseDir, $"/Saga/{DateTime.Now.ToString("yyyyMMdd")}.csv");
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
                using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)))
                {
                    Log.Logger = new LoggerConfiguration().WriteTo.TextWriter(writer, outputTemplate: "{Message}").CreateLogger();
                    Transfer();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error");
                new LoggerConfiguration().WriteTo.RollingFile(Path.Combine(baseDir,"/Error/Log-{Date}.txt")).CreateLogger().Information("{@ex}", ex);
            }
            Console.ReadKey();
        }
        static void Transfer()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudTableClient client = account.CreateCloudTableClient();
            var table = client.GetTableReference("Sagas");
            DateTime now = DateTime.Now;
            string sagaName = "Sagas" + now.ToString("yyyyMMdd");
            string sagaErrorName = "Sagas" + now.ToString("yyyyMMdd") + "Error";
            CloudStorageAccount transferAccount = CloudStorageAccount.Parse(transferConnectionString);
            CloudTableClient transferClient = transferAccount.CreateCloudTableClient();
            var sagaTable = transferClient.GetTableReference(sagaName);
            var sagaErrorTable = transferClient.GetTableReference(sagaErrorName);
            sagaTable.CreateIfNotExists();
            sagaErrorTable.CreateIfNotExists();
            var group = table.CreateQuery<SagaStateRecord>().ToList().Take(1).GroupBy(s => s.PartitionKey);
            Console.WriteLine("start");
            foreach (var item in group)
            {
                for (int i = 0; i < item.Count(); i++)
                {
                    var saga = item.ElementAt(i);
                    InsertTable(saga.CurrentProcessingStatus == (int)DepositSagaStatus.Fault ? sagaErrorTable : sagaTable, saga);
                    DeleteTable(table, saga);
                    Console.WriteLine("transfer partitionkey:" + saga.PartitionKey);
                }
            }

            var listSaga = sagaTable.CreateQuery<SagaStateRecord>().ToList().Select(s => Util.InitData(s)).ToList();
            listSaga.AddRange(sagaErrorTable.CreateQuery<SagaStateRecord>().ToList().Select(s => Util.InitData(s)).ToList());
            Log.Information(Util.SaveAsCSV<SagaStateRecordResult>(listSaga));
        }
    }
}
