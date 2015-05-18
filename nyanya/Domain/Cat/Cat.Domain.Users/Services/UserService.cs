// FileInformation: nyanya/Cqrs.Domain.User/UserService.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/15   3:11 PM

using Cat.Domain.Users.Database;
using Cat.Domain.Users.Models;
using Cat.Domain.Users.Services.DTO;
using Cat.Domain.Users.Services.Interfaces;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Cat.Domain.Users.Services
{
    public class UserService : IUserService, ITokenDigestProvider
    {
        #region Private Fields

        private readonly Func<UserContext> userContextFactory;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserService" /> class.
        /// </summary>
        public UserService()
        {
            this.userContextFactory = () => new UserContext();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserService" /> class.
        /// </summary>
        /// <param name="userContextFactory">The user context factory.</param>
        public UserService(Func<UserContext> userContextFactory)
        {
            this.userContextFactory = userContextFactory;
        }

        #endregion Public Constructors

        #region ITokenDigestProvider Members

        public string GetDigestForUser(string name)
        {
            using (UserContext context = new UserContext())
            {
                UserLoginInfo info = context.ReadonlyQuery<UserLoginInfo>().FirstOrDefault(u => u.LoginName == name);
                return info != null ? info.Salt.Substring(0, 6) : "-1";
            }
        }

        #endregion ITokenDigestProvider Members

        #region IUserService Members

        /// <summary>
        ///     Checks the bank card avaliable asynchronous.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>银行卡已经被绑定返回true，否则返回false</returns>
        public async Task<bool> CheckBankCardAvaliableAsync(string userIdentifier, string bankCardNo)
        {
            using (UserContext context = new UserContext())
            {
                return await context.Query<BankCard>().AnyAsync(c => c.UserIdentifier == userIdentifier && c.BankCardNo == bankCardNo);
            }
        }

        /// <summary>
        ///     Checks the bank card avaliable asynchronous.
        /// </summary>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>银行卡已经被绑定返回true，否则返回false</returns>
        public async Task<bool> CheckBankCardAvaliableAsync(string bankCardNo)
        {
            using (UserContext context = new UserContext())
            {
                return await context.ReadonlyQuery<BankCard>().AnyAsync(b => b.BankCardNo == bankCardNo);
            }
        }

        /// <summary>
        ///     Credentials the no avaliable asynchronous.
        /// </summary>
        /// <param name="credentialNo">The bank card no.</param>
        /// <returns>证件已经被使用，返回false，证件没有被使用，返回true</returns>
        public async Task<bool> CheckCredentialNoAvaliableAsync(string credentialNo)
        {
            using (UserContext context = new UserContext())
            {
                return !(await context.ReadonlyQuery<YLUserInfo>().AnyAsync(u => u.CredentialNo == credentialNo));
            }
        }

        /// <summary>
        ///     Binding bank card result
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns></returns>
        public async Task<AddBankCardResult> AddBankCardStatusAsync(string userIdentifier, string bankCardNo)
        {
            AddBankCardResult result = null;
            using (UserContext context = new UserContext())
            {
                var q = await context.ReadonlyQuery<BankCardRecord>().Where(b => b.UserIdentifier == userIdentifier && b.BankCardNo == bankCardNo).OrderByDescending(b => b.VerifingTime).Select(b => new { Status = b.Verified, Message = b.Remark }).FirstOrDefaultAsync();
                if (q != null)
                {
                    result = new AddBankCardResult() { Status = q.Status.HasValue ? Convert.ToInt32(q.Status) : 2, Message = q.Message };
                }
            }
            return result;
        }

        /// <summary>
        ///     Checks the payment password asynchronous.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<CheckPaymentPasswordResult> CheckPaymentPasswordAsync(string userIdentifier, string password)
        {
            using (UserContext context = new UserContext())
            {
                UserPaymentInfo user = await context.Query<UserPaymentInfo>().FirstOrDefaultAsync(u => u.UserIdentifier == userIdentifier);

                // 用户名无效
                if (user == null)
                {
                    return new CheckPaymentPasswordResult { RemainCount = 0, Successful = false };
                }

                // 账号被锁定
                if (user.LastFailedTime.Date.Equals(DateTime.Today.Date))
                {
                    if (user.FailedCount >= 3)
                    {
                        return new CheckPaymentPasswordResult { RemainCount = 0, Successful = false };
                    }
                }

                // 密码错误
                if (!CryptographyUtils.Check(password, user.Salt, user.EncryptedPassword))
                {
                    user.FailedCount += 1;
                    user.LastFailedTime = DateTime.Now;
                    await context.SaveChangesAsync();
                    return new CheckPaymentPasswordResult { RemainCount = 3 - user.FailedCount, Successful = false };
                }

                // 验证成功
                //支付密码失败次数清零
                if (user.FailedCount > 0)
                {
                    user.FailedCount = 0;
                    await context.SaveChangesAsync();
                }
                return new CheckPaymentPasswordResult { RemainCount = 3, Successful = true };
            }
        }

        public async Task<bool> CompareWithLoginPassword(string userIdentifier, string password)
        {
            using (UserContext context = new UserContext())
            {
                var data = await context.ReadonlyQuery<UserLoginInfo>().Where(u => u.UserIdentifier == userIdentifier)
                    .Select(u => new { u.Salt, u.EncryptedPassword }).FirstOrDefaultAsync();
                if (string.IsNullOrEmpty(data.EncryptedPassword))
                {
                    return false;
                }
                return CryptographyUtils.Check(password, data.Salt, data.EncryptedPassword);
            }
        }

        public async Task<bool> Exsits(string cellphone)
        {
            using (UserContext context = new UserContext())
            {
                return await context.ReadonlyQuery<User>().AnyAsync(u => u.Cellphone == cellphone);
            }
        }

        public async Task<string> GetSignUpUserId(string cellphone)
        {
            using (UserContext context = new UserContext())
            {
                User user = await context.ReadonlyQuery<User>().Include(p => p.BankCards).FirstOrDefaultAsync(p => p.Cellphone == cellphone);

                //无此用户,号码未注册
                if (user.IsNull() || user.UserIdentifier.Equals(""))
                {
                    return GuidUtils.NewGuidString();
                }
                //已注册金银猫或已注册兴业且非临时用户，不可再注册
                if (user.UserType == 10 || (user.BankCards.IsNotNull() && user.BankCards.Count > 0 && user.UserType == 1))
                {
                    return "";
                }
                //兴业临时用户
                return user.UserIdentifier;
            }
        }

        public CurrentUser GetCurrentUser(IPrincipal principal)
        {
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                return new CurrentUser();
            }

            string token = principal.Identity.Name;

            if (String.IsNullOrWhiteSpace(token) || token.Split(new[] { ',' }).Count() != 3)
            {
                return new CurrentUser();
            }

            string[] tokens = token.Split(new[] { ',' });
            DateTime expiryTime = DateTime.MinValue;
            long expiry;
            if (long.TryParse(tokens[2], out expiry))
            {
                expiryTime = DateTime.FromBinary(expiry);
            }
            return new CurrentUser
            {
                Cellphone = tokens[1],
                Identifier = tokens[0],
                ExpiryTime = expiryTime
            };
        }

        public async Task SetDefaultBankCardAsync(string userIdentifier, string bankCardNo)
        {
            using (UserContext context = this.userContextFactory.Invoke())
            {
                BankCard card = await context.Query<BankCard>().FirstOrDefaultAsync(c => c.UserIdentifier == userIdentifier && c.BankCardNo == bankCardNo);
                if (card == null)
                {
                    return;
                }

                List<BankCard> defaultCards = await context.Query<BankCard>().Where(c => c.UserIdentifier == userIdentifier && c.IsDefault).ToListAsync();
                foreach (BankCard defaultCard in defaultCards)
                {
                    defaultCard.IsDefault = false;
                }

                card.IsDefault = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task<SignInResult> SignInAsync(string loginName, string password)
        {
            using (UserContext context = new UserContext())
            {
                UserLoginInfo user = await context.Query<UserLoginInfo>().FirstOrDefaultAsync(u => u.LoginName == loginName);

                // 用户名无效
                if (user == null)
                {
                    return new SignInResult { RemainCount = -1, Successful = false, UserExist = false, UserLoginInfo = null };
                }

                // 账号被锁定
                if (user.LastFailedSignInTime.Date.Equals(DateTime.Today.Date))
                {
                    if (user.LoginFailedCount >= 5)
                    {
                        return new SignInResult { RemainCount = -1, Successful = false, UserExist = true, UserLoginInfo = user };
                    }
                }

                // 密码错误
                if (!CryptographyUtils.Check(password, user.Salt, user.EncryptedPassword))
                {
                    user.LoginFailedCount += 1;
                    user.LastFailedSignInTime = DateTime.Now;
                    await context.SaveChangesAsync();
                    return new SignInResult { RemainCount = 5 - user.LoginFailedCount, Successful = false, UserExist = true, UserLoginInfo = user };
                }

                // 若为兴业用户则将其更改为金银猫用户
                var baseUser = await context.Query<User>().FirstOrDefaultAsync(u => u.UserIdentifier == user.UserIdentifier);
                if (baseUser != null && baseUser.UserType == 1)
                {
                    baseUser.UserType = 11;
                }

                // 登陆成功
                user.LoginFailedCount = 0;
                //user.LastFailedSignInTime = DateTime.Now.AddDays(-2);
                user.LastSuccessSignInTime = DateTime.Now;
                await context.SaveChangesAsync();

                return new SignInResult { RemainCount = 5, Successful = true, UserExist = true, UserLoginInfo = user };
            }
        }

        public async Task<CheckCellPhoneResult> CheckCellPhoneAsync(string cellphone)
        {
            User user;
            using (UserContext context = new UserContext())
            {
                user = await context.ReadonlyQuery<User>().Where(u => u.Cellphone == cellphone).FirstOrDefaultAsync();
            }

            if (user != null)
            {
                if (user.UserType == 1)
                {
                    return new CheckCellPhoneResult() { Result = await this.HasBankCard(user.UserIdentifier), UserType = user.UserType };
                }
                else
                {
                    return new CheckCellPhoneResult() { Result = true, UserType = user.UserType };
                }
            }
            else
            {
                return new CheckCellPhoneResult() { Result = false };
            }
        }

        public async Task<bool> HasBankCard(string userIdentifier)
        {
            using (UserContext context = new UserContext())
            {
                return await context.ReadonlyQuery<BankCard>().Where(b => b.UserIdentifier == userIdentifier).CountAsync() > 0;
            }
        }

        #endregion IUserService Members
    }
}