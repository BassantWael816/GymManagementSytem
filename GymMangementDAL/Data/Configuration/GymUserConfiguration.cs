using GymMangementDAL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Data.Configuration
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(50);

            builder.Property(x => x.Email).HasColumnType("varchar").HasMaxLength(100);

            builder.Property(x => x.Phone).HasColumnType("varchar").HasMaxLength(11);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("CK_GymUserValidEmail", "Email like '_%@_%._%'");
                tb.HasCheckConstraint("CK_GymUserValidPhone", "phone like '01%' and Phone not like '%[^0-9]%'");
            });

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();

            builder.OwnsOne(x => x.Address, AddressBuilder =>
            {
                AddressBuilder.Property(x => x.Street).HasColumnName("Street").HasColumnType("varchar").HasMaxLength(30);
                AddressBuilder.Property(x => x.City).HasColumnName("City").HasColumnType("varchar").HasMaxLength(30);
                AddressBuilder.Property(x => x.BuildingNumber).HasColumnName("BuildingNumber");
            });
        }
    }
}
