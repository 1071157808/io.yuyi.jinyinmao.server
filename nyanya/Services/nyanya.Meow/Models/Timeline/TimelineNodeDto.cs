// FileInformation: nyanya/nyanya.Meow/TimelineNodeDto.cs
// CreatedTime: 2014/09/16   6:18 PM
// LastUpdatedTime: 2014/09/16   6:20 PM

using System;
using Cat.Domain.Orders.ReadModels;
using Infrastructure.Lib.Extensions;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     时间线节点
    /// </summary>
    public class TimelineNodeDto
    {
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
        public int Type { get; set; }
    }

    internal static class TimelineNodeExtensions
    {
        internal static TimelineNodeDto ToTimelineNodeDto(this TimelineNode node)
        {
            DateTime now = DateTime.Today;

            return new TimelineNodeDto
            {
                Identifier = node.Identifier,
                Interest = node.Interest,
                IsPast = node.Time < now,
                Principal = node.Principal,
                Time = node.Time.ToMeowFormat(),
                Type = Convert.ToInt32(node.Type)
            };
        }
    }
}