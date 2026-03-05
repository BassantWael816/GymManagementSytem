namespace GymMangementBLL.ViewModels.MemberSessionViewModels
{
    public class MemberSessionViewModel
    {
        public int MemberId { get; set; }
        public int SessionId { get; set; }
        public string MemberName { get; set; } = null!;
        public string BookingDate { get; set; } = null!;
        public bool IsAttended { get; set; }
    }
}
