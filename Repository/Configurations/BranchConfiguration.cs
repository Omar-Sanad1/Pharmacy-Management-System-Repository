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
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.Property(b => b.EmailAddress)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(b => b.BranchName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(b => b.BranchCode)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(b => b.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(b => b.Location)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(b => b.Manager)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(b => b.OperationalStatus)
                  .IsRequired()
                  .HasMaxLength(100);

            ///////////////////////////////////////////////////////////////////////////

            builder.HasMany(b => b.Employees)
                   .WithOne(b => b.Branch)
                   .HasForeignKey(b => b.BranchID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.Sales)
                   .WithOne(b => b.Branch)
                   .HasForeignKey(b => b.BranchID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
