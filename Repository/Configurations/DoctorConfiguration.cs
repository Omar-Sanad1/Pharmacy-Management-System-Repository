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
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.Property(d => d.LicenseNumber)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(d => d.FullName)
                  .IsRequired()
                  .HasMaxLength(200);

            builder.Property(d => d.PhoneNumber)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(d => d.Specialization)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(d => d.MedicalFacility)
                  .IsRequired()
                  .HasMaxLength(250);

            builder.Property(d => d.Status)
                 .IsRequired()
                 .HasMaxLength(100);

            ///////////////////////////////////////////////////////////////

            builder.HasOne(d => d.Branch)
                   .WithMany(d => d.Doctors)
                   .HasForeignKey(d => d.BranchID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.Prescriptions)
                   .WithOne(d => d.Doctor)
                   .HasForeignKey(d => d.DoctorID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
