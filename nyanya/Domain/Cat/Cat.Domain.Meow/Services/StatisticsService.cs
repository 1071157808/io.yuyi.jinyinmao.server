using Cat.Domain.Meow.Services.DTO;
using Cat.Domain.Meow.Services.Interfaces;
using Cat.Domain.Users.Database;
using Cat.Domain.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cat.Domain.Orders.Database;
using Cat.Domain.Orders.ReadModels;
using Cat.Domain.Products.Database;
using Cat.Domain.Products.ReadModels;

namespace Cat.Domain.Meow.Services
{
    public class StatisticsService : IStatisticsService
    {
        public async Task<StatisticsResult> GetStatisticsAsync(DateTime startTime,DateTime endTime)
        {
            var UserStatistics = await this.GetUserStatistics(startTime, endTime);
            var OrderStatistics = await this.GetOrderStatistics(startTime, endTime);
            int OnSaleProductNum = await this.GetOnSaleProductNum(startTime, endTime);
            var statistics = new StatisticsResult
            {
                RegisterUserNum = UserStatistics.RegisterUserNum,
                SuccessLoginNum = UserStatistics.SuccessLoginNum,
                FailedLoginNum = UserStatistics.FailedLoginNum,
                SuccessBankCardNum = UserStatistics.SuccessBankCardNum,
                FailedBankCardNum = UserStatistics.FailedBankCardNum,
                SuccessOrderNum = OrderStatistics.SuccessOrderNum,
                FailedOrderNum = OrderStatistics.FailedOrderNum,
                OnSaleProductNum = OnSaleProductNum
            };
            return statistics;
        }

        private async Task<StatisticsResult> GetUserStatistics(DateTime startTime,DateTime endTime)
        {
            int RegisterUserNum = 0;            //注册用户数
            int SuccessLoginNum = 0;            //登录成功用户数
            int FailedLoginNum = 0;             //登录失败用户数
            int SuccessBankCardNum = 0;         //绑卡成功数
            int FailedBankCardNum = 0;          //绑卡失败数
            using (UserContext context = new UserContext())
            {
                RegisterUserNum = await context.ReadonlyQuery<UserLoginInfo>().Where(o => o.SignUpTime >= startTime && o.SignUpTime <= endTime).CountAsync();
                SuccessLoginNum = await context.ReadonlyQuery<UserLoginInfo>().Where(o => o.LastSuccessSignInTime >= startTime && o.LastSuccessSignInTime <= endTime).CountAsync();
                FailedLoginNum = await context.ReadonlyQuery<UserLoginInfo>().Where(o => o.LastFailedSignInTime >= startTime && o.LastFailedSignInTime <= endTime).CountAsync();
                SuccessBankCardNum = await context.ReadonlyQuery<BankCardRecord>().Where(o => o.VerifingTime >= startTime && o.VerifingTime <= endTime && o.Verified != null && o.Verified == true).CountAsync();
                FailedBankCardNum = await context.ReadonlyQuery<BankCardRecord>().Where(o => o.VerifingTime >= startTime && o.VerifingTime <= endTime && (o.Verified == false || o.Verified ==null)).CountAsync();
            }
            var statisticsResult = new StatisticsResult
            {
                RegisterUserNum = RegisterUserNum,
                SuccessLoginNum = SuccessLoginNum,
                FailedLoginNum = FailedLoginNum,
                SuccessBankCardNum = SuccessBankCardNum,
                FailedBankCardNum = FailedBankCardNum
            };

            return statisticsResult;
        }

        private async Task<StatisticsResult> GetOrderStatistics(DateTime startTime, DateTime endTime)
        {
            int SuccessOrderNum = 0;        //成功订单数
            int FailedOrderNum = 0;         //失败订单数（去掉“余额不足”）

            using (OrderContext context = new OrderContext())
            {
                SuccessOrderNum = await context.ReadonlyQuery<OrderInfo>().Where(o => o.OrderTime >= startTime && o.OrderTime <= endTime && o.IsPaid == true).CountAsync();
                FailedOrderNum = await context.ReadonlyQuery<OrderInfo>().Where(o => o.OrderTime >= startTime && o.OrderTime <= endTime && o.IsPaid == false && !o.PaymentInfoTransDesc.Contains("余额不足")).CountAsync();
            }
            var statisticsResult = new StatisticsResult
            {
                SuccessOrderNum = SuccessOrderNum,
                FailedOrderNum = FailedOrderNum
            };
            return statisticsResult;
        }

        private async Task<int> GetOnSaleProductNum(DateTime startTime, DateTime endTime)
        {
            int OnSaleProductNum = 0;       //在售产品数
            using (ProductContext context = new ProductContext())
            {
                OnSaleProductNum = await context.ReadonlyQuery<ProductInfo>().Where(o => (!o.SoldOut && o.SoldOutTime == null &&
                    ((o.LaunchTime>=o.StartSellTime && o.LaunchTime<=endTime) || (o.LaunchTime<o.StartSellTime && o.StartSellTime <=endTime)) && o.EndSellTime >= startTime)).CountAsync();
            }
            return OnSaleProductNum;
        }
    }
}
