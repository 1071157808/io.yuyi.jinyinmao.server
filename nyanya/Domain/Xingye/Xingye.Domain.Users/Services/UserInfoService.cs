// FileInformation: nyanya/Cqrs.Domain.User/UserInfoService.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/25   3:08 PM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xingye.Domain.Users.Database;
using Xingye.Domain.Users.Models;
using Xingye.Domain.Users.ReadModels;
using Xingye.Domain.Users.Services.Interfaces;

namespace Xingye.Domain.Users.Services
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
                }).ToListAsync();
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

        public async Task<UserInfo> GetUserInfoByPhoneNoAsync(string cellPhoneNo)
        {
            using (UserContext context = this.UserContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<UserInfo>().FirstOrDefaultAsync(u => u.Cellphone == cellPhoneNo);
            }
        }

        public async Task<YSBUserInfo> GetYSBInfoAsync(string userIdentifier)
        {
            using (UserContext context = this.UserContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<YSBUserInfo>().FirstOrDefaultAsync(u => u.UserIdentifier == userIdentifier);
            }
        }

        #endregion Public Methods
    }
}