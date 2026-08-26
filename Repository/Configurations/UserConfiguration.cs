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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(u => u.EmailAddress)
                  .IsRequired()
                  .HasMaxLength(250);

            builder.Property(u => u.PasswordHash)
                  .IsRequired()
                  .HasMaxLength(250);

            ////////////////////////////////////////////////////////////////

            builder.HasOne(u => u.Role)
                   .WithMany(u => u.Users)
                   .HasForeignKey(u => u.RoleID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
