// FileInformation: nyanya/Xingye.Domain.Users/YLUserInfo.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:30 PM

using System;
using Domian.Models;
using Xingye.Commands.Users;

namespace Xingye.Domain.Users.Models
{
    public class YLUserInfo : IEntity
    {
        public Credential Credential { get; set; }

        public string CredentialNo { get; set; }

        public string RealName { get; set; }

        public byte[] RowVersion { get; set; }

        public string UserIdentifier { get; set; }

        // false 是指未开始验证或者未完成验证
        // true指验证通过
        // 验证失败的记录会被删除
        public bool Verified { get; set; }

        public DateTime? VerifiedTime { get; set; }

        public DateTime VerifingTime { get; set; }
    }
}