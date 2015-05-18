// FileInformation: nyanya/Xingye.Domain.Users/IUserInfoService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:25 PM

using System.Collections.Generic;
using System.Threading.Tasks;
using Xingye.Domain.Users.Models;
using Xingye.Domain.Users.ReadModels;
using Domian.Models;

namespace Xingye.Domain.Users.Services.Interfaces
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

        Task<UserInfo> GetUserInfoByPhoneNoAsync(string cellPhoneNo);

        Task<YSBUserInfo> GetYSBInfoAsync(string userIdentifier);
    }
}