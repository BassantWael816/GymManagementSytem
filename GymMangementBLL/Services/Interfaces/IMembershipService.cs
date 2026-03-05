using GymMangementBLL.ViewModels.MembershipViewModels;

namespace GymMangementBLL.Services.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<MembershipViewModel> GetActiveMemberships();
        bool CreateMembership(CreateMembershipViewModel createdMembership);
        bool CancelMembership(int memberId, int planId);
        IEnumerable<MemberSelectViewModel> GetMembersForDropDown();
    }
}
