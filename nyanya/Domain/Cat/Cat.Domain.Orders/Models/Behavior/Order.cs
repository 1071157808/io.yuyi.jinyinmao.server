// ***********************************************************************
// Assembly         : nyanya
// Author           : Siqi Lu
// Created          : 2015-03-04  6:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-22  10:47 PM
// ***********************************************************************
// <copyright file="Order.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Cat.Commands.Orders;
using Cat.Commands.Products;
using Cat.Commands.Users;
using Cat.Domain.Orders.Database;
using Cat.Domain.Orders.Services;
using Cat.Domain.Orders.Services.Interfaces;
using Cat.Domain.Products.Models;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services;
using Cat.Domain.Products.Services.Interfaces;
using Cat.Domain.Users.Helper;
using Cat.Domain.Users.Services;
using Cat.Domain.Users.Services.Interfaces;
using Cat.Events.Orders;
using Cat.Events.Yilian;
using Domian.Bus;
using Domian.Config;
using Infrastructure.Cache.Couchbase;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using Newtonsoft.Json;

namespace Cat.Domain.Orders.Models
{
    public partial class Order
    {
        private static readonly IProductService productService = new ProductService();
        private readonly string StCht = "零壹贰叁肆伍陆柒捌玖";
        private readonly string StIntUnit = "万千佰拾亿千佰拾万千佰拾元角分厘毫";

        protected ICommandBus CommandBus
        {
            get { return CqrsDomainConfig.Configuration.CommandBus; }
        }

        protected IEventBus EventBus
        {
            get { return CqrsDomainConfig.Configuration.EventBus; }
        }

        private IExactOrderInfoService OrderInfoService
        {
            get { return new OrderInfoService(); }
        }

        private IExactProductInfoService ProductInfoService
        {
            get { return new ProductInfoService(); }
        }

        private IUserInfoService UserInfoService
        {
            get { return new UserInfoService(); }
        }

        public async Task<decimal> GetExtraInterest(BuildInvestingOrder command)
        {
            if (command.ActivityNo > 0 && command.ActivityNo < 7)
            {
                var userInfo = await this.UserInfoService.GetUserInfoAsync(command.UserIdentifier);
                if (userInfo == null)
                {
                    return 0;
                }
                if (this.ProductInfo.ProductCategory != ProductCategory.JINYINMAO)
                {
                    return 0;
                }

                //前置条件：2015年1月9日-2015年1月16日期间
                var beginTime = new DateTime(2015, 1, 9);
                var endTime = new DateTime(2015, 1, 17);

                //是否过期
                if (DateTime.Now < beginTime || DateTime.Now > endTime)
                {
                    return 0;
                }

                string activityNo1 = "AC150109";
                if (!this.CheckCanUseActivityItem(command.ActivityNo))
                {
                    return 0;
                }

                var queryLuckRecordResult = this.CommandBus.ObjectExcute(new UseAward(userInfo.UserIdentifier, activityNo1, (7 - command.ActivityNo) * 10 + 10));
                var queryLuckRecordData = JsonConvert.DeserializeObject(queryLuckRecordResult.Data.ToString(), typeof(int?)) as int?;
                if (queryLuckRecordData == null || queryLuckRecordData == 0)
                {
                    return 0;
                }

                return this.ComputeOriginalInterest(this.Principal, this.ValueDate, this.SettleDate, ((decimal)command.ActivityNo) / 10);
            }
            if (command.ActivityNo == 5000)
            {
                var userInfo = await this.UserInfoService.GetUserInfoAsync(command.UserIdentifier);
                if (userInfo == null)
                {
                    return 0;
                }
                if (this.ProductInfo.ProductCategory != ProductCategory.JINYINMAO)
                {
                    return 0;
                }

                //前置条件：2015年1月17日-2015年1月23日期间
                var beginTime = new DateTime(2015, 1, 17);
                var endTime = new DateTime(2015, 1, 24);

                //是否过期
                if (DateTime.Now < beginTime || DateTime.Now > endTime)
                {
                    return 0;
                }

                var userActivityOrders = await this.OrderInfoService.GetTheUserBetweenDateActivityOrders(userInfo.UserIdentifier, beginTime, endTime);
                if (userActivityOrders != null && userActivityOrders.Count > 0) return 0;

                var sum = command.UnitPrice * command.ShareCount;
                return sum < 50 ? 0 : this.ComputeOriginalInterest(500, this.ValueDate, this.SettleDate, this.Yield);
            }

            return 0;
        }

