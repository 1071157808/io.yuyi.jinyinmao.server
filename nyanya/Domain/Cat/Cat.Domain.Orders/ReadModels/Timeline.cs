// FileInformation: nyanya/Cat.Domain.Orders/Timeline.cs
// CreatedTime: 2014/09/15   3:57 PM
// LastUpdatedTime: 2014/09/16   6:20 PM

using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Lib.Extensions;

namespace Cat.Domain.Orders.ReadModels
{
    public class Timeline
    {
        public Timeline()
        {
            this.Nodes = new List<TimelineNode>();
            this.Timestamp = DateTime.Now.ToTimestamp();
        }

        public List<TimelineNode> Nodes { get; set; }

        public string Timestamp { get; set; }

        public TimelineNode GetBeginningNode()
        {
            return this.Nodes.FirstOrDefault(n => n.Type == TimelineNodeType.Beginning);
        }

        public TimelineNode GetEndNode()
        {
            return this.Nodes.FirstOrDefault(n => n.Type == TimelineNodeType.End);
        }

        public IEnumerable<TimelineNode> GetPastNodes()
        {
            int todayIndex = this.GetTodayNodeIndex();
            return this.Nodes.Take(todayIndex);
        }

        public IEnumerable<TimelineNode> GetPastTimelineNodes()
        {
            return this.Nodes.TakeWhile(n => n.Type == TimelineNodeType.Today);
        }

        public TimelineNode GetTodayNode()
        {
            return this.Nodes.FirstOrDefault(n => n.Type == TimelineNodeType.Today);
        }

        public int GetTodayNodeIndex()
        {
            return this.Nodes.FindIndex(n => n.Type == TimelineNodeType.Today);
        }
    }
}