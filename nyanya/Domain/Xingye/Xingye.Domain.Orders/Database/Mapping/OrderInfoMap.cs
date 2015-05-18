// FileInformation: nyanya/Cqrs.Domain.Order/OrderInfoMap.cs
// CreatedTime: 2014/08/10   1:24 PM
// LastUpdatedTime: 2014/08/12   12:44 PM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Orders.ReadModels;

namespace Xingye.Domain.Orders.Database.Mapping
{
    internal class OrderInfoMap : EntityTypeConfiguration<OrderInfo>
    {
        internal OrderInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderIdentifier);

            // Table & Column Mappings
            this.ToTable("OrderInfo", "dbo");
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderIdentifier");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BankCardCity).HasColumnName("BankCardCity");
            this.Property(t => t.BankCardNo).HasColumnName("BankCardNo");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.ConsignmentAgreementName).HasColumnName("ConsignmentAgreementName");
            this.Property(t => t.EndorseImageLink).HasColumnName("EndorseImageLink");
            this.Property(t => t.EndorseImageThumbnailLink).HasColumnName("EndorseImageThumbnailLink");
            this.Property(t => t.ExtraInterest).HasColumnName("ExtraInterest");
            this.Property(t => t.HasResult).HasColumnName("HasResult");
            this.Property(t => t.Interest).HasColumnName("Interest");
            this.Property(t => t.InvestorCellphone).HasColumnName("InvestorCellphone");
            this.Property(t => t.InvestorCredential).HasColumnName("InvestorCredential");
            this.Property(t => t.InvestorCredentialNo).HasColumnName("InvestorCredentialNo");
            this.Property(t => t.InvestorRealName).HasColumnName("InvestorRealName");
            this.Property(t => t.IsPaid).HasColumnName("IsPaid");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.OrderTime).HasColumnName("OrderTime");
            this.Property(t => t.OrderType).HasColumnName("OrderType");
            this.Property(t => t.PledgeAgreementName).HasColumnName("PledgeAgreementName");
            this.Property(t => t.Principal).HasColumnName("Principal");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductNo).HasColumnName("ProductNo");
            this.Property(t => t.ProductNumber).HasColumnName("ProductNumber");
            this.Property(t => t.ProductUnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.ResultTime).HasColumnName("ResultTime");
            this.Property(t => t.RepaymentDeadline).HasColumnName("RepaymentDeadline");
            this.Property(t => t.SettleDate).HasColumnName("SettleDate");
            this.Property(t => t.ShareCount).HasColumnName("ShareCount");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.UserIdentifier).HasColumnName("InvestorUserIdentifier");
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
            this.Property(t => t.PaymentInfoTransDesc).HasColumnName("TransDesc");
            this.Property(t => t.Yield).HasColumnName("Yield");
        }
    }
}