        public string GetTransResult(double num, int digCount)
        {
            if (num <= 0)
            {
                return "零元";
            }
            int numLen;
            String stActuUnit;

            string stIntResult = "";

            num = Math.Round(num, digCount);
            num = num * Math.Pow(10, digCount);
            string stNum = num.ToString();
            numLen = num.ToString().Length;
            stActuUnit = this.StIntUnit.Substring(12 + digCount - numLen + 1, numLen);
            for (int i = 0; i < numLen; i++) //connect the stnumtemp and stint
            {
                stIntResult = stIntResult + this.StCht[stNum[i] - 48] + stActuUnit[i];
            }
            stIntResult = stIntResult.Replace("零拾", "零");

            stIntResult = stIntResult.Replace("零佰", "零");
            stIntResult = stIntResult.Replace("零千", "零");

            stIntResult = stIntResult.Replace("零角", "零");
            stIntResult = stIntResult.Replace("零分", "零");
            stIntResult = stIntResult.Replace("零厘", "零");
            stIntResult = stIntResult.Replace("零毫", "零");

            stIntResult = stIntResult.Replace("零零零", "零");
            stIntResult = stIntResult.Replace("零零", "零");

            stIntResult = stIntResult.Replace("零万", "万");
            stIntResult = stIntResult.Replace("零亿", "亿");
            stIntResult = stIntResult.Replace("零元", "元");
            if (stIntResult[stIntResult.Length - 1] == '零')
            {
                stIntResult = stIntResult.Substring(0, stIntResult.Length - 1);
            }
            return stIntResult;
        }

        public async Task GotYilianPaymentResultAsync(IYilianPaymentResultEvent @event)
        {
            if (@event.Result)
            {
                await this.YilianPaymentSucceededAsync(@event);
            }
            else
            {
                await this.YilianPaymentFailedAsync(@event);
            }
        }

        public async Task SetOrderInterestAsync(decimal interest)
        {
            Guard.IdentifierMustBeAssigned(this.OrderIdentifier, this.GetType().ToString());
            using (OrderContext context = new OrderContext())
            {
                var order = await context.Query<Order>().FirstOrDefaultAsync(p => p.OrderIdentifier == this.OrderIdentifier);
                if (order != null)
                {
                    order.Interest = interest;
                    await context.SaveChangesAsync();
                }
            }
        }

        internal static async Task BuildOrderAsync(BuildInvestingOrder command)
        {
            Order order = BuildOrderAsync(command.OrderIdentifier, command.ProductType, command.UnitPrice * command.ShareCount);
            await order.BuildAsync(command);
        }

        internal static async Task<Order> LoadData(string orderIdentifier)
        {
            Guard.ArgumentNotNullOrEmpty(orderIdentifier, "orderIdentifier");
            using (OrderContext context = new OrderContext())
            {
                return await context.ReadonlyQuery<Order>().Include(o => o.PaymentInfo).Include(o => o.InvestorInfo)
                    .Include(o => o.ProductInfo).FirstAsync(o => o.OrderIdentifier == orderIdentifier);
            }
        }

        internal async Task<Order> LoadData()
        {
            Guard.IdentifierMustBeAssigned(this.OrderIdentifier, this.GetType().ToString());
            using (OrderContext context = new OrderContext())
            {
                return await context.ReadonlyQuery<Order>().Include(o => o.PaymentInfo).Include(o => o.InvestorInfo)
                    .Include(o => o.ProductInfo).FirstAsync(o => o.OrderIdentifier == this.OrderIdentifier);
            }
        }

