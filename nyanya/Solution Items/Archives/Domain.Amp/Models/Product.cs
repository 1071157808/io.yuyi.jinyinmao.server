// FileInformation: nyanya/Domain.Amp/Product.cs
// CreatedTime: 2014/03/25   10:04 AM
// LastUpdatedTime: 2014/03/27   4:34 PM

using System;

namespace Domain.Amp.Models
{
    //TODO:Ensuring the New Instances Get Proxies
    public partial class Product
    {
        public virtual string BankName { get; set; }

        public virtual Nullable<DateTime> DueDate { get; set; }

        public virtual Nullable<int> Duration { get; set; }

        public virtual int Id { get; set; }

        public virtual int IsRecommand { get; set; }

        public virtual Nullable<int> MaxNumber { get; set; }

        public virtual Nullable<int> MinNumber { get; set; }

        public virtual string Name { get; set; }

        public virtual string ProductIdentifier { get; set; }

        public virtual Nullable<DateTime> PubBegin { get; set; }

        public virtual Nullable<DateTime> PubEnd { get; set; }

        public virtual Nullable<byte> RaiseStatus { get; set; }

        public virtual Nullable<byte> SalesStatus { get; set; }

        public virtual Nullable<DateTime> SettleDay { get; set; }

        public virtual Nullable<int> TotalNumber { get; set; }

        public virtual Nullable<int> Unit { get; set; }

        public virtual Nullable<DateTime> ValueDay { get; set; }

        public virtual Nullable<decimal> Yield { get; set; }
    }
}