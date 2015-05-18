// FileInformation: nyanya/Domian.Passport/UserInfoService.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/06/15   6:45 PM

using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Domain.Passport.Models;
using Domian.Passport.Services.Interfaces;

namespace Domian.Passport.Services
{
    public class CurrentUser
    {
        public string Cellphone { get; set; }

        public string Guid { get; set; }

        public string Name
        {
            get { return this.Cellphone; }
        }

        public DateTime SignUpTime { get; set; }
    }

    public class UserInfoService : IUserInfoService, ITokenDigestProvider
    {
        #region ITokenDigestProvider Members

        public string GetDigestForUser(string name)
        {
            using (PassportContext context = new PassportContext())
            {
                UserInfo userInfo = context.UserInfos.FirstOrDefault(u => u.Cellphone == name);
                return userInfo != null ? userInfo.Salt.Substring(0, 6) : "-1";
            }
        }

        #endregion ITokenDigestProvider Members

        #region IUserInfoService Members

        public async Task<bool> Exist(string cellphone)
        {
            using (PassportContext db = new PassportContext())
            {
                return await db.UserInfos.CountAsync(u => u.Cellphone == cellphone) > 0;
            }
        }

        public CurrentUser GetCurrentUser(IPrincipal principal)
        {
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                return new CurrentUser();
            }

            string token = principal.Identity.Name;

            if (String.IsNullOrWhiteSpace(token) || token.Split(new[] { ',' }).Count() != 5)
            {
                return new CurrentUser();
            }

            string[] tokens = token.Split(new[] { ',' });
            DateTime signUpTime;
            DateTime.TryParse(tokens[2], out signUpTime);
            return new CurrentUser
            {
                Cellphone = tokens[0],
                Guid = tokens[1],
                SignUpTime = signUpTime
            };
        }

        public async Task<CurrentUser> GetCurrentUser(string name)
        {
            using (PassportContext context = new PassportContext())
            {
                var user = await context.UserInfos.Where(u => u.Cellphone == name).Select(u => new { u.Cellphone, u.Guid, u.CreatedAt }).FirstOrDefaultAsync();
                return user == null ? new CurrentUser() : new CurrentUser
                {
                    Cellphone = user.Cellphone,
                    Guid = user.Guid,
                    SignUpTime = user.CreatedAt
                };
            }
        }

        #endregion IUserInfoService Members
    }
}