using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xingye.Domain.Yilian.ReadModels;

namespace Xingye.Domain.Yilian.Database.Mapping
{
    internal class YilianQueryViewMap: EntityTypeConfiguration<YilianQueryView>
    {
        internal YilianQueryViewMap()
        {
            // Primary Key
            this.HasKey(t => t.RequestIdentifier);

            // Table & Column Mappings
            this.ToTable("YilianQueryView", "dbo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RequestIdentifier).HasColumnName("RequestIdentifier");
            this.Property(t => t.QueryResult).HasColumnName("QueryResult");
            this.Property(t => t.GatewayResult).HasColumnName("GatewayResult");
            this.Property(t => t.CallbackResult).HasColumnName("CallbackResult");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.SequenceNo).HasColumnName("SequenceNo");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderIdentifier");
            this.Property(t => t.IsPayment).HasColumnName("IsPayment");
        }
    }
}
