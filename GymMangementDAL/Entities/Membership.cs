using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Entities
{
    public class Membership : BaseEntity
    {
        //StartDate == CreatedAt
        public DateTime EndDate { get; set; }

        //ReadOnly Property
        public string Status
        {
            get
            {
                if (EndDate >= DateTime.Now)
                    return "Active";
                else
                    return "Expired";
            }
        }

        [ForeignKey(nameof(Plan))]
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;

        [ForeignKey(nameof(Member))]
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
    }
}
