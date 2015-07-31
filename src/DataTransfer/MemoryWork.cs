// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : MemoryWork.cs
// Created          : 2015-07-31  8:58 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-31  9:02 PM
// ***********************************************************************
// <copyright file="MemoryWork.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataTransfer.Models;
using Newtonsoft.Json;
using Orleans;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace DataTransfer
{
    public class MemoryWork
    {
        [SuppressMessage("ReSharper", "NotAccessedField.Local")]
        private static readonly Guid JBYProductId;

        private static readonly string StrDefaultJBYProductId = "5e35201f315e41d4b11f014d6c01feb8";

        static MemoryWork()
        {
            string strJBYProductId = ConfigurationManager.AppSettings.Get("StrJBYProductId");
            strJBYProductId = string.IsNullOrEmpty(strJBYProductId) ? StrDefaultJBYProductId : strJBYProductId;
            JBYProductId = new Guid(strJBYProductId);

            if (GrainClient.IsInitialized)
            {
                return;
            }
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            GrainClient.Initialize(Path.Combine(baseDir, "ReleaseConfiguration.xml"));
            Console.ReadKey();
        }

        public static async Task Run()
        {
            await RegularProductFactory.GetGrain(Guid.NewGuid()).GetRegularProductInfoAsync();
            List<RegularProductMigrationDto> productList = await GetProductsAsync();
            foreach (var item in productList)
            {
                var product = await RegularProductFactory.GetGrain(item.ProductId).MigrateAsync(item);
                Console.WriteLine(JsonConvert.SerializeObject(item));
                Console.WriteLine(JsonConvert.SerializeObject(product));
            }

            List<UserMigrationDto> userList = await GetUsersAsync();
            foreach (var item in userList)
            {
                await UserFactory.GetGrain(item.UserId).MigrateAsync(item);
            }
        }

        private static async Task<List<RegularProductMigrationDto>> GetProductsAsync()
        {
            using (var context = new OldDBContext())
            {
                List<string> list = await context.JsonProduct.AsNoTracking().Select(item => item.Data).ToListAsync();
                return list.Select(x => JsonConvert.DeserializeObject<RegularProductMigrationDto>(x)).ToList();
            }
        }

        private static async Task<List<UserMigrationDto>> GetUsersAsync()
        {
            using (var context = new OldDBContext())
            {
                List<string> list = await context.JsonUser.AsNoTracking().Select(x => x.Data).ToListAsync();
                return list.Select(x => JsonConvert.DeserializeObject<UserMigrationDto>(x)).ToList();
            }
        }
    }
}