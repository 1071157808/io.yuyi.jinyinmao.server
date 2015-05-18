// FileInformation: nyanya/Cqrs.Domain.User/CheckPaymentPasswordResult.cs
// CreatedTime: 2014/07/28   3:03 PM
// LastUpdatedTime: 2014/07/28   3:04 PM

namespace Cat.Domain.Users.Services.DTO
{
    public class CheckPaymentPasswordResult
    {
        public bool Lock
        {
            get { return this.RemainCount < 1; }
        }

        public int RemainCount { get; set; }

        public bool Successful { get; set; }
    }
}