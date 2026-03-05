using GymMangementBLL.ViewModels.MemberSessionViewModels;
using GymMangementBLL.ViewModels.MembershipViewModels;
using GymMangementBLL.ViewModels.SessionViewModels;

namespace GymMangementBLL.Services.Interfaces
{
    public interface IMemberSessionService
    {
        // Index: sessions grouped by status for management
        IEnumerable<SessionViewModel> GetUpcomingSessions();
        IEnumerable<SessionViewModel> GetOngoingSessions();

        // Upcoming session: view booked members + cancel booking
        IEnumerable<MemberSessionViewModel> GetMembersForUpcomingSession(int sessionId);

        // Ongoing session: view members + mark attendance
        IEnumerable<MemberSessionViewModel> GetMembersForOngoingSession(int sessionId);

        // Create booking
        bool CreateBooking(CreateBookingViewModel booking);

        // Cancel booking
        bool CancelBooking(int sessionId, int memberId);

        // Mark attendance
        bool MarkAttendance(int sessionId, int memberId);

        // Members dropdown for booking form
        IEnumerable<MemberSelectViewModel> GetMembersNotBookedForSession(int sessionId);
    }
}
