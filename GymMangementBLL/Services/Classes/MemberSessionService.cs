using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.MemberSessionViewModels;
using GymMangementBLL.ViewModels.MembershipViewModels;
using GymMangementBLL.ViewModels.SessionViewModels;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;
using AutoMapper;

namespace GymMangementBLL.Services.Classes
{
    public class MemberSessionService : IMemberSessionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MemberSessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public IEnumerable<SessionViewModel> GetUpcomingSessions()
        {
            var sessions = unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory()
                .Where(s => s.StartDate > DateTime.Now);

            return MapSessionsWithSlots(sessions);
        }

        public IEnumerable<SessionViewModel> GetOngoingSessions()
        {
            var sessions = unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory()
                .Where(s => s.StartDate <= DateTime.Now && s.EndDate > DateTime.Now);

            return MapSessionsWithSlots(sessions);
        }

        public IEnumerable<MemberSessionViewModel> GetMembersForUpcomingSession(int sessionId)
        {
            var bookings = unitOfWork.GetRepository<MemberSession>()
                .GetAll(x => x.SessionId == sessionId);

            return bookings.Select(b => new MemberSessionViewModel
            {
                MemberId = b.MemberId,
                SessionId = b.SessionId,
                MemberName = unitOfWork.GetRepository<Member>().GetById(b.MemberId)?.Name ?? "Unknown",
                BookingDate = b.CreatedAt.ToString("MM/dd/yyyy hh:mm:ss tt"),
                IsAttended = b.IsAttended
            });
        }

        public IEnumerable<MemberSessionViewModel> GetMembersForOngoingSession(int sessionId)
        {
            // Same data, different view (attendance management)
            return GetMembersForUpcomingSession(sessionId);
        }

        public bool CreateBooking(CreateBookingViewModel booking)
        {
            try
            {
                // Session must exist and be upcoming
                var session = unitOfWork.SessionRepository.GetById(booking.SessionId);
                if (session is null || session.StartDate <= DateTime.Now) return false;

                // Member must exist
                var member = unitOfWork.GetRepository<Member>().GetById(booking.MemberId);
                if (member is null) return false;

                // Member must have active membership
                var hasActiveMembership = unitOfWork.GetRepository<Membership>()
                    .GetAll(x => x.MemberId == booking.MemberId && x.Status == "Active")
                    .Any();
                if (!hasActiveMembership) return false;

                // Must not already be booked
                var alreadyBooked = unitOfWork.GetRepository<MemberSession>()
                    .GetAll(x => x.SessionId == booking.SessionId && x.MemberId == booking.MemberId)
                    .Any();
                if (alreadyBooked) return false;

                // Must have available slots
                var bookedCount = unitOfWork.SessionRepository.GetCountOfBookedSlots(booking.SessionId);
                if (bookedCount >= session.Capacity) return false;

                var memberSession = new MemberSession
                {
                    MemberId = booking.MemberId,
                    SessionId = booking.SessionId,
                    IsAttended = false
                };

                unitOfWork.GetRepository<MemberSession>().Add(memberSession);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CancelBooking(int sessionId, int memberId)
        {
            try
            {
                // Can only cancel upcoming sessions
                var session = unitOfWork.SessionRepository.GetById(sessionId);
                if (session is null || session.StartDate <= DateTime.Now) return false;

                var booking = unitOfWork.GetRepository<MemberSession>()
                    .GetAll(x => x.SessionId == sessionId && x.MemberId == memberId)
                    .FirstOrDefault();

                if (booking is null) return false;

                unitOfWork.GetRepository<MemberSession>().Delete(booking);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool MarkAttendance(int sessionId, int memberId)
        {
            try
            {
                // Can only mark attendance for ongoing sessions
                var session = unitOfWork.SessionRepository.GetById(sessionId);
                if (session is null || session.StartDate > DateTime.Now || session.EndDate <= DateTime.Now)
                    return false;

                var booking = unitOfWork.GetRepository<MemberSession>()
                    .GetAll(x => x.SessionId == sessionId && x.MemberId == memberId)
                    .FirstOrDefault();

                if (booking is null || booking.IsAttended) return false;

                booking.IsAttended = true;
                unitOfWork.GetRepository<MemberSession>().Update(booking);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberSelectViewModel> GetMembersNotBookedForSession(int sessionId)
        {
            var bookedMemberIds = unitOfWork.GetRepository<MemberSession>()
                .GetAll(x => x.SessionId == sessionId)
                .Select(x => x.MemberId)
                .ToList();

            // Only members with active membership can book
            var activeMemberIds = unitOfWork.GetRepository<Membership>()
                .GetAll(x => x.Status == "Active")
                .Select(x => x.MemberId)
                .ToList();

            var availableMembers = unitOfWork.GetRepository<Member>()
                .GetAll(x => activeMemberIds.Contains(x.Id) && !bookedMemberIds.Contains(x.Id));

            return availableMembers.Select(m => new MemberSelectViewModel { Id = m.Id, Name = m.Name });
        }

        #region Helper Methods

        private IEnumerable<SessionViewModel> MapSessionsWithSlots(IEnumerable<Session> sessions)
        {
            var mapped = mapper.Map<IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mapped)
                session.AvailableSlots = session.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            return mapped;
        }

        #endregion
    }
}
