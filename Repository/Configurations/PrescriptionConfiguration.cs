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
    public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.Property(p => p.PrescriptionStatus)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Notes)
                   .IsRequired()
                   .HasMaxLength(250);
            ///////////////////////////////////////////////////////////////////////////

            builder.HasOne(p => p.Employee)
                   .WithMany(p => p.Prescriptions)
                   .HasForeignKey(p => p.EmployeeID)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Customer)
                   .WithMany(p => p.Prescriptions)
                   .HasForeignKey(p => p.CustomerID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