        internal void PublishOrderBuildedEvent()
        {
            Guard.IdentifierMustBeAssigned(this.OrderIdentifier, this.GetType().ToString());
            Guard.ArgumentNotNull(this.PaymentInfo, "PaymentInfo");
            Guard.ArgumentNotNull(this.InvestorInfo, "InvestorInfo");

            this.EventBus.Publish(new OrderBuilded(this.OrderIdentifier, this.GetType())
            {
                Amount = this.Principal,
                BankCardNo = this.PaymentInfo.BankCardNo,
                BankName = this.PaymentInfo.BankName,
                Cellphone = this.InvestorInfo.Cellphone,
                CityName = this.PaymentInfo.City,
                CredentialCode = this.InvestorInfo.Credential.CredentialTypeCode(),
                CredentialNo = this.InvestorInfo.CredentialNo,
                OrderType = this.OrderType,
                ProductIdentifier = this.ProductInfo.ProductIdentifier,
                ProductNo = this.ProductInfo.ProductNo,
                RealName = this.InvestorInfo.RealName,
                SequenceNo = this.OrderNo,
                UserIdentifier = this.InvestorInfo.UserIdentifier
            });

            Console.WriteLine(this.OrderNo);
        }

        protected virtual async Task BuildAsync(BuildInvestingOrder command)
        {
            Guard.IdentifierMustBeAssigned(this.OrderIdentifier, this.GetType().ToString());

            this.InvestorInfo = this.BuildInvestorInfo(command.InvestorCellphone, command.InvestorCredential, command.InvestorCredentialNo, command.InvestorRealName, command.UserIdentifier);
            this.OrderNo = command.OrderNo;
            this.OrderTime = command.OrderTime;
            this.ProductInfo = this.BuildPorductInfo(command.EndorseImageLink, command.EndorseImageThumbnailLink, command.RepaymentDeadline, command.ProductIdentifier,
                command.ProductName, command.ProductNo, command.ProductNumber, command.Yield, command.UnitPrice, command.ProductCategory, command.SubProductNo);
            this.PaymentInfo = this.BuildPaymentInfo(command.InvestorBankCardNo, command.InvestorBankName, command.InvestorCity);
            this.ShareCount = command.ShareCount;
            this.Principal = command.UnitPrice * command.ShareCount;
            this.SettleDate = command.SettleDate;
            this.Yield = command.Yield;
            this.UserIdentifier = command.UserIdentifier;
            this.ValueDate = this.ComputeValueDate(command.ValueDateMode, command.ValueDate, command.OrderTime);
            this.Interest = command.ProductType == ProductType.ZCBAcceptance ? 0 : this.ComputeOriginalInterest(this.Principal, this.ValueDate, this.SettleDate, this.Yield);
            this.ExtraInterest = await GetExtraInterest(command);
            this.ClientType = command.ClientType;
            this.FlgMoreI1 = command.FlgMoreI1;
            this.FlgMoreI2 = command.FlgMoreI2;
            this.FlgMoreS1 = command.FlgMoreS1;
            this.FlgMoreS2 = command.FlgMoreS2;
            this.IpClient = command.IpClient;

            if (command.ProductType != ProductType.ZCBAcceptance || !this.OrderNo.StartsWith("Z"))
            {
                bool freezeShareCountResult = this.FreezeShareCount();
                if (!freezeShareCountResult)
                {
                    throw new CommandExcuteFaildException(command.CommandId, "{0} does not have enough share count {1}".FmtWith(command.ProductIdentifier, command.ShareCount),
                        "1000", "产品份额不足，请稍后再试");
                }
            }

            using (OrderContext context = new OrderContext())
            {
                try
                {
                    if (command.ProductType == ProductType.ZCBAcceptance)
                    {
                        ZCBBill zcbBill = this.BuildZCBBill(command.OrderIdentifier, command.UserIdentifier, command.OrderNo, 10, 0, command.UnitPrice * command.ShareCount, command.InvestorBankCardNo, command.InvestorBankName, command.InvestorCity, command.ProductIdentifier, ZCBBillStatus.InvestPaying, ZCBBillStatus.InvestPaying.ToZCBBillStatusString());
                        if (this.OrderNo.StartsWith("Z"))
                        {
                            zcbBill.ResultTime = DateTime.Now;
                            zcbBill.Status = ZCBBillStatus.InvestSuccess;
                            zcbBill.Remark = "付款成功";
                            using (OrderContext orderContext = new OrderContext())
                            {
                                ZCBUser zcbUser = await orderContext.Query<ZCBUser>().Where(o => o.UserIdentifier == this.UserIdentifier).FirstOrDefaultAsync();
                                zcbUser.TotalRedeemInterest += command.ShareCount;
                                await orderContext.SaveChangesAsync();
                            }
                        }
                        context.Add(zcbBill);
                    }
                    await context.SaveAsync(this);
                }
                catch (Exception e)
                {
                    this.UnfreezeShareCount();
                    throw new CommandExcuteFaildException(command.CommandId, "ProductIdentifer:{0}-OrderNo:{1}".FmtWith(command.ProductIdentifier, command.OrderNo), e,
                        "1001", "下单失败，请稍后再试");
                }
            }

            if (command.ProductType == ProductType.ZCBAcceptance && this.OrderNo.StartsWith("Z"))
            {
                return;
            }

            this.EventBus.Publish(new OrderBuilded(this.OrderIdentifier, this.GetType())
            {
                Amount = this.Principal,
                BankCardNo = this.PaymentInfo.BankCardNo,
                BankName = this.PaymentInfo.BankName,
                Cellphone = this.InvestorInfo.Cellphone,
                CityName = this.PaymentInfo.City,
                CredentialCode = this.InvestorInfo.Credential.CredentialTypeCode(),
                CredentialNo = this.InvestorInfo.CredentialNo,
                OrderType = this.OrderType,
                ProductIdentifier = this.ProductInfo.ProductIdentifier,
                ProductNo = this.OrderType == OrderType.ZCBAcceptance ? this.ProductInfo.ProductNo + "_" + DateTime.Now.ToString("yyyyMMdd") : this.ProductInfo.ProductNo,
                RealName = this.InvestorInfo.RealName,
                SequenceNo = this.OrderNo,
                UserIdentifier = this.InvestorInfo.UserIdentifier
            });
        }

