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
    public class MedicineConfiguration : IEntityTypeConfiguration<Medicine>
    {
        public void Configure(EntityTypeBuilder<Medicine> builder)
        {
            builder.Property(m => m.MedicineName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(m => m.ScientificName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(m => m.BrandName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(m => m.Description)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(m => m.Category)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(m => m.DosageForm)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(m => m.Strength)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(m => m.Barcode)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(m => m.Manufacturer)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(m => m.PrescriptionRequirement)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(m => m.AvailabilityStatus)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(m => m.SellingPrice)
                   .IsRequired()
                   .HasColumnType("decimal(16,2)");

            builder.Property(m => m.PurchasePrice)
                   .IsRequired()
                   .HasColumnType("decimal(16,2)");

            //////////////////////////////////////////////////////////////////////////////////
            

        }
    }
}
