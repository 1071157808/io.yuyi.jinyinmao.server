// FileInformation: nyanya/Cat.Domain.Yilian/RequestInfo.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:24 PM

using Domian.Models;

namespace Cat.Domain.Yilian.Models
{
    public class RequestInfo : IValueObject
    {
        public string AccountName { get; set; }

        public string AccountNo { get; set; }

        public string BankName { get; set; }

        public string City { get; set; }

        public string IdNo { get; set; }

        public int IdType { get; set; }

        public string MobileNo { get; set; }

        public string OrderIdentifier { get; set; }

        public string ProductIdentifier { get; set; }

        public string ProductNo { get; set; }

        public string Province { get; set; }

        public string RequestIdentifier { get; set; }

        public string UserIdentifier { get; set; }
    }
}