        protected virtual InvestorInfo BuildInvestorInfo(string cellphone, Credential credential, string credentialNo, string realName, string userIdentifier)
        {
            return new InvestorInfo
            {
                Cellphone = cellphone,
                Credential = credential,
                CredentialNo = credentialNo,
                OrderIdentifier = this.OrderIdentifier,
                RealName = realName,
                UserIdentifier = userIdentifier
            };
        }

        protected virtual PaymentInfo BuildPaymentInfo(string bankCardNo, string bankName, string city)
        {
            return new PaymentInfo
            {
                BankCardNo = bankCardNo,
                BankName = bankName,
                City = city,
                HasResult = false,
                IsPaid = false,
                OrderIdentifier = this.OrderIdentifier,
                ResultTime = null
            };
        }

        protected virtual ProductInfo BuildPorductInfo(string endorseImageLink, string endorseImageThumbnailLink, DateTime repaymentDeadline, string productIdentifier, string productName, string productNo, int productNumber, decimal yield, decimal unitPrice, ProductCategory productCategory, string subProductNo)
        {
            return new ProductInfo
            {
                EndorseImageLink = endorseImageLink,
                EndorseImageThumbnailLink = endorseImageThumbnailLink,
                OrderIdentifier = this.OrderIdentifier,
                ProductIdentifier = productIdentifier,
                ProductName = productName,
                ProductNo = productNo,
                ProductNumber = productNumber,
                ProductYield = yield,
                RepaymentDeadline = repaymentDeadline,
                UnitPrice = unitPrice,
                ProductCategory = productCategory,
                SubProductNo = subProductNo
            };
        }

        protected virtual ZCBBill BuildZCBBill(string orderIdentifier, string userIdentifier, string orderNo, int type, decimal redeemInterest, decimal principal, string bankCardNo, string bankName, string city, string productIdentifier, ZCBBillStatus status, string remark)
        {
            return new ZCBBill
            {
                SN = orderNo,
                OrderIdentifier = orderIdentifier,
                Type = type,
                CreateTime = DateTime.Now,
                UserIdentifier = userIdentifier,
                RedeemInterest = redeemInterest,
                Principal = principal,
                BankCardNo = bankCardNo,
                BankName = bankName,
                City = city,
                ProductIdentifier = productIdentifier,
                Status = status,
                Remark = remark,
                AgreementName = ""
            };
        }

