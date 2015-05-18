// FileInformation: nyanya/Cqrs.Domain.User/SignInResult.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/07/28   3:10 PM

using Cat.Domain.Users.Models;

namespace Cat.Domain.Users.Services.DTO
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

        public string RCode { get; set; }

        public UserLoginInfo UserLoginInfo { get; set; }
    }
}