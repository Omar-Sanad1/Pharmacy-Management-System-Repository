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
    public class Supplier_MedicineConfiguration : IEntityTypeConfiguration<Supplier_Medicine>
    {
        public void Configure(EntityTypeBuilder<Supplier_Medicine> builder)
        {
            builder.HasKey(sm => new
            {
                sm.SupplierID,
                sm.MedicineID
            });

            builder.HasOne(x => x.Supplier)
                   .WithMany(x => x.supplier_Medicines)
                   .HasForeignKey(x => x.SupplierID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Medicine)
                   .WithMany(x => x.supplier_Medicines)
                   .HasForeignKey(x => x.MedicineID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
