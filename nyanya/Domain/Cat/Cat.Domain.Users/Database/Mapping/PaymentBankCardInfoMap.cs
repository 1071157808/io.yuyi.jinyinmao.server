// FileInformation: nyanya/Cqrs.Domain.User/PaymentBankCardInfoMap.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/12   1:19 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Users.ReadModels;

namespace Cat.Domain.Users.Database.Mapping
{
    internal class PaymentBankCardInfoMap : EntityTypeConfiguration<PaymentBankCardInfo>
    {
        internal PaymentBankCardInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.BankCardNo);

            // Table & Column Mappings
            this.ToTable("PaymentBankCardInfo", "dbo");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.BankCardNo).HasColumnName("BankCardNo");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Credential).HasColumnName("Credential");
            this.Property(t => t.CredentialNo).HasColumnName("CredentialNo");
            this.Property(t => t.RealName).HasColumnName("RealName");
        }
    }
}