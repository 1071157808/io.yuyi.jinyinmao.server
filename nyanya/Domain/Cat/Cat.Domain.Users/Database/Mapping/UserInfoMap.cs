// FileInformation: nyanya/Cqrs.Domain.User/UserInfoMap.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/12   1:19 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Users.ReadModels;

namespace Cat.Domain.Users.Database.Mapping
{
    internal class UserInfoMap : EntityTypeConfiguration<UserInfo>
    {
        internal UserInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.UserIdentifier);

            // Table & Column Mappings
            this.ToTable("UserInfos", "dbo");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.BankCardNo).HasColumnName("BankCardNo");
            this.Property(t => t.BankCardsCount).HasColumnName("BankCardsCount");
            this.Property(t => t.CardBankName).HasColumnName("CardBankName");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.Credential).HasColumnName("Credential");
            this.Property(t => t.CredentialNo).HasColumnName("CredentialNo");
            this.Property(t => t.LoginName).HasColumnName("LoginName");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.SignUpTime).HasColumnName("SignUpTime");
            this.Property(t => t.PaymentPasswordSetTime).HasColumnName("PaymentPasswordSetTime");
            this.Property(t => t.Verified).HasColumnName("Verified");
            this.Property(t => t.YSBIdCard).HasColumnName("YSBIdCard");
            this.Property(t => t.YSBRealName).HasColumnName("YSBRealName");
            this.Property(t => t.UserType).HasColumnName("UserType");
        }
    }
}