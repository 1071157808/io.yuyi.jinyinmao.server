// FileInformation: nyanya/Cat.Domain.Orders/IOrderInfoService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/15   6:37 PM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cat.Domain.Orders.ReadModels;
using Cat.Domain.Orders.Services.DTO;
using Domian.DTO;
using Cat.Commands.Products;

namespace Cat.Domain.Orders.Services.Interfaces
{
    public enum OrderSortingStrategy
    {
        ByOrderTime = 0,
        BySettleDate = 1
    }

    public interface IExactOrderInfoService : IOrderInfoService
    {
    }

    public interface IOrderInfoService : IOrderInfoService<OrderInfo>
    {
        Task<string> GetConsignmentAgreementAsync(string orderIdentifier);

        Task<InvestingInfo> GetInvestingInfoAsync(string userIdentifier);

        Task<decimal> GetMaxIncomeSpeedAsync(decimal yield = 7);
        Task<IList<string>> GetUnResultOrdersAsync(DateTime time);
        Task<int> GetPaidCountAsync();

        Task<string> GetPledgeAgreementAsync(string orderIdentifier);

        Task<IList<SettlingProductInfo>> GetSettlingProductInfosAsync(string userIdentifier);

        Task<decimal> GetTheUserIncomeSpeedAsync(string userIdentifier);

        Task<IList<OrderInfo>> GetTheUserOrders(string userIdentifier, string productIdentifier);

        Task<List<OrderInfo>> GetTheUserActivityOrders(string userIdentifier);

        Task<List<OrderInfo>> GetTheUserOrdersForLuckhub(string userIdentifier, DateTime beginTime, DateTime endTime);
        /// <summary>
        /// 获取累计总收益
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetTotalInterestAsync();

        Task<IList<OrderInfo>> GetTheUserBeforeDateActivityOrders(string userIdentifier, DateTime lastTime);
        Task<IList<OrderInfo>> GetTheUserFixDateActivityOrders(string userIdentifier, DateTime lastTime);
        Task<IList<OrderInfo>> GetTheUserBetweenDateActivityOrders(string userIdentifier, DateTime beginTime, DateTime endTime);
        Task<decimal> GetTotalSaleAmountByProductAsync(string productIdentifier);
        Task<decimal> GetTotalInterestByProductAsync(string productIdentifier);
        Task<int> GetTotalCurrentAmountByProductAsync(string productIdentifier);
        Task<int> GetTotalRedeemInterestByProductAsync(string productIdentifier);
        Task<IList<string>> GetZcbOrderIdentifierList();
    }

    public interface IOrderInfoService<TOrder> where TOrder : OrderInfo
    {
        #region Public Methods

        Task<IPaginatedDto<TOrder>> GetFailedOrderInfosAsync(string userIdentifier,ProductCategory productCategory = ProductCategory.JINYINMAO, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime);

        Task<TOrder> GetOrderInfoAsync(string userIdentifier, string orderIdentifier);

        Task<TOrder> GetOrderInfoAsync(string orderIdentifier);

        Task<IPaginatedDto<TOrder>> GetOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime);

        Task<IPaginatedDto<TOrder>> GetPaidOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime);

        Task<IPaginatedDto<TOrder>> GetPayingOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime);

        Task<IPaginatedDto<TOrder>> GetSuccessfulOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime, ProductCategory productCategory = ProductCategory.JINYINMAO);

        #endregion Public Methods
    }
}