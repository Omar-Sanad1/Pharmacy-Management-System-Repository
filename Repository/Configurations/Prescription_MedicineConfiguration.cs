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
    public class Prescription_MedicineConfiguration : IEntityTypeConfiguration<Prescription_Medicine>
    {
        public void Configure(EntityTypeBuilder<Prescription_Medicine> builder)
        {
            builder.HasKey(pm => new
            {
                pm.PrescriptionID,
                pm.MedicineID
            });

            builder.HasOne(x => x.Medicine)
                   .WithMany(x => x.prescription_Medicines)
                   .HasForeignKey(x => x.MedicineID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Prescription)
                   .WithMany(x => x.prescription_Medicines)
                   .HasForeignKey(x => x.PrescriptionID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
