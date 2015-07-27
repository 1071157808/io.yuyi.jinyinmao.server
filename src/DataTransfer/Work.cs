using DataTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using System.Diagnostics;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain;

namespace DataTransfer
{
    public class Work
    {
        private static Dictionary<string, object>  orderArgs = new Dictionary<string, object>();
        private static Dictionary<string, object> userArgs = new Dictionary<string, object>();
        private static Dictionary<string, object> productArgs = new Dictionary<string, object>();

        private static List<SettleAccountTransaction> settleAccountTransactionList = new List<SettleAccountTransaction>();



        public static void Run()
        {
            orderArgs.Add("Comment", "由原订单数据迁移");
            userArgs.Add("Comment", "由原用户数据迁移");
            productArgs.Add("Comment", "由原产品数据迁移");


            //get products
            RegularProductTransfer(productArgs);


            Console.ReadKey();
        }

        private static void RegularProductTransfer(Dictionary<string, object> productArgs)
        {
            using (var context = new OldDBContext())
            {
                var oldProductList = context.TransRegularProductState.Where(p => p.ProductId != "cc93b32c0536487fac57014b5b3de4b1").Take(10).ToList();

                foreach (var oldProduct in oldProductList)
                {

                    //-1 condition, null
                    var agreement1 = context.Agreements.Where(a => a.Id == oldProduct.Agreement1).FirstOrDefault();
                    var agreement2 = context.Agreements.Where(a => a.Id == oldProduct.Agreement2).FirstOrDefault();

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
                        ProductCategory = Utils.getProductCategory(oldProduct.ProductCategory, oldProduct.ProductType),
                        ProductName = Utils.getProductName(oldProduct.ProductName),
                        ProductNo = oldProduct.ProductNo,
                        Repaid = oldProduct.Repaid,
                        RepaidTime = null,
                        RepaymentDeadline = oldProduct.RepaymentDeadline,
                        RiskManagement = oldProduct.RiskManagement,
                        RiskManagementInfo = oldProduct.RiskManagementInfo,
                        RiskManagementMode = Utils.getRiskManagementMode(oldProduct.RiskManagementMode),
                        SettleDate = Utils.getDate(oldProduct.SettleDate),
                        SoldOut = oldProduct.SoldOut,
                        SoldOutTime = oldProduct.SoldOutTime,
                        StartSellTime = oldProduct.StartSellTime,
                        UnitPrice = (int)(oldProduct.UnitPrice * 100),
                        Usage = oldProduct.Usage,
                        ValueDate = null,
                        ValueDateMode = 0,
                        Yield = (int)(oldProduct.Yield * 100)
                    };

                    var oldOrderList = context.TransOrderInfo.Where(o => o.ProductId == oldProduct.ProductId).ToList();

                    Dictionary<Guid, OrderInfo> Orders = new Dictionary<Guid, OrderInfo>();

                    foreach (var oldOrder in oldOrderList)
                    {
                        var oldUser = context.TransUserInfo.Where(u => u.UserId == oldOrder.UserId).FirstOrDefault();
                        UserInfo userInfo = new UserInfo
                        {
                            Args = userArgs,
                            Balance = -1,
                            BankCardsCount = (int)oldUser.BankCardsCount,
                            Cellphone = oldUser.Cellphone,
                            ClientType = oldUser.ClientType,
                            Closed = false,
                            ContractId = oldUser.ContractId,
                            Credential = Utils.getCredential(oldUser.Credential),
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
                            LoginNames = new List<string>() { oldUser.LoginNames },
                            MonthWithdrawalCount = oldUser.MonthWithdrawalCount,
                            OutletCode = Utils.getOutletCode(oldUser.OutletCode),
                            PasswordErrorCount = oldUser.PasswordErrorCount,
                            PaymentPasswordErrorCount = (int)oldUser.PaymentPasswordErrorCount,
                            RealName = oldUser.RealName,
                            RegisterTime = oldUser.RegisterTime,
                            TodayJBYWithdrawalAmount = oldUser.TodayJBYWithdrawalAmount,
                            TodayWithdrawalCount = oldUser.TodayWithdrawalCount,
                            TotalInterest = oldUser.TotalInterest,
                            TotalPrincipal = oldUser.TotalPrincipal,
                            UserId = new Guid(oldUser.UserId),
                            Verified = (bool)oldUser.Verified,
                            VerifiedTime = oldUser.VerifiedTime,
                            WithdrawalableAmount = oldUser.WithdrawalableAmount
                        };

                        //购买transaction
                        Guid transactionId = Guid.NewGuid();

                        OrderInfo orderInfo = new OrderInfo
                        {
                            AccountTransactionId = transactionId,
                            Args = orderArgs,
                            Cellphone = oldOrder.Cellphone,
                            ExtraInterest = (long)(oldOrder.ExtraInterest * 100),
                            ExtraInterestRecords = new List<Yuyi.Jinyinmao.Domain.ExtraInterestRecord>(),
                            ExtraYield = oldOrder.ExtraYield * 100,
                            Interest = (long)(oldOrder.Interest * 100),
                            IsRepaid = oldOrder.IsRepaid,
                            OrderId = new Guid(oldOrder.OrderId),
                            OrderNo = oldOrder.OrderNo,
                            OrderTime = oldOrder.OrderTime,
                            Principal = (long)(oldOrder.Principal * 100),
                            ProductCategory = Utils.getProductCategory(oldOrder.ProductCategory, oldOrder.ProductType),
                            ProductId = new Guid(oldOrder.ProductId),
                            ProductSnapshot = null,
                            RepaidTime = null,
                            ResultCode = 10000,
                            ResultTime = oldOrder.ResultTime,
                            SettleDate = Utils.getDate(oldOrder.SettleDate),
                            TransDesc = "充值成功，购买理财产品",
                            UserId = new Guid(oldOrder.UserId),
                            UserInfo = userInfo,
                            ValueDate = Utils.getDate(oldOrder.ValueDate),
                            Yield = (int)(oldOrder.Yield * 100)
                        };

                        SettleAccountTransaction chongZhiTransaction = GenerateTransaction(TranscationState.ChongZhi, orderInfo, userInfo);
                        SettleAccountTransaction gouMaiTransaction = GenerateTransaction(TranscationState.GouMai, orderInfo, userInfo);
                        SettleAccountTransaction benJinTransaction = GenerateTransaction(TranscationState.BenJin, orderInfo, userInfo);
                        SettleAccountTransaction liXiTransaction = GenerateTransaction(TranscationState.LiXi, orderInfo, userInfo);
                        SettleAccountTransaction quXianTransaction = GenerateTransaction(TranscationState.QuXian, orderInfo, userInfo);
                        settleAccountTransactionList.Add(chongZhiTransaction);
                        settleAccountTransactionList.Add(gouMaiTransaction);
                        settleAccountTransactionList.Add(benJinTransaction);
                        settleAccountTransactionList.Add(liXiTransaction);
                        settleAccountTransactionList.Add(quXianTransaction);





                    }


                }
            }
        }

        private static SettleAccountTransaction GenerateTransaction(TranscationState type, OrderInfo order, UserInfo user)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Comment", "由原流水数据迁移");
            dic.Add("IsRepaid", order.IsRepaid);

            using (var context = new OldDBContext())
            {
                var oldTransaction = context.TransSettleAccountTransaction.Where(t => t.OrderId == order.OrderId.ToString().Replace("-", "") && order.ProductId.ToString() != "cc93b32c0536487fac57014b5b3de4b1").FirstOrDefault();
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
                    ResultTime = oldTransaction.CallbackTime == null ? order.OrderTime : oldTransaction.CallbackTime,
                    SequenceNo = order.OrderNo,
                    //Trade
                    //TradeCode
                    //TransactionId
                    TransactionTime = order.OrderTime,
                    //TransDesc 
                    UserId = order.UserId,
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



    }
}
