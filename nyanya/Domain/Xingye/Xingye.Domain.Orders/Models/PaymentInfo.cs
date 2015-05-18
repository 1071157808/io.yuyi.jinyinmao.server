// FileInformation: nyanya/Cqrs.Domain.Order/PaymentInfo.cs
// CreatedTime: 2014/07/29   10:36 AM
// LastUpdatedTime: 2014/08/04   2:17 AM

using System;
using Domian.Models;

namespace Xingye.Domain.Orders.Models
{
    public class PaymentInfo : IEntity
    {
        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string City { get; set; }

        public bool HasResult { get; set; }

        public bool IsPaid { get; set; }

        public string OrderIdentifier { get; set; }

        public DateTime? ResultTime { get; set; }

        public byte[] RowVersion { get; set; }

        public string TransDesc { get; set; }
    }
}