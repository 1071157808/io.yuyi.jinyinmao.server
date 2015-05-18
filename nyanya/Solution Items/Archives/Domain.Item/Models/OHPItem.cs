// FileInformation: nyanya/Domain.Meow/OHPItem.cs
// CreatedTime: 2014/04/22   11:32 AM
// LastUpdatedTime: 2014/05/05   2:27 PM

using System;

namespace Domain.Item.Models
{
    public partial class OHPItem : Item
    {
        public virtual Nullable<decimal> ExtraInterest { get; set; }

        public virtual Nullable<int> OrderId { get; set; }
    }
}