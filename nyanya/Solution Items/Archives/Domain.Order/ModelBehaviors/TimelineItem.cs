// FileInformation: nyanya/Domain.Order/TimelineItem.cs
// CreatedTime: 2014/04/21   1:30 AM
// LastUpdatedTime: 2014/04/29   5:10 PM

using System;
using System.Threading.Tasks;
using Domain.Order.Services;
using Domain.Order.Services.Interfaces;

namespace Domain.Order.Models
{
    public partial class TimelineItem
    {
        public static TimelineItem GetBeginningItem(DateTime time)
        {
            return new TimelineItem { Type = TimeLineItemType.Beginning, Time = time, Identifier = "Beginning", Interest = 0, Principal = 0 };
        }

        public static TimelineItem GetEndItem(string message)
        {
            return new TimelineItem { Type = TimeLineItemType.End, Identifier = message, Interest = 0, Principal = 0 };
        }

        public static async Task<TimelineItem> GetTodayItem(string guid)
        {
            IOrderSummaryService orderSummaryService = new OrderSummaryService();
            InvestmentSummaryDto investmentSummary = await orderSummaryService.GetInvestingSummary(guid);

            TimelineItem todayItem = new TimelineItem
            {
                Type = TimeLineItemType.Today,
                Time = DateTime.Today,
                Principal = investmentSummary.InvestingPrice,
                Interest = investmentSummary.ExpectedEarnings,
                Identifier = "Today"
            };

            return todayItem;
        }
    }
}