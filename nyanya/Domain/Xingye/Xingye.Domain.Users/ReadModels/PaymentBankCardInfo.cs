// FileInformation: nyanya/Xingye.Domain.Users/PaymentBankCardInfo.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:30 PM

using Xingye.Commands.Users;

namespace Xingye.Domain.Users.ReadModels
{
    public class PaymentBankCardInfo
    {
        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string Cellphone { get; set; }

        public string City { get; set; }

        public Credential Credential { get; set; }

        public string CredentialNo { get; set; }

        public string RealName { get; set; }

        public string UserIdentifier { get; set; }
    }
}