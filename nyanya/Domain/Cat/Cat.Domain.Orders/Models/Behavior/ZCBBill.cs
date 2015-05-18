using Cat.Commands.Orders;
using Cat.Domain.Orders.Database;
using Cat.Domain.Products.Database;
using Cat.Domain.Products.Models;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services;
using Cat.Domain.Products.Services.Interfaces;
using Cat.Domain.Users.ReadModels;
using Cat.Domain.Users.Services;
using Cat.Domain.Users.Services.Interfaces;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using Domian.Bus;
using Domian.Config;
using Cat.Events.Orders;
using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Cat.Domain.Orders.Models
{
    public partial class ZCBBill
    {
        public ZCBBill(string userIdentifier, string productIdentifier)
        {
            this.UserIdentifier = userIdentifier;
            this.ProductIdentifier = productIdentifier;
        }

        private IUserInfoService UserInfoService
        {
            get { return new UserInfoService(); }
        }

        private IZCBProductInfoService zcbProductInfoService
        {
            get { return new ZCBProductInfoService(); }
        }

        protected IEventBus EventBus
        {
            get { return CqrsDomainConfig.Configuration.EventBus; }
        }

        /// <summary>
        /// Builds the redeem principal asynchronous.未加锁
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        internal async Task<int> BuildRedeemPrincipalAsync(BuildRedeemPrincipal command)
        {
            DateTime dateTime = DateTime.Now.Date;
            int delayCount = 1;
            this.AddOrderIdentifier(GuidUtils.NewGuidString());
            //银行卡信息
            var bankCard = await UserInfoService.GetBankCardInfoAsync(command.UserIdentifier, command.BankCardNo);
            var product = await zcbProductInfoService.GetProductInfoByIdentifierAsync(command.ProductIdentifier);
            if (product == null)
            {
                return 0;
            }
            //每日限额
            decimal preRemain = product.PerRemainRedeemAmount;
            using (OrderContext context = new OrderContext())
            {
                ZCBUser user = await context.ReadonlyQuery<ZCBUser>().FirstOrDefaultAsync(p => p.UserIdentifier == command.UserIdentifier && p.ProductIdentifier == command.ProductIdentifier);
                //今日投入的金额，不计算付款中的
                decimal todayPrincipal =
                   await context.ReadonlyQuery<ZCBBill>()
                       .Where(p => p.UserIdentifier == command.UserIdentifier && p.ProductIdentifier == command.ProductIdentifier && p.Type == 10 && DbFunctions.TruncateTime(p.CreateTime) == dateTime && p.Status == ZCBBillStatus.InvestSuccess)
                       .SumAsync(p => (decimal?)p.Principal) ?? 0;
                //取款中的金额
                decimal curRedeemPrincipal = await context.ReadonlyQuery<ZCBBill>().Where(p => p.UserIdentifier == command.UserIdentifier && p.ProductIdentifier == command.ProductIdentifier && p.Type == 20 && p.Status == ZCBBillStatus.RedeemApplication).SumAsync(p => (decimal?)p.Principal) ?? 0;

                bool finallyRedeem;
                if (todayPrincipal > 0)
                {
                    if (command.RedeemPrincipal > user.CurrentPrincipal - todayPrincipal - curRedeemPrincipal)
                    {
                        return 0;
                    }
                    finallyRedeem = false;
                }
                else
                {
                    finallyRedeem = command.RedeemPrincipal >= user.CurrentPrincipal - curRedeemPrincipal;
                    if (command.RedeemPrincipal > user.CurrentPrincipal - curRedeemPrincipal + user.TotalInterest - user.TotalRedeemInterest || user.CurrentPrincipal - curRedeemPrincipal <= 0)
                    {
                        return 0;
                    }
                }

                var zcbBill = this.BuildZCBRedeemBill(bankCard, user, todayPrincipal, product.ConsignmentAgreementName, curRedeemPrincipal, dateTime, finallyRedeem, command.RedeemPrincipal);
                if (zcbBill.Principal <= 0)
                {
                    return 0;
                }
                //计算到账时间

                //最慢的回款时间
                var maxDate = (await context.ReadonlyQuery<ZCBBill>().MaxAsync(p => p.DelayDate ?? DateTime.MinValue)).Date;
                if (maxDate >= dateTime)
                {
                    var principal = await context.ReadonlyQuery<ZCBBill>().Where(p => DbFunctions.TruncateTime(p.DelayDate) == maxDate && p.Type == 20)
                        .SumAsync(p => (decimal?)p.Principal) ?? 0;

                    zcbBill.DelayDate = preRemain - principal - zcbBill.Principal < 0 ? maxDate.AddDays(1).Date : maxDate;
                    delayCount += (int)(zcbBill.DelayDate - dateTime).GetValueOrDefault().TotalDays;
                }
                zcbBill.Remark = "预计{0}日内到帐，节假日顺延".FormatWith(delayCount);

                context.Add(zcbBill);
                await context.SaveChangesAsync();
            }
            return delayCount;
        }

        private ZCBBill AddOrderIdentifier(string orderIdentifier)
        {
            this.OrderIdentifier = orderIdentifier;
            return this;
        }

        private AgreementsInfo BuildAgreementsInfo(int consignmentAgreementId, string consignmentAgreementName, ZCBBill bill)
        {
            string content = "";
            using (ProductContext context = new ProductContext())
            {
                var firstOrDefault =
                    context.ReadonlyQuery<Agreement>().FirstOrDefault(p => p.Id == consignmentAgreementId);
                if (firstOrDefault != null)
                    content = firstOrDefault.Content;
            }

            AgreementsInfo agreementsInfo = new AgreementsInfo()
            {
                OrderIdentifier = bill.OrderIdentifier,
                ConsignmentAgreementContent =content,
                ConsignmentAgreementName = consignmentAgreementName,
                PledgeAgreementContent = "",
                PledgeAgreementName = ""
            };
            return agreementsInfo;
        }

        private string FillAgreementWithData(string content,InvestorInfo investorInfo,DateTime createTime)
        {
            return content
                .Replace("<--证件号码-->", investorInfo.CredentialNo.WithHtmlUnderline())
                .Replace("<--用户姓名-->", investorInfo.RealName.WithHtmlUnderline())
                .Replace("<--用户ID-->", investorInfo.Cellphone.WithHtmlUnderline())
                .Replace("<--订单生成日期-->", "{0}年{1}月{2}日".FmtWith(createTime.Year, createTime.Month, createTime.Day).WithHtmlUnderline())
                ;
        }

        /// <summary>
        /// Builds the ZCB redeem bill.不赋值OrderIdentifier,userIdentifier,productIdentifier
        /// </summary>
        /// <param name="bankCard">The bank card.</param>
        /// <param name="user">The user.</param>
        /// <param name="todayPrincipal">The today principal.</param>
        /// <param name="consignmentAgreementName">Name of the consignment agreement.</param>
        /// <param name="curRedeemPrincipal">The current redeem principal.</param>
        /// <param name="deleyDateTime">The deley date time.</param>
        /// <param name="finallyRedeem">if set to <c>true</c> [finally redeem].</param>
        /// <param name="redeemPrincipal">The redeem principal.</param>
        /// <returns></returns>
        private ZCBBill BuildZCBRedeemBill(PaymentBankCardInfo bankCard, ZCBUser user, decimal todayPrincipal, string consignmentAgreementName
            , decimal curRedeemPrincipal, DateTime deleyDateTime, bool finallyRedeem, decimal redeemPrincipal)
        {
            SN = SequenceNoUtils.GenerateNo('R');
            CreateTime = DateTime.Now;
            Type = 20;
            BankCardNo = bankCard.BankCardNo;
            BankName = bankCard.BankName;
            City = bankCard.City;
            Status = ZCBBillStatus.RedeemApplication;
            Principal =
                finallyRedeem
                    ? user.CurrentPrincipal - todayPrincipal - curRedeemPrincipal
                    : redeemPrincipal;
            RedeemInterest = finallyRedeem ? user.TotalInterest - user.TotalRedeemInterest : 0;
            AgreementName = "";
            DelayDate = deleyDateTime;

            return this;
        }

        public async Task<bool> SetZCBRedeemBillResult()
        {
            if (SN.IsNullOrEmpty())
            {
                return false;
            }
            ZCBProductInfo product = null;
            ZCBBill bill = null;
            using (OrderContext context = new OrderContext())
            {
                bill = await context.Query<ZCBBill>().FirstOrDefaultAsync(p => p.SN == this.SN);
                if (bill != null && bill.Status == ZCBBillStatus.RedeemApplication && bill.Type == 20 &&
                    bill.ResultTime == null)
                {
                    product = await zcbProductInfoService.GetProductInfoByIdentifierAsync(bill.ProductIdentifier);
                    if (product == null) return true;
                    bill.ResultTime = DateTime.Now;
                    bill.Status = ZCBBillStatus.RedeemSuccess;
                    bill.AgreementName = product.ConsignmentAgreementName;
                    context.Add(bill);

                    var agreementsInfo = BuildAgreementsInfo(product.ConsignmentAgreementId, product.ConsignmentAgreementName, bill);
                    InvestorInfo investorInfo = await context.ReadonlyQuery<InvestorInfo>().FirstOrDefaultAsync(p => p.UserIdentifier == bill.UserIdentifier);
                    string content = this.FillAgreementWithData(agreementsInfo.ConsignmentAgreementContent, investorInfo, bill.CreateTime);
                    agreementsInfo.ConsignmentAgreementContent = content;
                    context.Add(agreementsInfo);
                    
                    using (var tran = context.Database.BeginTransaction(IsolationLevel.RepeatableRead))
                    {
                        try
                        {
                            ZCBUser user = await context.Query<ZCBUser>().FirstOrDefaultAsync(p => p.ProductIdentifier == bill.ProductIdentifier && p.UserIdentifier == bill.UserIdentifier);
                            user.CurrentPrincipal -= bill.Principal;
                            user.TotalRedeemInterest += bill.RedeemInterest;
                            context.Add(user);
                            await context.SaveChangesAsync();
                            tran.Commit();
                        }
                        catch (Exception e)
                        {
                            tran.Rollback();
                            throw;
                        }
                    }

                    this.EventBus.Publish(new SetRedeemBillResult(bill.OrderIdentifier, this.GetType())
                    {
                        Principal = bill.Principal,
                        RedeemInterest = bill.RedeemInterest,
                        BankCardNo = bill.BankCardNo,
                        BankName = bill.BankName,
                        Cellphone = investorInfo.Cellphone,
                        SN = bill.SN
                    });
                }
            }
            return true;
        }
    }
}