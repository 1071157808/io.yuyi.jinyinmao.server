// FileInformation: nyanya/Services.WebAPI.V1.nyanya/TimelineItemDto.cs
// CreatedTime: 2014/04/02   11:30 AM
// LastUpdatedTime: 2014/04/26   11:19 AM

namespace Services.WebAPI.V1.nyanya.Models.Order
{
    /// <summary>
    ///     时间线节点
    /// </summary>
    public class TimelineItemDto
    {
        #region Public Properties

        /// <summary>
        ///     Identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        ///     收益
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is past.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is past; otherwise, <c>false</c>.
        /// </value>
        public bool IsPast { get; set; }

        /// <summary>
        ///     本金
        /// </summary>
        public decimal Principal { get; set; }

        /// <summary>
        ///     Gets or sets the status.(Order.Status or Product.RaiseStatus)
        /// </summary>
        /// <value>
        ///     The status.
        /// </value>
        public int Status { get; set; }

        /// <summary>
        ///     时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        ///     类型  TimeLineItemType =>
        ///     -- Beginning = 0, (起始节点)
        ///     -- Order = 10,（订单节点）
        ///     -- Today = 20,（今天节点）
        ///     -- Product = 30,（产品节点）
        ///     -- End = 40（终止节点）
        /// </summary>
        //public TimeLineItemType Type { get; set; }
        public int Type
        {
            get { return 10; }
        }

        #endregion Public Properties
    }

    //internal static class TimeLineItemExtension
    //{
    //    public static TimelineItemDto ToTimelineItemDto(this TimelineItem item)
    //    {
    //        TimelineItemDto dto = Mapper.Map<TimelineItem, TimelineItemDto>(item);
    //        dto.Time = item.Time.HasValue ? item.Time.Value.ToString("yyyy-MM-ddTHH:mm:ss") : "";
    //        dto.IsPast = item.Time.HasValue && item.Time.Value < DateTime.Now.Date;
    //        return dto;
    //    }
    //}
}