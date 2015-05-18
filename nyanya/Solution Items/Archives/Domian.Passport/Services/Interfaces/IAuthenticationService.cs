// FileInformation: nyanya/Domian.Passport/IAuthenticationService.cs
// CreatedTime: 2014/04/02   4:26 PM
// LastUpdatedTime: 2014/04/02   4:28 PM

using System;
using System.Threading.Tasks;

namespace Domian.Passport.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<SignInResult> SignIn(string userName, string password);

        void SignOut();

        Task<string> SignUp(string name, string password);
    }

    public class SignInResult
    {
        public bool Lock
        {
            get { return this.RemainCount < 1; }
        }

        public int RemainCount { get; set; }

        public bool Successful { get; set; }

        public bool UserExist { get; set; }
    }
}