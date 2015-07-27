// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Program.cs
// Created          : 2015-07-27  3:21 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  4:14 PM
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

namespace DataTransfer
{
    internal class Program
    {
        // private static string connectionString = "BlobEndpoint=https://jymstoredev.blob.core.chinacloudapi.cn/;QueueEndpoint=https://jymstoredev.queue.core.chinacloudapi.cn/;TableEndpoint=https://jymstoredev.table.core.chinacloudapi.cn/;AccountName=jymstoredev;AccountKey=1dCLRLeIeUlLAIBsS9rYdCyFg3UNU239MkwzNOj3BYbREOlnBmM4kfTPrgvKDhSmh6sRp2MdkEYJTv4Ht3fCcg==";
        private static readonly CloudTable TransJBYTransaction = null;

        private static readonly CloudTable TransOrder = null;
        private static readonly CloudTable TransRegularProduct = null;
        private static readonly CloudTable TransTransaction = null;

        public static void Main(string[] args)
        {
            Work.Run();
            //var id = Guid.NewGuid();
            //Console.WriteLine(id.ToString());

            //try
            //{
            //    CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            //    CloudTableClient client = account.CreateCloudTableClient();
            //    TableRequestOptions options = new TableRequestOptions();
            //    options.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(2), 10);
            //    client.DefaultRequestOptions = options;
            //    transOrder = client.GetTableReference("TransOrder");
            //    transTransaction = client.GetTableReference("TransTransaction");
            //    transRegularProduct = client.GetTableReference("TransRegularProduct");
            //    transJBYTransaction = client.GetTableReference("TransJBYTransaction");
            //    transOrder.CreateIfNotExists();
            //    transTransaction.CreateIfNotExists();
            //    transRegularProduct.CreateIfNotExists();
            //    transJBYTransaction.CreateIfNotExists();

            //    using (var context = new OldDBContext())
            //    {
            //        Dictionary<string, object> orderArgs = new Dictionary<string, object>();
            //        Dictionary<string, object> userArgs = new Dictionary<string, object>();
            //        Dictionary<string, object> productArgs = new Dictionary<string, object>();

            //        orderArgs.Add("Comment", "由原订单数据迁移");
            //        userArgs.Add("Comment", "由原用户数据迁移");
            //        productArgs.Add("Comment", "由原产品数据迁移");

            //        #region OrderRegion
            //        var oldOrderList = context.TransOrderInfo.Where(x=>x.ProductId != "cc93b32c0536487fac57014b5b3de4b1").Take(5).ToList();

            //        foreach (var oldOrder in oldOrderList)
            //        {
            //            var oldUser = context.TransUserInfo.Where(u => u.UserId == oldOrder.UserId).FirstOrDefault();

            //            UserInfo user = new UserInfo
            //            {
            //                Args = userArgs,
            //                Balance = -1,
            //                BankCardsCount = (int)oldUser.BankCardsCount,
            //                Cellphone = oldUser.Cellphone,
            //                ClientType = oldUser.ClientType,
            //                Closed = false,
            //                ContractId = oldUser.ContractId,
            //                Credential = Utils.getCredential(oldUser.Credential),
            //                CredentialNo = oldUser.CredentialNo,
            //                Crediting = -1,
            //                Debiting = 0,
            //                HasSetPassword = oldUser.HasSetPassword > 0,
            //                HasSetPaymentPassword = oldUser.HasSetPaymentPassword > 0,
            //                InvestingInterest = -1,
            //                InvestingPrincipal = -1,
            //                InviteBy = oldUser.InviteBy,
            //                JBYAccrualAmount = oldUser.JBYAccrualAmount * 100,
            //                JBYLastInterest = -1,
            //                JBYTotalAmount = -1,
            //                JBYTotalInterest = -1,
            //                JBYTotalPricipal = -1,
            //                JBYWithdrawalableAmount = -1,
            //                LoginNames = new List<string>() { oldUser.LoginNames },
            //                MonthWithdrawalCount = oldUser.MonthWithdrawalCount,
            //                OutletCode = Utils.getOutletCode(oldUser.OutletCode),
            //                PasswordErrorCount = oldUser.PasswordErrorCount,
            //                PaymentPasswordErrorCount = (int)oldUser.PaymentPasswordErrorCount,
            //                RealName = oldUser.RealName,
            //                RegisterTime = oldUser.RegisterTime,
            //                TodayJBYWithdrawalAmount = oldUser.TodayJBYWithdrawalAmount,
            //                TodayWithdrawalCount = oldUser.TodayWithdrawalCount,
            //                TotalInterest = oldUser.TotalInterest,
            //                TotalPrincipal = oldUser.TotalPrincipal,
            //                UserId = new Guid(oldUser.UserId),
            //                Verified = (bool)oldUser.Verified,
            //                VerifiedTime = oldUser.VerifiedTime,
            //                WithdrawalableAmount = oldUser.WithdrawalableAmount
            //            };

            //            //购买
            //            Guid transactionId = Guid.NewGuid();

            //            //orderDic.Add("IsRepaid",oldOrder.IsRepaid);
            //            OrderInfo order = new OrderInfo
            //            {
            //                AccountTransactionId = transactionId,
            //                Args = orderArgs,
            //                Cellphone = oldOrder.Cellphone,
            //                ExtraInterest = (long)(oldOrder.ExtraInterest * 100),
            //                ExtraInterestRecords = new List<Yuyi.Jinyinmao.Domain.ExtraInterestRecord>(),
            //                ExtraYield = oldOrder.ExtraYield * 100,
            //                Interest = (long)(oldOrder.Interest * 100),
            //                IsRepaid = oldOrder.IsRepaid,
            //                OrderId = new Guid(oldOrder.OrderId),
            //                OrderNo = oldOrder.OrderNo,
            //                OrderTime = oldOrder.OrderTime,
            //                Principal = (long)(oldOrder.Principal * 100),
            //                ProductCategory = Utils.getProductCategory(oldOrder.ProductCategory, oldOrder.ProductType),
            //                ProductId = new Guid(oldOrder.ProductId),
            //                ProductSnapshot = null,
            //                RepaidTime = null,
            //                ResultCode = 10000,
            //                ResultTime = oldOrder.ResultTime,
            //                SettleDate = Utils.getDate(oldOrder.SettleDate),
            //                TransDesc = "充值成功，购买理财产品",
            //                UserId = new Guid(oldOrder.UserId),
            //                UserInfo = user,
            //                ValueDate = Utils.getDate(oldOrder.ValueDate),
            //                Yield = (int)(oldOrder.Yield * 100)
            //            };
            //            Console.WriteLine(oldOrder.OrderId);
            //           //SaveOrderInfoToAzure(order);
            //            SaveDataToAzure<OrderEntity, OrderInfo>(transOrder, order, order.UserId.ToString(), order.OrderId.ToString());

            //            //create transaction

            //            if (order.ProductId == new Guid("cc93b32c0536487fac57014b5b3de4b1"))
            //            {
            //                JBYTransactionTransfer(order, user);
            //            }
            //            else
            //            {
            //                RegularTransactionTransfer(order, user);
            //            }
            //        }
            //        #endregion

            //        #region productRegion
            //       // RegularProductTransfer(productArgs);
            //        #endregion
            //    }

            //    Console.ReadKey();

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }

