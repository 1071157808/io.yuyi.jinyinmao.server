// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : MemoryWork.cs
// Created          : 2015-08-02  11:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-03  5:14 AM
// ***********************************************************************
// <copyright file="MemoryWork.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataTransfer.Models;
using Moe.Lib;
using Newtonsoft.Json;
using Orleans;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace DataTransfer
{
    /// <summary>
    ///     MemoryWork.
    /// </summary>
    public class MemoryWork
    {
        [SuppressMessage("ReSharper", "NotAccessedField.Local")]
        private static readonly Guid JBYProductId;

        private static readonly int ProductExecuteDataCount;
        private static readonly string StrDefaultJBYProductId = "5e35201f315e41d4b11f014d6c01feb8";
        private static readonly int UserExecuteDataCount;

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
            GrainClient.SetResponseTimeout(TimeSpan.FromMinutes(2));

            ProductExecuteDataCount = 2000;
            UserExecuteDataCount = 4000;
            int.TryParse(ConfigurationManager.AppSettings.Get("ProductExecuteDataCount"), out ProductExecuteDataCount);
            int.TryParse(ConfigurationManager.AppSettings.Get("UserExecuteDataCount"), out UserExecuteDataCount);
        }

        /// <summary>
        ///     Runs this instance.
        /// </summary>
        /// <returns>Task.</returns>
        public static async Task RunProduct()
        {
            try
            {
                await ProductTaskAsync();
                //await UserTaskAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetExceptionString());
                throw;
            }
        }

        /// <summary>
        ///     Runs the user.
        /// </summary>
        /// <returns>Task.</returns>
        public static async Task RunUser()
        {
            try
            {
                await UserTaskAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetExceptionString());
                throw;
            }
        }

        #region 创建多个数据迁移任务

        private static async Task ProductTaskAsync()
        {
            double count = await GetProductCountAsync();
            List<Task> list = new List<Task>();
            try
            {
                for (int i = 0; i < Math.Ceiling(count / ProductExecuteDataCount); i++)
                {
                    list.Add(ProductMigrationAsync(ProductExecuteDataCount, i * ProductExecuteDataCount, i));
                    if (list.Count == 14)
                    {
                        await Task.WhenAll(list);
                        list.Clear();
                    }
                }

                await Task.WhenAll(list);
            }
            catch (Exception exception)
            {
                WriteException(exception);
                throw;
            }
        }

        private static async Task UserTaskAsync()
        {
            double count = await GetUserCountAsync();
            List<Task> list = new List<Task>();

            try
            {
                for (int i = 0; i < Math.Ceiling(count / UserExecuteDataCount); i++)
                {
                    list.Add(UserMigrationAsync(UserExecuteDataCount, i * UserExecuteDataCount, i));
                    if (list.Count == 14)
                    {
                        await Task.WhenAll(list);
                        list.Clear();
                    }
                }

                await Task.WhenAll(list);
            }
            catch (Exception exception)
            {
                WriteException(exception);
                throw;
            }
        }

        #endregion 创建多个数据迁移任务

        #region 分批次数据转移

        /// <summary>
        ///     Writes the exception.
        /// </summary>
        /// <param name="e">The e.</param>
        public static void WriteException(Exception e)
        {
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            if (e.InnerException != null)
            {
                WriteException(e.InnerException);
            }

            AggregateException e1 = e as AggregateException;
            if (e1 != null)
            {
                foreach (Exception innerException in e1.InnerExceptions)
                {
                    WriteException(innerException);
                }
            }

            DbEntityValidationException exception = e as DbEntityValidationException;
            if (exception != null)
            {
                Console.WriteLine(
                    exception.EntityValidationErrors.Select(
                        err => err.ValidationErrors.Select(v => v.ErrorMessage + v.PropertyName).Join(",")).Join(","));
            }

            Console.WriteLine(e.Message);
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            var c = Console.ReadKey();
            Console.WriteLine(c);
        }

        private static async Task ProductMigrationAsync(int takeCount, int skipCount, int threadId)
        {
            try
            {
                int i = 0;
                List<string> list = null;
                using (var context = new OldDBContext())
                {
                    try
                    {
                        list = await context.JsonProduct.AsNoTracking().OrderBy(x => x.ProductId).Skip(skipCount).Take(takeCount).Select(x => x.Data).ToListAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("?????????????????????????????????????");
                        Console.WriteLine("?????????????????????????????????????");
                        Console.WriteLine("?????????????????????????????????????");
                        Console.WriteLine("?????????????????????????????????????");
                        Console.WriteLine("?????????????????????????????????????");
                        Console.WriteLine("?????????????????????????????????????");
                        Console.WriteLine("?????????????????????????????????????");
                        Console.WriteLine("?????????????????????????????????????");
                        Console.WriteLine("?????????????????????????????????????");
                        Console.WriteLine("?????????????????????????????????????");
                        Console.WriteLine("?????????????????????????????????????");
                        Console.WriteLine("?????????????????????????????????????");
                        Console.WriteLine("?????????????????????????????????????");
                        WriteException(e);
                    }
                }
                if (list != null)
                {
                    foreach (RegularProductMigrationDto product in list.Select(x => JsonConvert.DeserializeObject<RegularProductMigrationDto>(x)))
                    {
                        i++;
                        try
                        {
                            //                            product.RiskManagement = product.RiskManagementMode;
                            //                            string riskManagementMode = "商票贷";
                            //                            string nv = product.ProductNo.Substring(0, 2).ToUpperInvariant();
                            //                            switch (nv)
                            //                            {
                            //                                case "DB":
                            //                                    riskManagementMode = "担保贷";
                            //                                    break;
                            //
                            //                                case "BL":
                            //                                    riskManagementMode = "保理贷";
                            //                                    break;
                            //
                            //                                case "A1":
                            //                                    riskManagementMode = "银保贷";
                            //                                    break;
                            //
                            //                                case "B1":
                            //                                    riskManagementMode = "银保贷";
                            //                                    break;
                            //
                            //                                case "YB":
                            //                                    riskManagementMode = "银保贷";
                            //                                    break;
                            //                            }
                            //
                            //                            product.RiskManagementMode = riskManagementMode;

                            var info = await RegularProductFactory.GetGrain(product.ProductId).MigrateAsync(product);
                            Console.WriteLine(info.ProductId.ToGuidString());
                            Console.WriteLine(i);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            WriteException(e);
                            Console.WriteLine(product.ToJson());
                            WriteException(e);
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetExceptionString());
                var c = Console.ReadKey();
                Console.WriteLine(c);
                throw;
            }
        }

        private static async Task UserMigrationAsync(int takeCount, int skipCount, int threadId)
        {
            int i = 0;
            try
            {
                List<string> list = null;
                using (var context = new OldDBContext())
                {
                    try
                    {
                        list = await context.JsonUser.AsNoTracking().OrderBy(x => x.UserId).Skip(skipCount).Take(takeCount).Select(x => x.Data).ToListAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        Console.WriteLine("??????????????????????????????????????????????????????");
                        WriteException(e);
                    }
                }
                if (list != null)
                {
                    foreach (UserMigrationDto item in list.Select(x => JsonConvert.DeserializeObject<UserMigrationDto>(x)))
                    {
                        try
                        {
                            foreach (KeyValuePair<Guid, Order> order in item.Orders)
                            {
                                order.Value.ResultTime = order.Value.OrderTime;
                            }

                            foreach (var t in item.SettleAccount)
                            {
                                if (t.Value.SequenceNo.IsNullOrEmpty())
                                {
                                    t.Value.SequenceNo = string.Empty;

                                    Console.WriteLine(item.Cellphone + t.Value.TransactionId);
                                    Console.WriteLine(item.Cellphone + t.Value.TransactionId);
                                    Console.WriteLine(item.Cellphone + t.Value.TransactionId);
                                    Console.WriteLine(item.Cellphone + t.Value.TransactionId);
                                    Console.WriteLine(item.Cellphone + t.Value.TransactionId);
                                    Console.WriteLine(item.Cellphone + t.Value.TransactionId);
                                    Console.WriteLine(item.Cellphone + t.Value.TransactionId);
                                    Console.WriteLine(item.Cellphone + t.Value.TransactionId);
                                    Console.WriteLine(item.Cellphone + t.Value.TransactionId);
                                }

                                t.Value.BankCardNo = string.Empty;
                            }

                            var info = await UserFactory.GetGrain(item.UserId).MigrateAsync(item);
                            Console.WriteLine(info.Cellphone);
                            Console.WriteLine(++i);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            Console.WriteLine("#########################################################");
                            WriteException(e);
                            Console.WriteLine(item.Cellphone);
                            var c = Console.ReadKey();
                            Console.WriteLine(c);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.GetExceptionString());
                WriteException(exception);
                var c = Console.ReadKey();
                Console.WriteLine(c);
                throw;
            }
        }

        #endregion 分批次数据转移

        #region 获取Product和User数量

        private static async Task<double> GetProductCountAsync()
        {
            using (OldDBContext context = new OldDBContext())
            {
                double count = await context.JsonProduct.AsNoTracking().Select(x => x.ProductId).CountAsync();
                return count;
            }
        }

        private static async Task<double> GetUserCountAsync()
        {
            using (OldDBContext context = new OldDBContext())
            {
                double count = await context.JsonUser.AsNoTracking().Select(x => x.UserId).CountAsync();
                return count;
            }
        }

        #endregion 获取Product和User数量
    }
}