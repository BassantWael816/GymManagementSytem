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
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("CK_SessionCapacity", "Capacity between 1 and 25");
                tb.HasCheckConstraint("CK_SessionEndDateCheck", "EndDate > StartDate");
            });


            builder.ToTable("Sessions").HasOne(s => s.SessionTrainer).WithMany(t => t.TainerSessions).HasForeignKey(s => s.TrainerId);
            builder.ToTable("Sessions").HasOne(s => s.SessionCategory).WithMany(c => c.Sessions).HasForeignKey(s => s.CategoryId);
        }
    }
}
