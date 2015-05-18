// FileInformation: nyanya/Domain.Meow/Item.cs
// CreatedTime: 2014/04/22   11:05 AM
// LastUpdatedTime: 2014/04/23   6:16 PM

using System;

namespace Domain.Meow.Models
{
    public partial class Item
    {
        public virtual Category Category { get; set; }

        public virtual int CategoryId { get; set; }

        public virtual DateTime Expires { get; set; }

        public virtual int Id { get; set; }

        public virtual bool IsUsed { get; set; }

        public virtual string OwnerGuid { get; set; }

        public virtual DateTime ReceiveTime { get; set; }

        public virtual Nullable<DateTime> UseTime { get; set; }
    }
}