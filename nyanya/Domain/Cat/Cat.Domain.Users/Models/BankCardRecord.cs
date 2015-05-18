// FileInformation: nyanya/Cat.Domain.Users/BankCardRecord.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:24 PM

using Domian.Models;
using System;

namespace Cat.Domain.Users.Models
{
    public class BankCardRecord : IEntity
    {
        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string Cellphone { get; set; }

        public string CityName { get; set; }

        public long ClientType { get; set; }

        public long FlgMoreI1 { get; set; }

        public long FlgMoreI2 { get; set; }

        public string FlgMoreS1 { get; set; }

        public string FlgMoreS2 { get; set; }

        public int Id { get; set; }

        public string IpClient { get; set; }

        public bool IsForAuth
        {
            get { return this.SequenceNo.StartsWith("A"); }
        }

        public string Remark { get; set; }

        public byte[] RowVersion { get; set; }

        public string SequenceNo { get; set; }

        public string UserIdentifier { get; set; }

        public bool? Verified { get; set; }

        public DateTime? VerifiedTime { get; set; }

        public DateTime VerifingTime { get; set; }
    }
}
