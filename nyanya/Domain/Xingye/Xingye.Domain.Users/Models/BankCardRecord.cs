// FileInformation: nyanya/Xingye.Domain.Users/BankCardRecord.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:24 PM

using System;
using Domian.Models;

namespace Xingye.Domain.Users.Models
{
    public class BankCardRecord : IEntity
    {
        public string BankCardNo { get; set; }

        public string BankName { get; set; }

        public string Cellphone { get; set; }

        public string CityName { get; set; }

        public int Id { get; set; }

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