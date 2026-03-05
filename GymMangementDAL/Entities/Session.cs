using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        public Trainer SessionTrainer { get; set; } = null!;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category SessionCategory { get; set; } = null!;

        public ICollection<MemberSession> SessionMembers { get; set; } = null!;
    }
}
