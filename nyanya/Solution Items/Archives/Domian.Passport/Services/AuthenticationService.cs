// FileInformation: nyanya/Domian.Passport/AuthenticationService.cs
// CreatedTime: 2014/04/15   6:57 PM
// LastUpdatedTime: 2014/05/05   11:43 PM

using System;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Domain.Passport.Models;
using Domian.Passport.Services.Interfaces;

namespace Domian.Passport.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService userService;

        public AuthenticationService(IUserService userService)
        {
            this.userService = userService;
        }

        #region IAuthenticationService Members

        public async Task<SignInResult> SignIn(string userName, string password)
        {
            using (PassportContext db = new PassportContext())
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Name == userName);

                // 用户名无效
                if (user == null)
                {
                    return new SignInResult { RemainCount = -1, Successful = false, UserExist = false };
                }

                // 账号被锁定
                if (user.LastFailSignInAt.GetValueOrDefault(DateTime.MinValue).Date.Equals(DateTime.Today.Date))
                {
                    if (user.MLogonFailedCount.HasValue && user.MLogonFailedCount >= 5)
                    {
                        return new SignInResult { RemainCount = -1, Successful = false, UserExist = true };
                    }
                }

                // 密码错误
                if (!Verify(user, password))
                {
                    user.MLogonFailedCount = user.MLogonFailedCount.GetValueOrDefault();
                    user.MLogonFailedCount += 1;
                    user.LastFailSignInAt = DateTime.Now;
                    await db.SaveChangesAsync();
                    return new SignInResult { RemainCount = 5 - user.MLogonFailedCount.GetValueOrDefault(), Successful = false, UserExist = true };
                }

                // 登陆成功
                user.MLogonFailedCount = 0;
                user.LastFailSignInAt = DateTime.Now.AddDays(-2);
                user.LastSuccessfulSignInAt = DateTime.Now;
                await db.SaveChangesAsync();

                object cache = HttpRuntime.Cache.Get("AppSettings");
                int cookieValidityDuration = cache == null ? 300 : ((dynamic)cache).CookieValidityDuration;
                DateTime expiry = DateTime.Now.AddSeconds(cookieValidityDuration);
                FormsAuthentication.SetAuthCookie(string.Format("{0},{1},{2},{3},{4}", user.Name, user.Uuid, user.CreatedAt.ToShortDateString(), expiry.ToBinary(), user.Salt.Substring(0, 6)), true);
                return new SignInResult { RemainCount = 5, Successful = true, UserExist = true };
            }
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public async Task<string> SignUp(string name, string password)
        {
            User user = await this.userService.BuildUser(name, password);
            object cache = HttpRuntime.Cache.Get("AppSettings");
            int cookieValidityDuration = cache == null ? 300 : ((dynamic)cache).CookieValidityDuration;
            DateTime expiry = DateTime.Now.AddSeconds(cookieValidityDuration);
            FormsAuthentication.SetAuthCookie(string.Format("{0},{1},{2},{3},{4}", user.Name, user.Uuid, user.CreatedAt.ToShortDateString(), expiry.ToBinary(), user.Salt.Substring(0, 6)), true);
            return user.Uuid;
        }

        #endregion IAuthenticationService Members

        public static string ComputeDigest(string salt, string password)
        {
            string passwordMixSalt = string.Format("--{0}--{1}--", salt, password);

            return BitConverter.ToString(new SHA256CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(passwordMixSalt))).Replace("-", "").ToLower();
        }

        public static string ComputeSalt(string name)
        {
            string seed = string.Format("--{0}--{1}--", DateTime.Now, name);
            return BitConverter.ToString(new SHA256CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(seed))).Replace("-", "").ToLower();
        }

        private static bool Verify(User user, string password)
        {
            string passwordDigest = ComputeDigest(user.Salt, password);

            return string.Equals(passwordDigest, user.EncryptedPassword);
        }
    }
}