// FileInformation: nyanya/Domain.Order/IOrderSummaryService.cs
// CreatedTime: 2014/04/03   12:59 PM
// LastUpdatedTime: 2014/04/29   5:10 PM

using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Order.Models;

namespace Domain.Order.Services.Interfaces
{
    public interface IOrderSummaryService
    {
        Task<List<OrderSummary>> GetIntereSettledOrders(string userGuid);

        Task<List<OrderSummary>> GetInvestingOrders(string userGuid);

        Task<InvestmentSummaryDto> GetInvestingSummary(string userGuid);

        Task<InvestmentSummaryDto> GetInvestmentSummary(string userGuid);
    }

    /// <summary>
    ///     投资概况
    /// </summary>
    public class InvestmentSummaryDto
    {
        /// <summary>
        ///     已获收益
        /// </summary>
        public decimal Earnings { get; set; }

        /// <summary>
        ///     预期收益
        /// </summary>
        public decimal ExpectedEarnings { get; set; }

        /// <summary>
        ///     在投金额
        /// </summary>
        public decimal InvestingPrice { get; set; }
    }
}