// FileInformation: nyanya/Cqrs.Domain.User/UserLoginInfo.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/07/31   10:34 AM

using System;
using Domian.Models;

namespace Cat.Domain.Users.Models
{
    public class UserLoginInfo : IEntity
    {
        public string EncryptedPassword { get; set; }

        public DateTime LastFailedSignInTime { get; set; }

        public int LoginFailedCount { get; set; }

        public string LoginName { get; set; }

        public string Salt { get; set; }

        public DateTime SignUpTime { get; set; }

        public string UserIdentifier { get; set; }

        public DateTime? LastSuccessSignInTime { get; set; }
    }
}