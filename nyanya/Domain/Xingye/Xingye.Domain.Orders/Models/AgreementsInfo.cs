// FileInformation: nyanya/Xingye.Domain.Orders/AgreementsInfo.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using Domian.Models;

namespace Xingye.Domain.Orders.Models
{
    public class AgreementsInfo : IValueObject
    {
        public string ConsignmentAgreementContent { get; set; }

        public string ConsignmentAgreementName { get; set; }

        public string OrderIdentifier { get; set; }

        public string PledgeAgreementContent { get; set; }

        public string PledgeAgreementName { get; set; }
    }
}