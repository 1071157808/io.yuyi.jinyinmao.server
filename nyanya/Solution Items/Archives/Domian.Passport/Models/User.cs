// FileInformation: nyanya/Domian.Passport/User.cs
// CreatedTime: 2014/04/15   6:57 PM
// LastUpdatedTime: 2014/04/21   11:52 PM

using System;

namespace Domain.Passport.Models
{
    public partial class User
    {
        public virtual Nullable<int> AppId { get; set; }

        public virtual string Cellphone { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual string Email { get; set; }

        public virtual string EncryptedPassword { get; set; }

        public virtual Nullable<int> ErrorSignInCount { get; set; }

        public virtual Nullable<int> HandlerId { get; set; }

        public virtual int Id { get; set; }

        public virtual Nullable<int> InvitationCodeId { get; set; }

        public virtual Nullable<DateTime> LastFailSignInAt { get; set; }

        public virtual Nullable<DateTime> LastSuccessfulSignInAt { get; set; }

        public virtual Nullable<int> MLogonFailedCount { get; set; }

        public virtual string Name { get; set; }

        public virtual string Salt { get; set; }

        public virtual string UnspayToken { get; set; }

        public virtual DateTime UpdatedAt { get; set; }

        public virtual string Uuid { get; set; }
    }
}