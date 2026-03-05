using System.ComponentModel.DataAnnotations;

namespace GymMangementBLL.ViewModels.MemberSessionViewModels
{
    public class CreateBookingViewModel
    {
        [Required(ErrorMessage = "Member Is Required")]
        public int MemberId { get; set; }

        public int SessionId { get; set; }
    }
}
