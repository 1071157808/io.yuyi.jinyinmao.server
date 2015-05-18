// FileInformation: nyanya/nyanya.Cat/TimeLineOrderInfoDto.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   10:52 AM

namespace nyanya.Cat.Order
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