        protected virtual decimal ComputeOriginalInterest(decimal principal, DateTime valueDate, DateTime settleDate, decimal yield)
        {
            // yield = 7.98 => 7.98% = 7.98 / 100 = 0.0798
            return principal * yield * (settleDate - valueDate).Days / new decimal(36000);
        }

        protected virtual DateTime ComputeValueDate(ValueDateMode valueDateMode, DateTime? valueDate, DateTime orderTime)
        {
            if (valueDateMode == ValueDateMode.FixedDate)
            {
                return valueDate ?? orderTime.Date;
            }

            if (valueDateMode == ValueDateMode.T0)
            {
                return orderTime.Date;
            }

            if (valueDateMode == ValueDateMode.T1)
            {
                return orderTime.AddDays(1).Date;
            }

            return orderTime.Date;
        }

        protected bool FreezeShareCount()
        {
            return productService.FreezeShareCount(this.ProductInfo.ProductIdentifier, this.ShareCount);
        }

        protected bool UnfreezeShareCount()
        {
            return productService.UnfreezeShareCount(this.ProductInfo.ProductIdentifier, this.ShareCount);
        }

        private static Order BuildOrderAsync(string orderIdentifier, ProductType productType, decimal principal)
        {
            if (productType == ProductType.BankAcceptance)
            {
                return new BAOrder(orderIdentifier);
            }

            if (productType == ProductType.TradeAcceptance)
            {
                return new TAOrder(orderIdentifier);
            }

            if (productType == ProductType.ZCBAcceptance)
            {
                return new ZCBOrder(orderIdentifier);
            }
            return new Order(orderIdentifier);
        }

        private static ZCBUser BuildZCBUser(string productIdentifier, string userIdentifier, string productNo, decimal totalPrincipal, decimal currentPrincipal)
        {
            return new ZCBUser
            {
                ProductIdentifier = productIdentifier,
                UserIdentifier = userIdentifier,
                TotalPrincipal = totalPrincipal,
                CurrentPrincipal = currentPrincipal,
                ProductNo = productNo
            };
        }

        private void AddAgreements(AgreementsPackage agreements)
        {
            string consignmentAgreement = this.FillAgreementWithData(agreements.ConsignmentAgreementContent);
            string pledgeAgreement = this.FillAgreementWithData(agreements.PledgeAgreementContent);
            this.AgreementsInfo = new AgreementsInfo
            {
                ConsignmentAgreementContent = consignmentAgreement,
                ConsignmentAgreementName = agreements.ConsignmentAgreementName,
                OrderIdentifier = this.OrderIdentifier,
                PledgeAgreementContent = pledgeAgreement,
                PledgeAgreementName = agreements.PledgeAgreementName
            };
        }

        private void AddPledgeAgreement(AgreementsPackage agreements)
        {
            this.AgreementsInfo = new AgreementsInfo
            {
                ConsignmentAgreementContent = "",
                ConsignmentAgreementName = "",
                OrderIdentifier = this.OrderIdentifier,
                PledgeAgreementContent = this.FillAgreementWithData(agreements.PledgeAgreementContent),
                PledgeAgreementName = agreements.PledgeAgreementName
            };
        }

        private async Task<bool> CanLessPerRemainRedeemAmount(string productNo, decimal redeemAmount)
        {
            return await productService.CanLessPerRemainRedeemAmount(productNo, redeemAmount);
        }

        private bool CheckCanUseActivityItem(int activity)
        {
            if (activity == 1 && this.Principal > 0)
            {
                return true;
            }
            if (activity == 2 && this.Principal >= 10000)
            {
                return true;
            }
            if (activity > 2 && activity <= 6 && this.Principal >= activity * 10000)
            {
                return true;
            }

            return false;
        }

