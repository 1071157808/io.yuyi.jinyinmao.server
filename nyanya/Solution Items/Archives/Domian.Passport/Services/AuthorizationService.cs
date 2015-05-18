// FileInformation: nyanya/Domian.Passport/AuthorizationService.cs
// CreatedTime: 2014/03/31   3:30 PM
// LastUpdatedTime: 2014/03/31   6:30 PM

using Domain.Common;
using Domain.Passport.Models;
using Infrastructure.Cache.Couchbase;
using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Domian.Passport.Services
{
    public class AuthorizationService : DisposableService
    {
        private readonly PassportContext db = new PassportContext();
        private readonly PassportCache passportCache = new PassportCache();

        public async Task<bool> Authorize(string userName, string password)
        {
            UserInfo user = await this.db.UserInfos.FirstOrDefaultAsync(u => u.RealName == userName);

            return user != null && Verify(user, password);
        }

        public void DeleteToken(string token)
        {
            passportCache.RemoveToken(token);
        }

        public string GetUserGuidFromToken(string token)
        {
            return this.ValidateToken(token) ? this.passportCache.GetTokenInfo(token).GetValue("user_uuid").ToString() : null;
        }

        public async Task<bool> SignIn(string userName, string password, HttpContext context)
        {
            UserInfo user = await this.db.UserInfos.FirstOrDefaultAsync(u => u.RealName == userName);

            if (user != null && Verify(user, password) && !String.IsNullOrWhiteSpace(user.Guid))
            {
                string token = Guid.NewGuid().ToString().Replace("-", "");
                var tokenContent = new { user_uuid = user.Guid, app_id = 2, is_auto_login = 1 };

                if (passportCache.SaveToken(token, JsonConvert.SerializeObject(tokenContent)))
                {
                    context.Response.Cookies.Add(new HttpCookie("MeowAuthenticationToken", token));
                }
                else
                {
                    throw new Exception("Can not store token for " + userName);
                }
                return true;
            }
            return false;
        }

        public bool ValidateToken(string token)
        {
            return this.passportCache.ValidateToken(token);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }

        private static string ComputeDigest(string salt, string password)
        {
            string passwordMixSalt = string.Format("--{0}--{1}--", salt, password);

            return BitConverter.ToString(new SHA256CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(passwordMixSalt))).Replace("-", "").ToLower();
        }

        private static bool Verify(UserInfo user, string password)
        {
            string passwordDigest = ComputeDigest(user.Salt, password);

            return string.Equals(passwordDigest, user.Password);
        }
    }
}