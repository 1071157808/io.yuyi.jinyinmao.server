// FileInformation: nyanya/Domain.Amp/TopProduct.cs
// CreatedTime: 2014/03/30   8:08 PM
// LastUpdatedTime: 2014/04/21   11:47 PM

using System;

namespace Domain.Amp.Models
{
    public class TopProduct
    {
        public virtual string BankName { get; set; }

        public virtual Nullable<int> Duration { get; set; }

        public virtual int Id { get; set; }

        public virtual int IsBest { get; set; }

        public virtual Nullable<int> MinNumber { get; set; }

        public virtual string Name { get; set; }

        public virtual string ProductIdentifier { get; set; }

        public virtual Nullable<int> TotalNumber { get; set; }

        public virtual Nullable<int> Unit { get; set; }

        public virtual Nullable<decimal> Yield { get; set; }
    }
}