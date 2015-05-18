// FileInformation: nyanya/Cqrs.Domain.User/UserPaymentInfo.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/12   1:19 PM

using System;
using Domian.Models;

namespace Cat.Domain.Users.Models
{
    public class UserPaymentInfo : IEntity
    {
        public string EncryptedPassword { get; set; }

        public int FailedCount { get; set; }

        public DateTime LastFailedTime { get; set; }

        public string Salt { get; set; }

        public DateTime SetTime { get; set; }

        public string UserIdentifier { get; set; }
    }
}