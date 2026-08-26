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
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.FullName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Address)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(c => c.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(c => c.MedicalNotes)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(c => c.AccountStatus)
                   .IsRequired()
                   .HasMaxLength(100);

            /////////////////////////////////////////////////////////////////////

            builder.HasMany(c => c.Sales)
                   .WithOne(c => c.Customer)
                   .HasForeignKey(c => c.CustomerID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.User)
                   .WithOne(c => c.Customer)
                   .HasForeignKey<Customer>(c => c.UserID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
