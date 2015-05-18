// FileInformation: nyanya/Domian.Passport/User.cs
// CreatedTime: 2014/04/01   2:12 AM
// LastUpdatedTime: 2014/04/01   2:57 PM

using Domian.Passport.Services;

namespace Domain.Passport.Models
{
    public partial class User
    {
        public void ResetPassword(string password)
        {
            string salt = AuthenticationService.ComputeSalt(this.Name);
            string passwordDigest = AuthenticationService.ComputeDigest(salt, password);
            this.Salt = salt;
            this.EncryptedPassword = passwordDigest;
            this.MLogonFailedCount = 0;
        }
    }
}