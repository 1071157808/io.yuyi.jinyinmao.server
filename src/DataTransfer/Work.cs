// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Work.cs
// Created          : 2015-07-27  6:28 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  6:49 PM
// ***********************************************************************
// <copyright file="Work.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DataTransfer.Models;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace DataTransfer
{
    /// <summary>
    ///     Work.
    /// </summary>
    public class Work
    {
        private static readonly List<JBYAccountTransaction> JBYAccountTransactionList = new List<JBYAccountTransaction>();
        private static readonly Dictionary<string, object> OrderArgs = new Dictionary<string, object>();
        private static readonly Dictionary<string, object> ProductArgs = new Dictionary<string, object>();

        [SuppressMessage("ReSharper", "CollectionNeverQueried.Local")]
        private static readonly List<SettleAccountTransaction> SettleAccountTransactionList = new List<SettleAccountTransaction>();

        private static readonly Dictionary<string, object> UserArgs = new Dictionary<string, object>();

        /// <summary>
        ///     Runs this instance.
        /// </summary>
        public static void Run()
        {
            OrderArgs.Add("Comment", "由原订单数据迁移");
            UserArgs.Add("Comment", "由原用户数据迁移");
            ProductArgs.Add("Comment", "由原产品数据迁移");

            //get products
            RegularProductTransfer(ProductArgs);
            UserTransfer();
            Console.ReadKey();
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

        private static SettleAccountTransaction GenerateTransaction(TranscationState type, OrderInfo order, UserInfo user)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>
            {
                { "Comment", "由原流水数据迁移" },
                { "IsRepaid", order.IsRepaid }
            };

            using (var context = new OldDBContext())
            {
                var oldTransaction = context.TransSettleAccountTransaction.FirstOrDefault(t => t.OrderId == order.OrderId.ToString().Replace("-", "") && order.ProductId.ToString() != "cc93b32c0536487fac57014b5b3de4b1");
                if (oldTransaction == null) return null;
                //pre deal
                SettleAccountTransaction transaction = new SettleAccountTransaction
                {
                    Amount = order.Principal * 100,
                    Args = dic,
                    BankCardNo = oldTransaction.BankCardNo,
                    //ChannelCode
                    OrderId = order.OrderId,
                    ResultCode = 1,
                    ResultTime = oldTransaction.CallbackTime ?? order.OrderTime,
                    SequenceNo = order.OrderNo,
                    //Trade
                    //TradeCode
                    //TransactionId
                    TransactionTime = order.OrderTime,
                    //TransDesc
                    UserId = order.UserId
                    //UserInfo = user
                };

                //suf deal
                switch (type)
                {
                    case TranscationState.ChongZhi:
                        transaction.ChannelCode = 10010;
                        transaction.Trade = Trade.Debit;
                        transaction.TradeCode = 1005051001;
                        transaction.TransactionId = Guid.NewGuid();
                        transaction.TransDesc = "个人钱包账户充值";
                        //transaction.OrderId = Guid.Empty;

                        break;

                    case TranscationState.GouMai:
                        transaction.ChannelCode = 10000;
                        transaction.Trade = Trade.Debit;
                        transaction.TradeCode = 1005012004;
                        transaction.TransactionId = order.AccountTransactionId;
                        transaction.TransDesc = "购买银票或者商票产品(银行专区)";
                        transaction.BankCardNo = string.Empty;

                        break;

                    case TranscationState.BenJin:
                        transaction.ChannelCode = 10000;
                        transaction.Trade = Trade.Credit;
                        transaction.TradeCode = 1005011104;
                        transaction.TransactionId = Guid.NewGuid();
                        transaction.TransDesc = "钱包收到银票或者商票产品返还本金(银行专区)";

                        break;

                    case TranscationState.LiXi:
                        transaction.ChannelCode = 10000;
                        transaction.Trade = Trade.Credit;
                        transaction.TradeCode = 1005011105;
                        transaction.TransactionId = Guid.NewGuid();
                        transaction.TransDesc = "钱包收到银票或者商票产品结算利息(银行专区)";

                        break;

                    case TranscationState.QuXian:
                        transaction.ChannelCode = 10010;
                        transaction.Trade = Trade.Credit;
                        transaction.TradeCode = 1005052001;
                        transaction.TransactionId = Guid.NewGuid();
                        transaction.TransDesc = "个人钱包账户取现";

                        break;

                    default:
                        transaction.ChannelCode = -1;

                        break;
                }

                return transaction;
            }
        }

        #region ProductTransfer

        [SuppressMessage("ReSharper", "FunctionComplexityOverflow")]
        private static void RegularProductTransfer(Dictionary<string, object> productArgs)
        {
            using (var context = new OldDBContext())
            {
                var oldProductList = context.TransRegularProductState.Where(p => p.ProductId != "cc93b32c0536487fac57014b5b3de4b1").Take(10).ToList();

                if (oldProductList.Count == 0) return;

                foreach (var oldProduct in oldProductList)
                {
                    #region product

                    //-1 condition, null
                    if (oldProduct == null) continue;

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

                    #region orders

                    var oldOrderList = context.TransOrderInfo.Where(o => o.ProductId == oldProduct.ProductId).ToList();

                    Dictionary<Guid, OrderInfo> orders = new Dictionary<Guid, OrderInfo>();

                    foreach (var oldOrder in oldOrderList)
                    {
                        var oldUser = context.TransUserInfo.FirstOrDefault(u => u.UserId == oldOrder.UserId);

                        // TODO: oldUser null 值判断
                        if (oldUser == null) continue;

                        UserInfo userInfo = new UserInfo
                        {
                            Args = UserArgs,
                            Balance = -1,
                            BankCardsCount = oldUser.BankCardsCount.GetValueOrDefault(),
                            Cellphone = oldUser.Cellphone,
                            ClientType = oldUser.ClientType,
                            Closed = false,
                            ContractId = oldUser.ContractId,
                            Credential = Utils.GetCredential(oldUser.Credential),
                            CredentialNo = oldUser.CredentialNo,
                            Crediting = -1,
                            Debiting = 0,
                            HasSetPassword = oldUser.HasSetPassword > 0,
                            HasSetPaymentPassword = oldUser.HasSetPaymentPassword > 0,
                            InvestingInterest = -1,
                            InvestingPrincipal = -1,
                            InviteBy = oldUser.InviteBy,
                            JBYAccrualAmount = oldUser.JBYAccrualAmount * 100,
                            JBYLastInterest = -1,
                            JBYTotalAmount = -1,
                            JBYTotalInterest = -1,
                            JBYTotalPricipal = -1,
                            JBYWithdrawalableAmount = -1,
                            LoginNames = new List<string> { oldUser.LoginNames },
                            MonthWithdrawalCount = oldUser.MonthWithdrawalCount,
                            OutletCode = Utils.GetOutletCode(oldUser.OutletCode),
                            PasswordErrorCount = oldUser.PasswordErrorCount,
                            PaymentPasswordErrorCount = oldUser.PaymentPasswordErrorCount.GetValueOrDefault(),
                            RealName = oldUser.RealName,
                            RegisterTime = oldUser.RegisterTime,
                            TodayJBYWithdrawalAmount = oldUser.TodayJBYWithdrawalAmount,
                            TodayWithdrawalCount = oldUser.TodayWithdrawalCount,
                            TotalInterest = oldUser.TotalInterest,
                            TotalPrincipal = oldUser.TotalPrincipal,
                            UserId = new Guid(oldUser.UserId),
                            Verified = oldUser.Verified.GetValueOrDefault(),
                            VerifiedTime = oldUser.VerifiedTime,
                            WithdrawalableAmount = oldUser.WithdrawalableAmount
                        };

                        //购买transaction
                        Guid transactionId = Guid.NewGuid();

                        OrderInfo orderInfo = new OrderInfo
                        {
                            AccountTransactionId = transactionId,
                            Args = OrderArgs,
                            Cellphone = oldOrder.Cellphone,
                            ExtraInterest = (long)(oldOrder.ExtraInterest * 100),
                            ExtraInterestRecords = new List<ExtraInterestRecord>(),
                            ExtraYield = oldOrder.ExtraYield * 100,
                            Interest = (long)(oldOrder.Interest * 100),
                            IsRepaid = oldOrder.IsRepaid,
                            OrderId = new Guid(oldOrder.OrderId),
                            OrderNo = oldOrder.OrderNo,
                            OrderTime = oldOrder.OrderTime,
                            Principal = (long)(oldOrder.Principal * 100),
                            ProductCategory = Utils.GetProductCategory(oldOrder.ProductCategory, oldOrder.ProductType),
                            ProductId = new Guid(oldOrder.ProductId),
                            ProductSnapshot = null,
                            RepaidTime = null,
                            ResultCode = 10000,
                            ResultTime = oldOrder.ResultTime,
                            SettleDate = Utils.GetDate(oldOrder.SettleDate),
                            TransDesc = "充值成功，购买理财产品",
                            UserId = new Guid(oldOrder.UserId),
                            UserInfo = userInfo,
                            ValueDate = Utils.GetDate(oldOrder.ValueDate),
                            Yield = (int)(oldOrder.Yield * 100)
                        };

                        SettleAccountTransaction chongZhiTransaction = GenerateTransaction(TranscationState.ChongZhi, orderInfo, userInfo);
                        SettleAccountTransaction gouMaiTransaction = GenerateTransaction(TranscationState.GouMai, orderInfo, userInfo);
                        SettleAccountTransaction benJinTransaction = GenerateTransaction(TranscationState.BenJin, orderInfo, userInfo);
                        SettleAccountTransaction liXiTransaction = GenerateTransaction(TranscationState.LiXi, orderInfo, userInfo);
                        SettleAccountTransaction quXianTransaction = GenerateTransaction(TranscationState.QuXian, orderInfo, userInfo);
                        SettleAccountTransactionList.Add(chongZhiTransaction);
                        SettleAccountTransactionList.Add(gouMaiTransaction);
                        SettleAccountTransactionList.Add(benJinTransaction);
                        SettleAccountTransactionList.Add(liXiTransaction);
                        SettleAccountTransactionList.Add(quXianTransaction);

                        orders.Add(orderInfo.OrderId, orderInfo);
                    }

                    #endregion orders

                    regularProduct.Orders = orders;

                    #endregion product

                    Console.WriteLine(JsonConvert.SerializeObject(regularProduct));
                }
            }
        }

        #endregion ProductTransfer

        #region UserTransfer

        [SuppressMessage("ReSharper", "LoopCanBePartlyConvertedToQuery")]
        private static void UserTransfer()
        {
            using (var context = new OldDBContext())
            {
                var transUserInfos = context.TransUserInfo.Take(10).ToList();
                foreach (var transUserInfo in transUserInfos)
                {
                    if (transUserInfo == null) continue;

                    #region userinfo

                    UserInfo userInfo = new UserInfo
                    {
                        Args = UserArgs,
                        Balance = -1,
                        BankCardsCount = transUserInfo.BankCardsCount.GetValueOrDefault(),
                        Cellphone = transUserInfo.Cellphone,
                        ClientType = transUserInfo.ClientType,
                        Closed = false,
                        ContractId = transUserInfo.ContractId,
                        Credential = Utils.GetCredential(transUserInfo.Credential),
                        CredentialNo = !string.IsNullOrWhiteSpace(transUserInfo.CredentialNo) ? transUserInfo.CredentialNo : string.Empty,
                        Crediting = -1,
                        Debiting = 0,
                        HasSetPassword = transUserInfo.HasSetPassword.GetValueOrDefault() > 0,
                        HasSetPaymentPassword = transUserInfo.HasSetPaymentPassword.GetValueOrDefault() > 0,
                        InvestingInterest = -1,
                        InvestingPrincipal = -1,
                        InviteBy = !string.IsNullOrWhiteSpace(transUserInfo.InviteBy) ? transUserInfo.InviteBy : string.Empty,
                        JBYAccrualAmount = -1,
                        JBYLastInterest = -1,
                        JBYTotalAmount = -1,
                        JBYTotalInterest = -1,
                        JBYTotalPricipal = -1,
                        JBYWithdrawalableAmount = -1,
                        LoginNames = new List<string> { transUserInfo.LoginNames },
                        MonthWithdrawalCount = transUserInfo.MonthWithdrawalCount,
                        OutletCode = Utils.GetOutletCode(transUserInfo.OutletCode),
                        PasswordErrorCount = transUserInfo.PasswordErrorCount,
                        PaymentPasswordErrorCount = transUserInfo.PaymentPasswordErrorCount.GetValueOrDefault(),
                        RealName = string.IsNullOrWhiteSpace(transUserInfo.RealName)?string.Empty: transUserInfo.RealName,
                        RegisterTime = transUserInfo.RegisterTime,
                        TodayJBYWithdrawalAmount = transUserInfo.TodayJBYWithdrawalAmount,
                        TodayWithdrawalCount = transUserInfo.TodayWithdrawalCount,
                        TotalInterest = transUserInfo.TotalInterest,
                        TotalPrincipal = transUserInfo.TotalPrincipal,
                        UserId = new Guid(transUserInfo.UserId),
                        Verified = transUserInfo.Verified.GetValueOrDefault(),
                        VerifiedTime = transUserInfo.VerifiedTime,
                        WithdrawalableAmount = transUserInfo.WithdrawalableAmount
                    };

                    #endregion userinfo

                    #region Order
                    var orders = context.TransOrderInfo.Where(o => o.UserId == transUserInfo.UserId).ToList().Select(x => new Order()
                    {
                        AccountTransactionId = GetSettleTransactionId(x.OrderId),
                        Args = OrderArgs,
                        Cellphone = x.Cellphone,
                        ExtraInterest = (long)x.ExtraInterest,
                        ExtraInterestRecords = new List<ExtraInterestRecord>(),
                        ExtraYield = (x.ExtraYield * 100),
                        Interest = (long)x.Interest,
                        IsRepaid = x.IsRepaid,
                        OrderId = new Guid(x.OrderId),
                        OrderNo = x.OrderNo,
                        OrderTime = x.OrderTime,
                        Principal = (long)(x.Principal * 100),
                        ProductCategory = Utils.GetProductCategory(x.ProductCategory, x.ProductType),
                        ProductId = new Guid(x.ProductId),
                        ProductSnapshot = null,
                        RepaidTime = null,
                        ResultCode = 10000,
                        SettleDate = Utils.GetDate(x.SettleDate),
                        TransDesc = "充值成功，购买理财产品",
                        UserId = new Guid(x.UserId),
                        UserInfo = userInfo,
                        ValueDate = Utils.GetDate(x.ValueDate),
                        Yield = (int)(x.Yield * 100)
                    }).ToList();

                    #endregion Order

                    var user = new UserMigrationDto
                    {
                        Args = UserArgs,
                        BankCards = Utils.GetBankCards(transUserInfo.UserId),
                        Cellphone = transUserInfo.Cellphone,
                        ClientType = transUserInfo.ClientType,
                        Closed = false,
                        ContractId = transUserInfo.ContractId,
                        Credential = Utils.GetCredential(transUserInfo.Credential),
                        CredentialNo = transUserInfo.CredentialNo,
                        EncryptedPassword = transUserInfo.EncryptedPassword,
                        EncryptedPaymentPassword = string.IsNullOrWhiteSpace(transUserInfo.EncryptedPaymentPassword)?string.Empty: transUserInfo.EncryptedPaymentPassword,
                        InviteBy = string.IsNullOrEmpty(transUserInfo.InviteBy)?string.Empty: transUserInfo.InviteBy,
                        JBYAccount = null,
                        LoginNames = new List<string> { transUserInfo.LoginNames },
                        Orders = Utils.CreateOrders(orders),
                        OutletCode = transUserInfo.OutletCode,
                        PaymentSalt = string.IsNullOrEmpty(transUserInfo.PaymentSalt) ? string.Empty : transUserInfo.PaymentSalt,
                        RealName = string.IsNullOrEmpty(transUserInfo.RealName)?string.Empty: transUserInfo.RealName,
                        RegisterTime = transUserInfo.RegisterTime,
                        Salt =  transUserInfo.Salt,
                        SettleAccount = GetSettleAccountTransaction(transUserInfo.UserId),
                        UserId = new Guid(transUserInfo.UserId),
                        Verified = transUserInfo.Verified.GetValueOrDefault(),
                        VerifiedTime = transUserInfo.VerifiedTime
                    };

                    Console.WriteLine(JsonConvert.SerializeObject(user));
                }
            }
        }

        #endregion UserTransfer

        private static Guid GetSettleTransactionId(string orderId)
        {
            return SettleAccountTransactionList.Where(x => x.OrderId == new Guid(orderId) && x.TradeCode == 10000).Select(x => x.TransactionId).FirstOrDefault();
        }

        private static void JBYTransactionTransfer(OrderInfo order, UserInfo user)
        {
            JBYAccountTransaction transChongZhi = GenerateJBYTransaction(TranscationState.ChongZhi, order, user);
            JBYAccountTransaction transToJBY = GenerateJBYTransaction(TranscationState.ToJBY, order, user);
            JBYAccountTransaction transRecieveByQianBao = GenerateJBYTransaction(TranscationState.RecieveByQianBao, order, user);
            JBYAccountTransaction transToQianBao = GenerateJBYTransaction(TranscationState.ToQianBao, order, user);
            JBYAccountTransaction transRecieveByJBY = GenerateJBYTransaction(TranscationState.RecieveByJBY, order, user);
            JBYAccountTransaction transQuXian = GenerateJBYTransaction(TranscationState.QuXian, order, user);
            JBYAccountTransactionList.Add(transChongZhi);
            JBYAccountTransactionList.Add(transToJBY);
            JBYAccountTransactionList.Add(transRecieveByQianBao);
            JBYAccountTransactionList.Add(transToQianBao);
            JBYAccountTransactionList.Add(transRecieveByJBY);
            JBYAccountTransactionList.Add(transQuXian);
        }

        private static void StorageDataToTempDB<T>(IEnumerable<T> list) where T : class
        {
            using (var context = new OldDBContext())
            {
                foreach (var item in list)
                {
                    context.Set<T>().Add(item);
                }
                context.SaveChanges();
            }
        }

        #region 通过UserId查询流水

        /// <summary>
        ///     通过UserId查询流水
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Dictionary<Guid, SettleAccountTransaction> GetSettleAccountTransaction(string userId)
        {
            Dictionary<Guid, SettleAccountTransaction> dic = new Dictionary<Guid, SettleAccountTransaction>();
            if (SettleAccountTransactionList != null && SettleAccountTransactionList.Count != 0)
            {
                var list = SettleAccountTransactionList.Where(x => x.UserId == new Guid(userId)).ToList();
                if (list.Count != 0)
                {
                    foreach (var item in list)
                    {
                        dic.Add(item.TransactionId, item);
                    }
                }
            }
            return dic;
        }
        #endregion

        public static Dictionary<Guid, JBYAccountTransaction> GetJBYAccountTransaction(string userId)
        {
            return null;
        }

    }
}