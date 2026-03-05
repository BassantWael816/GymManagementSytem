using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Entities
{
    public class Member : GymUser
    {
        //JoinDate == CreatedAt
        public string Photo { get; set; } = null!;
        public HealthRecord HealthRecord { get; set; } = null!;

        public ICollection<Membership> Memberships { get; set; } = null!;
        public ICollection<MemberSession> MemberSessions { get; set; } = null!;
    }
}
