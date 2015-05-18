// FileInformation: nyanya/Cqrs.Domain.User/YSBUserInfo.cs
// CreatedTime: 2014/07/17   12:13 AM
// LastUpdatedTime: 2014/07/17   12:14 AM

using Domian.Models;

namespace Cat.Domain.Users.Models
{
    public class YSBUserInfo : IValueObject
    {
        public string IdCard { get; set; }

        public string RealName { get; set; }

        public string UserIdentifier { get; set; }
    }
}