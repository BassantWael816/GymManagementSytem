namespace GymMangementBLL.ViewModels.MembershipViewModels
{
    public class MembershipViewModel
    {
        public int MemberId { get; set; }
        public int PlanId { get; set; }
        public string MemberName { get; set; } = null!;
        public string PlanName { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
