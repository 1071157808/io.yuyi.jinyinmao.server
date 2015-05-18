// FileInformation: nyanya/Xingye.Domain.Users/UserInfo.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:29 PM

using System;
using Infrastructure.Lib.Extensions;
using Xingye.Commands.Users;

namespace Xingye.Domain.Users.ReadModels
{
    public class UserInfo
    {
        public string BankCardNo { get; set; }

        public int BankCardsCount { get; set; }

        public string CardBankName { get; set; }

        public string Cellphone { get; set; }

        public Credential? Credential { get; set; }

        public string CredentialNo { get; set; }

        public bool HasDefaultBankCard
        {
            get { return this.BankCardNo.IsNotNullOrEmpty(); }
        }

        public bool HasSetPaymentPassword
        {
            get { return this.PaymentPasswordSetTime.IsNotNull(); }
        }

        public bool HasYSBInfo
        {
            get { return this.YSBRealName.IsNotNullOrEmpty(); }
        }

        public int Id { get; set; }

        public string LoginName { get; set; }

        public DateTime? PaymentPasswordSetTime { get; set; }

        public string RealName { get; set; }

        public DateTime SignUpTime { get; set; }

        public string UserIdentifier { get; set; }

        public bool? Verified { get; set; }

        public string YSBIdCard { get; set; }

        public string YSBRealName { get; set; }

        /// <summary>
        /// 用户类型（10金银猫 20兴业）
        /// </summary>
        public int UserType { get; set; }
    }
}