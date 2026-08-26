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
    public class Customer_MedicineConfiguration : IEntityTypeConfiguration<Customer_Medicine>
    {
        public void Configure(EntityTypeBuilder<Customer_Medicine> builder)
        {
            builder.HasKey(cm => new
            {
                cm.MedicineID,
                cm.CustomerID
            });

            builder.HasOne(x => x.Customer)
                   .WithMany(x => x.customer_Medicines)
                   .HasForeignKey(x => x.CustomerID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Medicine)
                  .WithMany(x => x.customer_Medicines)
                  .HasForeignKey(x => x.MedicineID)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