        private static JBYAccountTransaction GenerateJBYTransaction(TranscationState type, OrderInfo order, UserInfo user)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>
            {
                { "Comment", "由原流水数据迁移" },
                { "IsRepaid", order.IsRepaid }
            };

            using (var context = new OldDBContext())
            {
                var oldTransaction = context.TransSettleAccountTransaction.FirstOrDefault(t => t.OrderId == order.OrderId.ToString().Replace("-", ""));

                // TODO: oldTransaction null 值判断
                if (oldTransaction == null)
                {
                    return null;
                }

                //pre deal
                JBYAccountTransaction transaction = new JBYAccountTransaction
                {
                    Amount = order.Principal * 100,
                    ProductId = new Guid("cc93b32c0536487fac57014b5b3de4b1"),
                    Args = dic,
                    //BankCardNo = oldTransaction.BankCardNo,
                    //ChannelCode
                    //OrderId = order.OrderId,
                    ResultCode = 1,
                    ResultTime = oldTransaction.CallbackTime ?? order.OrderTime,
                    //SequenceNo = order.OrderNo,
                    //Trade
                    //TradeCode
                    //TransactionId
                    TransactionTime = order.OrderTime,
                    //TransDesc
                    UserId = order.UserId,
                    UserInfo = user
                };

                //suf deal
                switch (type)
                {
                    case TranscationState.ChongZhi:

                        transaction.Trade = Trade.Debit;
                        transaction.TradeCode = 1005051001;
                        transaction.TransactionId = Guid.NewGuid();
                        transaction.TransDesc = "个人钱包账户充值";
                        //transaction.OrderId = Guid.Empty;

                        break;

                    case TranscationState.ToJBY:
                        transaction.Trade = Trade.Credit;
                        transaction.TradeCode = 1005012003;
                        transaction.TransactionId = order.AccountTransactionId;
                        transaction.TransDesc = "钱包金额转为金包银金额";
                        break;

                    case TranscationState.RecieveByQianBao:
                        transaction.Trade = Trade.Debit;
                        transaction.TradeCode = 2001051102;
                        transaction.TransactionId = Guid.NewGuid();
                        transaction.TransDesc = "金包银金额收到钱包转入金额";
                        break;

                    case TranscationState.ToQianBao:
                        transaction.Trade = Trade.Credit;
                        transaction.TradeCode = 2001012002;
                        transaction.TransactionId = Guid.NewGuid();
                        transaction.TransDesc = "金包银金额转为钱包金额";
                        break;

                    case TranscationState.RecieveByJBY:
                        transaction.Trade = Trade.Debit;
                        transaction.TradeCode = 1005011103;
                        transaction.TransactionId = Guid.NewGuid();
                        transaction.TransDesc = "钱包收到金包银转入金额";
                        break;

                    case TranscationState.QuXian:
                        transaction.Trade = Trade.Credit;
                        transaction.TradeCode = 1005052001;
                        transaction.TransactionId = Guid.NewGuid();
                        transaction.TransDesc = "个人钱包账户取现";
                        break;
                }
                return transaction;
            }
        }

