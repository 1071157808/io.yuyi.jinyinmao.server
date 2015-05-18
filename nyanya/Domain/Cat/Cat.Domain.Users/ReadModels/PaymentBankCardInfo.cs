// FileInformation: nyanya/Cat.Domain.Users/PaymentBankCardInfo.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:25 PM

using Cat.Commands.Users;

namespace Cat.Domain.Users.ReadModels
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