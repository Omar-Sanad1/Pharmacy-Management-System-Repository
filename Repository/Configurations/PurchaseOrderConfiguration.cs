using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configurations
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.Property(p => p.ApprovalStatus)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.PaymentStatus)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.PurchaseOrderStatus)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.TotalAmount)
                  .IsRequired()
                  .HasColumnType("decimal(16,2)");

            ///////////////////////////////////////////////////////////////////////////////////

            builder.HasOne(p => p.Branch)
                   .WithMany(p => p.PurchaseOrders)
                   .HasForeignKey(p => p.BranchID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
