// FileInformation: nyanya/Cqrs.Domain.Order/InvestorInfo.cs
// CreatedTime: 2014/07/27   7:49 PM
// LastUpdatedTime: 2014/07/27   7:50 PM

using Domian.Models;
using Xingye.Commands.Users;

namespace Xingye.Domain.Orders.Models
{
    public class InvestorInfo : IValueObject
    {
        public string Cellphone { get; set; }

        public Credential Credential { get; set; }

        public string CredentialNo { get; set; }

        public string OrderIdentifier { get; set; }

        public string RealName { get; set; }

        public string UserIdentifier { get; set; }
    }
}