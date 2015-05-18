// FileInformation: nyanya/Cqrs.Domain.User/BankCardSummaryInfo.cs
// CreatedTime: 2014/07/28   11:35 AM
// LastUpdatedTime: 2014/08/08   12:02 PM

namespace Cat.Domain.Users.ReadModels
{
    public class BankCardSummaryInfo
    {
        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public int Id { get; set; }

        public bool IsDefault { get; set; }
    }
}