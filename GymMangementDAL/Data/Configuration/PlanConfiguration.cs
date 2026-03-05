using GymMangementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Data.Configuration
{
    internal class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name).HasColumnType("varchar").HasMaxLength(50);

            builder.Property(p => p.Description).HasColumnType("varchar").HasMaxLength(200);

            builder.Property(p => p.Price).HasColumnType("decimal(10,2)");

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("CK_PlanDurationDaysConstraint", "DurationDays between 1 and 365");
            });
        }
    }
}
