// FileInformation: nyanya/Xingye.Domain.Orders/IOrderInfoService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using Domian.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xingye.Domain.Orders.ReadModels;
using Xingye.Domain.Orders.Services.DTO;

namespace Xingye.Domain.Orders.Services.Interfaces
{
    public enum OrderSortingStrategy
    {
        ByOrderTime = 0,
        BySettleDate = 1
    }

    public enum OrderMode
    {
        AllOrder = 0,
        SuccessfulOrder = 10,
        FailedOrder = 20
    }

    public interface IExactOrderInfoService : IOrderInfoService
    {
    }

    public interface IOrderInfoService : IOrderInfoService<OrderInfo>
    {
        Task<string> GetConsignmentAgreementAsync(string orderIdentifier);

        Task<InvestingInfo> GetInvestingInfoAsync(string userIdentifier);

        Task<decimal> GetMaxIncomeSpeedAsync(decimal yield = 7);

        Task<int> GetPaidCountAsync();

        Task<string> GetPledgeAgreementAsync(string orderIdentifier);

        Task<IList<SettlingProductInfo>> GetSettlingProductInfosAsync(string userIdentifier);

        Task<decimal> GetTheUserIncomeSpeedAsync(string userIdentifier);

        /// <summary>
        /// 获取累计总收益
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetTotalInterestAsync();
    }

    public interface IOrderInfoService<TOrder> where TOrder : OrderInfo
    {
        #region Public Methods

        Task<IPaginatedDto<TOrder>> GetFailedOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime);

        Task<TOrder> GetOrderInfoAsync(string userIdentifier, string orderIdentifier);

        Task<TOrder> GetOrderInfoAsync(string orderIdentifier);

        Task<IPaginatedDto<TOrder>> GetOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime);

        Task<IPaginatedDto<TOrder>> GetPaidOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime);

        Task<IPaginatedDto<TOrder>> GetPayingOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime);

        Task<IPaginatedDto<TOrder>> GetSuccessfulOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime);

        #endregion Public Methods
    }
}