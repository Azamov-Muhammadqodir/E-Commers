using E_Commers.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commers.Infrastructure.Persistence.Configuration
{
    public class UserCnfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.HasIndex(x=>x.Username).IsUnique();
            builder.HasIndex(x=>x.Email).IsUnique();
            builder.HasIndex(x=>x.Phone).IsUnique();
            builder.Property(user => user.Phone)
                .HasMaxLength(20)
                .IsRequired();
            builder.Property(user => user.Password).IsRequired();

        }
    }
}
