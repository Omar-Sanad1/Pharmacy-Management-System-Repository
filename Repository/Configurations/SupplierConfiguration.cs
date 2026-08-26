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
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.Property(s => s.CompanyName)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(s => s.ContactPerson)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(s => s.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(s => s.EmailAddress)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(s => s.Address)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(s => s.TaxInformation)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(s => s.PaymentTerms)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(s => s.SupplierStatus)
                   .IsRequired()
                   .HasMaxLength(100);

            ////////////////////////////////////////////////////////////////////////////////

            builder.HasMany(s => s.PurchaseOrders)
                   .WithOne(s => s.Supplier)
                   .HasForeignKey(s => s.SupplierID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