        private string FillAgreementWithData(string content)
        {
            decimal principal = this.Principal.ToFloor(2);
            decimal interest = this.Interest.ToFloor(2);
            return content.Replace("<--协议编号-->", this.OrderNo.WithHtmlUnderline())
                .Replace("<--证件号码-->", this.InvestorInfo.CredentialNo.WithHtmlUnderline())
                .Replace("<--用户账户名-->", this.InvestorInfo.RealName.WithHtmlUnderline())
                .Replace("<--用户账号开户行-->", this.PaymentInfo.BankName.WithHtmlUnderline())
                .Replace("<--用户账号-->", this.PaymentInfo.BankCardNo.WithHtmlUnderline())
                .Replace("<--用户姓名-->", this.InvestorInfo.RealName.WithHtmlUnderline())
                .Replace("<--用户ID-->", this.InvestorInfo.Cellphone.WithHtmlUnderline())
                .Replace("<--订单生成日期-->", "{0}年{1}月{2}日".FmtWith(this.OrderTime.Year, this.OrderTime.Month, this.OrderTime.Day).WithHtmlUnderline())
                .Replace("<--投资金额-->", principal.ToString(CultureInfo.InvariantCulture).WithHtmlUnderline())
                .Replace("<--投资利息-->", interest.ToString(CultureInfo.InvariantCulture).WithHtmlUnderline())
                .Replace("<--投资金额大写-->", this.GetTransResult(Convert.ToDouble(principal), 2).WithHtmlUnderline())
                .Replace("<--投资利息大写-->", this.GetTransResult(Convert.ToDouble(interest), 2).WithHtmlUnderline());
        }

        private async Task YilianPaymentFailedAsync(IYilianPaymentResultEvent @event)
        {
            Order order;
            using (OrderContext context = new OrderContext())
            {
                order = await context.Query<Order>().Include(o => o.PaymentInfo).Include(o => o.InvestorInfo).Include(o => o.ProductInfo).FirstOrDefaultAsync(o => o.OrderIdentifier == @event.OrderIdentifier);
                if (order == null || order.PaymentInfo == null)
                {
                    throw new ApplicationBusinessException("Data missing.Order {0}".FmtWith(this.OrderIdentifier));
                }

                if (order.PaymentInfo.HasResult)
                {
                    return;
                }
                if (order.OrderType == OrderType.ZCBAcceptance)
                {
                    ZCBBill zcbBill = await context.Query<ZCBBill>().FirstOrDefaultAsync(o => o.OrderIdentifier == @event.OrderIdentifier);
                    zcbBill.ResultTime = DateTime.Now;
                    zcbBill.Status = ZCBBillStatus.InvestFailed;
                    zcbBill.Remark = ZCBBillStatus.InvestFailed.ToZCBBillStatusString() + "," + @event.Message;
                    context.Add(zcbBill);
                }
                order.PaymentInfo.HasResult = true;
                order.PaymentInfo.IsPaid = @event.Result;
                order.PaymentInfo.ResultTime = DateTime.Now;
                order.PaymentInfo.TransDesc = @event.Message;
                await context.SaveChangesAsync();
            }

            if (!ProductShareCache.Paying(order.ProductInfo.ProductIdentifier, -order.ShareCount))
            {
                throw new ApplicationBusinessException("订单支付失败 => 解锁份额失败{0}|{1}|{2}"
                    .FmtWith(order.OrderIdentifier, order.ProductInfo.ProductIdentifier, order.ShareCount));
            }

            this.EventBus.Publish(new OrderPaymentFailed(order.OrderIdentifier, this.GetType())
            {
                Amount = order.Principal,
                BankCardNo = order.PaymentInfo.BankCardNo,
                BankName = order.PaymentInfo.BankName,
                Cellphone = order.InvestorInfo.Cellphone,
                CityName = order.PaymentInfo.City,
                CredentialCode = order.InvestorInfo.Credential.CredentialTypeCode(),
                CredentialNo = order.InvestorInfo.CredentialNo,
                Message = @event.Message,
                OrderIdentifier = order.OrderIdentifier,
                OrderNo = order.OrderNo,
                ProductIdentifier = order.ProductInfo.ProductIdentifier,
                ProductNo = order.ProductInfo.ProductNo,
                RealName = order.InvestorInfo.RealName,
                ShareCount = order.ShareCount,
                UserIdentifier = order.InvestorInfo.UserIdentifier
            });
        }

