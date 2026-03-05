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
    internal class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.Property(ms => ms.CreatedAt).HasColumnName("StartDate").HasDefaultValueSql("GETDATE()");

            builder.HasKey(ms => new { ms.PlanId, ms.MemberId });

            builder.Ignore(ms => ms.Id);
        }
    }
}
