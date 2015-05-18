// FileInformation: nyanya/Domian.Passport/UserInfo.cs
// CreatedTime: 2014/03/31   3:59 PM
// LastUpdatedTime: 2014/04/21   11:53 PM

using System;

namespace Domain.Passport.Models
{
    public partial class UserInfo
    {
        public virtual string Cellphone { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual Nullable<DateTime> FailedAt { get; set; }

        public virtual Nullable<int> FailuresCount { get; set; }

        public virtual string Guid { get; set; }

        public virtual int Id { get; set; }

        public virtual string IdCard { get; set; }

        public virtual string Password { get; set; }

        public virtual string RealName { get; set; }

        public virtual string Salt { get; set; }
    }
}