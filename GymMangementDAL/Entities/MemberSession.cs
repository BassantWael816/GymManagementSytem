using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Entities
{
    public class MemberSession : BaseEntity
    {
        //BookingDate == CreatedAt OfBaseEntity
        public bool IsAttended { get; set; }

        [ForeignKey("Member")]
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        [ForeignKey("Session")]
        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;
    }
}
