// FileInformation: nyanya/Cqrs.Domain.User/UserInfoService.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/25   3:08 PM

using Cat.Domain.Users.Database;
using Cat.Domain.Users.Models;
using Cat.Domain.Users.ReadModels;
using Cat.Domain.Users.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Cat.Domain.Users.Services
{
    public class UserInfoService : IExactUserInfoService
    {
        #region Private Properties

        private Func<UserContext> UserContextFactory
        {
            get { return () => new UserContext(); }
        }

        #endregion Private Properties

        #region Public Methods

        public async Task<PaymentBankCardInfo> GetBankCardInfoAsync(string userIdentifier, string bankCardNo)
        {
            using (UserContext context = this.UserContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<PaymentBankCardInfo>().FirstOrDefaultAsync(i => i.UserIdentifier == userIdentifier && i.BankCardNo == bankCardNo);
            }
        }

        public async Task<List<BankCardSummaryInfo>> GetBankCardsInfoAsync(string userIdentifier)
        {
            using (UserContext context = this.UserContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<BankCard>().Where(c => c.UserIdentifier == userIdentifier).Select(c => new BankCardSummaryInfo
                {
                    BankCardNo = c.BankCardNo,
                    BankName = c.BankName,
                    Id = c.Id,
                    IsDefault = c.IsDefault
                }).OrderBy(p => p.Id).ToListAsync();
            }
        }

        public async Task<UserLoginInfo> GetLoginInfoAsync(string loginName)
        {
            using (UserContext context = this.UserContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<UserLoginInfo>().FirstOrDefaultAsync(u => u.LoginName == loginName);
            }
        }

        public async Task<UserInfo> GetUserInfoAsync(string userIdentifier)
        {
            using (UserContext context = this.UserContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<UserInfo>().FirstOrDefaultAsync(u => u.UserIdentifier == userIdentifier);
            }
        }

        public async Task<YSBUserInfo> GetYSBInfoAsync(string userIdentifier)
        {
            using (UserContext context = this.UserContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<YSBUserInfo>().FirstOrDefaultAsync(u => u.UserIdentifier == userIdentifier);
            }
        }

        public async Task<int> GetAddBankFailCountAsync(string userIdentifier, string bankCardNo)
        {
            using (UserContext context = this.UserContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<BankCardRecord>().CountAsync(u => u.UserIdentifier == userIdentifier
                    && u.BankCardNo == bankCardNo
                    && DbFunctions.TruncateTime(u.VerifingTime) == DbFunctions.TruncateTime(DateTime.Now)
                    && (!u.Verified.HasValue || !u.Verified.Value));
            }
        }

        #endregion Public Methods
    }
}