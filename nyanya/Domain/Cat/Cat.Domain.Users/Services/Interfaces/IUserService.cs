// FileInformation: nyanya/Cat.Domain.Users/IUserService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:24 PM

using Cat.Domain.Users.Models;
using Cat.Domain.Users.Services.DTO;
using Domian.Models;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Cat.Domain.Users.Services.Interfaces
{
    public interface IUserService : IDomainService
    {
        /// <summary>
        ///     Checks the bank card avaliable asynchronous.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>银行卡已经被绑定返回true，否则返回false</returns>
        Task<bool> CheckBankCardAvaliableAsync(string userIdentifier, string bankCardNo);

        Task<AddBankCardResult> AddBankCardStatusAsync(string userIdentifier, string bankCardNo);

        /// <summary>
        ///     Checks the bank card avaliable asynchronous.
        /// </summary>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>银行卡已经被绑定返回true，否则返回false</returns>
        Task<bool> CheckBankCardAvaliableAsync(string bankCardNo);

        Task<bool> CheckCredentialNoAvaliableAsync(string credentialNo);

        Task<CheckPaymentPasswordResult> CheckPaymentPasswordAsync(string userIdentifier, string password);

        Task<bool> CompareWithLoginPassword(string userIdentifier, string password);

        Task<string> GetSignUpUserId(string cellphone);

        Task<bool> Exsits(string cellphone);

        CurrentUser GetCurrentUser(IPrincipal principal);

        Task SetDefaultBankCardAsync(string userIdentifier, string bankCardNo);

        Task<SignInResult> SignInAsync(string loginName, string password);

        Task<CheckCellPhoneResult> CheckCellPhoneAsync(string cellphone);

        Task<bool> HasBankCard(string userIdentifier);
    }

    public class CurrentUser
    {
        public CurrentUser()
        {
            this.ExpiryTime = DateTime.MinValue;
            this.Cellphone = "";
            this.Identifier = "";
        }

        public string Cellphone { get; set; }

        public DateTime ExpiryTime { get; set; }

        public string Identifier { get; set; }

        public string Name
        {
            get { return this.Cellphone; }
        }

        //public User User { get; set; }
    }
}