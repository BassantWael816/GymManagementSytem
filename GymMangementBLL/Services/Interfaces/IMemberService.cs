using GymMangementBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();
        bool CreateMember(CreateMemberViewModel CreatedMember);
        MemberViewModel? GetMemberDetails(int  memberId);
        HealthRecordViewModel? GetHealthRecordDetails(int memberId);
        MemberToUpdateViewModel? GetMemberToUpdate(int memberId);
        bool UpdateMemberDetails(int Id , MemberToUpdateViewModel UpdatedMember);
        bool RemoveMember(int MemberId);
    }

}
