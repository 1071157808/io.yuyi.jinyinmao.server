// FileInformation: nyanya/Cat.Domain.Orders/TimelineNode.cs
// CreatedTime: 2014/09/15   3:46 PM
// LastUpdatedTime: 2014/09/15   6:37 PM

using System;
using Domian.Models;

namespace Cat.Domain.Orders.ReadModels
{
    public enum TimelineNodeType
    {
        Beginning = 0,
        Order = 10,
        Today = 20,
        Product = 30,
        End = 40
    }

    public class TimelineNode : IReadModel
    {
        public string Identifier { get; set; }

        public decimal Interest { get; set; }

        public decimal Principal { get; set; }

        public DateTime Time { get; set; }

        public TimelineNodeType Type { get; set; }
    }
}