        private static void JBYTransactionTransfer(OrderInfo order, UserInfo user)
        {
            List<JBYAccountTransaction> transactionList = new List<JBYAccountTransaction>();
            JBYAccountTransaction transChongZhi = GenerateJBYTransaction(TranscationState.ChongZhi, order, user);
            JBYAccountTransaction transToJBY = GenerateJBYTransaction(TranscationState.ToJBY, order, user);
            JBYAccountTransaction transRecieveByQianBao = GenerateJBYTransaction(TranscationState.RecieveByQianBao, order, user);
            JBYAccountTransaction transToQianBao = GenerateJBYTransaction(TranscationState.ToQianBao, order, user);
            JBYAccountTransaction transRecieveByJBY = GenerateJBYTransaction(TranscationState.RecieveByJBY, order, user);
            JBYAccountTransaction transQuXian = GenerateJBYTransaction(TranscationState.QuXian, order, user);
            transactionList.Add(transChongZhi);
            transactionList.Add(transToJBY);
            transactionList.Add(transRecieveByQianBao);
            transactionList.Add(transToQianBao);
            transactionList.Add(transRecieveByJBY);
            transactionList.Add(transQuXian);
            SaveJBYTransactionToAzure(TransJBYTransaction, transactionList);
        }

        private static void RegularProductTransfer(Dictionary<string, object> productArgs)
        {
            using (var context = new OldDBContext())
            {
                var oldProductList = context.TransRegularProductState.Where(p => p.ProductId != "cc93b32c0536487fac57014b5b3de4b1").Take(10).ToList();

                foreach (var oldProduct in oldProductList)
                {
                    //-1 condition, null
                    var agreement1 = context.Agreements.FirstOrDefault(a => a.Id == oldProduct.Agreement1);
                    var agreement2 = context.Agreements.FirstOrDefault(a => a.Id == oldProduct.Agreement2);

                    RegularProductMigrationDto regularProduct = new RegularProductMigrationDto
                    {
                        Agreement1 = agreement1 != null ? agreement1.Content : string.Empty,
                        Agreement2 = agreement2 != null ? agreement2.Content : string.Empty,
                        Args = productArgs,
                        BankName = oldProduct.BankName, //186 items null, ignore
                        Drawee = oldProduct.Drawee,
                        DraweeInfo = oldProduct.DraweeInfo,
                        EndorseImageLink = oldProduct.EndorseImageLink,
                        EndSellTime = oldProduct.EndSellTime,
                        EnterpriseInfo = oldProduct.EnterpriseInfo,
                        EnterpriseLicense = oldProduct.EnterpriseInfo,
                        EnterpriseName = oldProduct.EnterpriseName,
                        FinancingSumAmount = (long)(oldProduct.FinancingSumAmount * oldProduct.UnitPrice * 100),
                        IssueNo = oldProduct.IssueNo,
                        IssueTime = oldProduct.IssueTime,
                        Period = oldProduct.Period,
                        PledgeNo = oldProduct.PledgeNo,
                        ProductCategory = Utils.GetProductCategory(oldProduct.ProductCategory, oldProduct.ProductType),
                        ProductName = Utils.GetProductName(oldProduct.ProductName),
                        ProductNo = oldProduct.ProductNo,
                        Repaid = oldProduct.Repaid,
                        RepaidTime = null,
                        RepaymentDeadline = oldProduct.RepaymentDeadline,
                        RiskManagement = oldProduct.RiskManagement,
                        RiskManagementInfo = oldProduct.RiskManagementInfo,
                        RiskManagementMode = Utils.GetRiskManagementMode(oldProduct.RiskManagementMode),
                        SettleDate = Utils.GetDate(oldProduct.SettleDate),
                        SoldOut = oldProduct.SoldOut,
                        SoldOutTime = oldProduct.SoldOutTime,
                        StartSellTime = oldProduct.StartSellTime,
                        UnitPrice = (int)(oldProduct.UnitPrice * 100),
                        Usage = oldProduct.Usage,
                        ValueDate = null,
                        ValueDateMode = 0,
                        Yield = (int)(oldProduct.Yield * 100)
                    };
                    SaveDataToAzure<RegularProductEntity, RegularProductMigrationDto>(TransRegularProduct, regularProduct, regularProduct.ProductCategory.ToString(), oldProduct.ProductId);
                }
            }
        }

