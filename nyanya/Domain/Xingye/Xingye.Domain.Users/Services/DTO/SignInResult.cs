// FileInformation: nyanya/Cqrs.Domain.User/SignInResult.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/07/28   3:10 PM

using Xingye.Domain.Users.Models;

namespace Xingye.Domain.Users.Services.DTO
{
    public class SignInResult
    {
        public bool Lock
        {
            get { return this.RemainCount < 1; }
        }

        public int RemainCount { get; set; }

        public bool Successful { get; set; }

        public bool UserExist { get; set; }

        public UserLoginInfo UserLoginInfo { get; set; }
    }
}