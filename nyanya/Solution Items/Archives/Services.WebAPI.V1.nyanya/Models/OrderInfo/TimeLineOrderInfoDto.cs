// FileInformation: nyanya/Services.WebAPI.V1.nyanya/TimeLineOrderInfoDto.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/08/09   6:24 PM

namespace Services.WebAPI.V1.nyanya.Models.Order
{
    /// <summary>
    ///     时间线订单节点详情
    /// </summary>
    public class TimeLineOrderInfoDto
    {
        /// <summary>
        ///     订单内容
        /// </summary>
        public OrderWithItemDto Order { get; set; }

        /// <summary>
        ///     产品内容
        /// </summary>
        //public SummaryProductDto Product { get; set; }
    }
}