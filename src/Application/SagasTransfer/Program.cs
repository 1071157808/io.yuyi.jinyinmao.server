// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-07-09  10:01 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-09  3:52 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
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
            Console.WriteLine("-S[OPTIONAL]     SourceStorageAccount: a string represents the source storage account, eg.BlobEndpoint = https://jymstoredevlocal.blob.core.chinacloudapi.cn/;QueueEndpoint=https://jymstoredevlocal.queue.core.chinacloudapi.cn/;TableEndpoint=https://jymstoredevlocal.table.core.chinacloudapi.cn/;AccountName=jymstoredevlocal;AccountKey=sw0XYWye73+JhBp1vNLpH9lUOUWit7nphWW2AFC322ucEBAXFZaRvcsRyhosGsD1VK3bUnCnW0nRSoW0yh2uDA==\r\n" +
                              "-D[OPTIONAL]     DestinationStorageAccount: a string represents the destination storage account, eg. same with the -S parameter\r\n" +
                              "-P[OPTIONAL]     Logdir: a string represents the absolute path of log file, eg. D://Logs/");

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string command;
            do
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Please input parameters:");
                command = string.Empty;
                try
                {
                    string input;
                    do
                    {
                        input = Console.ReadLine();
                        command += input;
                    } while (!string.IsNullOrEmpty(input));

                    IList<string> list = command.Split(' ').ToList();
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
                    Transfer();
                    // Console.WriteLine($"DataStr:{connectionString},TranStr:{transferConnectionString},Path:{baseDir}");
                }
                catch (Exception ex)
                {
                    FileHelper.WriteTo(baseDir, ex.Message);
                }
            } while (command.ToUpperInvariant() != "Q" && command.ToUpperInvariant() != "QUIT");
            Console.ReadKey();
        }

        private static void Transfer()
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
            var group = table.CreateQuery<SagaStateRecord>().ToList().GroupBy(s => s.PartitionKey);
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
        }
    }
}