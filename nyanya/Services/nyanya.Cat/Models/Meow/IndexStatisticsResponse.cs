// FileInformation: nyanya/nyanya.Cat/IndexStatisticsResponse.cs
// CreatedTime: 2014/09/09   3:59 PM
// LastUpdatedTime: 2014/09/09   4:03 PM

namespace nyanya.Cat.Models
{
    /// <summary>
    ///     IndexStatistics
    /// </summary>
    public class IndexStatistics
    {
        /// <summary>
        ///     累计购买订单数
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        ///     线下网点数量
        /// </summary>
        public int OutletCount { get; set; }

        /// <summary>
        ///     累计还款期数
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 累计创造总收益
        /// </summary>
        public decimal TotalInterest { get; set; }
    }
}