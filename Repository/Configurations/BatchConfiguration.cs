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
    public class BatchConfiguration : IEntityTypeConfiguration<Batch>
    {
        public void Configure(EntityTypeBuilder<Batch> builder)
        {
            builder.Property(b => b.BatchNumber)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(b => b.BatchNumber)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(b => b.PurchasePrice)
                   .IsRequired()
                   .HasColumnType("decimal(16,2)");

            /////////////////////////////////////////////////////////////////////

            builder.HasOne(b => b.Medicine)
                   .WithMany(b => b.Batches)
                   .HasForeignKey(b => b.MedicineID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
