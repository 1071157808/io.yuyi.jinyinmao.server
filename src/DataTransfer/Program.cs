// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Program.cs
// Created          : 2015-07-27  6:28 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  6:42 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using DataTransfer.Models;
using DataTransfer.Models.Entity;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;
using System.Threading.Tasks;
using Moe.Lib;

namespace DataTransfer
{
    internal class Program
    {
        // private static string connectionString = "BlobEndpoint=https://jymstoredev.blob.core.chinacloudapi.cn/;QueueEndpoint=https://jymstoredev.queue.core.chinacloudapi.cn/;TableEndpoint=https://jymstoredev.table.core.chinacloudapi.cn/;AccountName=jymstoredev;AccountKey=1dCLRLeIeUlLAIBsS9rYdCyFg3UNU239MkwzNOj3BYbREOlnBmM4kfTPrgvKDhSmh6sRp2MdkEYJTv4Ht3fCcg==";
        // private static readonly CloudTable TransJBYTransaction = null;

        private static readonly CloudTable TransOrder = null;
        private static readonly CloudTable TransRegularProduct = null;
        private static readonly CloudTable TransTransaction = null;

        public static void Main(string[] args)
        {
            try
            {
                Work.Run().Wait();
            }
            catch (AggregateException exception)
            {
                Console.WriteLine(exception.Message + exception.StackTrace);
                foreach (var item in exception.InnerExceptions)
                {
                    Console.WriteLine(item.GetExceptionString());
                }
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetExceptionString());
                Console.ReadKey();
            }
           
            Console.ReadKey();
        }
    }
}