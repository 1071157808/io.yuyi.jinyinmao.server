// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Work.cs
// Created          : 2015-08-02  11:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-03  3:56 AM
// ***********************************************************************
// <copyright file="Work.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DataTransfer.Models;
using Moe.Lib;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace DataTransfer
{
    /// <summary>
    ///     Work.
    /// </summary>
    [SuppressMessage("ReSharper", "ConvertConditionalTernaryToNullCoalescing")]
    [SuppressMessage("ReSharper", "ConstantConditionalAccessQualifier")]
    public class Work
    {
        private static readonly Guid JBYProductId;
        private static readonly Dictionary<string, object> OrderArgs = new Dictionary<string, object>();
        private static readonly Dictionary<string, object> ProductArgs = new Dictionary<string, object>();
        private static readonly int ProductExecuteDataCount;
        private static readonly string StrDefaultJBYProductId;
        private static readonly Dictionary<string, object> UserArgs = new Dictionary<string, object>();
        private static readonly int UserExecuteDataCount;

        static Work()
        {
            StrDefaultJBYProductId = ConfigurationManager.AppSettings.Get("StrJBYProductId").IsNotNullOrEmpty()
                ? ConfigurationManager.AppSettings.Get("StrJBYProductId")
                : "5e35201f315e41d4b11f014d6c01feb8";
            ProductExecuteDataCount = ConfigurationManager.AppSettings.Get("ProductExecuteDataCount").IsNotNullOrEmpty()
                ? int.Parse((ConfigurationManager.AppSettings.Get("ProductExecuteDataCount")))
                : 2000;
            UserExecuteDataCount = ConfigurationManager.AppSettings.Get("UserExecuteDataCount").IsNotNullOrEmpty()
                ? int.Parse((ConfigurationManager.AppSettings.Get("UserExecuteDataCount")))
                : 2000;
            JBYProductId = Guid.ParseExact(StrDefaultJBYProductId, "N");
        }

        #region Runs this instance

        /// <summary>
        ///     Runs this instance.
        /// </summary>
        public static async Task Run()
        {
            OrderArgs.Add("Comment", "由原订单数据迁移");
            UserArgs.Add("Comment", "由原用户数据迁移");
            ProductArgs.Add("Comment", "由原产品数据迁移");
            //await ProductTask();
            await UserTask();

            //ProductTransfer();
        }

        #endregion Runs this instance

        private static async Task<UserInfo> CreateUserInfoAsync(string strUserId)
        {
            using (OldDBContext context = new OldDBContext())
            {
                TransUserInfo oldUser = await context.TransUserInfo.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == strUserId);
                if (oldUser == null) return null;

                UserInfo userInfo = new UserInfo
                {
                    Args = UserArgs,
                    Balance = -1,
                    BankCardsCount = oldUser.BankCardsCount.GetValueOrDefault(),
                    Cellphone = oldUser.Cellphone.Substring(0, 11),
                    ClientType = oldUser.ClientType,
                    Closed = false,
                    ContractId = oldUser.ContractId,
                    Credential = Utils.GetCredential(oldUser.Credential),
                    CredentialNo = oldUser.CredentialNo == null ? string.Empty : oldUser.CredentialNo,
                    Crediting = -1,
                    Debiting = 0,
                    HasSetPassword = oldUser.HasSetPassword > 0,
                    HasSetPaymentPassword = oldUser.HasSetPaymentPassword > 0,
                    InvestingInterest = -1,
                    InvestingPrincipal = -1,
                    InviteBy = oldUser.InviteBy == null ? string.Empty : oldUser.InviteBy,
                    JBYAccrualAmount = -1,
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
                    TodayJBYWithdrawalAmount = -1,
                    TodayWithdrawalCount = -1,
                    TotalInterest = -1,
                    TotalPrincipal = -1,
                    UserId = Guid.ParseExact(oldUser.UserId, "N"),
                    Verified = oldUser.Verified.GetValueOrDefault(),
                    VerifiedTime = oldUser.VerifiedTime,
                    WithdrawalableAmount = -1
                };
                return userInfo;
            }
        }

        [SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
        [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
        private static async Task<Guid> GetSettleTransactionIdAsync(Guid orderId, Guid productId)
        {
            using (var context = new OldDBContext())
            {
                if (productId == JBYProductId)
                {
                    List<string> datas = await context.JsonJBYAccountTransaction.AsNoTracking().Where(x => x.OrderId == orderId).Select(x => x.Data).ToListAsync();
                    List<JBYAccountTransaction> list =
                        datas.Select(x => JsonConvert.DeserializeObject<JBYAccountTransaction>(x)).ToList();
                    return list.Where(x => x.TradeCode == 2001051102 || x.TradeCode == 2001012002).Select(x => x.TransactionId).FirstOrDefault();
                }
                else
                {
                    List<string> datas = await context.JsonSettleAccountTransaction.AsNoTracking().Where(x => x.OrderId == orderId).Select(x => x.Data).ToListAsync();
                    List<SettleAccountTransaction> list =
                        datas.Select(x => JsonConvert.DeserializeObject<SettleAccountTransaction>(x)).ToList();
                    return list.Where(x => x.TradeCode == 1005012004).Select(x => x.TransactionId).FirstOrDefault();
                }
            }
        }

        private static async Task<bool> ProductExistsAsync(Guid productId)
        {
            using (var context = new OldDBContext())
            {
                return await context.JsonProduct.AsNoTracking().AnyAsync(x => x.ProductId == productId);
            }
        }

        private static async Task<bool> UserExistsAsync(Guid userId)
        {
            using (var context = new OldDBContext())
            {
                return await context.JsonUser.AsNoTracking().AnyAsync(x => x.UserId == userId);
            }
        }

        #region ProductTransfer

        [SuppressMessage("ReSharper", "FunctionComplexityOverflow")]
        private static async Task ProductTransferAsync(int skipCount, int takeCount, int threadId)
        {
            int i = 0;
            using (var context = new OldDBContext())
            {
                var oldProductList = context.TransRegularProductState.AsNoTracking().OrderBy(o => o.ProductId).Skip(skipCount).Take(takeCount);

                if (!oldProductList.Any()) return;

                foreach (var oldProduct in oldProductList)
                {
                    bool result = await ProductExistsAsync(Guid.ParseExact(oldProduct.ProductId, "N"));
                    if (result) continue;
                    if (oldProduct.ProductId == StrDefaultJBYProductId && await context.JsonJBYAccountTransaction.AsNoTracking().Select(a => a.Id).CountAsync() > 0) continue;

                    #region product

                    Agreements agreement1 = await context.Agreements.AsNoTracking().FirstOrDefaultAsync(a => a.Id == oldProduct.Agreement1);
                    Agreements agreement2 = await context.Agreements.AsNoTracking().FirstOrDefaultAsync(a => a.Id == oldProduct.Agreement2);

                    RegularProductMigrationDto product = new RegularProductMigrationDto
                    {
                        Agreement1 = agreement1 != null ? agreement1.Content : string.Empty,
                        Agreement2 = agreement2 != null ? agreement2.Content : string.Empty,
                        Args = ProductArgs,
                        BankName = oldProduct.BankName == null ? string.Empty : oldProduct.BankName, //186 items null, ignore
                        Drawee = oldProduct.Drawee == null ? string.Empty : oldProduct.Drawee,
                        DraweeInfo = oldProduct.DraweeInfo == null ? string.Empty : oldProduct.DraweeInfo,
                        EndorseImageLink = oldProduct.EndorseImageLink,
                        EndSellTime = oldProduct.EndSellTime,
                        EnterpriseInfo = oldProduct.EnterpriseInfo.IsNullOrEmpty() ? string.Empty : oldProduct.EnterpriseInfo,
                        EnterpriseLicense = oldProduct.EnterpriseLicense.IsNullOrEmpty() ? string.Empty : oldProduct.EnterpriseLicense,
                        EnterpriseName = oldProduct.EnterpriseName.IsNullOrEmpty() ? string.Empty : oldProduct.EnterpriseName,
                        FinancingSumAmount = (long)(oldProduct.FinancingSumAmount * oldProduct.UnitPrice * 100),
                        IssueNo = oldProduct.IssueNo,
                        IssueTime = oldProduct.IssueTime,
                        Period = oldProduct.Period,
                        PledgeNo = oldProduct.PledgeNo.IsNullOrEmpty() ? "-1" : oldProduct.PledgeNo,
                        ProductCategory = await Utils.GetProductCategoryAsync(oldProduct.ProductCategory, oldProduct.ProductType),
                        ProductName = Utils.GetProductName(oldProduct.ProductName),
                        ProductNo = oldProduct.ProductNo,
                        ProductId = Guid.ParseExact(oldProduct.ProductId, "N"),
                        Repaid = oldProduct.Repaid,
                        RepaidTime = oldProduct.RepaymentDeadline.Date,
                        RepaymentDeadline = oldProduct.RepaymentDeadline,
                        RiskManagement = Utils.GetRiskManagementMode(oldProduct.RiskManagementMode),
                        RiskManagementInfo = oldProduct.RiskManagementInfo == null ? oldProduct.RiskManagement : oldProduct.RiskManagementInfo,
                        RiskManagementMode = "商票贷",
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

                    string nv = product.ProductNo.Substring(0, 2).ToUpperInvariant();
                    switch (nv)
                    {
                        case "DB":
                            product.RiskManagementMode = "担保贷";
                            break;

                        case "BL":
                            product.RiskManagementMode = "保理贷";
                            break;

                        case "A1":
                            product.RiskManagementMode = "银保贷";
                            break;

                        case "B1":
                            product.RiskManagementMode = "银保贷";
                            break;

                        case "YB":
                            product.RiskManagementMode = "银保贷";
                            break;
                    }

                    #region orders

                    Dictionary<Guid, OrderInfo> orders = new Dictionary<Guid, OrderInfo>();
                    if (product.ProductId == JBYProductId)
                    {
                        List<TransJbyOrderInfo> oldJbyOrderList = await context.TransJbyOrderInfo.AsNoTracking().ToListAsync();
                        foreach (TransJbyOrderInfo oldOrder in oldJbyOrderList)
                        //Parallel.ForEach(oldJbyOrderList, new ParallelOptions { MaxDegreeOfParallelism = 14 }, async oldOrder =>
                        {
                            Console.WriteLine("JBY" + oldOrder.OrderId);
                            UserInfo userInfo = await CreateUserInfoAsync(oldOrder.UserId);
                            if (userInfo != null)
                            {
                                OrderInfo orderInfo = new OrderInfo
                                {
                                    IsRepaid = oldOrder.IsRepaid.GetValueOrDefault(),
                                    Principal = (long)(oldOrder.Principal * 100),
                                    OrderTime = oldOrder.OrderTime,
                                    OrderId = Guid.ParseExact(oldOrder.OrderId, "N"),
                                    UserId = Guid.ParseExact(oldOrder.UserId, "N")
                                };

                                switch (oldOrder.Type)
                                {
                                    case 20:
                                        //                                    await GenerateJbyTransactionAsync(new List<TranscationState>
                                        //                                    {
                                        //                                        TranscationState.ChongZhi,
                                        //                                        TranscationState.QianBaoToJBY,
                                        //                                        TranscationState.JBYRecieveFromQianBao
                                        //                                    }, orderInfo, userInfo);
                                        await GenerateDepositJbyTransactionAsync(orderInfo, userInfo);
                                        break;

                                    case 50:
                                        //                                    await GenerateJbyTransactionAsync(new List<TranscationState>
                                        //                                    {
                                        //                                        TranscationState.QuXian,
                                        //                                        TranscationState.QianBaoRecieveFromJBY,
                                        //                                        TranscationState.JBYToQianBao
                                        //                                    }, orderInfo, userInfo);
                                        await GenerateWithdrawalJbyTransactionAsync(orderInfo, userInfo);
                                        break;
                                }
                            }
                        }

                        product.Orders = new Dictionary<Guid, OrderInfo>();
                    }
                    else
                    {
                        List<TransOrderInfo> oldOrderList = await context.TransOrderInfo.AsNoTracking().Where(o => o.ProductId == oldProduct.ProductId).ToListAsync();
                        foreach (var oldOrder in oldOrderList)
                        {
                            UserInfo userInfo = await CreateUserInfoAsync(oldOrder.UserId);
                            if (userInfo != null)
                            {
                                //购买transaction
                                Guid transactionId = Guid.NewGuid();

                                OrderInfo orderInfo = new OrderInfo
                                {
                                    AccountTransactionId = transactionId,
                                    Args = OrderArgs,
                                    Cellphone = oldOrder.Cellphone.Substring(0, 11),
                                    ExtraInterest = (long)(oldOrder.ExtraInterest * 100),
                                    ExtraInterestRecords = new List<ExtraInterestRecord>(),
                                    ExtraYield = oldOrder.ExtraYield * 100,
                                    Interest = (long)(oldOrder.Interest * 100),
                                    IsRepaid = oldOrder.IsRepaid,
                                    OrderId = Guid.ParseExact(oldOrder.OrderId, "N"),
                                    OrderNo = oldOrder.OrderNo == null ? string.Empty : oldOrder.OrderNo,
                                    OrderTime = oldOrder.OrderTime,
                                    Principal = (long)(oldOrder.Principal * 100),
                                    ProductCategory = product.ProductCategory,
                                    ProductId = product.ProductId,
                                    ProductSnapshot = new RegularProductInfo
                                    {
                                        Args = product.Args,
                                        BankName = product.BankName,
                                        Drawee = product.Drawee,
                                        DraweeInfo = product.DraweeInfo,
                                        EndorseImageLink = product.EndorseImageLink,
                                        EndSellTime = product.EndSellTime,
                                        EnterpriseInfo = product.EnterpriseInfo,
                                        EnterpriseLicense = product.EnterpriseLicense,
                                        EnterpriseName = product.EnterpriseName,
                                        FinancingSumAmount = product.FinancingSumAmount,
                                        IssueNo = product.IssueNo,
                                        IssueTime = product.IssueTime,
                                        Period = product.Period,
                                        PledgeNo = product.PledgeNo,
                                        ProductCategory = product.ProductCategory,
                                        ProductName = product.ProductName,
                                        ProductNo = product.ProductNo,
                                        ProductId = product.ProductId,
                                        Repaid = product.Repaid,
                                        RepaymentDeadline = product.RepaymentDeadline,
                                        RiskManagement = product.RiskManagement,
                                        RiskManagementInfo = product.RiskManagementInfo,
                                        RiskManagementMode = product.RiskManagementMode,
                                        SettleDate = product.SettleDate,
                                        SoldOut = product.SoldOut,
                                        SoldOutTime = product.SoldOutTime,
                                        StartSellTime = product.StartSellTime,
                                        UnitPrice = product.UnitPrice,
                                        Usage = product.Usage,
                                        ValueDateMode = 0,
                                        Yield = product.Yield
                                    },
                                    RepaidTime = oldOrder.IsRepaid ? oldOrder.SettleDate.Date : (DateTime?)null,
                                    ResultCode = 1,
                                    ResultTime = oldOrder.ResultTime,
                                    SettleDate = Utils.GetDate(oldOrder.SettleDate),
                                    TransDesc = "充值成功，购买理财产品",
                                    UserId = Guid.ParseExact(oldOrder.UserId, "N"),
                                    UserInfo = userInfo,
                                    ValueDate = Utils.GetDate(oldOrder.ValueDate),
                                    Yield = (int)(oldOrder.Yield * 100)
                                };

                                List<TranscationState> states = new List<TranscationState>
                                {
                                    TranscationState.ChongZhi,
                                    TranscationState.GouMai
                                };
                                if (orderInfo.IsRepaid)
                                {
                                    states.AddRange(new List<TranscationState>
                                    {
                                        TranscationState.BenJin,
                                        TranscationState.LiXi,
                                        TranscationState.QuXian
                                    });
                                }

                                await GenerateRegularTransactionAsync(states, orderInfo, userInfo);

                                orders.Add(orderInfo.OrderId, orderInfo);
                            }
                        }

                        product.Orders = orders;
                        context.JsonProduct.Add(new JsonProduct { Data = JsonConvert.SerializeObject(product), ProductId = product.ProductId });
                    }

                    #endregion orders

                    Console.WriteLine("product transfer start,threadId: " + threadId + ", count: " + ++i);

                    #endregion product
                }
                await context.SaveChangesAsync();
            }
        }

        #endregion ProductTransfer

        #region UserTransfer

        [SuppressMessage("ReSharper", "LoopCanBePartlyConvertedToQuery")]
        [SuppressMessage("ReSharper", "FunctionComplexityOverflow")]
        private static async Task UserTransferAsync(int skipCount, int takeCount, int threadId)
        {
            int i = 0;
            using (OldDBContext context = new OldDBContext())
            {
                List<TransUserInfo> transUserInfos = await context.TransUserInfo.AsNoTracking()
                    .OrderBy(x => x.UserId).Skip(skipCount).Take(takeCount).ToListAsync();
                foreach (TransUserInfo transUserInfo in transUserInfos)
                {
                    if (transUserInfo == null) continue;
                    bool result = await UserExistsAsync(Guid.ParseExact(transUserInfo.UserId, "N"));
                    if (result) continue;

                    #region userinfo

                    UserInfo userInfo = new UserInfo
                    {
                        Args = UserArgs,
                        Balance = -1,
                        BankCardsCount = transUserInfo.BankCardsCount.GetValueOrDefault(),
                        Cellphone = transUserInfo.Cellphone.Substring(0, 11),
                        ClientType = transUserInfo.ClientType,
                        Closed = false,
                        ContractId = transUserInfo.ContractId,
                        Credential = Utils.GetCredential(transUserInfo.Credential),
                        CredentialNo = transUserInfo.CredentialNo.IsNotNullOrEmpty() ? transUserInfo.CredentialNo : string.Empty,
                        Crediting = -1,
                        Debiting = 0,
                        HasSetPassword = transUserInfo.HasSetPassword.GetValueOrDefault() > 0,
                        HasSetPaymentPassword = transUserInfo.HasSetPaymentPassword.GetValueOrDefault() > 0,
                        InvestingInterest = -1,
                        InvestingPrincipal = -1,
                        InviteBy = transUserInfo.InviteBy.IsNotNullOrEmpty() ? transUserInfo.InviteBy : string.Empty,
                        JBYAccrualAmount = -1,
                        JBYLastInterest = -1,
                        JBYTotalAmount = -1,
                        JBYTotalInterest = -1,
                        JBYTotalPricipal = -1,
                        JBYWithdrawalableAmount = -1,
                        LoginNames = new List<string> { transUserInfo.LoginNames },
                        MonthWithdrawalCount = -1,
                        OutletCode = Utils.GetOutletCode(transUserInfo.OutletCode),
                        PasswordErrorCount = transUserInfo.PasswordErrorCount,
                        PaymentPasswordErrorCount = transUserInfo.PaymentPasswordErrorCount.GetValueOrDefault(),
                        RealName = transUserInfo.RealName.IsNullOrEmpty() ? string.Empty : transUserInfo.RealName,
                        RegisterTime = transUserInfo.RegisterTime,
                        TodayJBYWithdrawalAmount = -1,
                        TodayWithdrawalCount = -1,
                        TotalInterest = -1,
                        TotalPrincipal = -1,
                        UserId = Guid.ParseExact(transUserInfo.UserId, "N"),
                        Verified = transUserInfo.Verified.GetValueOrDefault(),
                        VerifiedTime = transUserInfo.VerifiedTime,
                        WithdrawalableAmount = -1
                    };

                    #endregion userinfo

                    #region Order

                    List<Order> listOrder = new List<Order>();
                    List<TransOrderInfo> orderInfos = await context.TransOrderInfo.AsNoTracking().Where(o => userInfo.Verified && o.UserId == transUserInfo.UserId && o.ProductId != StrDefaultJBYProductId).ToListAsync();
                    foreach (var x in orderInfos)
                    {
                        Guid accountTransactionId = await GetSettleTransactionIdAsync(Guid.ParseExact(x.OrderId, "N"), Guid.ParseExact(x.ProductId, "N"));
                        TransRegularProductState product =
                            await context.TransRegularProductState.AsNoTracking()
                                .FirstOrDefaultAsync(p => p.ProductId == x.ProductId);

                        string riskManagementMode = "商票贷";
                        if (product != null)
                        {
                            string nv = product.ProductNo.Substring(0, 2).ToUpperInvariant();
                            switch (nv)
                            {
                                case "DB":
                                    riskManagementMode = "担保贷";
                                    break;

                                case "BL":
                                    riskManagementMode = "保理贷";
                                    break;

                                case "A1":
                                    riskManagementMode = "银保贷";
                                    break;

                                case "B1":
                                    riskManagementMode = "银保贷";
                                    break;

                                case "YB":
                                    riskManagementMode = "银保贷";
                                    break;
                            }
                        }

                        listOrder.Add(new Order
                        {
                            AccountTransactionId = accountTransactionId,
                            Args = OrderArgs,
                            Cellphone = x.Cellphone.Substring(0, 11),
                            ExtraInterest = (long)(x.ExtraInterest * 100),
                            ExtraInterestRecords = new List<ExtraInterestRecord>(),
                            ExtraYield = (x.ExtraYield * 100),
                            Interest = (long)(x.Interest * 100),
                            IsRepaid = x.IsRepaid,
                            OrderId = Guid.ParseExact(x.OrderId, "N"),
                            OrderNo = x.OrderNo == null ? string.Empty : x.OrderNo,
                            OrderTime = x.OrderTime,
                            Principal = (long)(x.Principal * 100),
                            ProductCategory = await Utils.GetProductCategoryAsync(x.ProductCategory, x.ProductType),
                            ProductId = Guid.ParseExact(x.ProductId, "N"),
                            ProductSnapshot = product == null
                                ? new RegularProductInfo()
                                : new RegularProductInfo
                                {
                                    Args = ProductArgs,
                                    BankName = product.BankName,
                                    Drawee = product.Drawee,
                                    DraweeInfo = product.DraweeInfo,
                                    EndorseImageLink = product.EndorseImageLink,
                                    EndSellTime = product.EndSellTime,
                                    EnterpriseInfo =
                                        product.EnterpriseInfo.IsNullOrEmpty() ? string.Empty : product.EnterpriseInfo,
                                    EnterpriseLicense =
                                        product.EnterpriseLicense.IsNullOrEmpty()
                                            ? string.Empty
                                            : product.EnterpriseLicense,
                                    EnterpriseName =
                                        product.EnterpriseName.IsNullOrEmpty() ? string.Empty : product.EnterpriseName,
                                    FinancingSumAmount = product.FinancingSumAmount,
                                    IssueNo = product.IssueNo,
                                    IssueTime = product.IssueTime,
                                    Period = product.Period,
                                    PledgeNo = product.PledgeNo.IsNullOrEmpty() ? "-1" : product.PledgeNo,
                                    ProductCategory =
                                        await
                                            Utils.GetProductCategoryAsync(product.ProductCategory, product.ProductType),
                                    ProductName = Utils.GetProductName(product.ProductName),
                                    ProductNo = product.ProductNo == null ? string.Empty : product.ProductNo,
                                    ProductId = Guid.ParseExact(product.ProductId, "N"),
                                    Repaid = product.Repaid,
                                    RepaymentDeadline = product.RepaymentDeadline,
                                    RiskManagement = Utils.GetRiskManagementMode(product.RiskManagementMode),
                                    RiskManagementInfo = product.RiskManagementInfo,
                                    RiskManagementMode = riskManagementMode,
                                    SettleDate = Utils.GetDate(product.SettleDate),
                                    SoldOut = product.SoldOut,
                                    SoldOutTime = product.SoldOutTime,
                                    StartSellTime = product.StartSellTime,
                                    UnitPrice = (long)(product.UnitPrice * 100),
                                    Usage = product.Usage == null ? string.Empty : product.Usage,
                                    ValueDateMode = 0,
                                    Yield = (int)(product.Yield * 100)
                                },
                            // ReSharper disable once PossibleNullReferenceException
                            RepaidTime = product.RepaymentDeadline.Date,
                            ResultCode = 1,
                            SettleDate = Utils.GetDate(x.SettleDate),
                            TransDesc = "充值成功，购买理财产品",
                            UserId = userInfo.UserId,
                            UserInfo = userInfo,
                            ValueDate = Utils.GetDate(x.ValueDate),
                            Yield = (int)(x.Yield * 100)
                        });
                    }

                    Dictionary<Guid, Order> orders = listOrder.ToDictionary(x => x.OrderId);

                    #endregion Order

                    UserMigrationDto user = new UserMigrationDto
                    {
                        Args = UserArgs,
                        BankCards = await Utils.GetBankCards(transUserInfo.UserId),
                        Cellphone = userInfo.Cellphone.Substring(0, 11),
                        ClientType = userInfo.ClientType,
                        Closed = false,
                        ContractId = userInfo.ContractId,
                        Credential = userInfo.Credential,
                        CredentialNo = userInfo.CredentialNo,
                        EncryptedPassword = transUserInfo.EncryptedPassword,
                        EncryptedPaymentPassword = transUserInfo.EncryptedPaymentPassword.IsNullOrEmpty() ? string.Empty : transUserInfo.EncryptedPaymentPassword,
                        InviteBy = userInfo.InviteBy,
                        JBYAccount = await GetJBYAccountTransactionAsync(userInfo.UserId),
                        LoginNames = userInfo.LoginNames,
                        Orders = orders,
                        OutletCode = userInfo.OutletCode,
                        PaymentSalt = transUserInfo.PaymentSalt.IsNullOrEmpty() ? string.Empty : transUserInfo.PaymentSalt,
                        RealName = userInfo.RealName,
                        RegisterTime = userInfo.RegisterTime,
                        Salt = transUserInfo.Salt,
                        SettleAccount = await GetSettleAccountTransactionAsync(userInfo.UserId),
                        UserId = userInfo.UserId,
                        Verified = userInfo.Verified,
                        VerifiedTime = userInfo.VerifiedTime
                    };

                    string json = JsonConvert.SerializeObject(user);
                    context.JsonUser.Add(new JsonUser { Data = json, UserId = userInfo.UserId });
                    Console.WriteLine("user transfer start,threadId: " + threadId + ", count: " + ++i);
                    //Console.WriteLine(json);
                }
                await context.SaveChangesAsync();
            }
        }

        #endregion UserTransfer

        #region 创建多个Task

        private static async Task<int> GetProductCountAsync()
        {
            using (var context = new OldDBContext())
            {
                return await context.Set<TransRegularProductState>().AsNoTracking().CountAsync();
            }
        }

        private static async Task<int> GetUserCountAsync()
        {
            using (var context = new OldDBContext())
            {
                return await context.Set<TransUserInfo>().AsNoTracking().CountAsync();
            }
        }

        private static async Task ProductTask()
        {
            double count = await GetProductCountAsync();
            List<Task> list = new List<Task>();
            try
            {
                for (int j = 0; j < Math.Ceiling(count / ProductExecuteDataCount); j++)
                {
                    list.Add(ProductTransferAsync(j * ProductExecuteDataCount, ProductExecuteDataCount, j));

                    if (list.Count == 14)
                    {
                        await Task.WhenAll(list);
                        list.Clear();
                    }
                }
                await Task.WhenAll(list.ToArray());
            }
            catch (Exception exception)
            {
                MemoryWork.WriteException(exception);
                throw;
            }
        }

        private static async Task UserTask()
        {
            double count = await GetUserCountAsync();
            List<Task> list = new List<Task>();
            try
            {
                for (int j = 0; j < Math.Ceiling(count / UserExecuteDataCount); j++)
                {
                    list.Add(UserTransferAsync(j * UserExecuteDataCount, UserExecuteDataCount, j));

                    if (list.Count == 14)
                    {
                        await Task.WhenAll(list);
                        list.Clear();
                    }
                }

                await Task.WhenAll(list.ToArray());
            }
            catch (Exception exception)
            {
                MemoryWork.WriteException(exception);
                throw;
            }
        }

        #endregion 创建多个Task

        #region 生成流水

        private static async Task GenerateDepositJbyTransactionAsync(OrderInfo order, UserInfo user)
        {
            Dictionary<string, object> args = new Dictionary<string, object>
            {
                { "Comment", "由原金包银流水数据迁移" }
            };

            using (var db = new OldDBContext())
            {
                TransSettleAccountTransaction oldTransaction =
                    await
                        db.TransSettleAccountTransaction.AsNoTracking()
                            .FirstOrDefaultAsync(t => t.OrderId == order.OrderId.ToString().Replace("-", ""));

                SettleAccountTransaction t1 = new SettleAccountTransaction
                {
                    Amount = order.Principal,
                    Args = args,
                    BankCardNo = (oldTransaction == null || oldTransaction.BankCardNo.IsNullOrEmpty()) ? string.Empty : oldTransaction.BankCardNo,
                    ChannelCode = 10000,
                    OrderId = order.OrderId,
                    ResultCode = 1,
                    ResultTime = (oldTransaction?.CallbackTime != null) ? oldTransaction.CallbackTime : order.OrderTime,
                    SequenceNo = order.OrderNo,
                    Trade = Trade.Debit,
                    TradeCode = 1005051001,
                    TransactionId = Guid.NewGuid(),
                    TransactionTime = order.OrderTime,
                    TransDesc = "个人钱包账户充值",
                    UserId = order.UserId,
                    UserInfo = user
                };

                SettleAccountTransaction t2 = new SettleAccountTransaction
                {
                    Amount = order.Principal,
                    Args = args,
                    BankCardNo = (oldTransaction == null || oldTransaction.BankCardNo.IsNullOrEmpty()) ? string.Empty : oldTransaction.BankCardNo,
                    ChannelCode = 10000,
                    OrderId = order.OrderId,
                    ResultCode = 1,
                    ResultTime = (oldTransaction?.CallbackTime != null) ? oldTransaction.CallbackTime : order.OrderTime,
                    SequenceNo = order.OrderNo,
                    Trade = Trade.Credit,
                    TradeCode = 1005012003,
                    TransactionId = Guid.NewGuid(),
                    TransactionTime = order.OrderTime,
                    TransDesc = "钱包金额转为金包银金额",
                    UserId = order.UserId,
                    UserInfo = user
                };

                JBYAccountTransaction transaction = new JBYAccountTransaction
                {
                    Amount = order.Principal,
                    Args = args,
                    PredeterminedResultDate = (oldTransaction?.CallbackTime != null) ? oldTransaction.CallbackTime : order.OrderTime,
                    ProductId = JBYProductId,
                    ResultCode = 1,
                    ResultTime = (oldTransaction?.CallbackTime != null) ? oldTransaction.CallbackTime : order.OrderTime,
                    SettleAccountTransactionId = t2.TransactionId,
                    Trade = Trade.Debit,
                    TradeCode = 2001051102,
                    TransDesc = "申购成功",
                    TransactionId = order.OrderId,
                    TransactionTime = order.OrderTime,
                    UserId = order.UserId,
                    UserInfo = user
                };

                db.JsonSettleAccountTransaction.Add(
                    new JsonSettleAccountTransaction
                    {
                        OrderId = order.OrderId,
                        UserId = user.UserId,
                        Data = JsonConvert.SerializeObject(t1)
                    });

                db.JsonSettleAccountTransaction.Add(
                    new JsonSettleAccountTransaction
                    {
                        OrderId = order.OrderId,
                        UserId = user.UserId,
                        Data = JsonConvert.SerializeObject(t2)
                    });

                db.JsonJBYAccountTransaction.Add(
                    new JsonJBYAccountTransaction
                    {
                        OrderId = order.OrderId,
                        UserId = user.UserId,
                        Data = JsonConvert.SerializeObject(transaction)
                    });

                await db.SaveChangesAsync();
            }
        }

        private static async Task GenerateJbyTransactionAsync(IEnumerable<TranscationState> listType, OrderInfo order, UserInfo user)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>
            {
                { "Comment", "由原金包银流水数据迁移" }
            };

            using (var context = new OldDBContext())
            {
                Guid chongZhiId = Guid.NewGuid();
                Guid renGouId = Guid.NewGuid();
                Guid quXianId = Guid.NewGuid();
                Guid shuHuiId = Guid.NewGuid();
                foreach (TranscationState type in listType)
                {
                    TransSettleAccountTransaction oldTransaction =
                        await
                            context.TransSettleAccountTransaction.AsNoTracking()
                                .FirstOrDefaultAsync(t => t.OrderId == order.OrderId.ToString().Replace("-", ""));

                    //pre deal
                    JBYAccountTransaction transaction = new JBYAccountTransaction
                    {
                        Amount = order.Principal,
                        ProductId = JBYProductId,
                        Args = dic,
                        //BankCardNo = oldTransaction.BankCardNo,
                        //ChannelCode
                        //OrderId = order.OrderId,
                        PredeterminedResultDate = null,
                        ResultCode = 1,
                        ResultTime = (oldTransaction?.CallbackTime != null) ? oldTransaction.CallbackTime : order.OrderTime,
                        //SequenceNo = order.OrderNo,
                        //SettleAccountTransactionId = null,
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
                            chongZhiId = transaction.TransactionId;
                            break;

                        case TranscationState.QianBaoToJBY:
                            transaction.Trade = Trade.Credit;
                            transaction.TradeCode = 1005012003;
                            transaction.TransactionId = Guid.NewGuid();
                            transaction.TransDesc = "钱包金额转为金包银金额";
                            renGouId = transaction.TransactionId;
                            transaction.SettleAccountTransactionId = chongZhiId;
                            break;

                        case TranscationState.JBYRecieveFromQianBao:
                            transaction.Trade = Trade.Debit;
                            transaction.TradeCode = 2001051102;
                            transaction.TransactionId = order.OrderId;
                            transaction.TransDesc = "金包银金额收到钱包转入金额";
                            transaction.SettleAccountTransactionId = renGouId;
                            break;

                        case TranscationState.QuXian:
                            transaction.Trade = Trade.Credit;
                            transaction.TradeCode = 1005052001;
                            transaction.TransactionId = Guid.NewGuid();
                            transaction.TransDesc = "个人钱包账户取现";
                            quXianId = transaction.TransactionId;
                            break;

                        case TranscationState.QianBaoRecieveFromJBY:
                            transaction.Trade = Trade.Debit;
                            transaction.TradeCode = 1005011103;
                            transaction.TransactionId = Guid.NewGuid();
                            transaction.TransDesc = "钱包收到金包银转入金额";
                            shuHuiId = transaction.TransactionId;
                            transaction.SettleAccountTransactionId = quXianId;
                            break;

                        case TranscationState.JBYToQianBao:
                            transaction.Trade = Trade.Credit;
                            transaction.TradeCode = 2001012002;
                            transaction.TransactionId = order.OrderId;
                            transaction.TransDesc = "金包银金额转为钱包金额";
                            transaction.SettleAccountTransactionId = shuHuiId;
                            break;
                    }

                    context.JsonJBYAccountTransaction.Add(
                        new JsonJBYAccountTransaction
                        {
                            OrderId = order.OrderId,
                            UserId = user.UserId,
                            Data = JsonConvert.SerializeObject(transaction)
                        });
                }
                await context.SaveChangesAsync();
            }
        }

        private static async Task GenerateRegularTransactionAsync(IEnumerable<TranscationState> listType, OrderInfo order, UserInfo user)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>
            {
                { "Comment", "由原流水数据迁移" }
            };

            if (order.IsRepaid)
            {
                dic.Add("IsRepaid", order.IsRepaid);
            }

            using (OldDBContext context = new OldDBContext())
            {
                TransSettleAccountTransaction oldTransaction =
                    await context.TransSettleAccountTransaction.AsNoTracking()
                        .FirstOrDefaultAsync(t => t.OrderId == order.OrderId.ToString().Replace("-", ""));

                foreach (TranscationState type in listType)
                {
                    //pre deal
                    SettleAccountTransaction transaction = new SettleAccountTransaction
                    {
                        Amount = order.Principal,
                        Args = dic,
                        BankCardNo = oldTransaction.BankCardNo.IsNotNullOrEmpty() ? oldTransaction.BankCardNo : string.Empty,
                        //ChannelCode
                        OrderId = order.OrderId,
                        ResultCode = 1,
                        ResultTime = (oldTransaction?.CallbackTime != null) ? oldTransaction.CallbackTime : order.OrderTime,
                        SequenceNo = order.OrderNo,
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
                            transaction.ChannelCode = 10010;
                            transaction.Trade = Trade.Debit;
                            transaction.TradeCode = 1005051001;
                            transaction.TransactionId = Guid.NewGuid();
                            transaction.TransDesc = "充值成功";

                            break;

                        case TranscationState.GouMai:
                            transaction.ChannelCode = 10000;
                            transaction.Trade = Trade.Credit;
                            transaction.TradeCode = order.ProductCategory >= 100000000 && order.ProductCategory < 200000000 ? 1005012004 : 1005022004;
                            transaction.TransactionId = order.AccountTransactionId;
                            transaction.TransDesc = "支付成功";
                            transaction.BankCardNo = string.Empty;

                            break;

                        case TranscationState.BenJin:
                            transaction.ChannelCode = 10000;
                            transaction.Trade = Trade.Debit;
                            transaction.TradeCode = order.ProductCategory >= 100000000 && order.ProductCategory < 100000030 ? 1005011104 : 1005021104;
                            transaction.TransactionId = Guid.NewGuid();
                            transaction.TransDesc = "本金还款";

                            break;

                        case TranscationState.LiXi:
                            transaction.Amount = order.Interest + order.ExtraInterest;
                            transaction.ChannelCode = 10000;
                            transaction.Trade = Trade.Debit;
                            transaction.TradeCode = order.ProductCategory >= 100000000 && order.ProductCategory < 100000030 ? 1005011105 : 1005021105;
                            transaction.TransactionId = Guid.NewGuid();
                            transaction.TransDesc = "产品结息";

                            break;

                        case TranscationState.QuXian:
                            transaction.Amount = order.Principal + order.Interest + order.ExtraInterest;
                            transaction.ChannelCode = 10010;
                            transaction.Trade = Trade.Credit;
                            transaction.TradeCode = 1005052001;
                            transaction.TransactionId = Guid.NewGuid();
                            transaction.TransDesc = "取现成功";

                            break;

                        default:
                            transaction.ChannelCode = -1;

                            break;
                    }

                    context.JsonSettleAccountTransaction.Add(
                        new JsonSettleAccountTransaction
                        {
                            OrderId = order.OrderId,
                            UserId = user.UserId,
                            Data = JsonConvert.SerializeObject(transaction)
                        });
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task GenerateWithdrawalJbyTransactionAsync(OrderInfo order, UserInfo user)
        {
            Dictionary<string, object> args = new Dictionary<string, object>
            {
                { "Comment", "由原金包银流水数据迁移" }
            };

            using (var db = new OldDBContext())
            {
                TransSettleAccountTransaction oldTransaction =
                    await
                        db.TransSettleAccountTransaction.AsNoTracking()
                            .FirstOrDefaultAsync(t => t.OrderId == order.OrderId.ToString().Replace("-", ""));

                SettleAccountTransaction t1 = new SettleAccountTransaction
                {
                    Amount = order.Principal,
                    Args = args,
                    BankCardNo = (oldTransaction == null || oldTransaction.BankCardNo.IsNullOrEmpty()) ? string.Empty : oldTransaction.BankCardNo,
                    ChannelCode = 10000,
                    OrderId = order.OrderId,
                    ResultCode = 1,
                    ResultTime = (oldTransaction?.CallbackTime != null) ? oldTransaction.CallbackTime : order.OrderTime,
                    SequenceNo = order.OrderNo,
                    Trade = Trade.Credit,
                    TradeCode = 1005052001,
                    TransactionId = Guid.NewGuid(),
                    TransactionTime = order.OrderTime,
                    TransDesc = "个人钱包账户取现",
                    UserId = order.UserId,
                    UserInfo = user
                };

                SettleAccountTransaction t2 = new SettleAccountTransaction
                {
                    Amount = order.Principal,
                    Args = args,
                    BankCardNo = (oldTransaction == null || oldTransaction.BankCardNo.IsNullOrEmpty()) ? string.Empty : oldTransaction.BankCardNo,
                    ChannelCode = 10000,
                    OrderId = order.OrderId,
                    ResultCode = 1,
                    ResultTime = (oldTransaction?.CallbackTime != null) ? oldTransaction.CallbackTime : order.OrderTime,
                    SequenceNo = order.OrderNo,
                    Trade = Trade.Debit,
                    TradeCode = 1005011103,
                    TransactionId = Guid.NewGuid(),
                    TransactionTime = order.OrderTime,
                    TransDesc = "金包银赎回资金到账",
                    UserId = order.UserId,
                    UserInfo = user
                };

                JBYAccountTransaction transaction = new JBYAccountTransaction
                {
                    Amount = order.Principal,
                    Args = args,
                    PredeterminedResultDate = (oldTransaction?.CallbackTime != null) ? oldTransaction.CallbackTime : order.OrderTime,
                    ProductId = Guid.Parse("92CFADC4-91A5-4A09-8D0E-AC122C837F5B"),
                    ResultCode = 1,
                    ResultTime = (oldTransaction?.CallbackTime != null) ? oldTransaction.CallbackTime : order.OrderTime,
                    SettleAccountTransactionId = t2.TransactionId,
                    Trade = Trade.Credit,
                    TradeCode = 2001012002,
                    TransDesc = "赎回成功",
                    TransactionId = order.OrderId,
                    TransactionTime = order.OrderTime,
                    UserId = order.UserId,
                    UserInfo = user
                };

                db.JsonSettleAccountTransaction.Add(
                    new JsonSettleAccountTransaction
                    {
                        OrderId = order.OrderId,
                        UserId = user.UserId,
                        Data = JsonConvert.SerializeObject(t1)
                    });

                db.JsonSettleAccountTransaction.Add(
                    new JsonSettleAccountTransaction
                    {
                        OrderId = order.OrderId,
                        UserId = user.UserId,
                        Data = JsonConvert.SerializeObject(t2)
                    });

                db.JsonJBYAccountTransaction.Add(
                    new JsonJBYAccountTransaction
                    {
                        OrderId = order.OrderId,
                        UserId = user.UserId,
                        Data = JsonConvert.SerializeObject(transaction)
                    });

                await db.SaveChangesAsync();
            }
        }

        #endregion 生成流水

        #region 通过UserId查询流水

        /// <summary>
        ///     通过UserId查询金包银流水
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static async Task<Dictionary<Guid, JBYAccountTransaction>> GetJBYAccountTransactionAsync(Guid userId)
        {
            using (var context = new OldDBContext())
            {
                List<JsonJBYAccountTransaction> list = await context.JsonJBYAccountTransaction.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();
                return list.Select(item => JsonConvert.DeserializeObject<JBYAccountTransaction>(item.Data)).ToDictionary(x => x.TransactionId);
            }
        }

        /// <summary>
        ///     通过UserId查询流水
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static async Task<Dictionary<Guid, SettleAccountTransaction>> GetSettleAccountTransactionAsync(Guid userId)
        {
            using (var context = new OldDBContext())
            {
                List<JsonSettleAccountTransaction> list = await context.JsonSettleAccountTransaction.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();
                return list.Select(item => JsonConvert.DeserializeObject<SettleAccountTransaction>(item.Data)).ToDictionary(x => x.TransactionId);
            }
        }

        #endregion 通过UserId查询流水
    }
}