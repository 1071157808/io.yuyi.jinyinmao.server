// FileInformation: nyanya/Domian.Passport/Verification.cs
// CreatedTime: 2014/03/31   3:59 PM
// LastUpdatedTime: 2014/04/21   11:53 PM

using System;

namespace Domain.Passport.Models
{
    public class Verification
    {
        #region VerificationType enum

        public enum VerificationType
        {
            SignUp = 10,
            ResetPassword = 20,
            ChangePassword = 30
        }

        #endregion VerificationType enum

        public virtual DateTime BuildAt { get; set; }

        public virtual string Cellphone { get; set; }

        public virtual string Code { get; set; }

        public virtual int ErrorCount { get; set; }

        public virtual string Guid { get; set; }

        public virtual long Id { get; set; }

        public virtual int Times { get; set; }

        public virtual VerificationType Type { get; set; }

        public virtual bool Used { get; set; }

        public virtual bool Verified { get; set; }
    }
}