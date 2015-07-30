using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransfer.Models;
using Moe.Lib;
using Newtonsoft.Json;
using Orleans;
using Orleans.Runtime.Host;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace DataTransfer
{
    public class MemoryWork
    {
        private static readonly Guid JBYProductId;
        private static readonly string StrDefaultJBYProductId = "5e35201f315e41d4b11f014d6c01feb8";

        static MemoryWork()
        {
            string StrJBYProductId = ConfigurationManager.AppSettings.Get("StrJBYProductId");
            StrJBYProductId = string.IsNullOrEmpty(StrJBYProductId) ? StrDefaultJBYProductId : StrJBYProductId;
            JBYProductId = new Guid(StrJBYProductId);

            if (GrainClient.IsInitialized)
            {
                return;
            }
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            GrainClient.Initialize(Path.Combine(baseDir, "ReleaseConfiguration.xml"));
            Console.ReadKey();
        }

        public async static Task Run()
        {
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
                var user = await UserFactory.GetGrain(item.UserId).MigrateAsync(item);
            }
        }

        private async static Task<List<RegularProductMigrationDto>> GetProductsAsync()
        {
            using (var context = new OldDBContext())
            {
                var list = context.JsonProduct.Select(item => item.Data).Take(10).ToList();
                return await Task.Run(() => list.Select(item => JsonConvert.DeserializeObject<RegularProductMigrationDto>(item)).ToList());
            }
        }

        private async static Task<List<UserMigrationDto>> GetUsersAsync()
        {
            using (var context = new OldDBContext())
            {
                var list = context.JsonUser.Select(item => item.Data).ToList();
                return await Task.Run(() => list.Select(x => JsonConvert.DeserializeObject<UserMigrationDto>(x)).ToList());
            }
        }
    }
}