        private static void RegularTransactionTransfer(OrderInfo order, UserInfo user)
        {
            List<SettleAccountTransaction> transactionList = new List<SettleAccountTransaction>();
            //SettleAccountTransaction chongZhiTransaction = generateTransaction(TranscationState.ChongZhi, order, user);
            //SettleAccountTransaction gouMaiTransaction = generateTransaction(TranscationState.GouMai, order, user);
            //SettleAccountTransaction benJinTransaction = generateTransaction(TranscationState.BenJin, order, user);
            //SettleAccountTransaction liXiTransaction = generateTransaction(TranscationState.LiXi, order, user);
            //SettleAccountTransaction quXianTransaction = generateTransaction(TranscationState.QuXian, order, user);
            //transactionList.Add(chongZhiTransaction);
            //transactionList.Add(gouMaiTransaction);
            //transactionList.Add(benJinTransaction);
            //transactionList.Add(liXiTransaction);
            //transactionList.Add(quXianTransaction);
            SaveTransactionToAzure(transactionList);
        }

        private static void SaveDataToAzure<TResult, TSource>(CloudTable table, TSource source, string partitionKey, string rowKey) where TResult : TableEntity, new()
        {
            try
            {
                TResult entity = Utils.ReflectProperties<TResult, TSource>(source);
                entity.PartitionKey = partitionKey;
                entity.RowKey = rowKey;
                table.Execute(TableOperation.Insert(entity));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void SaveJBYTransactionToAzure(CloudTable table, IEnumerable<JBYAccountTransaction> list)
        {
            TableBatchOperation batch = new TableBatchOperation();
            try
            {
                foreach (var item in list)
                {
                    JBYAccountTransactionEntity entity = Utils.ReflectProperties<JBYAccountTransactionEntity, JBYAccountTransaction>(item);
                    entity.PartitionKey = item.ProductId.ToString();
                    entity.RowKey = item.TransactionId.ToString();
                    batch.InsertOrReplace(entity);
                }
                table.ExecuteBatch(batch);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void SaveOrderInfoToAzure(OrderInfo order)
        {
            try
            {
                OrderEntity orderEntity = Utils.ReflectProperties<OrderEntity, OrderInfo>(order);
                orderEntity.PartitionKey = order.UserId.ToString();
                orderEntity.RowKey = order.OrderId.ToString();
                TransOrder.Execute(TableOperation.InsertOrReplace(orderEntity));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void SaveTransactionToAzure(List<SettleAccountTransaction> transactionList)
        {
            try
            {
                for (int i = 0; i < transactionList.Count; i++)
                {
                    var transaction = transactionList.ElementAt(i);
                    TransactionEntity transactionEntity = Utils.ReflectProperties<TransactionEntity, SettleAccountTransaction>(transaction);
                    transactionEntity.PartitionKey = transaction.OrderId.ToString();
                    transactionEntity.RowKey = transaction.TransactionId.ToString();
                    var result = TransTransaction.Execute(TableOperation.InsertOrReplace(transactionEntity));
                    //batch.Insert(transactionEntity);
                    Console.WriteLine(JsonConvert.SerializeObject(result));
                }

                // transTransaction.ExecuteBatch(batch);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}