        private async Task YilianPaymentSucceededAsync(IYilianPaymentResultEvent @event)
        {
            Order order;
            using (OrderContext context = new OrderContext())
            {
                order = await context.Query<Order>().Include(o => o.PaymentInfo).Include(o => o.InvestorInfo).Include(o => o.ProductInfo).FirstOrDefaultAsync(o => o.OrderIdentifier == @event.OrderIdentifier);

                if (order == null || order.PaymentInfo == null)
                {
                    throw new ApplicationBusinessException("Data missing.Order {0}".FmtWith(this.OrderIdentifier));
                }

                if (order.PaymentInfo.HasResult)
                {
                    return;
                }
                if (order.OrderType == OrderType.ZCBAcceptance)
                {
                    AgreementsPackage agreementsPackage = await this.ProductInfoService.GetZCBPledgeAgreementInfoAsync(order.ProductInfo.ProductIdentifier);
                    ZCBBill zcbBill = await context.Query<ZCBBill>().FirstOrDefaultAsync(o => o.OrderIdentifier == @event.OrderIdentifier);
                    zcbBill.ResultTime = DateTime.Now;
                    zcbBill.Status = ZCBBillStatus.InvestSuccess;
                    zcbBill.Remark = ZCBBillStatus.InvestSuccess.ToZCBBillStatusString();
                    zcbBill.AgreementName = agreementsPackage.PledgeAgreementName;
                    order.AddPledgeAgreement(agreementsPackage);
                }
                else
                {
                    order.AddAgreements(await this.ProductInfoService.GetAgreementsInfoAsync(order.ProductInfo.ProductIdentifier));
                }
                order.ProductSnapshot = new ProductSnapshot { Snapshot = this.ProductInfoService.GetSnapshot(order.ProductInfo.ProductIdentifier) };
                order.PaymentInfo.HasResult = true;
                order.PaymentInfo.IsPaid = @event.Result;
                order.PaymentInfo.ResultTime = DateTime.Now;
                using (var tran = context.Database.BeginTransaction(IsolationLevel.RepeatableRead))
                {
                    try
                    {
                        if (order.OrderType == OrderType.ZCBAcceptance)
                        {
                            ZCBUser zcbUser = await context.Query<ZCBUser>().FirstOrDefaultAsync(o => o.UserIdentifier == order.UserIdentifier && o.ProductIdentifier == order.ProductInfo.ProductIdentifier);
                            if (zcbUser == null)
                            {
                                zcbUser = BuildZCBUser(order.ProductInfo.ProductIdentifier, order.UserIdentifier, order.ProductInfo.ProductNo, order.Principal, order.Principal);
                            }
                            else
                            {
                                zcbUser.TotalPrincipal = zcbUser.TotalPrincipal + order.Principal;
                                zcbUser.CurrentPrincipal = zcbUser.CurrentPrincipal + order.Principal;
                            }
                            context.Add(zcbUser);
                        }
                        await context.SaveChangesAsync();
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
            int remain;
            if (!ProductShareCache.Paid(order.ProductInfo.ProductIdentifier, order.ShareCount, out remain))
            {
                throw new ApplicationBusinessException("订单支付成功 => 解锁份额失败{0}|{1}|{2}"
                    .FmtWith(order.OrderIdentifier, order.ProductInfo.ProductIdentifier, order.ShareCount));
            }
            if (remain == 0 && order.OrderType != OrderType.ZCBAcceptance)
            {
                Product p = new Product(order.ProductInfo.ProductIdentifier);
                await p.SetSoldOutAsync();
            }
            this.EventBus.Publish(new OrderPaymentSuccessed(order.OrderIdentifier, this.GetType())
            {
                Amount = order.Principal,
                BankCardNo = order.PaymentInfo.BankCardNo,
                BankName = order.PaymentInfo.BankName,
                Cellphone = order.InvestorInfo.Cellphone,
                CityName = order.PaymentInfo.City,
                CredentialCode = order.InvestorInfo.Credential.CredentialTypeCode(),
                CredentialNo = order.InvestorInfo.CredentialNo,
                OrderIdentifier = order.OrderIdentifier,
                ProductIdentifier = order.ProductInfo.ProductIdentifier,
                ProductNo = order.ProductInfo.ProductNo,
                RealName = order.InvestorInfo.RealName,
                OrderNo = order.OrderNo,
                ShareCount = order.ShareCount,
                UserIdentifier = order.InvestorInfo.UserIdentifier
            });
        }
    }
}