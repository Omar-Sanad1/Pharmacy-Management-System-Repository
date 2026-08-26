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
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.FullName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.EmployeePosition)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(e => e.LicenseNumber)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(e => e.Shift)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.EmploymentStatus)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Salary)
                   .IsRequired()
                   .HasColumnType("decimal(16,2)");

            //////////////////////////////////////////////////////////////////////////////////////////

            builder.HasOne(e => e.User)
                   .WithOne(e => e.Employee)
                   .HasForeignKey<Employee>(e => e.UserID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
