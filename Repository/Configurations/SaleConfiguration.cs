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
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.Property(s => s.Discount)
                   .IsRequired()
                   .HasColumnType("decimal(16,2)");

            builder.Property(s => s.TotalAmount)
                  .IsRequired()
                  .HasColumnType("decimal(16,2)");

            builder.Property(s => s.Subtotal)
                  .IsRequired()
                  .HasColumnType("decimal(16,2)");

            builder.Property(s => s.Taxes)
                  .IsRequired()
                  .HasColumnType("decimal(16,2)");

            builder.Property(s => s.PaymentStatus)
                 .IsRequired()
                 .HasMaxLength(150);

            builder.Property(s => s.SaleStatus)
                 .IsRequired()
                 .HasMaxLength(150);

            //////////////////////////////////////////////////////////////////////////////////

            builder.HasOne(s => s.Employee)
                   .WithMany(s => s.Sales)
                   .HasForeignKey(s => s.EmployeeID)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
