// FileInformation: nyanya/Domain.Order/Timeline.cs
// CreatedTime: 2014/04/26   6:13 PM
// LastUpdatedTime: 2014/04/27   11:45 AM

using System;
using System.Collections.Generic;

namespace Domain.Order.Models
{
    public class Timeline
    {
        public List<TimelineItem> Items { get; set; }

        public string Timestamp { get; set; }

        public Timeline()
        {
            this.Items = new List<TimelineItem>();
            this.Timestamp = DateTime.Today.ToString("yyyyMMddHHmm_ss").Remove(14);
        }
    }
}