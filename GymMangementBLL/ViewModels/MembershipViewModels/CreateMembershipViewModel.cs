using System.ComponentModel.DataAnnotations;

namespace GymMangementBLL.ViewModels.MembershipViewModels
{
    public class CreateMembershipViewModel
    {
        [Required(ErrorMessage = "Member Is Required")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Plan Is Required")]
        public int PlanId { get; set; }
    }
}
