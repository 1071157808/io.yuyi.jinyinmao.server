// FileInformation: nyanya/Cat.Domain.Users/IUserInfoService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:25 PM

using System.Collections.Generic;
using System.Threading.Tasks;
using Cat.Domain.Users.Models;
using Cat.Domain.Users.ReadModels;
using Domian.Models;

namespace Cat.Domain.Users.Services.Interfaces
{
    public interface IExactUserInfoService : IUserInfoService
    {
    }

    public interface IUserInfoService : IDomainService
    {
        Task<PaymentBankCardInfo> GetBankCardInfoAsync(string userIdentifier, string bankCardNo);

        Task<List<BankCardSummaryInfo>> GetBankCardsInfoAsync(string userIdentifier);

        Task<UserLoginInfo> GetLoginInfoAsync(string loginName);

        Task<UserInfo> GetUserInfoAsync(string userIdentifier);

        Task<YSBUserInfo> GetYSBInfoAsync(string userIdentifier);

        Task<int> GetAddBankFailCountAsync(string userIdentifier, string bankCardNo);
    }
}