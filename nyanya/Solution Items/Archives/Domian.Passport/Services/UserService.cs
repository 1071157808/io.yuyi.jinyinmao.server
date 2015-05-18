// FileInformation: nyanya/Domian.Passport/UserService.cs
// CreatedTime: 2014/05/13   8:14 AM
// LastUpdatedTime: 2014/05/14   5:08 PM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Domain.GoldCat.Models;
using Domain.Passport.Models;
using Domian.Passport.Services.Interfaces;
using Microsoft.Ajax.Utilities;

namespace Domian.Passport.Services
{
    public class UserService : IUserService
    {
        #region IUserService Members

        public Task AddSourceInfo(string name, HttpRequestHeaders httpRequestHeaders)
        {
            IEnumerable<CookieHeaderValue> cookies = httpRequestHeaders.GetCookies("MeowSource").ToList();
            CookieState cookieState = cookies.FirstOrDefault().IfNotNull(chv => chv.Cookies.FirstOrDefault(cs => cs.Name == "MeowSource"));
            return AddSourceInfo(name, cookieState != null ? cookieState.Value : "");
        }

        public async Task<User> BuildUser(string name, string password)
        {
            string salt = AuthenticationService.ComputeSalt(name);
            string passwordDigest = AuthenticationService.ComputeDigest(salt, password);

            User user = new User
            {
                AppId = 2,
                Cellphone = name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EncryptedPassword = passwordDigest,
                ErrorSignInCount = 0,
                MLogonFailedCount = 0,
                LastFailSignInAt = DateTime.Now.AddDays(-2),
                Name = name,
                Salt = salt,
                Uuid = Guid.NewGuid().ToString().ToLower().Replace("-", "")
            };

            using (PassportContext passportContext = new PassportContext())
            {
                passportContext.Users.Add(user);
                await passportContext.SaveChangesAsync();
            }

            GoldCatUser goldCatUser = new GoldCatUser
            {
                Name = user.Name,
                Cellphone = user.Cellphone,
                Salt = user.Salt,
                EncryptedPassword = user.EncryptedPassword,
                CreatedAt = DateTime.Now,
                Guid = Guid.NewGuid().ToString().ToLower().Replace("-", ""),
                UpdatedAt = DateTime.Now
            };

            using (GoldCatContext goldCatContext = new GoldCatContext())
            {
                goldCatContext.GoldCatUsers.Add(goldCatUser);
                await goldCatContext.SaveChangesAsync();
            }

            using (GoldCatContext goldCatContext = new GoldCatContext())
            {
                GoldCatUserDetails details = new GoldCatUserDetails
                {
                    Answer = "answer",
                    ErrorAnswerCount = 0,
                    ErrorSignInCount = 0,
                    LastFailSignInAt = DateTime.Now.AddDays(-2),
                    LastSignInAt = DateTime.Now.AddDays(-2),
                    LastSignInIp = "127.0.0.1",
                    Question = "question",
                    UpdatedAt = DateTime.Now.AddDays(-2),
                    UserId = goldCatUser.Id
                };
                goldCatContext.GoldCatUserDetailses.Add(details);
                await goldCatContext.SaveChangesAsync();
            }

            return user;
        }

        public async Task ResetPassword(string cellphone, string password)
        {
            User user;
            using (PassportContext passportContext = new PassportContext())
            {
                user = await passportContext.Users.FirstAsync(u => u.Cellphone == cellphone);
                user.ResetPassword(password);
                await passportContext.SaveChangesAsync();
            }

            using (GoldCatContext goldCatContext = new GoldCatContext())
            {
                GoldCatUser goldCatUser = await goldCatContext.GoldCatUsers.FirstAsync(u => u.Cellphone == cellphone);
                goldCatUser.Salt = user.Salt;
                goldCatUser.EncryptedPassword = user.EncryptedPassword;
                await goldCatContext.SaveChangesAsync();
            }
        }

        #endregion IUserService Members

        private static async Task AddSourceInfo(string name, string sourceIdentifier)
        {
            int appId;
            if (!Int32.TryParse(sourceIdentifier, out appId) || appId > 299 || appId < 200) return;

            using (PassportContext passportContext = new PassportContext())
            {
                // UNDONE: Batch update
                //return passportContext.Users.UpdateAsync(u => u.Cellphone == name, user => new User { AppId = appId });
                User user = await passportContext.Users.FirstOrDefaultAsync(u => u.Cellphone == name);
                user.AppId = appId;
                await passportContext.SaveChangesAsync();
            }
        }
    }
}