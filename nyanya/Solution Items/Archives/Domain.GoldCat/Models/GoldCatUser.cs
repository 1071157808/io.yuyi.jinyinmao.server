// FileInformation: nyanya/Domain.GoldCat/User.cs
// CreatedTime: 2014/04/04   9:45 AM
// LastUpdatedTime: 2014/04/04   9:56 AM

using System;

namespace Domain.GoldCat.Models
{
    public class GoldCatUser
    {
        public virtual string Cellphone { get; set; }

        public virtual Nullable<DateTime> CreatedAt { get; set; }

        public virtual string EncryptedPassword { get; set; }

        public virtual string Guid { get; set; }

        public virtual int Id { get; set; }

        public virtual string IdCard { get; set; }

        public virtual string Name { get; set; }

        public virtual string RealName { get; set; }

        public virtual string Salt { get; set; }

        public virtual Nullable<DateTime> UpdatedAt { get; set; }
    }
}