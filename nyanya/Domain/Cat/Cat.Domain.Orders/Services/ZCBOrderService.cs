// ***********************************************************************
// Project          : nyanya
// Author           : Siqi Lu
// Created          : 2015-03-04  6:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-06  11:23 AM
// ***********************************************************************
// <copyright file="ZCBOrderService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cat.Commands.Orders;
using Cat.Domain.Orders.Database;
using Cat.Domain.Orders.Models;
using Cat.Domain.Orders.Services.DTO;
using Cat.Domain.Orders.Services.Interfaces;
using Cat.Domain.Products.Models;
using Domian.DTO;
using Infrastructure.Lib.Extensions;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Cat.Domain.Orders.Services
{
    public class ZCBOrderService : IZCBOrderService
    {
        #region IZCBOrderService Impl

        public async Task CheckRedeemApplication(ZCBUser zcbUser, IList<ZCBHistory> yieldHistories)
        {
            //因用户的总收益可能已改变，故需要更新当前用户信息
            using (var context = new OrderContext())
            {
                var user = zcbUser;
                zcbUser = await context.Query<ZCBUser>().FirstOrDefaultAsync(x => x.UserIdentifier == user.UserIdentifier && x.ProductIdentifier == user.ProductIdentifier);
            }
            //提现申请中的记录
            var doingBills = await this.GetDoingZcbBill(zcbUser);
            if (!doingBills.Any()) return;
            //检查是否需要重建缺失的收益记录
            await this.CehckRebuildLostUserBill(zcbUser, doingBills.Min(x => x.CreateTime), yieldHistories);
            //是否有提现收益的提现申请记录，没有则不处理
            if (!doingBills.Any(x => x.RedeemInterest > 0)) return;
            //用户收益记录
            var userBills = await this.GetUserBill(zcbUser);
            if (!userBills.Any()) return;
            //有提现收益的提现申请中记录
            var hasInterestDoingBills = doingBills.Where(x => x.RedeemInterest > 0).ToList();
            for (var i = 0; i < hasInterestDoingBills.Count; i++)
            {
                var currentBill = hasInterestDoingBills[i];
                var beforeBill = i == 0 ? null : hasInterestDoingBills[i - 1];

                var currentBillInterest = this.GetRedeemInterest(currentBill, userBills);
                var beforebillInterset = this.GetBeforeDoingBills(currentBill, beforeBill, doingBills).Sum(bill => this.GetRedeemInterest(bill, userBills));
                var newRedeemInterest = currentBillInterest + beforebillInterset;
                if (newRedeemInterest <= 0 || currentBill.RedeemInterest + newRedeemInterest > zcbUser.TotalInterest) continue;
                currentBill.RedeemInterest += newRedeemInterest;
                await this.UpdateRedeemInterest(currentBill);
            }
        }

        public async Task<ZCBUserRemainRedeemInfo> CheckRedeemPrincipalAsync(string userIdentifier, string productIdentifier)
        {
            DateTime dateTime = DateTime.Now.Date;
            using (OrderContext context = new OrderContext())
            {
                //未加锁
                ZCBUser user = await context.ReadonlyQuery<ZCBUser>().FirstOrDefaultAsync(p => p.UserIdentifier == userIdentifier && p.ProductIdentifier == productIdentifier);
                decimal todayPrincipal =
                    await context.ReadonlyQuery<ZCBBill>()
                        .Where(p => p.UserIdentifier == userIdentifier && p.ProductIdentifier == productIdentifier && p.Type == 10 && DbFunctions.TruncateTime(p.CreateTime) == dateTime && p.Status == ZCBBillStatus.InvestSuccess)
                        .SumAsync(p => (decimal?)p.Principal) ?? 0;
                decimal curRedeemPrincipal = await context.ReadonlyQuery<ZCBBill>()
                    .Where(p => p.UserIdentifier == userIdentifier && p.ProductIdentifier == productIdentifier && p.Type == 20 && p.Status == ZCBBillStatus.RedeemApplication)
                    .SumAsync(p => (decimal?)p.Principal) ?? 0;
                return new ZCBUserRemainRedeemInfo
                {
                    //剩余本金
                    RemainPrincipal = user.CurrentPrincipal - todayPrincipal - curRedeemPrincipal,
                    //剩余利息
                    RemainRedeemInterest = todayPrincipal > 0 ? 0 : user.TotalInterest - user.TotalRedeemInterest,
                    //今日投资额
                    TodayPrincipal = todayPrincipal
                };
            }
        }

        public async Task<int> CheckRedeemPrincipalCount(string userIdentifier)
        {
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
            using (OrderContext context = new OrderContext())
            {
                return await context.Database.SqlQuery<int>(@"select count(Id) from ZCBBill where UserIdentifier='{0}' and Type =20 and CONVERT(varchar(50),CreateTime,23) = '{1}'".FmtWith(userIdentifier, dateTime)).FirstAsync();
            }
        }

        public async Task<IList<ZCBUser>> GetActiveZcbUsers()
        {
            using (var context = new OrderContext())
            {
                return await context.ReadonlyQuery<ZCBUser>().Where(x => x.CurrentPrincipal > 0).ToListAsync();
            }
        }

        /// <summary>
        ///     获取用户当前未处理的提现金额
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <returns></returns>
        public async Task<decimal> GetUnRedeemPrincipal(string userIdentifier)
        {
            using (OrderContext context = new OrderContext())
            {
                string sql = @"select ISNULL(Sum(Principal),0) as Principal from ZCBBill where Type = 20 and Status = 40 and UserIdentifier = '{0}' group by ProductIdentifier".FmtWith(userIdentifier);
                return await context.Database.SqlQuery<decimal>(sql).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        ///     获取用户今天投资金额
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <returns></returns>
        public async Task<decimal> GetUserTodayInvesting(string userIdentifier)
        {
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
            using (OrderContext context = new OrderContext())
            {
                string sql = @"select IsNull(SUM(o.Principal),0) from Orders as o inner join PaymentInfo as p on o.OrderIdentifier = p.OrderIdentifier inner join ProductInfo as I
on o.OrderIdentifier = I.OrderIdentifier where CONVERT(varchar(50),o.OrderTime,23) = '{0}'
and p.IsPaid = 1 and o.OrderType = 30 and p.HasResult = 1 and o.UserIdentifier = '{1}' group by I.ProductIdentifier".FmtWith(dateTime, userIdentifier);
                return await context.Database.SqlQuery<decimal>(sql).FirstOrDefaultAsync();
            }
        }

        public async Task<IPaginatedDto<ZCBBill>> GetZCBBillListAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            IList<ZCBBill> items;

            using (OrderContext context = new OrderContext())
            {
                items = await context.ReadonlyQuery<ZCBBill>().Where(o => o.UserIdentifier == userIdentifier).OrderByDescending(o => o.CreateTime)
                    .ThenByDescending(o => o.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();

                totalCount = await context.ReadonlyQuery<ZCBBill>().Where(o => o.UserIdentifier == userIdentifier).CountAsync();
            }

            return new PaginatedDto<ZCBBill>(pageIndex, pageSize, totalCount, items);
        }

        public async Task<ZCBUser> GetZCBUserAsync(string userIdentifier)
        {
            using (OrderContext context = new OrderContext())
            {
                return await context.ReadonlyQuery<ZCBUser>().Where(o => o.UserIdentifier == userIdentifier).FirstOrDefaultAsync();
            }
        }

        public async Task<IPaginatedDto<ZCBUserBill>> GetZCBUserBillListAsync(string userIdentifier, DateTime startTime, DateTime endTime, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            IList<ZCBUserBill> items;
            using (OrderContext context = new OrderContext())
            {
                items = await context.ReadonlyQuery<ZCBUserBill>().Where(o => o.UserIdentifier == userIdentifier && DbFunctions.TruncateTime(o.BillDate) >= startTime && DbFunctions.TruncateTime(o.BillDate) <= endTime)
                    .OrderByDescending(o => o.BillDate).ThenByDescending(o => o.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
                totalCount = await context.ReadonlyQuery<ZCBUserBill>().Where(o => o.UserIdentifier == userIdentifier).CountAsync();
            }
            return new PaginatedDto<ZCBUserBill>(pageIndex, pageSize, totalCount, items);
        }

        public async Task SetYesterDayInterest(ZCBUser zcbUser, IList<ZCBHistory> yieldHistories)
        {
            using (var context = new OrderContext())
            {
                var billDate = DateTime.Now.AddDays(-1).Date;
                var isExists = await this.CheckUserBillExistsAlready(zcbUser, billDate, context);

                //当日没有收益记录
                if (!isExists)
                {
                    //历史
                    var yieldHistory = yieldHistories.Where(x => x.NextStartSellTime.Date <= billDate.Date).OrderByDescending(x => x.NextStartSellTime).ToList();
                    if (yieldHistory.Count <= 0) return;
                    //收益率（不计周六周日）
                    var yesterDayYield = yieldHistory.First().NextYield;
                    //再投金额
                    var currentPrincipal = await this.GetPrincipalByDate(zcbUser, billDate, context);
                    //再投金额小于等于0时不处理
                    if (currentPrincipal <= 0) return;
                    //收益
                    var yesterDayInterest = this.ComputeYesterDayInterest(currentPrincipal, yesterDayYield);
                    //添加ZCBUserBill
                    var newZcbUserBill = new ZCBUserBill
                    {
                        UserIdentifier = zcbUser.UserIdentifier,
                        ProductIdentifier = zcbUser.ProductIdentifier,
                        Principal = currentPrincipal,
                        Yield = yesterDayYield,
                        Interest = yesterDayInterest,
                        BillDate = billDate,
                        Remark = string.Empty
                    };
                    context.Add(newZcbUserBill);

                    //更新ZCBUser
                    var currentUser = await context.Query<ZCBUser>().FirstOrDefaultAsync(x => x.UserIdentifier == zcbUser.UserIdentifier && x.ProductIdentifier == zcbUser.ProductIdentifier);
                    currentUser.TotalInterest += yesterDayInterest;
                    currentUser.YesterdayInterest = yesterDayInterest;

                    await context.SaveChangesAsync();
                }
            }
        }

        #endregion IZCBOrderService Impl

        #region Private Method

        private static DateTime GetLastWorkDay(DateTime time)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTable table = account.CreateCloudTableClient().GetTableReference("DailyConfig");

            for (int i = 1; i < 100; i++)
            {
                DateTime day = time.AddDays(-i);
                string dateString = day.ToString("yyyyMMdd");
                var result = table.ExecuteQuery(new TableQuery().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, dateString)).Take(1)).ToList();

                var config = result.FirstOrDefault();
                EntityProperty value;
                if (config != null && config.Properties.TryGetValue("IsWorkday", out value)
                    && value.BooleanValue.GetValueOrDefault(false))
                {
                    return day;
                }
            }

            return time;
        }

        private static bool IsWorkDay(DateTime time)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTable table = account.CreateCloudTableClient().GetTableReference("DailyConfig");

            string dateString = time.ToString("yyyyMMdd");
            var result = table.ExecuteQuery(new TableQuery().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, dateString)).Take(1)).ToList();

            var config = result.FirstOrDefault();
            EntityProperty value;
            if (config != null && config.Properties.TryGetValue("IsWorkday", out value)
                && value.BooleanValue.GetValueOrDefault(false))
            {
                return true;
            }

            return false;
        }

        private async Task CehckRebuildLostUserBill(ZCBUser zcbUser, DateTime beginTime, IList<ZCBHistory> yieldHistories)
        {
            if (zcbUser == null || yieldHistories == null || !yieldHistories.Any()) return;
            using (var context = new OrderContext())
            {
                var minData = beginTime;
                var dayCount = (DateTime.Now.Date - minData.Date).Days;
                var dateList = new List<DateTime>();
                for (var i = 0; i < dayCount; i++)
                {
                    dateList.Add(minData.Date.AddDays(i));
                }
                var updateUserFlag = false;

                foreach (var billDate in dateList.Select(billDateTime => billDateTime.Date.AddDays(-1).Date))
                {
                    //是否有收益记录
                    if (await this.CheckUserBillExistsAlready(zcbUser, billDate, context)) continue;
                    //收益率
                    var yield = yieldHistories.OrderByDescending(x => x.NextStartSellTime).First(x => x.NextStartSellTime.Date <= billDate.Date).NextYield;
                    //再投金额
                    var principal = await this.GetPrincipalByDate(zcbUser, billDate, context);
                    //收益
                    var interest = this.ComputeYesterDayInterest(principal, yield);
                    //添加ZCBUserBill
                    var newZcbUserBill = new ZCBUserBill
                    {
                        UserIdentifier = zcbUser.UserIdentifier,
                        ProductIdentifier = zcbUser.ProductIdentifier,
                        Principal = principal,
                        Yield = yield,
                        Interest = interest,
                        BillDate = billDate,
                        Remark = string.Empty
                    };
                    context.Add(newZcbUserBill);
                    updateUserFlag = true;
                }

                if (updateUserFlag)
                {
                    await context.SaveChangesAsync();
                    await this.UpdateUser(zcbUser, context);
                }
            }
        }

        private async Task<bool> CheckUserBillExistsAlready(ZCBUser user, DateTime billDate, OrderContext context)
        {
            if (context == null) return false;
            var sql = string.Format(@"select count(1) from ZCBUserBill where UserIdentifier = '{0}' and ProductIdentifier = '{1}' and CONVERT(varchar(50),BillDate,23) = '{2}'", user.UserIdentifier, user.ProductIdentifier, billDate.ToString("yyyy-MM-dd"));
            return await context.Database.SqlQuery<int>(sql).FirstAsync() > 0;
        }

        private decimal ComputeInterest(decimal principal, DateTime valueDate, DateTime settleDate, decimal yield)
        {
            return principal * yield * (settleDate - valueDate).Days / new decimal(36000);
        }

        private decimal ComputeInterest(decimal principal, DateTime valueDate, DateTime settleDate, IEnumerable<ZCBUserBill> userBills)
        {
            var interest = userBills.Sum(userBill => this.ComputeYesterDayInterest(principal, userBill.Yield));

            return interest;
        }

        private decimal ComputeYesterDayInterest(decimal principal, decimal yield)
        {
            return principal * yield / new decimal(36000);
        }

        private IEnumerable<ZCBBill> GetBeforeDoingBills(ZCBBill currentBill, ZCBBill beforeBill, IEnumerable<ZCBBill> doingBills)
        {
            return beforeBill == null
                ? doingBills.Where(x => x.CreateTime <= currentBill.CreateTime && x.RedeemInterest == 0)
                : doingBills.Where(x => x.CreateTime <= currentBill.CreateTime && x.RedeemInterest == 0 && x.CreateTime > beforeBill.CreateTime);
        }

        private async Task<IList<ZCBBill>> GetDoingZcbBill(ZCBUser zcbUser)
        {
            using (var context = new OrderContext())
            {
                return await context.ReadonlyQuery<ZCBBill>().Where(x =>
                    x.Type == 20 &&
                    x.Status == ZCBBillStatus.RedeemApplication &&
                    x.UserIdentifier == zcbUser.UserIdentifier &&
                    x.ProductIdentifier == zcbUser.ProductIdentifier).OrderBy(x => x.CreateTime).ToListAsync();
            }
        }

        private async Task<decimal> GetPrincipalByDate(ZCBUser zcbUser, DateTime someTime, OrderContext context)
        {
            var successBill = await this.GetSuccessZcbBill(zcbUser, context);

            DateTime pricipalResultDate = GetLastWorkDay(IsWorkDay(someTime) ? someTime : GetLastWorkDay(someTime));

            var principal = successBill.Where(x =>
                x.Type == 10 &&
                x.Status == ZCBBillStatus.InvestSuccess &&
                x.ResultTime != null &&
                x.ResultTime.Value.Date <= pricipalResultDate.Date).Sum(x => x.Principal);

            var redeemPrincipal = successBill.Where(x =>
                x.Type == 20 &&
                x.Status == ZCBBillStatus.RedeemSuccess &&
                x.ResultTime != null &&
                x.ResultTime.Value.Date <= someTime.Date).Sum(x => x.Principal);

            return principal - redeemPrincipal;
        }

        private decimal GetRedeemInterest(ZCBBill currentBill, IEnumerable<ZCBUserBill> userBills)
        {
            if (currentBill.CreateTime.Date > DateTime.Now.Date) return 0;

            //提现申请日至今的用户收益记录
            var validBills = this.GetValidBills(currentBill.CreateTime, userBills);
            //历史收益
            var historyInterest = this.ComputeInterest(currentBill.Principal, currentBill.CreateTime.Date, DateTime.Now.Date, validBills);

            return historyInterest;
        }

        private async Task<IList<ZCBBill>> GetSuccessZcbBill(ZCBUser zcbUser, OrderContext context)
        {
            return await context.ReadonlyQuery<ZCBBill>().Where(x =>
                (x.Status == ZCBBillStatus.InvestSuccess || x.Status == ZCBBillStatus.RedeemSuccess) &&
                x.UserIdentifier == zcbUser.UserIdentifier &&
                x.ProductIdentifier == zcbUser.ProductIdentifier).OrderBy(x => x.CreateTime).ToListAsync();
        }

        private async Task<IList<ZCBUserBill>> GetUserBill(ZCBUser zcbUser)
        {
            using (var context = new OrderContext())
            {
                return await context.ReadonlyQuery<ZCBUserBill>().Where(x =>
                    x.UserIdentifier == zcbUser.UserIdentifier &&
                    x.ProductIdentifier == zcbUser.ProductIdentifier).OrderBy(x => x.BillDate).ToListAsync();
            }
        }

        private IEnumerable<ZCBUserBill> GetValidBills(DateTime startdate, IEnumerable<ZCBUserBill> userBills)
        {
            return userBills.Where(x => x.BillDate.Date <= DateTime.Now.Date && x.BillDate.Date >= startdate.Date).ToList();
        }

        private async Task UpdateRedeemInterest(ZCBBill bill)
        {
            using (var context = new OrderContext())
            {
                await context.SaveOrUpdateAsync(bill, b => true);
            }
        }

        private async Task UpdateUser(ZCBUser zcbUser, OrderContext context)
        {
            var userBills = await this.GetUserBill(zcbUser);
            if (userBills == null || userBills.Count == 0) return;
            var totalInterest = userBills.Sum(x => x.Interest);
            var yesterDayInterest = userBills.OrderByDescending(x => x.BillDate).First().Interest;

            var currentUser = await context.Query<ZCBUser>().FirstOrDefaultAsync(x => x.UserIdentifier == zcbUser.UserIdentifier && x.ProductIdentifier == zcbUser.ProductIdentifier);
            currentUser.TotalInterest = totalInterest;
            currentUser.YesterdayInterest = yesterDayInterest;
            await context.SaveChangesAsync();
        }

        #endregion Private Method
    }
}