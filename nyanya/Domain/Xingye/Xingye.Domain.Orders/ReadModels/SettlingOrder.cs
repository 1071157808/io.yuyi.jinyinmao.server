// FileInformation: nyanya/Xingye.Domain.Orders/SettlingOrder.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:31 PM

using System;
using Domian.Models;
using Xingye.Commands.Orders;

namespace Xingye.Domain.Orders.ReadModels
{
    public class SettlingOrder : IReadModel
    {
        public decimal ExtraInterest { get; set; }

        public decimal Interest { get; set; }

        public OrderType OrderType { get; set; }

        public decimal Principal { get; set; }

        public string ProductIdentifier { get; set; }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        public int ProductNumber { get; set; }

        public DateTime SettleDate { get; set; }

        public string UserIdentifier { get; set; }
    }
}