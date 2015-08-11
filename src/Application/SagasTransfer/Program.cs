// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Program.cs
// Created          : 2015-07-30  7:51 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-30  11:00 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Serilog;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Sagas;

namespace SagasTransfer
{
    internal class Program
    {
        private static string connectionString = "BlobEndpoint=https://jymstoredev.blob.core.chinacloudapi.cn/;QueueEndpoint=https://jymstoredev.queue.core.chinacloudapi.cn/;TableEndpoint=https://jymstoredev.table.core.chinacloudapi.cn/;AccountName=jymstoredev;AccountKey=1dCLRLeIeUlLAIBsS9rYdCyFg3UNU239MkwzNOj3BYbREOlnBmM4kfTPrgvKDhSmh6sRp2MdkEYJTv4Ht3fCcg==";
        private static string transferConnectionString = "BlobEndpoint=https://jymstoredevlocal.blob.core.chinacloudapi.cn/;QueueEndpoint=https://jymstoredevlocal.queue.core.chinacloudapi.cn/;TableEndpoint=https://jymstoredevlocal.table.core.chinacloudapi.cn/;AccountName=jymstoredevlocal;AccountKey=sw0XYWye73+JhBp1vNLpH9lUOUWit7nphWW2AFC322ucEBAXFZaRvcsRyhosGsD1VK3bUnCnW0nRSoW0yh2uDA==";
        private static void Main(string[] args)
        {
            string tableName = "Sagas";
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            baseDir = "e:/log";
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

                index = list.IndexOf("-T");
                if (-1 != index && list.Count >= index + 2)
                {
                    tableName = list[index + 1];
                }
                DirectoryInfo dir = new DirectoryInfo(Path.Combine(baseDir, tableName));
                if (!dir.Exists)
                {
                    dir.Create();
                }
                Console.WriteLine("Start");
                //Task.Run(() => Transfer("Sagas20150728", tableName + DateTime.Now.ToString("yyyyMMdd"))).Wait();
                //Task.Run(() => SaveToFile(dir.FullName, "Sagas")).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error");
                new LoggerConfiguration().WriteTo.RollingFile("Error/Log-{Date}.txt").CreateLogger().Error("{@ex}", ex);
            }
            Console.WriteLine("Finish");
            Console.ReadKey();
        }


        private static async Task Transfer(string sourceName, string targetName)
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                TableRequestOptions request = new TableRequestOptions
                {
                    RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(5), 10)
                };

                CloudStorageAccount sourceAccount = CloudStorageAccount.Parse(transferConnectionString);
                CloudTableClient sourceClient = sourceAccount.CreateCloudTableClient();
                CloudTable sourceTable = sourceClient.GetTableReference(sourceName);

                CloudStorageAccount targetAccount = CloudStorageAccount.Parse(transferConnectionString);
                CloudTableClient targetClient = targetAccount.CreateCloudTableClient();
                CloudTable targetTable = targetClient.GetTableReference(targetName);

                await sourceTable.CreateIfNotExistsAsync();
                await targetTable.CreateIfNotExistsAsync();

                TableQuery<SagaStateRecord> query =
                    new TableQuery<SagaStateRecord>().Where(
                        TableQuery.CombineFilters(
                            TableQuery.GenerateFilterConditionForInt("CurrentProcessingStatus", QueryComparisons.Equal,
                                (int)DepositSagaStatus.Finished), TableOperators.Or,
                            TableQuery.GenerateFilterConditionForInt("CurrentProcessingStatus", QueryComparisons.Equal,
                                (int)DepositSagaStatus.Fault))).Select(new string[] { "PartitionKey" });
                TableContinuationToken token = null;
                do
                {
                    TableQuerySegment<SagaStateRecord> segement =
                        await sourceTable.ExecuteQuerySegmentedAsync<SagaStateRecord>(query, token);
                    token = segement.ContinuationToken;

                    foreach (string partitionKey in segement.Results.Select(x => x.PartitionKey).Distinct())
                    {
                        TableQuery<SagaStateRecord> query1 =
                            new TableQuery<SagaStateRecord>().Where(TableQuery.GenerateFilterCondition("PartitionKey",
                                QueryComparisons.Equal, partitionKey));
                        TableBatchOperation batchInsert = new TableBatchOperation();
                        TableBatchOperation batchDel = new TableBatchOperation();
                        foreach (SagaStateRecord entity in sourceTable.ExecuteQuery(query1))
                        {
                            batchInsert.InsertOrReplace(entity);
                            batchDel.Delete(entity);
                        }
                        await targetTable.ExecuteBatchAsync(batchInsert);
                        await sourceTable.ExecuteBatchAsync(batchDel);
                    }

                } while (token != null);

                watch.Stop();
                Console.WriteLine(watch.Elapsed);
            }
            catch (AggregateException e)
            {
                new LoggerConfiguration().WriteTo.RollingFile("Error/Log-{Date}.txt")
                    .CreateLogger()
                    .Error("{@ex}", e.GetBaseException());
            }
            catch (Exception exception)
            {
                ILogger logger = new LoggerConfiguration().WriteTo.RollingFile("Error/Log-{Date}.txt")
                    .CreateLogger();
                logger.Error("{@ex}", exception.GetBaseException());

            }
        }

        private static async Task SaveToFile(string path, string tableName)
        {
            TableRequestOptions request = new TableRequestOptions
            {
                RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(5), 10)
            };

            CloudStorageAccount account = CloudStorageAccount.Parse(transferConnectionString);
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            //if (tableName == "Sagas")
            //{
            //    await Util.SaveToFile<SagaStateRecord>(table, path);
            //}
            await Util.SaveToFile<SagaStateRecord>(table, path);
        }
    }
}